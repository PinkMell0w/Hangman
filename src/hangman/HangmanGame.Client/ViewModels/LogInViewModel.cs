using HangmanGame.Client.Commands;
using HangmanGame.Client.HangmanGameService;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.Views;
using HangmanGame.Client.Views.SignUp;
using HangmanGame.Core.Core.DTOs;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        private readonly AuthServiceClient _authService;

        private string _credential;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public string Credential
        {
            get => _credential;
            set { _credential = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
        }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public bool IsNotLoading => !_isLoading;

        // public ICommand LogInCommand { get; }
        public ICommand NavigateToLobbyCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public LogInViewModel()
        {
            // _authService = new AuthServiceClient();
            // LogInCommand = new RelayCommand(_ => ExecuteLogIn(), _ => CanLogIn());
            NavigateToLobbyCommand = new RelayCommand(_ => NavigateToLobby());
            NavigateToSignUpCommand = new RelayCommand(_ => NavigateToSignUp());
            ForgotPasswordCommand = new RelayCommand(_ => MessageBox.Show("Funcionalidad no implementada aún."));
        }

        /* private bool CanLogIn() =>
            !string.IsNullOrWhiteSpace(Credential) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !_isLoading;

        private async void ExecuteLogIn()
        {
            _isLoading = true;
            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(IsNotLoading));

            var request = new SignInRequestDto
            {
                Password = Password
            };

            if (Credential.Contains("@"))
                request.Email = Credential.Trim();
            else
                request.Username = Credential.Trim();

            var response = await Task.Run(() => _authService.SignIn(request));

            _isLoading = false;
            OnPropertyChanged(nameof(IsNotLoading));

            if (response.Success)
            {
                SessionManager.Instance.SetSession(response.UserId, response.Username, response.Token);
                NavigateToLobby();
            }
            else
            {
                MessageBox.Show(response.Message);
            }
        }
        private void NavigateToLobby()
        {
            NavigationManager.Instance.Navigate(new LobbyPage());
        }

        private void NavigateToSignUp()
        {
            NavigationManager.Instance.Navigate(new SignUpPage());
        }
    }
}
