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

        public PlayerStats GetByUserId(int userId)
        {
            const string query = "SELECT * FROM PlayerStats WHERE userId = @userId";

            var conn = _context.GetOpenConnection();

            using (var cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    return new PlayerStats
                    {
                        StatsId = Convert.ToInt32(reader["statsId"]),
                        UserId = Convert.ToInt32(reader["userId"]),
                        GamesPlayed = Convert.ToInt32(reader["gamesPlayed"]),
                        GamesWon = Convert.ToInt32(reader["gamesWon"]),
                        TotalScore = Convert.ToInt32(reader["totalScore"]),
                        WinRate = Convert.ToDouble(reader["winRate"]),
                        UpdatedAt = reader["updatedat"] as DateTime?
                    };
                }
            }
        }
        public PlayerStats Get(int id) => throw new NotImplementedException();
        public IEnumerable<PlayerStats> GetAll() => throw new NotImplementedException();
        public void Update(PlayerStats entity) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
    }
}
