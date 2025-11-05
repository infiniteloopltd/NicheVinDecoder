# NicheVinDecoder

A specialized VIN (Vehicle Identification Number) decoder library for vehicles not well-covered by standard US NHTSA VIN decoders, including international manufacturers, specialty vehicles, and niche market segments.

Project supported by: https://www.vehicleregistrationapi.com/

## Purpose

The standard NHTSA VIN decoder works well for mainstream automotive manufacturers, but falls short when dealing with:

- **Specialty Vehicle Manufacturers**: RV manufacturers, trailer manufacturers, and specialty vehicle builders
- **International Vehicles**: Vehicles manufactured outside the US with different VIN structures
- **Niche Market Segments**: Electric scooters, specialty trailers, industrial vehicles, and custom manufacturers
- **Small Volume Manufacturers**: Manufacturers that don't follow standard NHTSA VIN patterns

This library fills that gap by providing detailed VIN decoding for these specialized manufacturers, extracting meaningful information about vehicle specifications, manufacturing details, and model information that would otherwise be lost.

## Features

- **Extensible Architecture**: Easy to add new manufacturer decoders
- **Rich Data Extraction**: Provides detailed vehicle information including body style, model year, plant location, and manufacturer-specific attributes
- **Comprehensive Error Handling**: Graceful handling of invalid VINs with detailed warning messages
- **Factory Pattern**: Automatic decoder selection based on World Manufacturer Identifier (WMI)
- **Unit Tested**: All decoders include comprehensive unit tests

## Supported Manufacturers

Currently supported manufacturers include:

| Manufacturer | WMI | Vehicle Types | Country |
|-------------|-----|---------------|---------|
| Appalachian Trailers Inc | 5Z5 | Utility Trailers, Car Haulers | USA |
| Brinkley RV, LLC | 7T0 | Travel Trailers, Fifth Wheels | USA |
| Highland Ridge RV | 58T | RV Trailers, Fifth Wheels | USA |
| Load Glide Trailers | 1L9 | Aluminum Flatbed Trailers | USA |
| Nine Tech Co., Ltd (Segway) | LTU | Electric Scooters/Motorcycles | China |
| XPO Manufacturing | 7F2 | Truck Trailers, Converter Dollies | USA |

## Usage

### Basic Usage

```csharp
using NicheVinDecoder;

// Decode a VIN
var result = VinDecoder.Decode("58TBM0BU8P3A13051");

if (result.IsValid)
{
    Console.WriteLine($"Manufacturer: {result.Manufacturer}");
    Console.WriteLine($"Model Year: {result.ModelYear}");
    Console.WriteLine($"Model: {result.Model}");
    Console.WriteLine($"Body Style: {result.BodyStyle}");
    
    // Access additional manufacturer-specific properties
    foreach (var prop in result.AdditionalProperties)
    {
        Console.WriteLine($"{prop.Key}: {prop.Value}");
    }
}
else
{
    Console.WriteLine("Invalid VIN or unsupported manufacturer");
    foreach (var warning in result.Warnings)
    {
        Console.WriteLine($"Warning: {warning.Message}");
    }
}
```

### Factory Pattern Usage

```csharp
using NicheVinDecoder.Core.Factory;

var factory = new VinDecoderFactory();
var decoder = factory.GetDecoder("58TBM0BU8P3A13051");

if (decoder != null)
{
    var result = decoder.Decode("58TBM0BU8P3A13051");
    // Process result...
}
```

### Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using NicheVinDecoder.Extensions;

var services = new ServiceCollection();
services.AddNicheVinDecoder();

