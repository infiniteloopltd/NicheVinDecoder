using NicheVinDecoder.Core.Base;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Manufacturers
{
    public class HighlandRidgeVinDecoder : BaseVinDecoder
    {
        public override string ManufacturerName => "Highland Ridge RV";

        public override IEnumerable<string> SupportedWMIs => new[] { "58T" };

        private static readonly Dictionary<char, string> TrailerTypeMap = new()
        {
            { 'A', "Ball Pull (Camping Trailer)" },
            { 'B', "Ball Pull (Travel Trailer)" },
            { 'C', "5th Wheel or Gooseneck Trailer" }
        };

        private static readonly Dictionary<char, string> MakeMap = new()
        {
            { 'E', "Entegra" },
            { 'H', "Highland Ridge" },
            { 'J', "Jayco" },
            { 'M', "Open Range" },
            { 'P', "Go Play" },
            { 'R', "Olympia" },
            { 'S', "StarCraft" },
            { 'T', "Silverstar" },
            { 'W', "Mesa Ridge" },
            { 'Y', "Travel Star" },
            { '4', "Olympia (MY 2022)" }
        };

        private static readonly Dictionary<char, string> BodyTypeMap = new()
        {
            { '0', "RV Trailer [Standard] Enclosed Living Quarters" },
            { 'H', "Hybrid RV Trailer [Expandable] Enclosed Living Quarters" },
            { 'S', "Sport Utility RV Trailer Enclosed Living Quarters" }
        };

        private static readonly Dictionary<char, string> AxleConfigurationMap = new()
        {
            { 'A', "One Axle (Single)" },
            { 'B', "Two Axles (Tandem)" },
            { 'C', "Three Axles (Triple/Tri-Axle)" }
        };

        private static readonly Dictionary<char, string> LengthMap = new()
        {
            { 'A', "Less than 6 ft" },
            { 'B', "6 ft - less than 8 ft" },
            { 'C', "8 ft - less than 10 ft" },
            { 'D', "10 ft - less than 12 ft" },
            { 'E', "12 ft - less than 14 ft" },
            { 'F', "14 ft - less than 16 ft" },
            { 'G', "16 ft - less than 18 ft" },
            { 'H', "18 ft - less than 20 ft" },
            { 'J', "20 ft - less than 22 ft" },
            { 'K', "22 ft - less than 24 ft" },
            { 'L', "24 ft - less than 26 ft" },
            { 'M', "26 ft - less than 28 ft" },
            { 'N', "28 ft - less than 30 ft" },
            { 'P', "30 ft - less than 32 ft" },
            { 'R', "32 ft - less than 34 ft" },
            { 'S', "34 ft - less than 36 ft" },
            { 'T', "36 ft - less than 38 ft" },
            { 'U', "38 ft - less than 40 ft" },
            { 'V', "40 ft and greater" }
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
            { '1', "903 S. Main St., Middlebury, IN 46540" },
            { '3', "3195 N. SR 5, Shipshewana, IN 46565" },
            { '7', "511 Hankins Road South, Kimberly, Idaho 83341" }
        };

        // Model year 2023 specific model codes (positions 12-13)
        private static readonly Dictionary<string, string> ModelCodeMap2023 = new()
        {
            // Olympia TT
            { "8B", "212FB" }, { "8D", "241BH" }, { "8E", "225CK" }, { "8T", "242RL" },
            // Open Range TT
            { "4C", "20FBS" }, { "4F", "26BH" }, { "4L", "26BHS" }, { "4P", "20MB" },
            { "4S", "26RLS" }, { "4Z", "28BHS" }, { "4B", "25TH" }, { "41", "233TH" },
            { "42", "263TH" }, { "4E", "182RB" }, { "4K", "19BH" }, { "4M", "180BHS" },
            { "4Y", "172FB" }, { "43", "188BHS" },
            // Mesa Ridge TT
            { "EB", "212FB" }, { "ED", "241BH" }, { "EE", "225CK" }, { "EL", "261BH" },
            { "EN", "252RB" }, { "EP", "232MD" }, { "ER", "262RL" }, { "ET", "242RL" },
            { "AG", "322RLS" }, { "A1", "330BHS" }, { "A2", "338BHS" },
            // Silverstar TT
            { "5C", "20FBS" }, { "5F", "26BH" }, { "5L", "26BHS" }, { "5P", "20MB" },
            { "5S", "26RLS" }, { "5Z", "28BHS" }, { "5B", "25TH" }, { "51", "233TH" },
            { "52", "263TH" }, { "5E", "182RB" }, { "5K", "19BH" }, { "5M", "180BHS" },
            { "5Y", "172FB" }, { "53", "188BHS" },
            { "RA", "16FBS" }, { "RB", "19MBH" }, { "RC", "18RBS" }, { "RD", "17BH" },
            { "RE", "17MDS" }, { "TL", "275RLS" }, { "TM", "290RLS" }, { "TX", "296BHS" },
            { "TY", "321BHS" }, { "ZB", "212FB" }, { "ZD", "241BH" }, { "ZE", "225CK" },
            { "ZL", "261BH" }, { "ZN", "252RB" }, { "ZP", "232MD" }, { "ZR", "262RL" },
            { "ZT", "242RL" }, { "MB", "322RLS" }, { "M1", "330BHS" }, { "M2", "338BHS" },
            { "YN", "322RLS" }, { "Y1", "330BHS" }, { "Y2", "338BHS" }
        };

        // Model year 2024 additions
        private static readonly Dictionary<string, string> ModelCodeMap2024 = new()
        {
            // Go Play TT
            { "72", "263TH" }, { "73", "188BHS" }, { "74", "25FB" }, { "7C", "20FBS" },
            { "7F", "26BH" }, { "7K", "19BH" }, { "7L", "26BHS" }, { "7M", "180BHS" },
            { "7P", "20MB" }, { "7S", "26RLS" }, { "7V", "27BH" }
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
                result.Warnings.Add(new VinDecodingWarning("VIN cannot be decoded by Highland Ridge decoder"));
                return result;
            }

            try
            {
                // Position 4: Trailer Type / Connection Type
                if (TrailerTypeMap.TryGetValue(vin[3], out var trailerType))
                {
                    result.AdditionalProperties["TrailerType"] = trailerType;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown trailer type code: {vin[3]}"));
                }

                // Position 5: make
                if (MakeMap.TryGetValue(vin[4], out var make))
                {
                    result.AdditionalProperties["make"] = make;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown make code: {vin[4]}"));
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

                // Position 7: Axle Configuration
                if (AxleConfigurationMap.TryGetValue(vin[6], out var axleConfig))
                {
                    result.AdditionalProperties["AxleConfiguration"] = axleConfig;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown axle configuration code: {vin[6]}"));
                }

                // Position 8: Length
                if (LengthMap.TryGetValue(vin[7], out var length))
                {
                    result.AdditionalProperties["Length"] = length;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown length code: {vin[7]}"));
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

                // Positions 12-13: Model Code (specific to model year)
                var modelCode = vin.Substring(11, 2);
                string modelDescription = null;

                // Try to get model description based on model year
                if (result.ModelYear.HasValue)
                {
                    switch (result.ModelYear.Value)
                    {
                        case 2023:
                            ModelCodeMap2023.TryGetValue(modelCode, out modelDescription);
                            break;
                        case 2024:
                            ModelCodeMap2024.TryGetValue(modelCode, out modelDescription);
                            if (modelDescription == null)
                                ModelCodeMap2023.TryGetValue(modelCode, out modelDescription);
                            break;
                        default:
                            // For other years, try 2023 as fallback
                            ModelCodeMap2023.TryGetValue(modelCode, out modelDescription);
                            break;
                    }
                }

                if (modelDescription != null)
                {
                    result.AdditionalProperties["ModelCode"] = modelCode;
                    result.AdditionalProperties["ModelDescription"] = modelDescription;
                }
                else
                {
                    result.Warnings.Add(new VinDecodingWarning($"Unknown model code: {modelCode}"));
                }

                // Positions 14-17: Sequential Production Number
                var sequentialNumber = vin.Substring(13, 4);
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
                    : "Highland Ridge";

                var modelDesc = result.AdditionalProperties.ContainsKey("ModelDescription")
                    ? result.AdditionalProperties["ModelDescription"].ToString()
                    : modelCode;

                var trailerTypeDesc = result.AdditionalProperties.ContainsKey("TrailerType")
                    ? (result.AdditionalProperties["TrailerType"].ToString().Contains("5th Wheel") ? "Fifth Wheel" : "Travel Trailer")
                    : "";

                result.Model = $"{makeDesc} {modelDesc} {trailerTypeDesc}".Trim();

                // Set vehicle type for additional context
                result.AdditionalProperties["VehicleType"] = "Recreational Vehicle";

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