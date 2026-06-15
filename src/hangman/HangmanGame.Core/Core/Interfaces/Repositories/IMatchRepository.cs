using System.Collections.Generic;
using HangmanGame.Core.Core.Domain;

namespace HangmanGame.Core.Core.Interfaces.Repositories
{
    public interface IMatchRepository : IRepository<Match>
    {
        List<Match> GetAvailableMatches();
        Match GetById(int matchId);
    }
}
