using borne.core.Entities;
using borne.core.IInfra;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.business.Services
{


    public class ConsommationService
    {
        private readonly IConsommationDAO consDAO;
        private readonly IReservationDAO resDAO;
        private readonly ITarifDAO tarifDAO;

        public ConsommationService(IConsommationDAO con, IReservationDAO res , ITarifDAO tar)
        {
            this.consDAO = con;
            this.resDAO = res;
            this.tarifDAO = tar; 

           
        }

        public Task<Consommation?> GetConsommationById(
            int id , CancellationToken cancellationToken = default
            )
        {
            ValidateId(id, nameof(id));
            return consDAO.GetById(id, cancellationToken);
        }

        public Task<Consommation?> GetConsommationByReadingId(
            Guid readingId,
                CancellationToken cancellationToken = default
            )
        {
            if(readingId == Guid.Empty)
            {
                throw new ArgumentException("reading Id est requis ! ", nameof(readingId));
            }
            return consDAO.GetByReadingId(readingId, cancellationToken);
        }

        public Task<List<Consommation>> GetAllConsommations(
            CancellationToken cancellationToken
            )
        {
            return consDAO.GetAll(cancellationToken);

        }

        public async Task<List<Consommation>> GetConsommationsByReservation(int resId , CancellationToken cancellationToken = default)
        {
            _ = await RequireReservationAsync(resId, cancellationToken);
            return await consDAO.GetByReservationId(resId, cancellationToken);
        }

        public Task<List<Consommation>> GetConsommationsByDate(
            DateTime date,CancellationToken cancellationToken = default
            )
        {
            if(date == default)
            {
                throw new ArgumentOutOfRangeException(nameof(date));
            }
            return consDAO.GetByDate(date, cancellationToken);
        }

        public Task<List<Consommation>> GetConsommationByDateRange(DateTime dateDeb , DateTime dateFin, CancellationToken cancellationToken = default)
        {
            ValidateDateRange(dateDeb, dateFin);
            return consDAO.GetByDateRange(dateDeb, dateFin, cancellationToken);
        }

        public Task<Consommation?> GetCurrentConsommationByBorneId(
            int idBorne,
            CancellationToken cancellationToken = default
            )
        {
            ValidateId(idBorne, nameof(idBorne));
            return consDAO.GetLatestByBorneId(idBorne, cancellationToken);
        }

        public Task<List<Consommation>> GetConsommationsByBorneId(int idBorne, CancellationToken cancellationToken)
        {
            ValidateId(idBorne, nameof(idBorne));
            return consDAO.GetByBorneId(idBorne, cancellationToken);
        }

        public Task<List<Consommation>> GetConsommationsByBorneIdAndDateRange(
        int idBorne,
        DateTime dateDebut,
        DateTime dateFin,
        CancellationToken cancellationToken = default)
        {
            ValidateId(idBorne, nameof(idBorne));
            ValidateDateRange(dateDebut, dateFin);
            return consDAO.GetByBorneIdAndDateRand(
                idBorne,
                dateDebut,
                dateFin,
                cancellationToken);
        }

        public async Task<bool> AddConsommation(
        Consommation consommation,
        CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(consommation);
            if (consommation.IdConsommation != 0)
            {
                throw new ArgumentException("une nouv reading n'a pas une bd id ");
            }

            ValidateReading(consommation);
            Reservation reservation = await RequireReservationAsync(
                consommation.ReservationId,
                cancellationToken);
            Tarif tarif = await RequireTarifAsync(consommation.TarifId, cancellationToken);
            ValidateTarifForReservation(tarif, reservation);

            consommation.Tarif = tarif;
            consommation.Reservation = reservation;
            return await consDAO.Add(consommation, cancellationToken);
        }

        public async Task UpdateConsommation(
        Consommation consommation,
        CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(consommation);
            ValidateId(consommation.IdConsommation, nameof(consommation.IdConsommation));
            Consommation? existing = await consDAO.GetById(
                consommation.IdConsommation,
                cancellationToken);
            if (existing is null)
            {
                throw new KeyNotFoundException("la consommation lire n'existe pas ");
            }

            if (existing.ReadingId != consommation.ReadingId)
            {
                throw new ArgumentException("reading id ne doive pas changee apres creation");
            }

            ValidateReading(consommation);
            Reservation reservation = await RequireReservationAsync(
                consommation.ReservationId,
                cancellationToken);
            Tarif tarif = await RequireTarifAsync(consommation.TarifId, cancellationToken);
            ValidateTarifForReservation(tarif, reservation);

            consommation.Tarif = tarif;
            consommation.Reservation = reservation;
            await consDAO.Update(consommation, cancellationToken);
        }
        public async Task DeleteConsommationAsync(
        int id,
        CancellationToken cancellationToken = default)
        {
            _ = await GetConsommationById(id, cancellationToken)
                ?? throw new KeyNotFoundException("cons reading n'existe pas ");
            await consDAO.Detele(id, cancellationToken);
        }
        private async Task<Reservation> RequireReservationAsync(
        int reservationId,
        CancellationToken cancellationToken)
        {
            ValidateId(reservationId, nameof(reservationId));
            return await resDAO.GetById(reservationId, cancellationToken)
                ?? throw new KeyNotFoundException("reservartion n'existe pas ");
        }
        private async Task<Tarif> RequireTarifAsync(
        int tarifId,
        CancellationToken cancellationToken)
        {
            ValidateId(tarifId, nameof(tarifId));
            Tarif tarif = await tarifDAO.GetById(tarifId, cancellationToken)
                ?? throw new KeyNotFoundException("tarif n'existe pas ");
            if (!Enum.IsDefined(tarif.TypeEnergie))
            {
                throw new InvalidOperationException("tarif a une type d'energie inexistante");
            }

            return tarif;
        }

        private static void ValidateReading(Consommation cons)
        {
            if(cons.Quantite <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cons.Quantite), "qte doit etre positive");

            }
            if(cons.Montant < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cons.Montant), " montant ne doit pas etre negative");
            }
            if(cons.Date == default){
                throw new ArgumentOutOfRangeException(nameof(cons.Date));

            }
            if(cons.ReadingId == Guid.Empty)
            {
                throw new ArgumentException("reading id est oblogatoire ! ", nameof(cons.ReadingId));

            }
            ValidateId(cons.ReservationId, nameof(cons.ReservationId));
            ValidateId(cons.TarifId, nameof(cons.TarifId));
        }

        private static void ValidateTarifForReservation(Tarif tarif, Reservation reservation)
        {
            if (tarif.TypeEnergie != reservation.TypeEnergie)
            {
                throw new ArgumentException("tarif energy type doit etre = Energie type de reservation !");
            }
        }

        private static void ValidateDateRange(DateTime dateDebut, DateTime dateFin)
        {
            if (dateDebut == default || dateFin == default || dateDebut >= dateFin)
            {
                throw new ArgumentException(
                    "date deb doit etre < datefin");
            }
        }

        private static void ValidateId(int id, string parameterName)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "ID doit etre positive");
            }
        }

    }
}
