using borne.core.Entities;
using borne.core.IInfra;
using borne.infrastructure.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace borne.infrastructure.Daos
{
    public sealed class MySqlConfigBorneDao : IConfigDAO
    {

        private readonly MySqlConnectionFactory cn; 

        public MySqlConfigBorneDao(MySqlConnectionFactory conn)
        {
            this.cn = conn;
        }
       
        public async Task<List<ConfigBorne>> GetAllConfigsBorne(CancellationToken cancellationtkn = default)
        {
            await using var connection = cn.CreateConnection();
            await connection.OpenAsync(cancellationtkn);
            await using var command = new MySqlCommand("Select id_borne,code_borne from borne order by id_borne", connection);
            await using var reader = await command.ExecuteReaderAsync(cancellationtkn);
            var bornes = new List<ConfigBorne>();
            while (await reader.ReadAsync(cancellationtkn))
            {
                bornes.Add(Map((MySqlDataReader)reader));
            }
            return bornes;


        }

        public async Task<ConfigBorne?> GetConfigBorneById(int idConfBorne,CancellationToken cancellationTkn = default)
        {
            await using var conn = cn.CreateConnection();
            await conn.OpenAsync(cancellationTkn);
            await using var command = new MySqlCommand("select id_borne, code_borne from borne where id_borne = @id", conn);
            MySqlData.AddParameter(command, "@id", MySqlDbType.Int32, idConfBorne);
            await using var reader = await command.ExecuteReaderAsync(cancellationTkn);
            return await reader.ReadAsync(cancellationTkn) ? Map((MySqlDataReader)reader) : null; 
        }

        public async Task<ConfigBorne?> GetConfigBorneByCode(string codeBorne,CancellationToken cancellationTkn = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(codeBorne);
            await using var conn = cn.CreateConnection();
            await conn.OpenAsync(cancellationTkn);
            await using var command = new MySqlCommand("select id_borne,code_borne from borne where code_borne = @code", conn);
            MySqlData.AddParameter(command, "@code", MySqlDbType.VarChar, codeBorne);
            await using var reader = await command.ExecuteReaderAsync(cancellationTkn);
            return await reader.ReadAsync(cancellationTkn) ? Map((MySqlDataReader)reader) : null; 
        }

        public async Task AddConfigBorne(
        ConfigBorne configBorne,
        CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(configBorne);
            ArgumentException.ThrowIfNullOrWhiteSpace(configBorne.coreBorne);

            await using var connection = cn.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                "INSERT INTO borne (code_borne) VALUES (@code)",
                connection
            );
            MySqlData.AddParameter(
                command,
                "@code",
                MySqlDbType.VarChar,
                configBorne.coreBorne
            );
            await command.ExecuteNonQueryAsync(cancellationToken);
            configBorne.idConfigBorne = checked((int)command.LastInsertedId);
        }

        public async Task UpdateConfigBorne(
       ConfigBorne configBorne,
       CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(configBorne);
            ArgumentException.ThrowIfNullOrWhiteSpace(configBorne.coreBorne);

            await using var connection = cn.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                "UPDATE borne SET code_borne = @code WHERE id_borne = @id",
                connection
            );
            MySqlData.AddParameter(
                command,
                "@code",
                MySqlDbType.VarChar,
                configBorne.coreBorne
            );
            MySqlData.AddParameter(
                command,
                "@id",
                MySqlDbType.Int32,
                configBorne.idConfigBorne
            );
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task DeleteConfigBorne(
        int idConfigBorne,
        CancellationToken cancellationToken = default)
        {
            await using var connection = cn.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand(
                "DELETE FROM borne WHERE id_borne = @id",
                connection
            );
            MySqlData.AddParameter(command, "@id", MySqlDbType.Int32, idConfigBorne);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        private static ConfigBorne Map(MySqlDataReader reader)
        {
            return new ConfigBorne
            {
                idConfigBorne = reader.GetInt32("id_borne"),
                coreBorne= reader.GetString("code_borne")
            };
        }

    }
}
