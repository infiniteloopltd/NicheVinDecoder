using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicheVinDecoder.Core.Base
{
    public abstract class BaseLegacyVinDecoder
    {
        public string Vin { get; }
        public string? Prefix { get; protected set; }
        public string? Make { get; protected set; }
        public string? Model { get; protected set; }
        public int? Year { get; protected set; }
        public string? Notes { get; protected set; }

        protected BaseLegacyVinDecoder(string vin)
        {
            Vin = vin;
        }

        public abstract void Decode();
    }
}