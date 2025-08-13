using NicheVinDecoder.Core.Base;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Manufacturers
{
    public class AppalachianVinDecoder : BaseVinDecoder
    {
        public override string ManufacturerName => "Appalachian Trailers Inc";

        public override IEnumerable<string> SupportedWMIs => new[] { "5Z5" };

        private static readonly Dictionary<char, string> TrailerTypeMap = new()
        {
            { 'B', "Bumper Pull" },
            { 'G', "Gooseneck" },
            { 'A', "Air Pintle" },
            { 'F', "Wedge" }
        };

        private static readonly Dictionary<char, string> BodyTypeMap = new()
        {
            { 'A', "Car Hauler" },
            { 'B', "Landscape" },
            { 'C', "Multi Car Hauler" },
            { 'D', "Equipment" },
            { 'E', "Enclosed" },
            { 'G', "Gooseneck" },
            { 'P', "Pintle" },
            { 'H', "Heavy Duty Gooseneck & Pintle" },
            { 'S', "Skid Steer" },
            { 'X', "Dump" }
        };

        private static readonly Dictionary<char, int> ModelYearMap = new()
        {
            { 'C', 2012 },
            { 'D', 2013 },
            { 'E', 2014 },
            { 'F', 2015 },
            { 'G', 2016 },
            { 'H', 2017 },
            { 'J', 2018 },
            { 'K', 2019 },
            { 'L', 2020 },
            { 'M', 2021 },
            { 'N', 2022 },
            { 'P', 2023 },
            { 'R', 2024 }
        };

        private static readonly Dictionary<char, string> ManufacturingLocationMap = new()
        {
            { 'S', "Salem, OH" }
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
                result.Warnings.Add(new VinDecodingWarning("VIN cannot be decoded by Appalachian decoder"));
                return result;
            }

            try
            {
                // Position 4: Trailer Type
                if (TrailerTypeMap.TryGetValue(vin[3], out var trailerType))
                {
                    result.AdditionalProperties["TrailerType"] = trailerType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown trailer type code: {vin[3]}"));
                }

                // Position 5: Body Type
                if (BodyTypeMap.TryGetValue(vin[4], out var bodyType))
                {
                    result.BodyStyle = bodyType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown body type code: {vin[4]}"));
                }

                // Position 6: Total Length in Feet
                if (char.IsDigit(vin[5]))
                {
                    result.AdditionalProperties["TotalLengthFeet"] = int.Parse(vin[5].ToString());
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Invalid length code: {vin[5]}"));
                }

                // Position 7: Number of Axles
                if (char.IsDigit(vin[6]))
                {
                    result.AdditionalProperties["NumberOfAxles"] = int.Parse(vin[6].ToString());
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Invalid axle count code: {vin[6]}"));
                }

                // Position 8: Check Digit (validate if needed)
                result.AdditionalProperties["CheckDigit"] = vin[7];

                // Position 10: Model Year
                if (ModelYearMap.TryGetValue(vin[9], out var modelYear))
                {
                    result.ModelYear = modelYear;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown model year code: {vin[9]}"));
                }

                // Position 11: Manufacturing Location
                if (ManufacturingLocationMap.TryGetValue(vin[10], out var location))
                {
                    result.AdditionalProperties["ManufacturingLocation"] = location;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown manufacturing location code: {vin[10]}"));
                }

                // Positions 12-17: Sequential Production Number
                var sequentialNumber = vin.Substring(11, 6);
                if (sequentialNumber.All(char.IsDigit))
                {
                    result.AdditionalProperties["SequentialProductionNumber"] = int.Parse(sequentialNumber);
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning("Invalid sequential production number format"));
                }

                // Generate a descriptive model name
                var trailerTypeDesc = result.AdditionalProperties.ContainsKey("TrailerType")
                    ? result.AdditionalProperties["TrailerType"].ToString()
                    : "Unknown Type";

                result.Model = $"{trailerTypeDesc} {result.BodyStyle}".Trim();

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

            // Additional validation specific to Appalachian
            // VIN must be exactly 17 characters
            if (vin.Length != 17)
                return false;

            // Check if WMI matches (positions 1-3)
            var wmi = vin.Substring(0, 3);
            return SupportedWMIs.Contains(wmi);
        }
    }
}
