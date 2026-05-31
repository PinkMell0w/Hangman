// Auto-generated proxy for IUserService
namespace HangmanGame.Client.HangmanGameService {
    using System.ServiceModel;

    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="HangmanGameService.IUserService")]
    public interface IUserService {
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/LoadProfile", ReplyAction="http://tempuri.org/IUserService/LoadProfileResponse")]
        HangmanGame.Core.Core.DTOs.ProfileResponseDto LoadProfile(HangmanGame.Core.Core.DTOs.LoadProfileRequestDto request);
    }

    public partial class UserServiceClient : System.ServiceModel.ClientBase<HangmanGame.Client.HangmanGameService.IUserService>, HangmanGame.Client.HangmanGameService.IUserService {
        public UserServiceClient() { }
        public UserServiceClient(string endpointConfigurationName) : base(endpointConfigurationName) { }
        public HangmanGame.Core.Core.DTOs.ProfileResponseDto LoadProfile(HangmanGame.Core.Core.DTOs.LoadProfileRequestDto request) {
            return base.Channel.LoadProfile(request);
        }
    }
}
