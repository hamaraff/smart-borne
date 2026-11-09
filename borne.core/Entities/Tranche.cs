using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace borne.core.Entities
{
    public class Tranche
    {
        public int IdTranche { get; set; }
        public decimal ValMin { get; set; }
        public decimal ValMax { get; set; }
        public decimal Prix { get; set; }
        public int IdTarif { get; set; }
    }
}