var serviceProvider = services.BuildServiceProvider();
var factory = serviceProvider.GetService<VinDecoderFactory>();
```

## Contributing

We welcome contributions to expand the library's coverage of niche manufacturers! Here's how you can contribute:

### Adding a New Manufacturer Decoder

1. **Create the Decoder Class**
   - Add a new class in the `Manufacturers/` folder
   - Inherit from `BaseVinDecoder`
   - Follow the naming convention: `[ManufacturerName]VinDecoder.cs`

2. **Implement Required Properties and Methods**

```csharp
using NicheVinDecoder.Core.Base;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Manufacturers
{
    public class YourManufacturerVinDecoder : BaseVinDecoder
    {
        public override string ManufacturerName => "Your Manufacturer Name";
        
        public override IEnumerable<string> SupportedWMIs => new[] { "ABC" }; // World Manufacturer Identifier(s)
        
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
                result.Warnings.Add(new VinDecodingWarning("VIN cannot be decoded by this decoder"));
                return result;
            }

            try
            {
                // Decode VIN positions according to manufacturer's specification
                // Position 4-8: Vehicle Descriptor Section
                // Position 9: Check digit
                // Position 10: Model year
                // Position 11: Plant code
                // Position 12-17: Sequential number
                
                // Example decoding logic:
                result.ModelYear = DecodeModelYear(vin[9]);
                result.BodyStyle = DecodeBodyStyle(vin[5]);
                // ... additional decoding logic
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Warnings.Add(new VinDecodingWarning($"Error decoding VIN: {ex.Message}"));
            }

            return result;
        }
    }
}
```

3. **Add Unit Tests**
   
   Add comprehensive tests to `UnitTests.cs`:

```csharp
[Test]
public void VinDecoder_WithValidYourManufacturerVin_ShouldDecodeCorrectly()
{
    // Arrange
    const string vin = "ABCXXXXXXXXXXXXXXX"; // Valid VIN for your manufacturer

    // Act
    var result = VinDecoder.Decode(vin);

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.IsValid, Is.True);
    Assert.That(result.VIN, Is.EqualTo(vin));
    Assert.That(result.Manufacturer, Is.EqualTo("Your Manufacturer Name"));
    Assert.That(result.ModelYear, Is.EqualTo(2023)); // Expected model year
    Assert.That(result.BodyStyle, Is.EqualTo("Expected Body Style"));
    Assert.That(result.Model, Is.EqualTo("Expected Model"));
    // Add assertions for additional properties
    Assert.That(result.AdditionalProperties["PropertyName"], Is.EqualTo("Expected Value"));
    Assert.That(result.Warnings, Is.Empty);
}
```


### What You'll Need

To create a decoder, gather the following information about your target manufacturer:

- **World Manufacturer Identifier (WMI)**: The first 3 characters of the VIN
- **VDS (Vehicle Descriptor Section)**: How positions 4-8 are structured
- **Check Digit Algorithm**: If the manufacturer uses position 9 as a check digit
- **Model Year Encoding**: How position 10 encodes the model year
- **Plant Code**: How position 11 identifies manufacturing location
- **Sequential Number**: How positions 12-17 work for that manufacturer
- **Sample VINs**: Real VINs for testing (ensure they're publicly available)

### Documentation Requirements

When contributing, please include:

- **Manufacturer documentation** or specification sheets showing VIN structure
- **Sample VINs** with their decoded meanings
- **Unit tests** covering various vehicle configurations
- **Comments in code** explaining any unique decoding logic

### Pull Request Guidelines

1. **Fork the repository** and create a feature branch
2. **Follow existing code style** and patterns
3. **Include comprehensive unit tests** for your decoder
4. **Update this README** to add your manufacturer to the supported list
5. **Provide documentation** about the VIN structure you're implementing
6. **Test thoroughly** with multiple sample VINs

## Development Setup

1. Clone the repository
2. Open in Visual Studio or your preferred IDE
3. Build the solution
4. Run the unit tests to ensure everything works

## Testing

Run the unit tests using:

```bash
dotnet test
```

All decoders should have comprehensive unit tests covering:
- Valid VIN decoding
- Invalid VIN handling
- Edge cases and error conditions
- All supported model years and configurations

## License

[Add your license information here]

## Acknowledgments

This project exists to fill gaps in VIN decoding coverage and relies on contributions from the community and publicly available manufacturer documentation.
