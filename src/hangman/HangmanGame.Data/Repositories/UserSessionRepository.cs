using System;
using System.Data.SqlClient;
using HangmanGame.Data.Context;

namespace HangmanGame.Data.Repositories
{
    public class UserSessionRepository
    {
        private readonly DatabaseContext _context;

        public UserSessionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void EndActiveSessionsForUser(int userId)
        {
            const string query = "UPDATE UserSession SET endedAt = GETUTCDATE() WHERE userId = @userId AND endedAt IS NULL";
            var conn = _context.GetOpenConnection();
            using (var cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        public void CreateSession(int userId, string token)
        {
            const string query = @"INSERT INTO UserSession (userId, token) VALUES (@userId, @token)";
            var conn = _context.GetOpenConnection();
            using (var cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@token", token);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
