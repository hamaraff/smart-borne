using borne.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace borne.core.IInfra
{
    public interface IConfigDAO
    {

        Task<List<ConfigBorne>> GetConfigBorneById(
            int idConfigBorne,
            CancellationToken cancellationTkn = default
            );

        Task<ConfigBorne> GetConfigBorneByCode(
            string codeBorne,
            CancellationToken cancellationTkn = default
            );

        Task AddConfigBorne(
            ConfigBorne configB,
            CancellationToken cancellationTkn = default
            );

        Task UpdateConfigBorne(
            ConfigBorne configB,
            CancellationToken cancellationTkn = default
            );

        Task DeleteConfigBorne(
            int idConfigBorne,
            CancellationToken cancellationTkn = default
            );
    }
}
