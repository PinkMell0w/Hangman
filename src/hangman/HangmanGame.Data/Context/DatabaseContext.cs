using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HangmanGame.Data.Context
{
    public class DatabaseContext
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public DatabaseContext()
        {
            _connectionString = ConfigurationManager.AppSettings["HangmanDB"];

            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException(
                    "HangmanDB connection string not found. Check database.config exists and is correct.");
        }

        public SqlConnection GetOpenConnection()
        {
            if (_connection == null)
                _connection = new SqlConnection(_connectionString);

            if (_connection.State == ConnectionState.Closed ||
                _connection.State == ConnectionState.Broken)
                _connection.Open();

            return _connection;
        }

        public SqlTransaction BeginTransaction()
        {
            GetOpenConnection();
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public SqlTransaction CurrentTransaction => _transaction;

        public void Commit()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}