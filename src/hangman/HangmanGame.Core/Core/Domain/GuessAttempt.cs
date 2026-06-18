using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.Domain
{
    public class GuessAttempt
    {
        public int GuessAttemptId { get; set; }
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public char Letter { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AttemptedAt { get; set; }
    }
}
