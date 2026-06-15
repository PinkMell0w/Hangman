using HangmanGame.Core.Core.Domain;
using HangmanGame.Data.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HangmanGame.Data.Repositories
{
    public class WordRepository
    {
        private readonly DatabaseContext _context;

        public WordRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Word> GetByCategory(string category)
        {
            const string query = @"
                    SELECT * From Word
                    WHERE  category = @category
                    ORDER BY difficulty";

            return ExecuteWordQuery(query,
                new SqlParameter("@category", category));
        }

        public IEnumerable<Word> GetByLanguage(string language)
        {
            const string query = @"
                    SELECT * FROM Word 
                    WHERE language = @language 
                    ORDER BY difficulty, category";

            return ExecuteWordQuery(query,
                new SqlParameter("@language", language));
        }

        public IEnumerable<Word> GetByLanguageAndCategory(string lang, string category)
        {
            const string query = @"
                    SELECT * FROM Word 
                    WHERE language = @lang
                    AND category = @category
                    ORDER BY difficulty";

            return ExecuteWordQuery(query,
                new SqlParameter("@language", lang),
                new SqlParameter("@category", category));
        }

        public IEnumerable<string> GetCategories(string lang)
        {
            const string query = @"
                    SELECT * From Word
                    WHERE  language = @lang
                    ORDER BY category";

            return (IEnumerable<string>)ExecuteWordQuery(query,
                new SqlParameter("@language", lang));
        }

        public IEnumerable<Word> GetByLanguageAndDifficulty(string language, string difficulty)
        {
            const string query = @"
                    SELECT * FROM Word 
                    WHERE language   = @language 
                      AND difficulty  = @difficulty 
                    ORDER BY category";

            return ExecuteWordQuery(query,
                new SqlParameter("@language", language),
                new SqlParameter("@difficulty", difficulty));
        }

        //reuse
        private IEnumerable<Word> ExecuteWordQuery(string query, params SqlParameter[] parameters)
        {
            var words = new List<Word>();
            var conn = _context.GetOpenConnection();

            using (var cmd = new SqlCommand(query, conn, _context.CurrentTransaction))
            {
                cmd.Parameters.AddRange(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        words.Add(MapReader(reader));
                }
            }

            return words;
        }

        private static Word MapReader(SqlDataReader reader)
        {
            return new Word
            {
                WordId = (int)reader["wordId"],
                Name = (string)reader["word"],
                Category = (string)reader["category"],
                Difficulty = (string)reader["difficulty"],
                Language = (string)reader["language"],
                AddedBy = (int)reader["addedBy"],
                CreatedAt = (DateTime)reader["createdAt"]
            };
        }
    }
}
