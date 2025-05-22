using System;
using System.Data.SqlClient;

namespace ViviGest.Utilities
{
    public class DBContextUtility : IDisposable
    {
        private const string SERVER = "LAPTOP-R455N33E";
        private const string DB_NAME = "vivigest";
        private const string DB_USER = "vivigest1";
        private const string DB_PASSWORD = "vivigest";

        private static readonly string ConnString =
            $"server={SERVER};database={DB_NAME};user id={DB_USER};password={DB_PASSWORD};MultipleActiveResultSets=true";

        private SqlConnection _connection;

        public DBContextUtility()
        {
            _connection = new SqlConnection(ConnString);
        }

        /// <summary>Abre la conexión.</summary>
        public bool Connect()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>Cierra la conexión.</summary>
        public void Disconnect()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }

        /// <summary>Retorna la instancia de SqlConnection.</summary>
        public SqlConnection CONN()
        {
            return _connection;
        }

        /// <summary>Implementación de IDisposable para usar 'using'.</summary>
        public void Dispose()
        {
            Disconnect();
            _connection.Dispose();
        }
    }
}
