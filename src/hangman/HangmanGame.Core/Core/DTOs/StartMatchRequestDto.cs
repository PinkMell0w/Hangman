using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class StartMatchRequestDto
    {
        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public int HostId { get; set; }
    }

    [DataContract]
    public class StartMatchResponseDto
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
