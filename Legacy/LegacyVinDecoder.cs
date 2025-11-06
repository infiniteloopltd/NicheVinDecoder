using NicheVinDecoder.Core.Interfaces;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Legacy
{
    public class LegacyVinDecoder : IVinDecoder
    {
        // Lazy-load embedded JSON once per AppDomain
        private static readonly Lazy<Dictionary<string, Legacy.Models.LegacyYearEntry>> _years
            = new(() => Legacy.LegacyDataLoader.LoadYears());

        private static readonly Lazy<Dictionary<string, Legacy.Models.LegacyModelEntry>> _models
            = new(() => Legacy.LegacyDataLoader.LoadAllModels());

        public string ManufacturerName { get; }
        public IEnumerable<string> SupportedWMIs { get; }

        public bool CanDecode(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin)) return false;
            // Legacy focus: pre-1981 are typically < 17 chars (be generous but bounded)
            return vin.Length >= 9 && vin.Length <= 16;
        }

        public VinDecodingResult Decode(string vin)
        {
            var result = new VinDecodingResult
            {
                VIN = vin
            };

            if (string.IsNullOrWhiteSpace(vin)) return result;
            if (_models.Value.Count == 0) return result;

            // Longest-prefix match: try 3, then 2 chars
            var tryPrefixes = new List<string>();
            if (vin.Length >= 3) tryPrefixes.Add(vin.Substring(0, 3));
            if (vin.Length >= 2) tryPrefixes.Add(vin.Substring(0, 2));

            string? matchedPrefix = tryPrefixes.FirstOrDefault(p => _models.Value.ContainsKey(p));
            if (matchedPrefix == null)
                return result; // unknown prefix

            var modelEntry = _models.Value[matchedPrefix];
            result.Manufacturer = modelEntry.make;
    
            result.AdditionalProperties = new Dictionary<string, object> { { "notes", modelEntry.notes } };

            // Model extraction (safe bounds)
            if (modelEntry.model_position_range is { Length: 2 } rng)
            {
                int start = Math.Max(0, rng[0] - 1);
                int end = Math.Max(0, rng[1] - 1);
                if (end >= start && start < vin.Length)
                {
                    int len = Math.Min(vin.Length - start, (end - start + 1));
                    var slice = vin.Substring(start, len);

                    // Match by StartsWith for codes of varying length
                    var match = modelEntry.model_map
                        .OrderByDescending(kv => kv.Key.Length) // prefer longest code
                        .FirstOrDefault(kv => slice.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase));
                    if (!string.IsNullOrEmpty(match.Value))
                        result.Model = match.Value;
                }
            }

            // Year decoding (if we have a rule for the matched prefix)
            if (_years.Value.TryGetValue(matchedPrefix, out var y) &&
                y.year_position > 0 &&
                y.year_position <= vin.Length &&
                !string.IsNullOrWhiteSpace(y.year_sequence))
            {
                var seq = y.year_sequence
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();
                var ch = vin[y.year_position - 1].ToString();
                int idx = Array.IndexOf(seq, ch);
                if (idx >= 0)
                    result.ModelYear = y.start_year + idx;
            }

            return result;
        }
    }
}
