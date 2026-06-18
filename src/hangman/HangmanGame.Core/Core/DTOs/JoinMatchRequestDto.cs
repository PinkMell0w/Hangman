using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class JoinMatchRequestDto
    {
        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public int UserId { get; set; }
    }

    [DataContract]
    public class JoinMatchResponseDto
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
