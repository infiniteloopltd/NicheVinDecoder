using NicheVinDecoder.Core.Factory;
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
            var vin = "1L9A1AU24M5282004";

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
    }
}
