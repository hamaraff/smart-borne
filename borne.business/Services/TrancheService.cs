using borne.core.Entities;
using borne.core.IInfra;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.business.Services
{
    public class TrancheService
    {

        private readonly ItrancheDAO trancheDAO; 
        private readonly ITarifDAO tarifDAO ;

        public Task<List<Tranche>> GetAllTranches(CancellationToken cancellationTkn = default)
        {
            return trancheDAO.GetAll();
        }

        public Task<Tranche> GetTrancheById(int idTranche, CancellationToken cancellationTkn = default)
        {
            ValidateId(idTranche, nameof(idTranche);
            return trancheDAO.GetById(idTranche);
        }

        public async Task<List<Tranche>> GetTranchesByTarif(int idTarif,CancellationToken cancellationToken = default)
        {
            await RequireTarifAsync(idTarif, cancellationToken);
            return await trancheDAO.GetByTarifId(tarifId, cancellationToken);

        }

        public async Task AddTranche(Tranche tranche, CancellationToken cancellationToken = default)
        {
            ValidateRangeAndPrice(tranche);
            await RequireTarif(tranche.IdTarif, cancellationToken);
            await ValidateNoOverlapAsync(tranche,null,cancellationToken);
            await trancheDAO.Add(tranche, cancellationToken);
        }

        public async Task UpdateTranche(
            Tranche tranche,
            CancellationToken cancellationToken = default
            )
        {
            ArgumentNullException.ThrowIfNull(tranche);
            ValidateId(tranche.IdTranche, nameof(tranche.IdTranche));
            await RequireTarifAsync(tranche.IdTarif, cancellationToken);
            if(await GetTrancheById(tranche.IdTranche,cancellationToken) is null)
            {
                throw new KeyNotFoundException("tarif band n'existe pas");
            }
            await ValidateNoOverlap(tranche,tranche.IdTranche, cancellationToken);
            await trancheDAO.Update(tranche, cancellationToken);
        }

        public async Task DeleteTrancheAsync(
        int idTranche,
        CancellationToken cancellationToken = default)
        {
            Tranche tranche = await GetTrancheByIdAsync(idTranche, cancellationToken)
                ?? throw new KeyNotFoundException("The tariff band n'existe pas ");
            await trancheDAO.Delete(tranche, cancellationToken);
        }

        public Task DeleteTranche(
            Tranche tranche,
            CancellationToken cancellationTkn = default
            )
        {
            ArgumentNullException.ThrowIfNull(tranche);
            return trancheDAO.Delete(tranche, cancellationTkn);
        }

        public async Task<decimal> CalculerMontant(
            int tarifId,
            decimal consommationCumulee,
            CancellationToken cancellationToken = default
            )
        {
            if(consommationCumulee < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(consommationCumulee), "La consommation cumulee ne peut pas etre negative");

            }

            await RequireTarif(tarifId, cancellationToken);
            if(consommationCumulee == 0)
            {
                return 0m;
            }
            List<Tranche> tranches = await trancheDAO.GetByTarifId(tarifId, cancellationToken);
            tranches = tranches.OrderBy(tranche => tranche.ValMin).ToList();
            if(tranches.Count == 0)
            {
                throw new InvalidOperationException("il n ya pas tariff bad configuree pour cet tarif");

            }
            cecimal amount = 0m;
            decimal previousLimit = 0m;
            foreach(Tranche tranche in tranches)
            {
                if(tranche.ValMin != previousLimit || tranche.ValMax <= tranche.ValMin || tranche.Prix <= 0)
                {
                    throw new InvalidOperationException("bands de tariff peut etre valid , et contigue ml 0 ")
                }
                decimal quantityInBand = Math.Max(0m,Math.Min(consommationCumulee,tranche.ValMax) - tranche.ValMin);
                amount += quantityInBand * tranche.Prix;
                previousLimit = tranche.ValMax;

                if(consommationCumulee <= tranche.ValMax)
                {
                    return amount;
                }
            }
            throw new InvalidOperationException("La consommation cumulee depasse la limite superieure de la derniere tranche");

        }

        private async Task<Tarif> RequireTarif(int tarifId,CancellationToken cancellationToken)
        {
            ValidateId(tarifId, nameof(tarifId));
            return await tarifDAO.GetById(tarifId, cancellationToken) ?? throw new KeyNotFoundException("tariff n'existe pas");

        }

        private async Task ValidateNoOverlapAsync(Tranche candidate,int excludeId,CancellationToken cancellationToken)
        {
            List<Tranche> existing = await trancheDAO.GetByTarifId(candidate.IdTarif, cancellationToken);
            bool overlaps = existing.Any(tranche =>
            tranche.IdTranche != excludeId &&
            candidate.ValMin < tranche.ValMax &&
            tranche.ValMin < candidate.ValMax
            );
            if(overlaps)
            {
                throw new InvalidOperationException("La nouvelle tranche chevauche une tranche existante pour le meme tarif");
            }

        }

        private static void ValidateRangeAndPrice(Tranche tranche)
        {
            if (tranche.ValMin < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tranche.ValMin));
            }

            if (tranche.ValMax <= tranche.ValMin)
            {
                throw new ArgumentException(" max doit etre superieur a min");
            }

            if (tranche.Prix <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tranche.Prix));
            }
        }

        private static void ValidateId(int id, string parameterName)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Id doit etre positif.");
            }
        }




    }
}
