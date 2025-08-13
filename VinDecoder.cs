using NicheVinDecoder.Core.Factory;
using NicheVinDecoder.Core.Models;

namespace NicheVinDecoder
{
    public class VinDecoder
    {
        private static readonly Lazy<VinDecoderFactory> _factory = new(() => new VinDecoderFactory());

        /// <summary>
        /// Decodes a VIN using the appropriate niche manufacturer decoder
        /// </summary>
        /// <param name="vin">The VIN to decode</param>
        /// <returns>VinDecodingResult if a decoder is found, null otherwise</returns>
        public static VinDecodingResult? Decode(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return null;

            try
            {
                var decoder = _factory.Value.GetDecoder(vin);
                return decoder?.Decode(vin);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if a VIN can be decoded by any registered decoder
        /// </summary>
        /// <param name="vin">The VIN to check</param>
        /// <returns>True if a decoder exists for this VIN, false otherwise</returns>
        public static bool CanDecode(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return false;

            try
            {
                var decoder = _factory.Value.GetDecoder(vin);
                return decoder != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
