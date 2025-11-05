using NicheVinDecoder.Core.Factory;
using NicheVinDecoder.Legacy;
using NUnit.Framework;

namespace NicheVinDecoder
{
    [TestFixture]
    internal class UnitTests
    {
        [Test]
        public void VinDecoderFactory_Unsupported()
        {
            // Arrange
            var factory = new VinDecoderFactory();
            const string vin = "2GNALBEK9F1113337";

            // Act
            var decoder = factory.GetDecoder(vin);
            Assert.That(decoder, Is.Null);
        }

        [Test]
        public void VinDecoderFactory_WithValidAppalachianVin_ShouldDecodeCorrectly()
        {
            // Arrange
            var factory = new VinDecoderFactory();
            const string vin = "5Z5BA162XJS000001";

            // Act
            var decoder = factory.GetDecoder(vin);
            var result = decoder?.Decode(vin);

            // Assert
            Assert.That(decoder, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.VIN, Is.EqualTo(vin));
            Assert.That(result.Manufacturer, Is.EqualTo("Appalachian Trailers Inc"));
            Assert.That(result.ModelYear, Is.EqualTo(2018)); // J = 2018
            Assert.That(result.BodyStyle, Is.EqualTo("Car Hauler"));
            Assert.That(result.Model, Is.EqualTo("Bumper Pull Car Hauler"));
            Assert.That(result.AdditionalProperties["TrailerType"], Is.EqualTo("Bumper Pull"));
            Assert.That(result.AdditionalProperties["SequentialProductionNumber"], Is.EqualTo(1));
            Assert.That(result.Warnings, Is.Empty);
        }

        [Test]
        public void VinDecoder_WithValidBrinkleyVin_ShouldDecodeCorrectly()
        {
            // Arrange
            const string vin = "7T0FZ362XPT000001";

            // Act
            var result = VinDecoder.Decode(vin);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.VIN, Is.EqualTo(vin));
            Assert.That(result.Manufacturer, Is.EqualTo("Brinkley RV, LLC"));
            Assert.That(result.ModelYear, Is.EqualTo(2023)); // P = 2023
            Assert.That(result.BodyStyle, Is.EqualTo("RV Fifth Wheel Hitch")); // F = Fifth Wheel
            Assert.That(result.Model, Is.EqualTo("Model Z, Narrow Body 36ft")); // Z = Model Z, Narrow Body + 36 = 36ft
            Assert.That(result.AdditionalProperties["TrailerModel"], Is.EqualTo("Model Z, Narrow Body"));
            Assert.That(result.AdditionalProperties["UnitLengthFeet"], Is.EqualTo(36));
            Assert.That(result.AdditionalProperties["AxleConfiguration"], Is.EqualTo("Two Axles")); // 2 = Two Axles
            Assert.That(result.AdditionalProperties["SequentialProductionNumber"], Is.EqualTo(1));
         
        }

