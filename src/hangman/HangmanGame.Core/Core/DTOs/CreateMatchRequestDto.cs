using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class CreateMatchRequestDto
    {
        [DataMember]
        public int HostId { get; set; }

        [DataMember]
        public int WordId { get; set; }

        [DataMember]
        public bool IsLocalNetwork { get; set; }
    }

    [DataContract]
    public class CreateMatchResponseDto
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int MatchId { get; set; }
    }
}
