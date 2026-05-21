using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.Interfaces.Repositories;
using HangmanGame.Data.Context;
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
        public PlayerStats GetByUserId(int userId)
        {
            throw new System.NotImplementedException();
        }

        public PlayerStats Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PlayerStats> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Add(PlayerStats stats)
        {
            const string query = @"
                INSERT INTO PlayerStats (userId, gamesPlayed, gamesWon, totalScore, winRate)
                VALUES (@UserId, 0, 0, 0, 0.00)";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", stats.UserId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(PlayerStats entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
