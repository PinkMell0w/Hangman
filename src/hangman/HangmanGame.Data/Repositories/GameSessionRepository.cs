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
    public class GameSessionRepository : IMatchRepository
    {
        private readonly DatabaseContext _context;

        public GameSessionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void AddGameSession(GameSession session)
        {
            const string query = @"
                INSERT INTO [GameSession] (matchId, wordId)
                VALUES (@matchId, @wordId);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlConnection conn = _context.GetOpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    cmd.Parameters.AddWithValue("@matchId", session.MatchId);
                    cmd.Parameters.AddWithValue("@wordId", session.WordId);

                    session.SessionId = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Add GameSession error: {ex.Message}");
                throw;
            }
        }

        public void FinalizeSession(int sessionId, string finalResult, int wrongAttempts, int? winnerId)
        {
            SqlConnection conn = _context.GetOpenConnection();

            const string query = @"
                UPDATE [GameSession] 
                SET [result] = @result, 
                    [wrongAttempts] = @wrongAttempts, 
                    [winnerId] = @winnerId,
                    [finishedAt] = GETDATE() 
                WHERE sessionId = @sessionId";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    cmd.Parameters.AddWithValue("@result", finalResult);
                    cmd.Parameters.AddWithValue("@wrongAttempts", wrongAttempts);
                    cmd.Parameters.AddWithValue("@sessionId", sessionId);

                    if (winnerId.HasValue)
                        cmd.Parameters.AddWithValue("@winnerId", winnerId.Value);
                    else
                        cmd.Parameters.AddWithValue("@winnerId", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FinalizeSession error: {ex.Message}");
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
