using System.ServiceModel;
using HangmanGame.Core.Core.DTOs;

namespace HangmanGame.Core.Core.Interfaces.Services
{
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
        CancelMatchResponseDto CancelMatch(CancelMatchRequestDto request);
    }
}
