using System.Configuration;
using System.Data.SqlClient;

namespace HangmanGame.Data.Context
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["DB_CONNECTION_STRING"].ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
