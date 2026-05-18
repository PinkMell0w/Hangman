using HangmanGame.Core.Core.Domain;

namespace HangmanGame.Core.Core.Interfaces.Repositories
{
    public interface IPlayerProfileRespository : IRepository<PlayerProfile>
    {
        PlayerProfile GetByUserId(int userId);
    }
}
