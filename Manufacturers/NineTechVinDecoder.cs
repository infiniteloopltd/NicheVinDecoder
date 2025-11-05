using NicheVinDecoder.Core.Base;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Manufacturers
{
    public class NineTechVinDecoder : BaseVinDecoder
    {
        public override string ManufacturerName => "Nine Tech Co., Ltd";

        public override IEnumerable<string> SupportedWMIs => new[] { "LTU" };

        private static readonly Dictionary<char, string> MakeMap = new()
        {
            { 'A', "SEGWAY" },
            { 'B', "ninebot" },
            { 'C', "SEGWAY-ninebot" },
            { 'D', "powered by SEGWAY" }
        };

        private static readonly Dictionary<char, string> LineModelMap = new()
        {
            { 'A', "A series" }, { 'B', "B series" }, { 'C', "C series" }, { 'D', "D series" },
            { 'E', "E series" }, { 'F', "F series" }, { 'G', "G series" }, { 'H', "H series" },
            { 'L', "L series" }, { 'M', "M series" }, { 'N', "N series" }, { 'P', "P series" }
        };

        private static readonly Dictionary<char, string> MotorcycleTypeMap = new()
        {
            { '1', "Basic (基础型)" },
            { '2', "Simplified (简化型)" },
            { '3', "Comfortable (舒适型)" },
            { '4', "Elite (精英型)" },
            { '5', "Professional (专业型)" },
            { '6', "Luxury (豪华型)" },
            { '7', "Technical (技术型)" },
            { '8', "Flagship (旗舰型)" },
            { '9', "Leasing type (租赁型)" }
        };

        private static readonly Dictionary<char, (string PowerRange, string Description)> EngineTypeMap = new()
        {
            { 'A', ("<0.5 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'B', ("0.5≤P<0.8 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'C', ("0.8≤P<1.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'D', ("1.0≤P<1.2 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'E', ("1.2≤P<1.3 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'F', ("1.3≤P<1.5 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'G', ("1.5≤P<1.8 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'H', ("1.8≤P<2.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'J', ("2.0≤P<2.5 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'K', ("2.5≤P<3.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'L', ("3.0≤P<3.5 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'M', ("3.5≤P<4.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'N', ("4.5≤P<5.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'P', ("5.5≤P<6.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'R', ("6.5≤P<7.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'S', ("7.5≤P<8.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'T', ("8.5≤P<9.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'U', ("9.5≤P<10.0 kW", "Electric Motor DC Permanent Magnet Brushless") },
            { 'V', ("10.0≤P kW", "Electric Motor DC Permanent Magnet Brushless") }
        };

        private static readonly Dictionary<char, string> NetBrakeHPMap = new()
        {
            { '1', "hp<0.5" }, { '2', "0.5≤hp<0.7" }, { '3', "0.7≤hp<1.0" }, { '4', "1.0≤hp<1.2" },
            { '5', "1.2≤hp<1.4" }, { '6', "1.4≤hp<1.6" }, { '7', "1.6≤hp<1.8" }, { '8', "1.8≤hp<2.0" },
            { '9', "2.0≤hp<2.5" }, { 'A', "2.5≤hp<3.0" }, { 'B', "3.5≤hp<4.0" }, { 'C', "4.5≤hp<5.0" },
            { 'D', "5.0≤hp<6.0" }, { 'E', "6.0≤hp<7.0" }, { 'F', "7.0≤hp<8.0" }, { 'G', "8.0≤hp<9.0" },
            { 'H', "9.0≤hp<10.0" }, { 'J', "10.0≤hp<15.0" }, { 'K', "15.0≤hp<20.0" }, { 'L', "20.0≤hp<25.0" },
            { 'M', "25.0≤hp<30.0" }, { 'N', "35.0≤hp<40.0" }, { 'P', "40.0≤hp<45.0" }, { 'R', "45.0≤hp<50.0" },
            { 'S', "50.0≤hp<60.0" }, { 'T', "60.0≤hp<70.0" }, { 'U', "70.0≤hp<80.0" }, { 'V', "80.0≤hp<90.0" },
            { 'W', "90.0≤hp<100.0" }, { 'X', "100.0≤hp" }
        };

        private static readonly Dictionary<char, int> ModelYearMap = new()
        {
            { 'L', 2020 }, { 'M', 2021 }, { 'N', 2022 }, { 'P', 2023 }, { 'R', 2024 }, { 'S', 2025 },
            { 'T', 2026 }, { 'V', 2027 }, { 'W', 2028 }, { 'X', 2029 }, { 'Y', 2030 }, { '1', 2031 },
            { '2', 2032 }, { '3', 2033 }, { '4', 2034 }, { '5', 2035 }, { '6', 2036 }, { '7', 2037 },
            { '8', 2038 }, { '9', 2039 }, { 'A', 2040 }, { 'B', 2041 }
        };

        private static readonly Dictionary<char, string> ProductionFacilityMap = new()
        {
            { '1', "1 Xingben Rd. Xinbei Dist. Changzhou. Jiangsu Province. China" }
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
                result.Warnings.Add(new VinDecodingWarning("VIN cannot be decoded by Nine Tech decoder"));
                return result;
            }

            try
            {
                // Position 4: make
                if (MakeMap.TryGetValue(vin[3], out var make))
                {
                    result.AdditionalProperties["make"] = make;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown make code: {vin[3]}"));
                }

                // Position 5: Line or Model
                if (LineModelMap.TryGetValue(vin[4], out var lineModel))
                {
                    result.AdditionalProperties["LineModel"] = lineModel;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown line/model code: {vin[4]}"));
                }

                // Position 6: Type of Motorcycle
                if (MotorcycleTypeMap.TryGetValue(vin[5], out var motorcycleType))
                {
                    result.AdditionalProperties["MotorcycleType"] = motorcycleType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown motorcycle type code: {vin[5]}"));
                }

                // Position 7: Engine Type
                if (EngineTypeMap.TryGetValue(vin[6], out var engineInfo))
                {
                    result.Engine = $"{engineInfo.Description} ({engineInfo.PowerRange})";
                    result.AdditionalProperties["EnginePowerRange"] = engineInfo.PowerRange;
                    result.AdditionalProperties["EngineType"] = engineInfo.Description;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown engine type code: {vin[6]}"));
                }

                // Position 8: Net Brake HP
                if (NetBrakeHPMap.TryGetValue(vin[7], out var netBrakeHP))
                {
                    result.AdditionalProperties["NetBrakeHP"] = netBrakeHP;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown net brake HP code: {vin[7]}"));
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

                // Position 11: Production Facility
                if (ProductionFacilityMap.TryGetValue(vin[10], out var facility))
                {
                    result.AdditionalProperties["ProductionFacility"] = facility;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown production facility code: {vin[10]}"));
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
                var makeDesc = result.AdditionalProperties.ContainsKey("make")
                    ? result.AdditionalProperties["make"].ToString()
                    : "Unknown";

                var lineDesc = result.AdditionalProperties.ContainsKey("LineModel")
                    ? result.AdditionalProperties["LineModel"].ToString()
                    : "Unknown";

                var typeDesc = result.AdditionalProperties.ContainsKey("MotorcycleType")
                    ? result.AdditionalProperties["MotorcycleType"].ToString()?.Split('(')[0].Trim()
                    : "Unknown";

                result.Model = $"{makeDesc} {lineDesc} {typeDesc}".Trim();
                result.BodyStyle = "Electric Motorcycle/Scooter";

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