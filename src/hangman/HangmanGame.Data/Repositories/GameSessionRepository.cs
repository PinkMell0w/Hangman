using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
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
                Debug.WriteLine($"AddGameSession error: {ex.Message}");
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

        public List<GetScoreBreakdownRequestDto> GetScoreHistoryByUserId(int userId)
        {
            var ledgerList = new List<GetScoreBreakdownRequestDto>();
            SqlConnection conn = _context.GetOpenConnection();

            const string query = @"
                SELECT 
                    gs.finishedAt AS MatchDate,
                    w.word AS Word,
                    uHost.username AS OpponentName,
                    10 AS PointsType,
                    'WORD_GUESSED' AS Category
                FROM [GameSession] gs
                JOIN [Match] m ON gs.matchId = m.matchId
                JOIN [Word] w ON gs.wordId = w.wordId
                JOIN [User] uHost ON m.hostId = uHost.userId
                WHERE gs.result = 'WIN' 
                    AND gs.winnerId = @userId
                    AND m.hostId != @userId

                UNION ALL

                SELECT 
                    gs.finishedAt AS MatchDate,
                    w.word AS Word,
                    uGuesser.username AS OpponentName,
                    5 AS PointsType,
                    'OPPONENT_FAILED' AS Category
                FROM [GameSession] gs
                JOIN [Match] m ON gs.matchId = m.matchId
                JOIN [Word] w ON gs.wordId = w.wordId
                JOIN [PlayerInMatch] pim ON m.matchId = pim.matchId
                JOIN [User] uGuesser ON pim.userId = uGuesser.userId
                WHERE gs.result = 'LOSS' 
                    AND gs.winnerId = @userId
                    AND m.hostId = @userId

                UNION ALL

                SELECT 
                    ISNULL(m.finishedAt, GETDATE()) AS MatchDate,
                    ISNULL(w.word, 'N/A') AS Word,
                    'System' AS OpponentName,
                    -3 AS PointsType,
                    'PENALIZATION' AS Category
                FROM [Match] m
                LEFT JOIN [GameSession] gs ON m.matchId = gs.matchId
                LEFT JOIN [Word] w ON gs.wordId = w.wordId
                WHERE m.status = 'CANCELLED'
                    AND (m.hostId = @userId OR m.matchId IN (SELECT matchId FROM PlayerInMatch WHERE userId = @userId))
        
                ORDER BY MatchDate DESC";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ledgerList.Add(new GetScoreBreakdownRequestDto
                            {
                                MatchDate = Convert.ToDateTime(reader["MatchDate"]),
                                Word = reader["Word"].ToString(),
                                OpponentName = reader["OpponentName"].ToString(),
                                PointsType = Convert.ToInt32(reader["PointsType"]),
                                Category = reader["Category"].ToString()
                            });
                        }
                    }
                }
                return ledgerList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetScoreHistoryByUserId error: {ex.Message}");
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
