using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
using HangmanGame.Core.Core.Interfaces.Services;
using HangmanGame.Data.Context;
using HangmanGame.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HangmanGame.Server.Services
{
    public class MatchService : IMatchService
    {
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

            try
            {
                var match = matchRepo.GetById(request.MatchId);

                if (match == null)
                    return FailDetails("Match not found");

                var host = userRepo.GetById(match.HostId);
                string hostName = host?.Username ?? "Unknown";
                var currentPlayerCount = playerInMatchRepo.GetPlayerCountByMatch(match.MatchId);

                return new GetMatchDetailsResponseDto
                {
                    Success = true,
                    Message = string.Empty,
                    Match = new MatchDto
                    {
                        MatchId = match.MatchId,
                        HostId = match.HostId,
                        HostName = hostName,
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
                    Status = "WAITING",
                    maxPlayers = 2,
                    IsLocalNetwork = false,
                    createdAt = DateTime.UtcNow
                };

                matchRepo.Add(match);

                playerRepo.AddPlayerToMatch(
                    match.MatchId,
                    request.HostId
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

        private static GetMatchDetailsResponseDto FailDetails(string message) =>
            new GetMatchDetailsResponseDto
            {
                Success = false,
                Message = message,
                Match = null
            };
    }
}
