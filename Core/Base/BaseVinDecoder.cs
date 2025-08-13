using NicheVinDecoder.Core.Interfaces;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder.Core.Base
{
    public abstract class BaseVinDecoder : IVinDecoder
    {
        public abstract string ManufacturerName { get; }
        public abstract IEnumerable<string> SupportedWMIs { get; }

        public virtual bool CanDecode(string vin)
        {
            return SupportedWMIs.Contains(vin.Substring(0, 3));
        }

        public abstract VinDecodingResult Decode(string vin);
    }
}
