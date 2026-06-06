using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

namespace HangmanGame.Core.Core.DTOs
{
    [DataContract]
    public class GetGameSessionRequestDto
    {
        [DataMember]
        public int SessionId { get; set; }
    }

    [DataContract]
    public class GetGameSessionResponseDto
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public GameSessionDto GameSession { get; set; }
    }

    [DataContract]
    public class GameSessionDto
    {
        [DataMember]
        public int SessionId { get; set; }

        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public int WordId { get; set; }

        [DataMember]
        public string Word { get; set; }

        [DataMember]
        public string HiddenWord { get; set; }

        [DataMember]
        public int WrongAttempts { get; set; }

        [DataMember]
        public int MaxAttempts { get; set; }

        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public int? WinnerId { get; set; }

        [DataMember]
        public List<char> GuessedLetters { get; set; }

        [DataMember]
        public DateTime StartedAt { get; set; }

        [DataMember]
        public DateTime? FinishedAt { get; set; }
    }
}
