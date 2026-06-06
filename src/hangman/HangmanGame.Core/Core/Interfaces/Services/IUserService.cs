using System.ServiceModel;
using HangmanGame.Core.Core.DTOs;

namespace HangmanGame.Core.Core.Interfaces.Services
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        ProfileResponseDto LoadProfile(LoadProfileRequestDto request);
        [OperationContract]
        UserCardResponseDto SearchUsers(UserCardRequestDto request);
    }
}
