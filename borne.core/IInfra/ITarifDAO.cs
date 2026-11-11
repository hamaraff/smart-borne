using borne.core.Entities;
using borne.core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace borne.core.IInfra
{
    public interface ITarifDAO
    {
        Task<List<Tarif>> GetAll(CancellationToken cancellationTkn = default);
        Task<Tarif> GetById(int id, CancellationToken cancellationTkn = default);
        Task<List<Tarif>> GetByEnergie(

            TypeEnergie typeEnergie,
            CancellationToken cancellationTkn = default
            );
        Task Add(Tarif tarif, CancellationToken cancellationTkn = default);
        Task Update(Tarif tarif, CancellationToken cancellationTkn = default);
        Task Delete(Tarif tarif, CancellationToken cancellationTkn = default);
    }
}
