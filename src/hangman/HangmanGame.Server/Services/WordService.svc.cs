using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
using HangmanGame.Core.Core.Interfaces.Services;
using HangmanGame.Data.Context;
using HangmanGame.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HangmanGame.Server.Services
{
    public class WordService : IWordService
    {
        public WordResponseDto GetWords(WordRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Language))
                return Fail("lang required");

            string lang = request.Language.Trim().ToLower();
            string category = request.Category?.Trim();
            string difficulty = request.Difficulty?.Trim().ToUpper();

            var validLanguages = new[] { "en", "es" };
            var validDifficulties = new[] { "EASY", "MEDIUM", "HARD" };

            if (!Array.Exists(validLanguages, l => l == lang))
                return Fail($"Unsupported language '{lang}'. Supported: en, es.");

            if (!string.IsNullOrEmpty(difficulty) &&
                !Array.Exists(validDifficulties, d => d == difficulty))
                return Fail($"Invalid difficulty '{difficulty}'. Use EASY, MEDIUM or HARD.");

            var context = new DatabaseContext();
            var wordRepo = new WordRepository(context);

            try
            {
                IEnumerable<Word> words;

                if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(difficulty))
                {
                    words = wordRepo
                        .GetByLanguageAndCategory(lang, category)
                        .Where(w => w.Difficulty == difficulty);
                }
                else if (!string.IsNullOrEmpty(category))
                {
                    words = wordRepo.GetByLanguageAndCategory(lang, category);
                }
                else if (!string.IsNullOrEmpty(difficulty))
                {
                    words = wordRepo.GetByLanguageAndDifficulty(lang, difficulty);
                }
                else
                {
                    words = wordRepo.GetByLanguage(lang);
                }

                var wordList = words.ToList();

                if (!wordList.Any())
                    return Fail("No words found for the given filters.");

                return new WordResponseDto
                {
                    Success = true,
                    Message = string.Empty,
                    Words = wordList.Select(w => new Word
                    {
                        WordId = w.WordId,
                        Name = w.Name,
                        Category = w.Category,
                        Difficulty = w.Difficulty,
                        Language = w.Language
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetWords error: {ex.Message}");
                return Fail("An error occurred while retrieving words.");
            }
            finally
            {
                context.Dispose();
            }
        }
        private static WordResponseDto Fail(string message) =>
        new WordResponseDto { Success = false, Message = message, Words = new List<Word>() };
    }
}
