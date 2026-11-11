using borne.core.Entities;
using borne.core.Enums;
using borne.core.IInfra;
using borne.infrastructure.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.infrastructure.Daos
{
    public sealed class MySqlSeuilDAO : ISeuil
    {
        private const string SelectColumns = "id_seuil, valeur, reservation_id, type_seuil";

        private readonly MySqlConnectionFactory connectionFactory;

        public MySqlSeuilDAO(MySqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<Seuil?> GetById(
            int idSeuil,
            CancellationToken cancellationToken = default)
        {
            await using var connection = connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                $"SELECT {SelectColumns} FROM seuil WHERE id_seuil = @id",
                connection
            );
            MySqlData.AddParameter(command, "@id", MySqlDbType.Int32, idSeuil);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            return await reader.ReadAsync(cancellationToken) ? Map((MySqlDataReader)reader) : null;
        }

        public async Task<List<Seuil>> GetAll(
            CancellationToken cancellationToken = default)
        {
            await using var connection = connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                $"SELECT {SelectColumns} FROM seuil ORDER BY id_seuil",
                connection
            );
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var thresholds = new List<Seuil>();
            while (await reader.ReadAsync(cancellationToken))
            {
                thresholds.Add(Map((MySqlDataReader)reader));
            }

            return thresholds;
        }

        public async Task<List<Seuil>> GetByReservationId(
            int reservationId,
            CancellationToken cancellationToken = default)
        {
            await using var connection = connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                $"SELECT {SelectColumns} FROM seuil WHERE reservation_id = @reservationId " +
                "ORDER BY id_seuil",
                connection
            );
            MySqlData.AddParameter(
                command,
                "@reservationId",
                MySqlDbType.Int32,
                reservationId
            );
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var thresholds = new List<Seuil>();
            while (await reader.ReadAsync(cancellationToken))
            {
                thresholds.Add(Map((MySqlDataReader)reader));
            }

            return thresholds;
        }

        public async Task Add(
            Seuil seuil,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(seuil);
            ValidateType(seuil.TypeSeuil);

            await using var connection = connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                "INSERT INTO seuil (valeur, reservation_id, type_seuil) " +
                "VALUES (@value, @reservationId, @type)",
                connection
            );
            AddParameters(command, seuil);
            await command.ExecuteNonQueryAsync(cancellationToken);
            seuil.IdSeuil = checked((int)command.LastInsertedId);
        }

        public async Task Update(
            Seuil seuil,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(seuil);
            ValidateType(seuil.TypeSeuil);

            await using var connection = connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                "UPDATE seuil SET valeur = @value, reservation_id = @reservationId, " +
                "type_seuil = @type WHERE id_seuil = @id",
                connection
            );
            AddParameters(command, seuil);
            MySqlData.AddParameter(command, "@id", MySqlDbType.Int32, seuil.IdSeuil);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task Delete(
            int idSeuil,
            CancellationToken cancellationToken = default)
        {
            await using var connection = connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                "DELETE FROM seuil WHERE id_seuil = @id",
                connection
            );
            MySqlData.AddParameter(command, "@id", MySqlDbType.Int32, idSeuil);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        private static Seuil Map(MySqlDataReader reader)
        {
            return new Seuil
            {
                IdSeuil = reader.GetInt32("id_seuil"),
                Valeur = reader.GetDecimal("valeur"),
                ReservationId = reader.GetInt32("reservation_id"),
                TypeSeuil = MySqlData.ReadEnum<TypeSeuil>(reader, "type_seuil")
            };
        }

        private static void AddParameters(MySqlCommand command, Seuil seuil)
        {
            MySqlData.AddParameter(command, "@value", MySqlDbType.Decimal, seuil.Valeur);
            MySqlData.AddParameter(
                command,
                "@reservationId",
                MySqlDbType.Int32,
                seuil.ReservationId
            );
            MySqlData.AddParameter(
                command,
                "@type",
                MySqlDbType.VarChar,
                seuil.TypeSeuil.ToString()
            );
        }

        private static void ValidateType(TypeSeuil typeSeuil)
        {
            if (!Enum.IsDefined(typeSeuil))
            {
                throw new ArgumentOutOfRangeException(nameof(typeSeuil));
            }
        }
    }
}
