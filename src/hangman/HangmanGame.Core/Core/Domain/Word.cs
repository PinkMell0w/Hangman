using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.Domain
{
    public class Word
    {
        public int WordId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Language { get; set; }
        public int AddedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
