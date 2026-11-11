using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace borne.infrastructure.Data
{
    public class MySqlConnectionFactory
    {

        private readonly string _connectionString;
        public MySqlConnectionFactory(string cn)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cn);
            _connectionString = cn;

        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
