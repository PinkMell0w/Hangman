using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.Domain
{
    public class PlayerStats
    {
        public int StatsId { get; set; }
        public int UserId { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int TotalScore { get; set; }
        public double WinRate { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
