using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.Interfaces.Repositories;

namespace HangmanGame.Core.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
        User GetByUsername(string username);
        bool ExistsByEmail(string email);
        bool ExistsByUsername(string username);
    }
}
