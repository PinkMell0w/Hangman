using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.Domain
{
    public class GameSession
    {
        public int SessionId { get; set; }
        public int MatchId { get; set; }
        public int WordId { get; set; }
        public int? WinnerId { get; set; }
        public int WrongAttempts { get; set; }
        public int MaxAttempts { get; set; }
        public string Result { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
