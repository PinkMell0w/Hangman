using HangmanGame.Core.Core.Domain;

namespace HangmanGame.Core.Core.Interfaces.Repositories
{
    public interface IWordRepository : IRepository<Word>
    {
        Word GetWordsByCategories(string category);
        Word GetWordsByLanguage(string language);
    }
}
