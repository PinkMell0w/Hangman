using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.Domain
{
    public class PlayerProfile
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string AvatarUrl { get; set; } 
        public string Bio { get; set; }
        public string Theme { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
