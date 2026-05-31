using HangmanGame.Core.Core.DTOs;
using HangmanGame.Core.Core.Interfaces.Services;
using HangmanGame.Data.Context;
using HangmanGame.Data.Repositories;
using System;
using System.Diagnostics;

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
    }
}
