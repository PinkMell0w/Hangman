using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.Interfaces.Repositories;
using HangmanGame.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HangmanGame.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly DatabaseContext _context;

        public MatchRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<Match> GetAvailableMatches()
        {
            const string query = @"
                SELECT m.matchId, m.hostId, m.wordId, m.[status], m.maxPlayers, m.isLocalNetwork, m.createdAt, m.finishedAt
                FROM [Match] m
                WHERE m.[status] = 'WAITING'
                AND m.createdAt > DATEADD(hour, -1, GETDATE())
                ORDER BY m.createdAt DESC";

            SqlConnection conn = _context.GetOpenConnection();
            List<Match> matches = new List<Match>();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            matches.Add(new Match
                            {
                                MatchId = (int)reader["matchId"],
                                HostId = (int)reader["hostId"],
                                WordId = reader["wordId"] != DBNull.Value ? (int)reader["wordId"] : 0,
                                Status = (string)reader["status"],
                                maxPlayers = (int)reader["maxPlayers"],
                                IsLocalNetwork = (bool)reader["isLocalNetwork"],
                                createdAt = (DateTime)reader["createdAt"],
                                finishedAt = reader["finishedAt"] != DBNull.Value ? (DateTime)reader["finishedAt"] : DateTime.MinValue
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAvailableMatches error: {ex.Message}");
                return new List<Match>();
            }

            return matches;
        }

        public void Add(Match match)
        {
            const string query = @"
                INSERT INTO [Match]
                    (
                        hostId,
                        wordId,
                        status,
                        maxPlayers,
                        isLocalNetwork,
                        createdAt,
                        finishedAt
                    )
                VALUES
                    (
                        @hostId,
                        @wordId,
                        @status,
                        @maxPlayers,
                        @isLocalNetwork,
                        @createdAt,
                        @finishedAt
                    );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlConnection conn = _context.GetOpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    cmd.Parameters.AddWithValue("@hostId", match.HostId);

                    if (match.WordId > 0)
                        cmd.Parameters.AddWithValue("@wordId", match.WordId);
                    else
                        cmd.Parameters.AddWithValue("@wordId", DBNull.Value);

                    cmd.Parameters.AddWithValue("@status", match.Status);
                    cmd.Parameters.AddWithValue("@maxPlayers", match.maxPlayers);
                    cmd.Parameters.AddWithValue("@isLocalNetwork", match.IsLocalNetwork);
                    cmd.Parameters.AddWithValue("@createdAt", match.createdAt);

                    if (match.finishedAt != DateTime.MinValue)
                        cmd.Parameters.AddWithValue("@finishedAt", match.finishedAt);
                    else
                        cmd.Parameters.AddWithValue("@finishedAt", DBNull.Value);

                    match.MatchId = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Add Match error: {ex.Message}");
                throw;
            }
        }

        public Match GetById(int matchId)
        {
            const string query = @"
                SELECT m.matchId, m.hostId, m.wordId, m.[status], m.maxPlayers, m.isLocalNetwork, m.createdAt, m.finishedAt
                FROM [Match] m
                WHERE m.matchId = @matchId";

            SqlConnection conn = _context.GetOpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
                {
                    cmd.Parameters.AddWithValue("@matchId", matchId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;

                        return new Match
                        {
                            MatchId = (int)reader["matchId"],
                            HostId = (int)reader["hostId"],
                            WordId = reader["wordId"] != DBNull.Value ? (int)reader["wordId"] : 0,
                            Status = (string)reader["status"],
                            maxPlayers = (int)reader["maxPlayers"],
                            IsLocalNetwork = (bool)reader["isLocalNetwork"],
                            createdAt = (DateTime)reader["createdAt"],
                            finishedAt = reader["finishedAt"] != DBNull.Value ? (DateTime)reader["finishedAt"] : DateTime.MinValue
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetById error: {ex.Message}");
                return null;
            }
        }

        public void UpdateStatus(int matchId, string newStatus)
        {
            var connection = _context.GetOpenConnection();
            var transaction = _context.CurrentTransaction;

            string query = "UPDATE [Match] SET [status] = @status WHERE matchId = @matchId";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@status", newStatus);
                command.Parameters.AddWithValue("@matchId", matchId);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Match entity) => throw new NotImplementedException();
        public void Delete(int id) => throw new NotImplementedException();
        public Match Get(int id) => throw new NotImplementedException();
        public IEnumerable<Match> GetAll() => throw new NotImplementedException();
    }
}

