using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
using HangmanGame.Core.Core.Enums;
using HangmanGame.Core.Core.Interfaces.Services;
using HangmanGame.Data.Context;
using HangmanGame.Data.Helpers;
using HangmanGame.Data.Repositories;
using HangmanGame.Server.Services.Helpers;
using System;
using System.Data.SqlClient;

namespace HangmanGame.Server.Services
{
    public class AuthService : IAuthService
    {
        public RegisterResponseDto Register(RegisterRequestDto request)
        {
            if (FieldValidator.IsEmptyOrWhitespace(request.FullName) ||
                FieldValidator.IsEmptyOrWhitespace(request.DateOfBirth) ||
                FieldValidator.IsEmptyOrWhitespace(request.PhoneNumber) ||
                FieldValidator.IsEmptyOrWhitespace(request.Username) ||
                FieldValidator.IsEmptyOrWhitespace(request.Email) ||
                FieldValidator.IsEmptyOrWhitespace(request.Password))
            {
                return Fail("All fields are required.");
            }

            if (FieldValidator.IsValidPwd(request.Password) == false)
            {
                return Fail("Password must be at least 8 characters and contain a mix of letters and numbers.");
            }

            var context = new DatabaseContext();
            var userRepo = new UserRepository(context);
            var profileRepo = new PlayerProfileRepository(context);
            var statsRepo = new PlayerStatsRepository(context);

            try //delete messages until end
            {
                if (userRepo.ExistsByEmail(request.Email))
                    return Fail("Email is already registered.");

                if (userRepo.ExistsByUsername(request.Username))
                    return Fail("Username is already taken.");

                context.BeginTransaction();

                string salt = PasswordHelper.GenerateSalt();
                string passwordHash = PasswordHelper.HashPassword(request.Password, salt);

                var user = new User
                {
                    FullName = request.FullName.Trim(),
                    RoleId = Roles.Player,
                    DateOfBirth = DateTime.Parse(request.DateOfBirth.Trim()),
                    PhoneNumber = request.PhoneNumber.Trim(),
                    Username = request.Username.Trim(),
                    Email = request.Email.Trim().ToLowerInvariant(),
                    PwdHash = passwordHash,
                    Salt = salt,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                userRepo.Add(user);

                profileRepo.Add(new PlayerProfile
                {
                    UserId = user.UserId,
                    Bio = "u should edit this later.",
                    Theme = "default"
                });

                statsRepo.Add(new PlayerStats
                {
                    UserId = user.UserId
                });

                context.Commit();

                return new RegisterResponseDto
                {
                    Success = true,
                    Message = "Account created successfully.",
                    UserId = user.UserId
                };
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error {ex.Number}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Procedure: {ex.Procedure}");
                System.Diagnostics.Debug.WriteLine($"Line: {ex.LineNumber}");

                context.Rollback();
                return Fail($"SQL Error {ex.Number}: {ex.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner: {ex.InnerException?.Message}");

                context.Rollback();
                return Fail($"Error: {ex.Message}");
            }
            finally
            {
                context.Dispose();
            }
        }

        public SignInResponseDto SignIn(SignInRequestDto request)
        {
            if (request == null || (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Username)) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new SignInResponseDto { Success = false, Message = "Invalid credentials." };
            }

            var context = new DatabaseContext();
            var userRepo = new UserRepository(context);
            var sessionRepo = new UserSessionRepository(context);

            try
            {
                // Resolve user by username or email
                User user = null;
                if (!string.IsNullOrWhiteSpace(request.Email))
                    user = userRepo.GetByEmail(request.Email.Trim().ToLowerInvariant());
                else if (!string.IsNullOrWhiteSpace(request.Username))
                    user = userRepo.GetByUsername(request.Username.Trim());

                if (user == null)
                    return new SignInResponseDto { Success = false, Message = "Invalid username/email or password." };

                if (!PasswordHelper.VerifyPassword(request.Password, user.Salt, user.PwdHash))
                    return new SignInResponseDto { Success = false, Message = "Invalid username/email or password." };

                string token = Guid.NewGuid().ToString("N");

                try
                {
                    context.BeginTransaction();
                    sessionRepo.EndActiveSessionsForUser(user.UserId);
                    sessionRepo.CreateSession(user.UserId, token);
                    context.Commit();
                }
                catch (SqlException ex)
                {
                    context.Rollback();
                    System.Diagnostics.Debug.WriteLine($"SignIn session DB error: {ex.Message}");
                }

                return new SignInResponseDto { Success = true, Message = "Signed in.", UserId = user.UserId, Token = token };
            }
            catch (Exception ex)
            {
                context.Rollback();
                System.Diagnostics.Debug.WriteLine($"SignIn error: {ex.Message}");
                return new SignInResponseDto { Success = false, Message = "An error occurred while signing in." };
            }
            finally
            {
                context.Dispose();
            }
        }

        private static RegisterResponseDto Fail(string message) =>
            new RegisterResponseDto { Success = false, Message = message };
    }
}
