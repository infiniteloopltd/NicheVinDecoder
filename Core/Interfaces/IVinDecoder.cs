using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Core.Interfaces
{
    public interface IVinDecoder
    {
        string ManufacturerName { get; }
        IEnumerable<string> SupportedWMIs { get; } // World Manufacturer Identifiers
        bool CanDecode(string vin);
        VinDecodingResult Decode(string vin);
    }
}
