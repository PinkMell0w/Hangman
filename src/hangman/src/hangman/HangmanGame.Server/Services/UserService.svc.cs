using HangmanGame.Core.Core.DTOs;
using HangmanGame.Core.Core.Interfaces.Services;
using HangmanGame.Data.Context;
using HangmanGame.Data.Repositories;
using System;

namespace HangmanGame.Server.Services
{
    public class UserService : IUserService
    {
        public ProfileResponseDto LoadProfile(LoadProfileRequestDto request)
        {
            if (request == null || request.UserId <= 0)
                return new ProfileResponseDto();

            var context = new DatabaseContext();
            var userRepo = new UserRepository(context);
            var profileRepo = new PlayerProfileRepository(context);
            var statsRepo = new PlayerStatsRepository(context);

            try
            {
                var user = userRepo.GetById(request.UserId);
                var profile = profileRepo.GetByUserId(request.UserId);
                var stats = statsRepo.GetByUserId(request.UserId);

                if (user == null) return new ProfileResponseDto();

                return new ProfileResponseDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = user.FullName,
                    GamesPlayed = stats?.GamesPlayed ?? 0,
                    GamesWon = stats?.GamesWon ?? 0,
                    TotalScore = stats?.TotalScore ?? 0,
                    WinRate = stats?.WinRate ?? 0.0,
                    AvatarUrl = profile?.AvatarUrl, // may be null in client-only config
                    Theme = profile?.Theme,
                    Bio = profile?.bio
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProfile error: {ex.Message}");
                return new ProfileResponseDto();
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}