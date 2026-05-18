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
                    (username, email, pwdHash, @salt, isActive, createdAt)
                OUTPUT INSERTED.userId
                VALUES
                    (@username, @email, @pwdHash, @salt, @isActive, @createdAt)";

            SqlConnection conn = _context.GetOpenConnection();
            SqlTransaction transaction = _context.CurrentTransaction;

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@pwdHash", user.pwdHash);
                cmd.Parameters.AddWithValue("@salt", user.Salt);
                cmd.Parameters.AddWithValue("@isActive", user.isActive);
                cmd.Parameters.AddWithValue("@createdAt", user.CreatedAt);
                try
                {
                    user.UserId = (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    throw new System.Exception("An error occurred while adding the user to the database.", ex);
                }
            }
        }

        public bool ExistsByEmail(string email) {
            const string query = "SELECT COUNT(1) FROM [User] WHERE email = @email";

            SqlConnection conn = _context.GetOpenConnection();
            
            using(SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddWithValue("@email", email);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool ExistsByUsername(string username)
        {
            const string query = "SELECT COUNT(1) FROM [User] WHERE username = @username";

            using (SqlConnection conn = _context.GetOpenConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    return (int)cmd.ExecuteScalar() > 0;
                }
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
            const string query =
                "SELECT * FROM [User] WHERE userId = @userId";

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

        public IEnumerable<User> GetAll() => throw new NotImplementedException();
        public void Update(User entity) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();

        private static User MapReader(SqlDataReader reader)
        {
            return new User
            {
                UserId = (int)reader["userId"],
                Username = (string)reader["username"],
                Email = (string)reader["email"],
                pwdHash = (string)reader["passwordHash"],
                Salt = (string)reader["salt"],
                isActive = (int)reader["isActive"],
                CreatedAt = (DateTime)reader["createdAt"],
                UpdatedAt = reader["updatedAt"] as DateTime?
            };
        }
    }
}
