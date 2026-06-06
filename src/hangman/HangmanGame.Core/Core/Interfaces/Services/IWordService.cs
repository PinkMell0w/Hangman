using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
using System.ServiceModel;

namespace HangmanGame.Core.Core.Interfaces.Services
{
    [ServiceContract]
    public interface IWordService
    {
        [OperationContract]
        WordResponseDto GetWords(WordRequestDto request);
    }
}
