using borne.core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.business.Services
{
    public static class TariffCalculator
    {

        public static decimal Calculate(BorneTariffSchedule schedule , decimal cumulativeQte)
        {
            ArgumentNullException.ThrowIfNull(schedule);
            if(cumulativeQte < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cumulativeQte));
            }

            List<Tranche> bands = ValidateAndOrder(schedule);
            if(cumulativeQte > bands[^1].ValMax)
            {
                throw new InvalidOperationException("La quantite cumulee depasse la valeur maximale de la derniere tranche");
            }

            decimal amount = 0m; 
            foreach (Tranche band in bands)
            {
                decimal quanityInBand = Math.Max(0m, Math.Min(cumulativeQte, band.ValMax) - band.ValMin);
                amount += quanityInBand * band.Prix;
                if(cumulativeQte <= band.ValMax)
                {
                    return amount;
                }
            }
            return amount; 
        }

        private static List<Tranche> ValidateAndOrder(BorneTariffSchedule schedule)
        {
            List<Tranche> bands = schedule.Tranches.OrderBy(band => band.ValMin).ToList();
            if (bands.Count == 0)
                throw new InvalidOperationException("il n'y a pas de tranches tarifaires configurées.");

            decimal expectedMinimum = 0m;
            foreach (Tranche band in bands)
            {
                if (band.IdTarif != schedule.Tarif.IdTarif ||
                    band.ValMin != expectedMinimum ||
                    band.ValMax <= band.ValMin ||
                    band.Prix <= 0)
                {
                    throw new InvalidOperationException(
                        "Les tranches tarifaires doivent être valides, contiguës et commencer à zéro.");
                }

                expectedMinimum = band.ValMax;
            }

            return bands;
        }
    }
}
