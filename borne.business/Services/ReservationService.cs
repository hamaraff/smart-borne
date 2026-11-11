using borne.core.Entities;
using borne.core.IInfra;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.business.Services
{
    public class ReservationService
    {
        private readonly IReservationDAO resDAO;
        
        public ReservationService(IReservationDAO resDAO)
        {
            this.resDAO = resDAO;
        }

        public Task<Reservation?> GetReservationById(int idReservation,CancellationToken cancellationToken = default)
        {
            ValidateId(idReservation, nameof(idReservation));
            return resDAO.GetById(idReservation, cancellationToken);


        }

        public Task<List<Reservation>> GetAllReservations(CancellationToken cancellationToken = default)
        {
            return resDAO.GetAll(cancellationToken);
        }

        public async Task AddReservation(Reservation res,CancellationToken cancellationToken = default) {
            ArgumentNullException.ThrowIfNull(res);
            if(res.IdReservation != 0)
            {
                throw new ArgumentException("une nouvelle reservation ne doit pas avoir d'Id avant l'ajout.");
            }
            ValidateReservation(res);
            await resDAO.Add(res, cancellationToken);
        }

        public async Task UpdateReservation(Reservation res, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(res);
            ValidateId(res.IdReservation, nameof(res.IdReservation));
            ValidateReservation(res);
            if(await resDAO.GetById(res.IdReservation,cancellationToken) is null)
            {
                throw new KeyNotFoundException("La reservation n'existe pas ");

            }
            await resDAO.Update(res, cancellationToken);
        }

        public async Task DeleteReservation(
            int resId,
            CancellationToken cancellationToken = default
            )
        {
            ValidateId(resId, nameof(resId));

            if(await resDAO.GetById(resId,cancellationToken) is null)
            {
                throw new KeyNotFoundException("la reservation n'existe pas ");
            }
            await resDAO.Delete(resId, cancellationToken);
        }

        private static void ValidateReservation(Reservation reservation)
        {
            if (reservation.DateDeb >= reservation.DateFin)
            {
                throw new ArgumentException("date deb doit < date fin");
            }

            ValidateId(reservation.IdBorne, nameof(reservation.IdBorne));
            if (!Enum.IsDefined(reservation.TypeEnergie))
            {
                throw new ArgumentOutOfRangeException(nameof(reservation.TypeEnergie));
            }
        }

        private static void ValidateId(int id, string parameterName)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "The ID must be positive.");
            }
        }


    }
}
