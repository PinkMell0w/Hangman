using HangmanGame.Core.Core.Domain;
using HangmanGame.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HangmanGame.Data.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            const string query = @"
                INSERT INTO [User]
                    (roleId, fullName, birthDate, phoneNumber, username, email, pwdHash, salt, isActive, createdAt)
                OUTPUT INSERTED.userId
                VALUES
                    (@roleId, @fullName, @birthDate, @phoneNumber, @username, @email, @pwdHash, @salt, @isActive, @createdAt)";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@fullName", user.FullName);
                cmd.Parameters.AddWithValue("@roleId", user.RoleId);
                cmd.Parameters.AddWithValue("@birthDate", user.DateOfBirth);
                cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@pwdHash", user.PwdHash);
                cmd.Parameters.AddWithValue("@salt", user.Salt);
                cmd.Parameters.AddWithValue("@isActive", user.IsActive);
                cmd.Parameters.AddWithValue("@createdAt", user.CreatedAt);

                user.UserId = (int)cmd.ExecuteScalar();
            }
        }

        public bool ExistsByEmail(string email)
        {
            const string query = "SELECT COUNT(1) FROM [User] WHERE email = @email";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@email", email);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool ExistsByUsername(string username)
        {
            const string query = "SELECT COUNT(1) FROM [User] WHERE username = @username";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@username", username);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public User GetByEmail(string email)
        {
            const string query = "SELECT * FROM [User] WHERE email = @email";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@email", email);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? MapReader(reader) : null;
                }
            }
        }

        public User GetByUsername(string username)
        {
            const string query = "SELECT * FROM [User] WHERE username = @username";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@username", username);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? MapReader(reader) : null;
                }
            }
        }

        public User GetById(int id)
        {
            const string query = "SELECT * FROM [User] WHERE userId = @userId";

            SqlConnection conn = _context.GetOpenConnection();

            using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@userId", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? MapReader(reader) : null;
                }
            }
        }

        // UserRepository.cs
        public List<User> SearchByUsername(string username, int excludeUserId)
        {
            const string query = @"
                    SELECT u.userId, u.username, u.isActive
                    FROM [User] u
                    WHERE u.username    LIKE @username
                      AND u.userId     != @excludeUserId
                      AND u.isActive    = 1
                    ORDER BY u.username";

            var users = new List<User>();
            var conn = _context.GetOpenConnection();

            using (var cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@username", $"%{username}%");
                cmd.Parameters.AddWithValue("@excludeUserId", excludeUserId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        users.Add(new User
                        {
                            UserId = Convert.ToInt32(reader["userId"]),
                            Username = Convert.ToString(reader["username"])
                        });
                }
            }
            return users;
        }

        public IEnumerable<User> GetAll() => throw new NotImplementedException();
        public void Update(User entity) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();

        private static User MapReader(SqlDataReader reader)
        {
            return new User
            {
                UserId = Convert.ToInt32(reader["userId"]),
                RoleId = Convert.ToInt32(reader["roleId"]),
                FullName = Convert.ToString(reader["fullName"]),
                DateOfBirth = Convert.ToDateTime(reader["birthDate"]),
                PhoneNumber = Convert.ToString(reader["phoneNumber"]),
                Username = Convert.ToString(reader["username"]),
                Email = Convert.ToString(reader["email"]),
                PwdHash = Convert.ToString(reader["pwdHash"]),
                Salt = Convert.ToString(reader["salt"]),
                IsActive = Convert.ToBoolean(reader["isActive"]),
                CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                UpdatedAt = reader["updatedAt"] as DateTime?
            };
        }
    }
}