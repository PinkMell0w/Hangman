using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class WordRequestDto
    {
        [DataMember] public string Language { get; set; }
        [DataMember] public string Category { get; set; }
        [DataMember] public string Difficulty { get; set; }
    }
}
