using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.Domain
{
    public class Match
    {
        public int MatchId { get; set; }
        public int HostId { get; set; }
        public int WordId { get; set; }
        public string Status { get; set; }
        public int maxPlayers { get; set; }
        public bool IsLocalNetwork { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime finishedAt { get; set; }
    }
}
