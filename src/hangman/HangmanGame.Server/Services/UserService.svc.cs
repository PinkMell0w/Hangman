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
    public class UserService : IUserService
    {
        public ProfileResponseDto LoadProfile(LoadProfileRequestDto request)
        {
            if (request == null || request.UserId <= 0) return new ProfileResponseDto
            {
                Success = false,
                Message = "Invalid request"
            };

            var context = new DatabaseContext();
            var userRepo = new UserRepository(context);
            var profileRepo = new PlayerProfileRepository(context);
            var statsRepo = new PlayerStatsRepository(context);

            try
            {
                var user = userRepo.GetById(request.UserId);
                var profile = profileRepo.GetByUserId(request.UserId);
                var stats = statsRepo.GetByUserId(request.UserId);

                if (user == null) return new ProfileResponseDto
                {
                    Success = false,
                    Message = "User not found"
                };

                return new ProfileResponseDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = user.FullName,
                    GamesPlayed = stats?.GamesPlayed ?? 0,
                    GamesWon = stats?.GamesWon ?? 0,
                    TotalScore = stats?.TotalScore ?? 0,
                    WinRate = stats?.WinRate ?? 0,
                    Theme = profile?.Theme ?? "default",
                    Bio = profile?.Bio ?? string.Empty,
                    Success = true,
                    Message = string.Empty
                };
            } catch (Exception ex)
            {
                Debug.WriteLine($"Loadprofile error: {ex.Message}");
                return new ProfileResponseDto
                {
                    Success = false,
                    Message = "An error occurred while loading the profile."
                };
            }
            finally
            {
                context.Dispose();
            }
        }

        public UserCardResponseDto SearchUsers(UserCardRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username))
                return FailSearch("Search term is required.");

            if (request.Username.Trim().Length < 3)
                return FailSearch("Search term must be at least 3 characters.");

            if (request.RequesterId <= 0)
                return FailSearch("Invalid session.");

            var context = new DatabaseContext();
            var userRepo = new UserRepository(context);
            var profileRepo = new PlayerProfileRepository(context);
            var statsRepo = new PlayerStatsRepository(context);

            try
            {
                var users = userRepo.SearchByUsername(
                    request.Username.Trim(), request.RequesterId);

                if (!users.Any())
                    return new UserCardResponseDto
                    {
                        Success = true,
                        Message = "No users found.",
                        Users = new List<UserCard>()
                    };

                var cards = new List<UserCard>();

                foreach (var user in users)
                {
                    var profile = profileRepo.GetByUserId(user.UserId);
                    var stats = statsRepo.GetByUserId(user.UserId);

                    cards.Add(new UserCard
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        AvatarUrl = profile?.AvatarUrl ?? "/Images/default-avatar.png",
                        TotalScore = stats?.TotalScore ?? 0,
                        WinRate = stats?.WinRate ?? 0
                    });
                }

                return new UserCardResponseDto
                {
                    Success = true,
                    Message = string.Empty,
                    Users = cards
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SearchUsers error: {ex.Message}");
                return FailSearch("An error occurred while searching for users.");
            }
            finally
            {
                context.Dispose();
            }
        }

        public UpdateProfileResponseDto UpdateProfile(UpdateProfileRequestDto request)
        {
            if (request == null || request.UserId <= 0)
            {
                return new UpdateProfileResponseDto
                {
                    Success = false,
                    Message = "Invalid update request."
                };
            }

            var context = new DatabaseContext();
            var profileRepo = new PlayerProfileRepository(context);

            try
            {
                bool isUpdated = profileRepo.UpdateProfileInfo(
                    request.UserId,
                    request.Username,
                    request.Bio
                );

                if (isUpdated)
                {
                    return new UpdateProfileResponseDto
                    {
                        Success = true,
                        Message = "Profile updated successfully."
                    };
                }
                else
                {
                    return new UpdateProfileResponseDto
                    {
                        Success = false,
                        Message = "No data has been updated."
                    };
                }
            }
            catch (Exception ex)
            {
                return new UpdateProfileResponseDto
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                };
            }
            finally
            {
                context.Dispose();
            }
        }

        public GetScoreBreakdownResponseDto GetScoreBreakdown(int userId)
        {
            if (userId <= 0)
            {
                return new GetScoreBreakdownResponseDto { Success = false, Message = "Invalid user." };
            }

            var context = new DatabaseContext();
            var gameSessionRepo = new GameSessionRepository(context);

            try
            {
                var historyLedger = gameSessionRepo.GetScoreHistoryByUserId(userId);

                return new GetScoreBreakdownResponseDto
                {
                    Success = true,
                    Ledger = historyLedger
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UserService error: {ex.Message}");
                return new GetScoreBreakdownResponseDto
                {
                    Success = false,
                    Message = "Internal error retrieving score data."
                };
            }
            finally
            {
                context.Dispose();
            }
        }

        private static UserCardResponseDto FailSearch(string message) =>
            new UserCardResponseDto
            {
                Success = false,
                Message = message,
                Users = new List<UserCard>()
            };
            }
}
