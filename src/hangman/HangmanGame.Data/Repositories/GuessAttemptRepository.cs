using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.Interfaces.Repositories;
using HangmanGame.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Data.Repositories
{
    public class GuessAttemptRepository : IMatchRepository
    {
        private readonly DatabaseContext _context;

        public GuessAttemptRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void AddGuessAttempt(GuessAttempt attempt)
        {
            // SQL Server automatically handles attemptId and attemptedAt!
            const string query = @"
                INSERT INTO [GuessAttempt] (sessionId, userId, letter, isCorrect)
                VALUES (@sessionId, @userId, @letter, @isCorrect);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlConnection conn = _context.GetOpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    cmd.Parameters.AddWithValue("@sessionId", attempt.SessionId);
                    cmd.Parameters.AddWithValue("@userId", attempt.UserId);
                    cmd.Parameters.AddWithValue("@letter", attempt.Letter.ToString());
                    cmd.Parameters.AddWithValue("@isCorrect", attempt.IsCorrect);

                    // Assuming your domain model maps attemptId to a property like GuessAttemptId or AttemptId
                    attempt.GuessAttemptId = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Add GuessAttempt error: {ex.Message}");
                throw;
            }
        }

        public void Add(Match entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Match Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Match> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Match> GetAvailableMatches()
        {
            throw new NotImplementedException();
        }

        public Match GetById(int matchId)
        {
            throw new NotImplementedException();
        }

        public void Update(Match entity)
        {
            throw new NotImplementedException();
        }
    }
}
