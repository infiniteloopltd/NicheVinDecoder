using NicheVinDecoder.Legacy.Models;
using Newtonsoft.Json;
using System.Reflection;

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

            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var serializer = new JsonSerializer();
            return serializer.Deserialize<Dictionary<string, T>>(jsonReader)
                ?? new Dictionary<string, T>();
        }

        public static Dictionary<string, LegacyYearEntry> LoadYears()
            => LoadJson<LegacyYearEntry>("legacy_year_map.json");

        public static Dictionary<string, LegacyModelEntry> LoadAllModels()
        {
            var combined = new Dictionary<string, LegacyModelEntry>();
            var serializer = new JsonSerializer();

            foreach (var res in Assembly.GetManifestResourceNames()
                                        .Where(n => n.Contains("legacy_model_map_") || n.Contains("legacy_model_supplemental")))
            {
                using var stream = Assembly.GetManifestResourceStream(res);
                if (stream == null) continue;

                using var reader = new StreamReader(stream);
                using var jsonReader = new JsonTextReader(reader);

                var models = serializer.Deserialize<Dictionary<string, LegacyModelEntry>>(jsonReader);
                if (models == null) continue;

                foreach (var kv in models)
                {
                    if (combined.ContainsKey(kv.Key))
                    {
                        // Merge model_map dictionaries if the key already exists
                        foreach (var modelKv in kv.Value.model_map)
                        {
                            combined[kv.Key].model_map[modelKv.Key] = modelKv.Value;
                        }
                    }
                    else
                    {
                        combined[kv.Key] = kv.Value;
                    }
                }
            }

            return combined;
        }
    }
}