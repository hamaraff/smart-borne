using borne.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace borne.core.IInfra
{
    public interface IConsommationDAO
    {

        Task<Consommation> GetById(
            int id,
            CancellationToken cancellationTkn = default
            );

        Task<Consommation> GetByReadingId(
            Guid readingId,
            CancellationToken cancellationTkn = default
            );

        Task<List<Consommation>> GetAll(
            CancellationToken cancellationTkn = default
            );

        Task<List<Consommation>> GetByReservationId(
            int resId, CancellationToken cancellationTkn = default
            );
        Task<List<Consommation>> GetByBorneId(
            int borneId, CancellationToken cancellationTkn = default);
        Task<List<Consommation>> GetByBorneIdAndDateRand(
            int idBorne,
            DateTime dateDeb,
            DateTime dateFin,
            CancellationToken cancellationTkn = default
            );

        Task<Consommation> GetLatestByBorneId(
            int idBorne,
            CancellationToken cancellationTkn = default
            );

        Task<List<Consommation>> GetByDate(
            DateTime date,
            CancellationToken cancellationTkn = default
            );

        Task<List<Consommation>> GetByDateRange(
            DateTime dateDeb,
            DateTime dateFin,
            CancellationToken cancellationTkn = default
            );

        Task<bool> Add(Consommation c, CancellationToken cancellationTkn = default);
        Task Update(Consommation c, CancellationToken cancellationTkn = default);
        Task Detele(int id, CancellationToken cancellationTkn = default);
    }
}
