using NicheVinDecoder.Legacy.Models;
using System.Reflection;
using System.Text.Json;

namespace NicheVinDecoder.Legacy
{
    public static class LegacyDataLoader
    {
        private static readonly Assembly Assembly = typeof(LegacyDataLoader).Assembly;

        public static Dictionary<string, T> LoadJson<T>(string resourceName)
        {
            var fullName = Assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (fullName == null)
                throw new InvalidOperationException($"Resource {resourceName} not found in assembly.");

            using var stream = Assembly.GetManifestResourceStream(fullName)
                ?? throw new InvalidOperationException($"Could not open {resourceName} resource stream.");

            return JsonSerializer.Deserialize<Dictionary<string, T>>(stream)
                ?? new Dictionary<string, T>();
        }


        public static Dictionary<string, LegacyYearEntry> LoadYears()
            => LoadJson<LegacyYearEntry>("legacy_year_map.json");

        public static Dictionary<string, LegacyModelEntry> LoadAllModels()
        {
            var combined = new Dictionary<string, LegacyModelEntry>();

            foreach (var res in Assembly.GetManifestResourceNames()
                                        .Where(n => n.Contains("legacy_model_map_") || n.Contains("legacy_model_supplemental")))
            {
                using var stream = Assembly.GetManifestResourceStream(res);
                if (stream == null) continue;
                var models = JsonSerializer.Deserialize<Dictionary<string, LegacyModelEntry>>(stream);
                if (models == null) continue;

                foreach (var kv in models)
                    combined[kv.Key] = kv.Value;
            }

            return combined;
        }
    }
}
