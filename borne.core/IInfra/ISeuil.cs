using borne.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace borne.core.IInfra
{
    public interface ISeuil
    {
        Task<Seuil> GetById(int idSeuil, CancellationToken cancellationTkn = default);
        Task<List<Seuil>> GetAll(CancellationToken cancellationTkn = default);

        Task<List<Seuil>> GetByReservationId(
            int reservationId,
            CancellationToken cancellationTkn = default
            );

        Task Add(Seuil s, CancellationToken cancellationTkn = default);
        Task Update(Seuil s, CancellationToken cancellationTkn = default);
        Task Delete(int idSeuil, CancellationToken cancellationTkn = default);
    }
}
