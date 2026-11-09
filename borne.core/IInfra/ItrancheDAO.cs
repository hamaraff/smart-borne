using borne.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace borne.core.IInfra
{
    public interface ItrancheDAO
    {

        Task<Tranche> GetById(int idTranche, CancellationToken cancellationTkn = default);
        Task<List<Tranche>> GetAll(CancellationToken cancellationTkn = default);
        Task<List<Tranche>> GetByTarifId(
            int tarifId,
            CancellationToken cancellationTkn = default
            );
        Task Add(Tranche tranche, CancellationToken cancellationTkn = default);
        Task Update(Tranche tranche, CancellationToken cancellationTkn = default);
        Task Delete(Tranche tranche, CancellationToken cancellationTkn = default);
    }
}
