using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.Interfaces.Repositories;
using HangmanGame.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangmanGame.Data.Repositories
{
    public class PlayerStatsRepository : IPlayerStatsRepository
    {
        private readonly DatabaseContext _context;

        public PlayerStatsRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(PlayerStats stats)
        {
            const string query = @"
            INSERT INTO PlayerStats (userId, gamesPlayed, gamesWon, totalScore, winRate)
            VALUES (@UserId, 0, 0, 0, 0.00)";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@UserId", stats.UserId);
                cmd.ExecuteNonQuery();
            }
        }

        public PlayerStats GetByUserId(int userId) => throw new NotImplementedException();
        public PlayerStats Get(int id) => throw new NotImplementedException();
        public IEnumerable<PlayerStats> GetAll() => throw new NotImplementedException();
        public void Update(PlayerStats entity) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}