        [Test]
        public void VinDecoder_WithValidLoadGlideVin_ShouldDecodeCorrectly()
        {
            // Arrange
            const string vin = "1L9A1AU24M5282004";

            // Act
            var result = VinDecoder.Decode(vin);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.VIN, Is.EqualTo(vin));
            Assert.That(result.Manufacturer, Is.EqualTo("Load Glide Trailers"));
            Assert.That(result.ModelYear, Is.EqualTo(2021)); // M = 2021
            Assert.That(result.BodyStyle, Is.EqualTo("Flatbed")); // A = Flatbed
            Assert.That(result.Model, Is.EqualTo("Aluminum Flatbed"));
            Assert.That(result.AdditionalProperties["Material"], Is.EqualTo("Aluminum"));
            Assert.That(result.AdditionalProperties["ConnectionType"], Is.EqualTo("Ball type pull"));
            Assert.That(result.AdditionalProperties["WMC"], Is.EqualTo("282"));
            Assert.That(result.AdditionalProperties["SequentialProductionNumber"], Is.EqualTo(4));
            Assert.That(result.Warnings, Is.Empty);
        }

        [Test]
        public void VinDecoder_WithValidNineTechVin_ShouldDecodeCorrectly()
        {
            // Arrange
            const string vin = "LTUAA5F45M1123456";

            // Act
            var result = VinDecoder.Decode(vin);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.VIN, Is.EqualTo(vin));
            Assert.That(result.Manufacturer, Is.EqualTo("Nine Tech Co., Ltd"));
            Assert.That(result.ModelYear, Is.EqualTo(2021)); // M = 2021
            Assert.That(result.BodyStyle, Is.EqualTo("Electric Motorcycle/Scooter"));
            Assert.That(result.Model, Is.EqualTo("SEGWAY A series Professional"));
            Assert.That(result.Engine, Is.EqualTo("Electric Motor DC Permanent Magnet Brushless (1.3≤P<1.5 kW)"));
            Assert.That(result.AdditionalProperties["make"], Is.EqualTo("SEGWAY"));
            Assert.That(result.AdditionalProperties["LineModel"], Is.EqualTo("A series"));
            Assert.That(result.AdditionalProperties["MotorcycleType"], Is.EqualTo("Professional (专业型)"));
            Assert.That(result.AdditionalProperties["NetBrakeHP"], Is.EqualTo("1.0≤hp<1.2"));
            Assert.That(result.AdditionalProperties["EnginePowerRange"], Is.EqualTo("1.3≤P<1.5 kW"));
            Assert.That(result.AdditionalProperties["SequentialProductionNumber"], Is.EqualTo(123456));
            Assert.That(result.Warnings, Is.Empty);
        }

        [Test]
        public void VinDecoder_WithValidXPOVin_ShouldDecodeCorrectly()
        {
            // Arrange
            const string vin = "7F21A533XM1123456";

            // Act
            var result = VinDecoder.Decode(vin);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.VIN, Is.EqualTo(vin));
            Assert.That(result.Manufacturer, Is.EqualTo("XPO Manufacturing"));
            Assert.That(result.ModelYear, Is.EqualTo(2021)); // M = 2021
            Assert.That(result.BodyStyle, Is.EqualTo("New Wedge Dry Freight Van"));
            Assert.That(result.Model, Is.EqualTo("Truck Trailer 53ft 3-Axle"));
            Assert.That(result.AdditionalProperties["VehicleType"], Is.EqualTo("Truck Trailer"));
            Assert.That(result.AdditionalProperties["LengthFeet"], Is.EqualTo(53));
            Assert.That(result.AdditionalProperties["NumberOfAxles"], Is.EqualTo(3));
            Assert.That(result.AdditionalProperties["PlantLocation"], Is.EqualTo("Fontana, CA"));
            Assert.That(result.AdditionalProperties["SequentialProductionNumber"], Is.EqualTo(123456));
            Assert.That(result.Warnings, Is.Empty);
        }


        [Test]
        public void VinDecoder_WithValidHighlandRidgeVin_ShouldDecodeCorrectly()
        {
            // Arrange
            const string vin = "58TBM0BU8P3A13051";

            // Act
            var result = VinDecoder.Decode(vin);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.VIN, Is.EqualTo(vin));
            Assert.That(result.Manufacturer, Is.EqualTo("Highland Ridge RV"));
            Assert.That(result.ModelYear, Is.EqualTo(2023)); // P = 2023
            Assert.That(result.BodyStyle, Is.EqualTo("RV Trailer [Standard] Enclosed Living Quarters")); // 0 = Standard RV
            Assert.That(result.Model, Is.EqualTo("Open Range 330BHS Travel Trailer")); // M = Open Range, A1 = 330BHS model
            Assert.That(result.AdditionalProperties["TrailerType"], Is.EqualTo("Ball Pull (Travel Trailer)")); // B = Ball Pull Travel Trailer
            Assert.That(result.AdditionalProperties["make"], Is.EqualTo("Open Range")); // M = Open Range
            Assert.That(result.AdditionalProperties["AxleConfiguration"], Is.EqualTo("Two Axles (Tandem)")); // B = Two Axles
            Assert.That(result.AdditionalProperties["Length"], Is.EqualTo("38 ft - less than 40 ft")); // U = 38-40 ft
            Assert.That(result.AdditionalProperties["PlantLocation"], Is.EqualTo("3195 N. SR 5, Shipshewana, IN 46565")); // 3 = Shipshewana plant
            Assert.That(result.AdditionalProperties["ModelCode"], Is.EqualTo("A1"));
            Assert.That(result.AdditionalProperties["ModelDescription"], Is.EqualTo("330BHS"));
            Assert.That(result.AdditionalProperties["SequentialProductionNumber"], Is.EqualTo(3051));
            Assert.That(result.AdditionalProperties["VehicleType"], Is.EqualTo("Recreational Vehicle"));
            Assert.That(result.Warnings, Is.Empty);
        }

        [Test]
        public void Legacy_Decode_Corvette1973()
        {
           

            const string vin = "194371S114477";

  
            var result = VinDecoder.Decode(vin);

            Assert.That("Chevrolet Corvette (C3)", Is.EqualTo(result.Manufacturer));
            Assert.That("Corvette Coupe", Is.EqualTo(result.Model));
            Assert.That(1971, Is.EqualTo(result.ModelYear));
        }
    }
}
