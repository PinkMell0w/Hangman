using HangmanGame.Core.Core.Domain;
using HangmanGame.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HangmanGame.Data.Repositories
{
    public class PlayerInMatchRepository
    {
        private readonly DatabaseContext _context;

        public PlayerInMatchRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int GetPlayerCountByMatch(int matchId)
        {
            const string query = "SELECT COUNT(*) FROM PlayerInMatch WHERE matchId = @matchId AND isKicked = 0";

            SqlConnection conn = _context.GetOpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    cmd.Parameters.AddWithValue("@matchId", matchId);
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetPlayerCountByMatch error: {ex.Message}");
                return 0;
            }
        }
    }
}
