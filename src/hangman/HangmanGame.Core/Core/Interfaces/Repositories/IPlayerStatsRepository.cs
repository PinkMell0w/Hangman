using HangmanGame.Core.Core.Domain;

namespace HangmanGame.Core.Core.Interfaces.Repositories
{
    public interface IPlayerStatsRepository : IRepository<PlayerStats>
    {
        PlayerStats GetByUserId(int userId);

        void AddPoints(int userId, int points);

        void DeductPoints(int userId, int points);
    }
}
