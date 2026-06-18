using System.Runtime.Serialization;
using HangmanGame.Core.Core.Domain;
using System.Collections.Generic;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class GetAvailableMatchesRequestDto
    {
        [DataMember]
        public int UserId { get; set; }
        public string Language { get; set; }
    }

    [DataContract]
    public class GetAvailableMatchesResponseDto
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public List<MatchDto> AvailableMatches { get; set; }
    }

    [DataContract]
    public class MatchDto
    {
        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public int HostId { get; set; }

        [DataMember]
        public string WordName { get; set; }

        [DataMember]
        public string HostName { get; set; }

        [DataMember]
        public string OpponentName { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int MaxPlayers { get; set; }

        [DataMember]
        public int CurrentPlayers { get; set; }

        [DataMember]
        public bool IsLocalNetwork { get; set; }

        [DataMember]
        public System.DateTime CreatedAt { get; set; }
    }
}
