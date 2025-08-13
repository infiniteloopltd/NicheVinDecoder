using NicheVinDecoder.Core.Base;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Manufacturers
{
    public class LoadGlideVinDecoder : BaseVinDecoder
    {
        public override string ManufacturerName => "Load Glide Trailers";

        public override IEnumerable<string> SupportedWMIs => new[] { "1L9" };

        private static readonly Dictionary<char, string> MaterialMap = new()
        {
            { 'A', "Aluminum" }
        };

        private static readonly Dictionary<char, string> ConnectionTypeMap = new()
        {
            { '1', "Ball type pull" }
        };

        private static readonly Dictionary<char, string> BodyTypeMap = new()
        {
            { 'A', "Flatbed" }
        };

        private static readonly Dictionary<char, string> LengthMap = new()
        {
            { 'U', "Length in Feet" }
        };

        private static readonly Dictionary<char, string> AxleMap = new()
        {
            { '2', "Number of Axles" }
        };

        // Model year mapping based on NHTSA standard from the handbook
        private static readonly Dictionary<char, int> ModelYearMap = new()
        {
            { 'A', 2010 }, { 'B', 2011 }, { 'C', 2012 }, { 'D', 2013 }, { 'E', 2014 },
            { 'F', 2015 }, { 'G', 2016 }, { 'H', 2017 }, { 'J', 2018 }, { 'K', 2019 },
            { 'L', 2020 }, { 'M', 2021 }, { 'N', 2022 }, { 'P', 2023 }, { 'R', 2024 },
            { 'S', 2025 }, { 'T', 2026 }, { 'V', 2027 }, { 'W', 2028 }, { 'X', 2029 },
            { 'Y', 2030 }, { '1', 2031 }, { '2', 2032 }, { '3', 2033 }, { '4', 2034 }
        };

        private static readonly Dictionary<char, string> PlantLocationMap = new()
        {
            { '5', "plant location" }
        };

        public override VinDecodingResult Decode(string vin)
        {
            var result = new VinDecodingResult
            {
                VIN = vin,
                Manufacturer = ManufacturerName,
                IsValid = true,
                Warnings = new List<VinDecodingWarning>(),
                AdditionalProperties = new Dictionary<string, object>()
            };

            if (!CanDecode(vin))
            {
                result.IsValid = false;
                result.Warnings.Add(new VinDecodingWarning("VIN cannot be decoded by Load Glide decoder"));
                return result;
            }

            try
            {
                // Position 4: Material Used
                if (MaterialMap.TryGetValue(vin[3], out var material))
                {
                    result.AdditionalProperties["Material"] = material;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown material code: {vin[3]}"));
                }

                // Position 5: Connection Type
                if (ConnectionTypeMap.TryGetValue(vin[4], out var connectionType))
                {
                    result.AdditionalProperties["ConnectionType"] = connectionType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown connection type code: {vin[4]}"));
                }

                // Position 6: Body Type
                if (BodyTypeMap.TryGetValue(vin[5], out var bodyType))
                {
                    result.BodyStyle = bodyType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown body type code: {vin[5]}"));
                }

                // Position 7: Length
                if (LengthMap.TryGetValue(vin[6], out var lengthDesc))
                {
                    result.AdditionalProperties["LengthType"] = lengthDesc;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown length code: {vin[6]}"));
                }

                // Position 8: Number of Axles
                if (AxleMap.TryGetValue(vin[7], out var axleDesc))
                {
                    result.AdditionalProperties["AxleConfiguration"] = axleDesc;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown axle configuration code: {vin[7]}"));
                }

                // Position 9: Check Digit
                result.AdditionalProperties["CheckDigit"] = vin[8];

                // Position 10: Model Year
                if (ModelYearMap.TryGetValue(vin[9], out var modelYear))
                {
                    result.ModelYear = modelYear;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown model year code: {vin[9]}"));
                }

                // Position 11: Plant Location
                if (PlantLocationMap.TryGetValue(vin[10], out var plantLocation))
                {
                    result.AdditionalProperties["PlantLocation"] = plantLocation;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown plant location code: {vin[10]}"));
                }

                // Positions 12-14: WMC (World Manufacturer Code)
                var wmc = vin.Substring(11, 3);
                result.AdditionalProperties["WMC"] = wmc;

                // Positions 15-17: Sequential Production Number
                var sequentialNumber = vin.Substring(14, 3);
                if (sequentialNumber.All(char.IsDigit))
                {
                    result.AdditionalProperties["SequentialProductionNumber"] = int.Parse(sequentialNumber);
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning("Invalid sequential production number format"));
                }

                // Generate a descriptive model name
                var materialDesc = result.AdditionalProperties.ContainsKey("Material")
                    ? result.AdditionalProperties["Material"].ToString()
                    : "Unknown Material";

                result.Model = $"{materialDesc} {result.BodyStyle}".Trim();

            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Warnings.Add(new VinDecodingWarning($"Error decoding VIN: {ex.Message}"));
            }

            return result;
        }

        public override bool CanDecode(string vin)
        {
            if (!base.CanDecode(vin))
                return false;

            // VIN must be exactly 17 characters
            if (vin.Length != 17)
                return false;

            // Check if WMI matches (positions 1-3)
            var wmi = vin.Substring(0, 3);
            return SupportedWMIs.Contains(wmi);
        }
    }
}
