using HangmanGame.Core.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class WordResponseDto
    {
        [DataMember] public bool Success { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public List<Word> Words { get; set; }
    }
}
