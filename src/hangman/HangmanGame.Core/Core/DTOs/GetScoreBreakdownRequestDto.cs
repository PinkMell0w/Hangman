using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class GetScoreBreakdownRequestDto
    {
        [DataMember]
        public DateTime MatchDate { get; set; }

        [DataMember]
        public string Word { get; set; }

        [DataMember]
        public string OpponentName { get; set; }

        [DataMember]
        public int PointsType { get; set; }

        [DataMember]
        public string Category { get; set; }
    }

    [DataContract]
    public class GetScoreBreakdownResponseDto
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public List<GetScoreBreakdownRequestDto> Ledger { get; set; } = new List<GetScoreBreakdownRequestDto>();
    }
}
