using borne.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace borne.core.IInfra
{
    public interface IReservationDAO
    {

        Task<Reservation> GetById(int idReservation, CancellationToken cancellationTkn = default);
        Task<List<Reservation>> GetAll(CancellationToken cancellationTkn = default);
        Task Add(Reservation r, CancellationToken cancellationTkn = default);
        Task Update(Reservation r, CancellationToken cancellationTkn = default);
        Task Delete(int idReservation, CancellationToken cancellationTkn = default);


    }
}
