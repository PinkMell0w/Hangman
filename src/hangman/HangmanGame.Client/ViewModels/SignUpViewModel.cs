using HangmanGame.Client.Commands;
using HangmanGame.Client.HangmanGameService;
using HangmanGame.Client.Helpers;
using HangmanGame.Core.Core.DTOs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly AuthServiceClient _authService;

        private string _fullName;
        private DateTime? _dateOfBirth;
        private string _phoneNumber;
        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _errorMessage;
        private bool _isLoading;

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set { _dateOfBirth = value; OnPropertyChanged(); }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
        }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public bool IsNotLoading => !_isLoading;

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToSignInCommand { get; }

        public SignUpViewModel()
        {
            _authService = new AuthServiceClient();
            RegisterCommand = new RelayCommand(_ => ExecuteRegister(), _ => CanRegister());
            NavigateToSignInCommand = new RelayCommand(_ => NavigateToSignIn());
        }

        private bool CanRegister() =>
            !string.IsNullOrWhiteSpace(FullName) &&
            !string.IsNullOrWhiteSpace(PhoneNumber) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ConfirmPassword) &&
            DateOfBirth.HasValue &&
            !_isLoading;
        private async void ExecuteRegister()
        {
            if (!DateOfBirth.HasValue)
            {
                ErrorMessage = "select a date";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            if (Password.Length < 8)
            {
                ErrorMessage = "Password must be at least 8 characters.";
                return;
            }

            if (DateOfBirth.Value >= DateTime.Today)
            {
                ErrorMessage = "seriously...";
                return;
            }

            _isLoading = true;
            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(IsNotLoading));

            var request = new RegisterRequestDto
            {
                FullName = FullName,
                DateOfBirth = DateOfBirth.Value.ToString("dd-MM-yyyy"),
                PhoneNumber = PhoneNumber,
                Username = Username,
                Email = Email,
                Password = Password
            };

            var response = await Task.Run(
                () => _authService.Register(request)
            );

            _isLoading = false;
            OnPropertyChanged(nameof(IsNotLoading));

            if (response.Success)
            {
                NavigateToSignIn();
            }
            else
            {
                MessageBox.Show(response.Message);
                ErrorMessage = response.Message;
            }
        }

        private void NavigateToSignIn()
        {
            NavigationManager.Instance.Navigate(new Views.SignInPage());
        }
    }
}
