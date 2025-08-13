using NicheVinDecoder.Core.Base;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Manufacturers.XPO
{
    public class XPOVinDecoder : BaseVinDecoder
    {
        public override string ManufacturerName => "XPO Manufacturing";

        public override IEnumerable<string> SupportedWMIs => new[] { "7F2" };

        private static readonly Dictionary<char, string> VehicleTypeMap = new()
        {
            { '1', "Truck Trailer" },
            { '3', "Converter Dolly" }
        };

        private static readonly Dictionary<char, string> BodyConstructionMap = new()
        {
            { 'A', "New Wedge Dry Freight Van" },
            { 'B', "New Wedge Dry Freight Van utilizing some reconditioned parts" },
            { 'C', "New Non-wedge Dry Freight Van" },
            { 'D', "New Non-wedge Dry Freight Van utilizing some reconditioned parts" },
            { 'G', "New Converter Dolly" },
            { 'H', "New Converter Dolly utilizing some reconditioned parts" }
        };

        private static readonly Dictionary<char, int> ModelYearMap = new()
        {
            { 'A', 2010 }, { 'B', 2011 }, { 'C', 2012 }, { 'D', 2013 }, { 'E', 2014 },
            { 'F', 2015 }, { 'G', 2016 }, { 'H', 2017 }, { 'J', 2018 }, { 'K', 2019 },
            { 'L', 2020 }, { 'M', 2021 }, { 'N', 2022 }, { 'P', 2023 }, { 'R', 2024 },
            { 'S', 2025 }, { 'T', 2026 }, { 'V', 2027 }, { 'W', 2028 }, { 'X', 2029 },
            { 'Y', 2030 }, { '1', 2031 }, { '2', 2032 }, { '3', 2033 }, { '4', 2034 },
            { '5', 2035 }, { '6', 2036 }, { '7', 2037 }, { '8', 2038 }, { '9', 2039 }
        };

        private static readonly Dictionary<char, string> PlantLocationMap = new()
        {
            { '1', "Fontana, CA" },
            { '2', "Searcy, AR" }
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
                result.Warnings.Add(new VinDecodingWarning("VIN cannot be decoded by XPO decoder"));
                return result;
            }

            try
            {
                // Position 4: Vehicle Type
                if (VehicleTypeMap.TryGetValue(vin[3], out var vehicleType))
                {
                    result.AdditionalProperties["VehicleType"] = vehicleType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown vehicle type code: {vin[3]}"));
                }

                // Position 5: Body Construction
                if (BodyConstructionMap.TryGetValue(vin[4], out var bodyConstruction))
                {
                    result.BodyStyle = bodyConstruction;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown body construction code: {vin[4]}"));
                }

                // Positions 6-7: Length (two-digit length)
                var lengthStr = vin.Substring(5, 2);
                if (lengthStr.All(char.IsDigit))
                {
                    var length = int.Parse(lengthStr);
                    result.AdditionalProperties["LengthFeet"] = length;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Invalid length code: {lengthStr}"));
                }

                // Position 8: Number of Axles
                if (char.IsDigit(vin[7]))
                {
                    var axleCount = int.Parse(vin[7].ToString());
                    result.AdditionalProperties["NumberOfAxles"] = axleCount;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Invalid axle count code: {vin[7]}"));
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

                // Position 11: Plant of Manufacture
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
                var vehicleTypeDesc = result.AdditionalProperties.ContainsKey("VehicleType")
                    ? result.AdditionalProperties["VehicleType"].ToString()
                    : "Unknown";

                var lengthDesc = result.AdditionalProperties.ContainsKey("LengthFeet")
                    ? $" {result.AdditionalProperties["LengthFeet"]}ft"
                    : "";

                var axleDesc = result.AdditionalProperties.ContainsKey("NumberOfAxles")
                    ? $" {result.AdditionalProperties["NumberOfAxles"]}-Axle"
                    : "";

                result.Model = $"{vehicleTypeDesc}{lengthDesc}{axleDesc}".Trim();

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