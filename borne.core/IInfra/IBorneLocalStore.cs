using borne.core.Entities;
using borne.core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace borne.core.IInfra
{
    public interface IBorneLocalStore
    {

        Task<bool> IsAvailable(CancellationToken cancellationToken = default);
        Task<Reservation> GetCachedReservation(CancellationToken cancellationToken = default);
        Task CacheReservation(Reservation res, CancellationToken cancellationToken = default);
        Task ClearReservation(CancellationToken cancellationToken = default);

        Task<BorneTariffSchedule> GetTariffSchedule(TypeEnergie energyType,
            string version = null,
            CancellationToken cancellationToken = default
            );

        Task CacheTariffSchedule(
            BorneTariffSchedule schedule,
            CancellationToken cancellationToken = default
            );

        Task DeactivateTariffSchedule(
            int tarifId,
            TypeEnergie energyType,
            CancellationToken cancellationToken = default
            );

        Task<BorneSessionState> GetSession(CancellationToken cancellationToken = default);
        Task SaveSession(BorneSessionState session, CancellationToken cancellationTkn = default);

        Task SaveSessionAndQueueEvent(
            BorneSessionState session,
            Guid eventId,
            string eventType,
            string payloadJson,
            DateTimeOffset createdAtUtc,
            CancellationToken cancellationTkn = default
            );
        Task<Consommation> GetLatestReading(CancellationToken cancellationTkn = default);
        Task<Consommation> GetReadingById(
            Guid readingId,
            CancellationToken cancellationTkn = default
            );

        Task SaveReadingAndQueueEvent(
            Consommation reading,
            BorneSessionState session,
            string eventType,
            string payloadJson,
            CancellationToken cancellationTkn = default
            );

        Task QueueEvent(
            Guid eventId,
            string eventType,
            string payloadJson,
            DateTimeOffset createdAtUtc,
            CancellationToken cancellationTkn = default
            );

        Task<IReadOnlyList<PendingBorneEvent>> GetPendingEvent(
            int limit,
            CancellationToken cancellationTkn = default
            );

        Task AcknowledgeEvent(Guid eventId, CancellationToken cancellationTkn = default);
        Task RecordEventAttempt(Guid eventId, CancellationToken cancellationTkn = default);

    }

    public interface ICardTagHasher
    {
        string Hash(string cardTag);
    }
}
