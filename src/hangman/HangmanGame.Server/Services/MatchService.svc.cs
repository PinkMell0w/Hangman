using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
using HangmanGame.Core.Core.Interfaces.Services;
using HangmanGame.Data.Context;
using HangmanGame.Data.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HangmanGame.Server.Services
{
    public class MatchService : IMatchService
    {
        private static ConcurrentDictionary<int, LiveGameRuntimeState> ActiveGames = new ConcurrentDictionary<int, LiveGameRuntimeState>();

        public GetAvailableMatchesResponseDto GetAvailableMatches(GetAvailableMatchesRequestDto request)
        {
            if (request == null || request.UserId <= 0)
                return Fail("Invalid request");

            var context = new DatabaseContext();
            var matchRepo = new MatchRepository(context);
            var userRepo = new UserRepository(context);

            try
            {
                var availableMatches = matchRepo.GetAvailableMatches();

                if (availableMatches == null || availableMatches.Count == 0)
                    return Fail("No available matches at the moment");

                var matchDtos = new List<MatchDto>();

                foreach (var match in availableMatches)
                {
                    var host = userRepo.GetById(match.HostId);
                    string hostName = host?.Username ?? "Unknown";

                    var playerInMatchRepo = new PlayerInMatchRepository(context);
                    var currentPlayerCount = playerInMatchRepo.GetPlayerCountByMatch(match.MatchId);

                    matchDtos.Add(new MatchDto
                    {
                        MatchId = match.MatchId,
                        HostId = match.HostId,
                        HostName = hostName,
                        Status = match.Status,
                        MaxPlayers = match.maxPlayers,
                        CurrentPlayers = currentPlayerCount,
                        IsLocalNetwork = match.IsLocalNetwork,
                        CreatedAt = match.createdAt
                    });
                }

                return new GetAvailableMatchesResponseDto
                {
                    Success = true,
                    Message = string.Empty,
                    AvailableMatches = matchDtos
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAvailableMatches error: {ex.Message}");
                return Fail("An error occurred while retrieving available matches");
            }
            finally
            {
                context.Dispose();
            }
        }

        private static GetAvailableMatchesResponseDto Fail(string message) =>
            new GetAvailableMatchesResponseDto
            {
                Success = false,
                Message = message,
                AvailableMatches = new List<MatchDto>()
            };

        public GetMatchDetailsResponseDto GetMatchById(GetMatchDetailsRequestDto request)
        {
            if (request == null || request.MatchId <= 0)
                return FailDetails("Invalid match ID");

            var context = new DatabaseContext();
            var matchRepo = new MatchRepository(context);
            var userRepo = new UserRepository(context);
            var playerInMatchRepo = new PlayerInMatchRepository(context);
            var wordRepo = new WordRepository(context);

            try
            {
                var match = matchRepo.GetById(request.MatchId);

                if (match == null)
                    return FailDetails("Match not found");

                var host = userRepo.GetById(match.HostId);
                string hostName = host?.Username ?? "Unknown";
                var currentPlayerCount = playerInMatchRepo.GetPlayerCountByMatch(match.MatchId);
                string opponentName = null;
                var wordRow = wordRepo.GetById(match.WordId);
                string targetWord = wordRow != null ? wordRow.Name : "";

                if (currentPlayerCount > 1)
                {
                    int guesserId = playerInMatchRepo.GetGuesserId(match.MatchId);
                    if (guesserId > 0)
                    {
                        var opponent = userRepo.GetById(guesserId);
                        opponentName = opponent?.Username;
                    }
                }

                return new GetMatchDetailsResponseDto
                {
                    Success = true,
                    Message = string.Empty,
                    Match = new MatchDto
                    {
                        MatchId = match.MatchId,
                        HostId = match.HostId,
                        WordName = targetWord,
                        HostName = hostName,
                        OpponentName = opponentName,
                        Status = match.Status,
                        MaxPlayers = match.maxPlayers,
                        CurrentPlayers = currentPlayerCount,
                        IsLocalNetwork = match.IsLocalNetwork,
                        CreatedAt = match.createdAt
                    }
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetMatchById error: {ex.Message}");
                return FailDetails("An error occurred while retrieving match details");
            }
            finally
            {
                context.Dispose();
            }
        }

        public CreateMatchResponseDto CreateMatch(CreateMatchRequestDto request)
        {
            var context = new DatabaseContext();
            var matchRepo = new MatchRepository(context);
            var playerRepo = new PlayerInMatchRepository(context);

            try
            {
                context.BeginTransaction();

                var match = new Match
                {
                    HostId = request.HostId,
                    WordId = request.WordId,
                    Status = "WAITING",
                    maxPlayers = 2,
                    IsLocalNetwork = false,
                    createdAt = DateTime.Now
                };

                matchRepo.Add(match);

                playerRepo.AddPlayerToMatch(
                    match.MatchId,
                    request.HostId,
                    "HOST"
                );

                context.Commit();

                return new CreateMatchResponseDto
                {
                    Success = true,
                    MatchId = match.MatchId
                };
            }
            catch
            {
                context.Rollback();
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public JoinMatchResponseDto JoinMatch(JoinMatchRequestDto request)
        {
            var context = new DatabaseContext();
            var playerRepo = new PlayerInMatchRepository(context);
            var matchRepo = new MatchRepository(context);

            if (playerRepo.GetPlayerCountByMatch(request.MatchId) >= 2)
            {
                return new JoinMatchResponseDto
                {
                    Success = false,
                    Message = "Match is full."
                };
            }

            try
            {
                context.BeginTransaction();

                playerRepo.AddPlayerToMatch(
                    request.MatchId,
                    request.UserId,
                    "GUESSER");

                matchRepo.UpdateStatus(request.MatchId, "READY");

                context.Commit();

                return new JoinMatchResponseDto
                {
                    Success = true,
                    Message = "Joined match successfully."
                };
            }
            catch (Exception ex)
            {
                context.Rollback();

                return new JoinMatchResponseDto
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            finally
            {
                context.Dispose();
            }
        }

        public StartMatchResponseDto StartMatch(StartMatchRequestDto request)
        {
            if (request == null || request.MatchId <= 0)
            {
                return new StartMatchResponseDto { Success = false, Message = "Invalid match start request." };
            }

            var context = new DatabaseContext();
            var matchRepo = new MatchRepository(context);
            var wordRepo = new WordRepository(context);
            var playerRepo = new PlayerInMatchRepository(context);

            try
            {
                context.BeginTransaction();

                matchRepo.UpdateStatus(request.MatchId, "IN_PROGRESS");
                var match = matchRepo.GetById(request.MatchId);
                var wordRow = wordRepo.GetById(match.WordId);
                string secretWord = wordRow != null ? wordRow.Name.ToUpper() : "";

                int guesserUserId = playerRepo.GetGuesserId(request.MatchId);

                string blankSpaces = string.Join(" ", Enumerable.Repeat("_", secretWord.Length));

                var freshGameState = new LiveGameRuntimeState
                {
                    MaskedWord = blankSpaces,
                    HangmanStage = 0,
                    CurrentTurn = "GUESSER",
                    MatchStatus = "PLAYING",
                    LastGuessedLetter = ' '
                };

                ActiveGames.AddOrUpdate(request.MatchId, freshGameState, (id, oldState) => freshGameState);

                context.Commit();

                return new StartMatchResponseDto
                {
                    Success = true,
                    Message = "Match started successfully."
                };
            }
            catch (Exception ex)
            {
                context.Rollback();

                return new StartMatchResponseDto
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            finally
            {
                context.Dispose();
            }
        }

        public CancelMatchResponseDto CancelMatch(CancelMatchRequestDto request)
        {
            if (request == null || request.MatchId <= 0)
            {
                return new CancelMatchResponseDto { Success = false, Message = "Invalid match cancellation request." };
            }

            var context = new DatabaseContext();
            var matchRepo = new MatchRepository(context);

            try
            {
                context.BeginTransaction();

                matchRepo.UpdateStatus(request.MatchId, "CANCELED");

                context.Commit();

                return new CancelMatchResponseDto
                {
                    Success = true,
                    Message = "Match canceled successfully."
                };
            }
            catch (Exception ex)
            {
                context.Rollback();

                return new CancelMatchResponseDto
                {
                    Success = false,
                    Message = $"Server error: {ex.Message}"
                };
            }
            finally
            {
                context.Dispose();
            }
        }

        public LiveGameRuntimeState GetLiveGameLoopState(int matchId)
        {
            if (ActiveGames.TryGetValue(matchId, out var state))
            {
                return state;
            }

            return new LiveGameRuntimeState
            {
                MaskedWord = "",
                HangmanStage = 0,
                CurrentTurn = "GUESSER",
                MatchStatus = "WAITING"
            };
        }

        public void SubmitGuesserLetter(int matchId, char letter)
        {
            if (ActiveGames.TryGetValue(matchId, out var state))
            {
                state.LastGuessedLetter = char.ToUpper(letter);
                state.CurrentTurn = "HOST_VALIDATION";
            }
        }

        public void SubmitHostValidation(int matchId, bool isCorrect, string manualUpdatedMaskedWord)
        {
            if (ActiveGames.TryGetValue(matchId, out var state))
            {
                if (isCorrect)
                {
                    state.MaskedWord = manualUpdatedMaskedWord;

                    if (!manualUpdatedMaskedWord.Contains("_"))
                    {
                        state.MatchStatus = "WON";
                    }
                }
                else
                {
                    state.HangmanStage++;

                    if (state.HangmanStage >= 6)
                    {
                        state.MatchStatus = "LOST";
                    }
                }

                if (state.MatchStatus == "PLAYING")
                {
                    state.CurrentTurn = "GUESSER";
                }
            }
        }

        private static GetMatchDetailsResponseDto FailDetails(string message) =>
            new GetMatchDetailsResponseDto
            {
                Success = false,
                Message = message,
                Match = null
            };
    }
}
