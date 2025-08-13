using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicheVinDecoder.Core.Models
{
    public class VinDecodingResult
    {
        public string VIN { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int? ModelYear { get; set; }
        public string BodyStyle { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public Dictionary<string, object> AdditionalProperties { get; set; }
        public List<VinDecodingWarning> Warnings { get; set; }
        public bool IsValid { get; set; }
    }
}
