using NicheVinDecoder.Core.Base;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Manufacturers
{
    public class BrinkleyVinDecoder : BaseVinDecoder
    {
        public override string ManufacturerName => "Brinkley RV, LLC";

        public override IEnumerable<string> SupportedWMIs => new[] { "7T0" };

        private static readonly Dictionary<char, string> BodyTypeMap = new()
        {
            { 'T', "RV Travel Trailer - Ball Hitch Coupler" },
            { 'F', "RV Fifth Wheel Hitch" }
        };

        private static readonly Dictionary<char, string> TrailerModelMap = new()
        {
            { 'Z', "Model Z, Narrow Body" },
            { 'A', "Model Z, Air" },
            { 'G', "Model G, Wide Body" },
            { '1', "Model I" },
            { '2', "Model I, 2" },
            { 'R', "Model R, Wide Body" }
        };

        private static readonly Dictionary<string, int> UnitLengthMap = new()
        {
            { "18", 18 }, { "19", 19 }, { "20", 20 }, { "21", 21 }, { "22", 22 }, { "23", 23 }, { "24", 24 }, { "25", 25 },
            { "26", 26 }, { "27", 27 }, { "28", 28 }, { "29", 29 }, { "30", 30 }, { "31", 31 }, { "32", 32 }, { "33", 33 },
            { "34", 34 }, { "35", 35 }, { "36", 36 }, { "37", 37 }, { "38", 38 }, { "39", 39 }, { "40", 40 }, { "41", 41 },
            { "42", 42 }, { "43", 43 }, { "44", 44 }, { "45", 45 }, { "46", 46 }, { "47", 47 }, { "48", 48 }, { "49", 49 }
        };

        private static readonly Dictionary<char, string> AxleConfigurationMap = new()
        {
            { '1', "One Axle" },
            { '2', "Two Axles" },
            { '3', "Three Axles" }
        };

        private static readonly Dictionary<char, int> ModelYearMap = new()
        {
            { 'P', 2023 }, { 'R', 2024 }, { 'S', 2025 }, { 'T', 2026 },
            { 'V', 2027 }, { 'W', 2028 }, { 'X', 2029 }, { 'Y', 2030 },
            { '1', 2031 }, { '2', 2032 }, { '3', 2033 }, { '4', 2034 }
        };

        private static readonly Dictionary<char, string> PlantLocationMap = new()
        {
            { '1', "Plant H - 2948 Hackberry Drive, Goshen, IN 46526" },
            { 'A', "Plant 1 - 1655 Brinkley Way East, Goshen, IN 46528" },
            { 'B', "Plant 2 - 1585 Brinkley Way East, Goshen, IN 46528" },
            { 'C', "Plant 3 - 1475 Brinkley Way East, Goshen, IN 46528" },
            { 'D', "Plant 4 - 1365 Brinkley Way East, Goshen, IN 46528" },
            { 'E', "Plant 5 - 1640 Brinkley Way East, Goshen, IN 46528" },
            { 'F', "Plant 6 - 1580 Brinkley Way East, Goshen, IN 46528" },
            { 'G', "Plant 7 - 1470 Brinkley Way East, Goshen, IN 46528" },
            { 'H', "Plant 8 - 1360 Brinkley Way East, Goshen, IN 46528" },
            { 'J', "Plant 9 - 1495 Brinkley Way East, Goshen, IN 46528" }
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
                result.Warnings.Add(new VinDecodingWarning("VIN cannot be decoded by Brinkley decoder"));
                return result;
            }

            try
            {
                // Position 4: Body Type
                if (BodyTypeMap.TryGetValue(vin[3], out var bodyType))
                {
                    result.BodyStyle = bodyType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown body type code: {vin[3]}"));
                }

                // Position 5: Trailer Model
                if (TrailerModelMap.TryGetValue(vin[4], out var trailerModel))
                {
                    result.AdditionalProperties["TrailerModel"] = trailerModel;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown trailer model code: {vin[4]}"));
                }

                // Positions 6-7: Unit Length
                var lengthCode = vin.Substring(5, 2);
                if (UnitLengthMap.TryGetValue(lengthCode, out var length))
                {
                    result.AdditionalProperties["UnitLengthFeet"] = length;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown unit length code: {lengthCode}"));
                }

                // Position 8: Axle Configuration
                if (AxleConfigurationMap.TryGetValue(vin[7], out var axleConfig))
                {
                    result.AdditionalProperties["AxleConfiguration"] = axleConfig;
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
                var modelDesc = result.AdditionalProperties.ContainsKey("TrailerModel")
                    ? result.AdditionalProperties["TrailerModel"].ToString()
                    : "Unknown Model";

                var lengthDesc = result.AdditionalProperties.ContainsKey("UnitLengthFeet")
                    ? $" {result.AdditionalProperties["UnitLengthFeet"]}ft"
                    : "";

                result.Model = $"{modelDesc}{lengthDesc}".Trim();

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
