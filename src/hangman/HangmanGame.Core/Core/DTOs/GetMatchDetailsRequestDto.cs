using System.Runtime.Serialization;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class GetMatchDetailsRequestDto
    {
        [DataMember]
        public int MatchId { get; set; }
    }

    [DataContract]
    public class GetMatchDetailsResponseDto
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public MatchDto Match { get; set; }
    }
}
