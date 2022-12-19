using System;
using System.Data.OleDb;

namespace SSP_1.DAL
{
    public class ConnectionProvider
    {
        private static readonly string _connectionStringDefaultPath = $"provider=Microsoft.Jet.OLEDB.4.0;data source=";
        private static Lazy<OleDbConnection> _connection;

        public static void CreateConnection(string path)
        {
            _connection = new Lazy<OleDbConnection>(() => new OleDbConnection(_connectionStringDefaultPath + path), true);

        }

        public static OleDbConnection Instance => _connection.Value;
    }
}
