using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace borne.core.Entities
{
    public class Consommation
    {
        public Guid ReadingId { get; set; } = Guid.NewGuid();
        public int IdConsommation { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantite  { get; set; }
        public decimal Montant { get; set; }
        public decimal MontantCumule { get; set; }

        public int TarifId { get; set; }
        public Tarif Tarif { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

    }
}
