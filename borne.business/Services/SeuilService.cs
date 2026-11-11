using borne.core.Entities;
using borne.core.Enums;
using borne.core.IInfra;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.business.Services
{
    public class SeuilService
    {
        private readonly ISeuil seuilDAO;
        private readonly IReservationDAO resDAO; 

        public SeuilService(ISeuil seuilDAO, IReservationDAO reservationDAO)
        {
            this.seuilDAO = seuilDAO;
            this.resDAO = reservationDAO;
        }

        public async Task AddSeuil(
            Seuil seuil,
            CancellationToken cancellationToken = default

        )
        {
            ArgumentNullException.ThrowIfNull(seuil);
            if(seuil.IdSeuil != 0)
            {
                throw new InvalidOperationException("Le seuil ne doit pas avoir d'Id avant l'ajout.");
            }

            ValidateValue(seuil.Valeur);
            ValidateType(seuil.TypeSeuil);

            Reservation res = await RequireReservationAsync(seuil.ReservationId, cancellationToken);
            seuil.Reservation = res;
            await seuilDAO.Add(seuil, cancellationToken);
                    }

        public async Task UpdateSeuil(
            Seuil seuil,
            CancellationToken cancellationToken= default
            )
        {
            ArgumentNullException.ThrowIfNull(seuil);
            ValidateId(seuil.IdSeuil,nameof(seuil.IdSeuil));
            if(await seuilDAO.GetById(seuil.IdSeuil,cancellationToken) is null)
            {
                throw new InvalidOperationException("Le seuil n'existe pas.");
            }
            ValidateValue(seuil.Valeur);
            ValidateType(seuil.TypeSeuil);
            seuil.Reservation = await RequireReservationAsync(
                seuil.ReservationId, cancellationToken
                );
            await seuilDAO.Update(seuil,cancellationToken);
        }
        public async Task DeleteSeuilAsync(
        int idSeuil,
        CancellationToken cancellationToken = default)
        {
            _ = await GetSeuilByIdAsync(idSeuil, cancellationToken)
                ?? throw new KeyNotFoundException("Seuil n'existe pas.");
            await seuilDAO.Delete(idSeuil, cancellationToken);
        }

        public async Task<Seuil?> GetSeuilByIdAsync(
        int idSeuil,
        CancellationToken cancellationToken = default)
        {
            ValidateId(idSeuil, nameof(idSeuil));
            return await seuilDAO.GetById(idSeuil, cancellationToken);
        }

        public Task<List<Seuil>> GetAllSeuilsAsync(
        CancellationToken cancellationToken = default)
        {
            return seuilDAO.GetAll(cancellationToken);
        }

        public async Task<List<Seuil>> GetSeuilsByReservationAsync(
        int reservationId,
        CancellationToken cancellationToken = default)
        {
            _ = await RequireReservationAsync(reservationId, cancellationToken);
            return await seuilDAO.GetByReservationId(reservationId, cancellationToken);
        }

        private async Task<Reservation> RequireReservationAsync(
            int reservationId,
            CancellationToken cancellationToken)
        {
            ValidateId(reservationId, nameof(reservationId));
            return await resDAO.GetById(reservationId, cancellationToken)
                ?? throw new KeyNotFoundException("Reservation n'existe pas.");
        }

        private static void ValidateValue(decimal value)
        {
            if(value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "La valeur du seuil doit être positive.");
            }
        }

        private static void ValidateType(TypeSeuil type)
        {
            if (!Enum.IsDefined(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type), "Le type de seuil n'est pas valide.");
            }
        }

        private static void ValidateId(int id , string parameterName)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "L'identifiant doit être positif.");
            }
        }
    }
}
