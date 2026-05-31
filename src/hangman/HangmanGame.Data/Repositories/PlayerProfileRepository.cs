using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.Interfaces.Repositories;
using HangmanGame.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangmanGame.Data.Repositories
{
    public class PlayerProfileRepository : IPlayerProfileRespository
    {
        private readonly DatabaseContext _context;

        public PlayerProfileRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(PlayerProfile profile)
        {
            const string query = @"
            INSERT INTO PlayerProfile (userId, theme, bio)
            VALUES (@UserId, @Theme, @Bio)";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@UserId", profile.UserId);
                cmd.Parameters.AddWithValue("@Theme", profile.Theme ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Bio", profile.Bio ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public PlayerProfile GetByUserId(int userId)
        {
            const string query = "SELECT * FROM PlayerProfile WHERE userId = @userId";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return new PlayerProfile
                    {
                        ProfileId = (int)reader["profileId"],
                        UserId = (int)reader["userId"],
                        AvatarUrl = reader["avatarUrl"] as string,
                        Theme = reader["theme"] as string,
                        Bio = reader["bio"] as string,
                        UpdatedAt = reader["updatedAt"] as DateTime?
                    };
                }
            }
        }

        public PlayerProfile Get(int id)        => throw new NotImplementedException();
        public IEnumerable<PlayerProfile> GetAll()  => throw new NotImplementedException();
        public void Update(PlayerProfile entity)   => throw new NotImplementedException();
        public void Delete(int id)                  => throw new NotImplementedException();
    }
}
