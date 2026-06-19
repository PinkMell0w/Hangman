using HangmanGame.Core.Core.DTOs;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HangmanGame.Core.Core.Interfaces.Services
{
    [DataContract]
    public class LiveGameRuntimeState
    {
        public int SessionId { get; set; }
        public int GuesserUserId { get; set; }

        [DataMember]
        public string MaskedWord { get; set; }

        [DataMember]
        public int HangmanStage { get; set; }

        [DataMember]
        public string CurrentTurn { get; set; } // "GUESSER" or "HOST_VALIDATION"

        [DataMember]
        public char LastGuessedLetter { get; set; }

        [DataMember]
        public string MatchStatus { get; set; } // "PLAYING", "WON", "LOST"
    }

    [ServiceContract]
    public interface IMatchService
    {
        [OperationContract]
        GetAvailableMatchesResponseDto GetAvailableMatches(GetAvailableMatchesRequestDto request);

        [OperationContract]
        GetMatchDetailsResponseDto GetMatchById(GetMatchDetailsRequestDto request);

        [OperationContract]
        CreateMatchResponseDto CreateMatch(CreateMatchRequestDto request);

        [OperationContract]
        JoinMatchResponseDto JoinMatch(JoinMatchRequestDto request);

        [OperationContract]
        StartMatchResponseDto StartMatch(StartMatchRequestDto request);

        [OperationContract]
        UpdateMatchWordResponseDto UpdateMatchWord(UpdateMatchWordRequestDto request);

        [OperationContract]
        CancelMatchResponseDto CancelMatch(CancelMatchRequestDto request);

        [OperationContract]
        LiveGameRuntimeState GetLiveGameLoopState(int matchId);

        [OperationContract]
        void SubmitGuesserLetter(int matchId, char letter);

        [OperationContract]
        void SubmitHostValidation(int matchId, bool isCorrect);
    }
}
