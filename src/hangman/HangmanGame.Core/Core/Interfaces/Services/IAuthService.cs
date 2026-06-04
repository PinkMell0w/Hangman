using HangmanGame.Core.Core.DTOs;
using System.ServiceModel;

namespace HangmanGame.Core.Core.Interfaces.Services
{
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        RegisterResponseDto Register(RegisterRequestDto request);

    }
}
