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

        public bool UpdateProfileInfo(int userId, string newUsername, string bio)
        {
            SqlConnection conn = _context.GetOpenConnection();

            string selectQuery = @"
            SELECT u.username, p.bio 
            FROM [User] u
            INNER JOIN [PlayerProfile] p ON u.userId = p.userId
            WHERE u.userId = @userId";

            string currentUsername = "";
            string currentBio = null;

            using (SqlCommand cmd = new SqlCommand(selectQuery, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentUsername = reader["username"].ToString();
                        currentBio = reader["bio"] != DBNull.Value ? reader["bio"].ToString() : null;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (currentUsername == newUsername &&
                currentBio == bio)
            {
                return false;
            }

            using (var localTx = conn.BeginTransaction())
            {
                try
                {
                    string userQuery = "" +
                        "UPDATE [User]" +
                        "SET [username] = @username, [updatedAt] = GETDATE()" +
                        "WHERE [userId] = @userId";
                    using (var cmd = new SqlCommand(userQuery, conn, localTx))
                    {
                        cmd.Parameters.AddWithValue("@username", newUsername);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                    }

                    string profileQuery = @"
                    UPDATE [PlayerProfile] 
                    SET [bio] = @bio, 
                        [updatedAt] = GETDATE() 
                    WHERE [userId] = @userId";

                    using (var cmd = new SqlCommand(profileQuery, conn, localTx))
                    {
                        cmd.Parameters.AddWithValue("@bio", (object)bio ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@userId", userId);

                        cmd.ExecuteNonQuery();
                    }

                    localTx.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    localTx.Rollback();
                    System.Diagnostics.Debug.WriteLine($"UpdateProfileInfo error: {ex.Message}");
                    throw;
                }
            }
        }

        public PlayerProfile Get(int id)        => throw new NotImplementedException();
        public IEnumerable<PlayerProfile> GetAll()  => throw new NotImplementedException();
        public void Update(PlayerProfile entity)   => throw new NotImplementedException();
        public void Delete(int id)                  => throw new NotImplementedException();
    }
}
