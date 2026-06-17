using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.UserServiceReference;
using HangmanGame.Client.Views;
using HangmanGame.Core.Core.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private readonly UserServiceClient _userService;

        private string _username;
        private string _fullName;
        private int _gamesPlayed;
        private int _gamesWon;
        private double _winRate;
        private string _theme;
        private string _bio;
        private string _errorMessage;
        private bool _isLoading;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }
        public int GamesPlayed
        {
            get => _gamesPlayed;
            set { _gamesPlayed = value; OnPropertyChanged(); }
        }
        public int GamesWon
        {
            get => _gamesWon;
            set { _gamesWon = value; OnPropertyChanged(); }
        }
        public double WinRate
        {
            get => _winRate;
            set { _winRate = value; OnPropertyChanged(); }
        }
        public string Theme
        {
            get => _theme;
            set { _theme = value; OnPropertyChanged(); }
        }
        public string Bio
        {
            get => _bio;
            set { _bio = value; OnPropertyChanged(); }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
        }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public bool IsNotLoading => !_isLoading;

        public ICommand LoadProfileCommand { get; }
        public ICommand NavigateToLobbyCommand { get; }

        public ProfilePageViewModel()
        {
            _userService = new UserServiceClient();
            LoadProfileCommand = new RelayCommand(_ => LoadProfile());
            NavigateToLobbyCommand = new RelayCommand(_ => NavigateToLobby());
            LoadProfile();
        }

        private async void LoadProfile()
        {
            if (!SessionManager.Instance.IsSignedIn) return;

            _isLoading = true;
            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(IsNotLoading));

            var request = new LoadProfileRequestDto { UserId = SessionManager.Instance.CurrentUserId };
            var response = await Task.Run(() => _userService.LoadProfile(request));

            _isLoading = false;
            OnPropertyChanged(nameof(IsNotLoading));

            if (response.Success)
            {
                Username = response.Username;
                FullName = response.FullName;
                GamesPlayed = response.GamesPlayed;
                GamesWon = response.GamesWon;
                WinRate = response.WinRate;
                Theme = response.Theme;
                Bio = response.Bio;
            } else
            {
                MessageBox.Show(response.Message);
            }
        }

        private void NavigateToLobby()
        {
            NavigationManager.Instance.Navigate(new LobbyPage());
        }
    }
}