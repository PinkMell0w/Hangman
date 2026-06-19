using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.UserServiceReference;
using HangmanGame.Client.Views;
using HangmanGame.Core.Core.DTOs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private string _bio;
        private string _errorMessage;
        private bool _isLoading;
        private bool _isEditing;
        private bool _isShowingStats;

        private string _originalUsername;
        private string _originalBio;

        private ObservableCollection<GetScoreBreakdownRequestDto> _wonMatches = new ObservableCollection<GetScoreBreakdownRequestDto>();
        private ObservableCollection<GetScoreBreakdownRequestDto> _rivalFailedMatches = new ObservableCollection<GetScoreBreakdownRequestDto>();
        private ObservableCollection<GetScoreBreakdownRequestDto> _penalties = new ObservableCollection<GetScoreBreakdownRequestDto>();

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

        public ObservableCollection<GetScoreBreakdownRequestDto> WonMatches
        {
            get => _wonMatches;
            set { _wonMatches = value; OnPropertyChanged(); }
        }
        public ObservableCollection<GetScoreBreakdownRequestDto> RivalFailedMatches
        {
            get => _rivalFailedMatches;
            set { _rivalFailedMatches = value; OnPropertyChanged(); }
        }
        public ObservableCollection<GetScoreBreakdownRequestDto> Penalties
        {
            get => _penalties;
            set { _penalties = value; OnPropertyChanged(); }
        }

        public Visibility ViewModeVisibility => _isEditing ? Visibility.Collapsed : Visibility.Visible;
        public Visibility EditModeVisibility => _isEditing ? Visibility.Visible : Visibility.Collapsed;
        public Visibility StatsVisibility => _isShowingStats ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ProfileDetailsVisibility => _isShowingStats ? Visibility.Collapsed : Visibility.Visible;

        public Visibility ModifyButtonsVisibility => (_isEditing || _isShowingStats) ? Visibility.Collapsed : Visibility.Visible;

        public string ToggleStatsButtonText => _isShowingStats ? Properties.Resources.Button_hideDetails : Properties.Resources.Button_seeScores;

        public ICommand LoadProfileCommand { get; }
        public ICommand NavigateToLobbyCommand { get; }
        public ICommand ModifyProfileCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand ToggleStatsCommand { get; }

        public ProfilePageViewModel()
        {
            _userService = new UserServiceClient();
            LoadProfileCommand = new RelayCommand(_ => LoadProfile());
            NavigateToLobbyCommand = new RelayCommand(_ => NavigateToLobby());

            ModifyProfileCommand = new RelayCommand(_ => ModifyProfile());
            SaveChangesCommand = new RelayCommand(_ => SaveChanges());
            GoBackCommand = new RelayCommand(_ => GoBack());

            ToggleStatsCommand = new RelayCommand(_ => ToggleStats());
            LoadProfile();
        }

        private async void LoadProfile()
        {
            if (!SessionManager.Instance.IsSignedIn) return;

            _isLoading = true;
            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(IsNotLoading));

            int currentUserId = SessionManager.Instance.CurrentUserId;
            var request = new LoadProfileRequestDto { UserId = currentUserId };

            var response = await Task.Run(() => _userService.LoadProfile(request));
            var breakdownResponse = await Task.Run(() => _userService.GetScoreBreakdown(currentUserId));

            _isLoading = false;
            OnPropertyChanged(nameof(IsNotLoading));

            if (response.Success)
            {
                Username = response.Username;
                FullName = response.FullName;
                GamesPlayed = response.GamesPlayed;
                GamesWon = response.GamesWon;
                WinRate = response.WinRate;
                Bio = response.Bio;
            }
            else
            {
                MessageBox.Show(response.Message);
            }

            if (breakdownResponse != null && breakdownResponse.Success)
            {
                WonMatches.Clear();
                RivalFailedMatches.Clear();
                Penalties.Clear();

                foreach (var item in breakdownResponse.Ledger.Where(x => x.Category == "WORD_GUESSED"))
                    WonMatches.Add(item);

                foreach (var item in breakdownResponse.Ledger.Where(x => x.Category == "OPPONENT_FAILED"))
                    RivalFailedMatches.Add(item);

                foreach (var item in breakdownResponse.Ledger.Where(x => x.Category == "PENALIZATION"))
                    Penalties.Add(item);
            }
            else if (breakdownResponse != null && !breakdownResponse.Success)
            {
                MessageBox.Show(breakdownResponse.Message);
            }
        }

        private void NavigateToLobby()
        {
            NavigationManager.Instance.Navigate(new LobbyPage());
        }

        private void ModifyProfile()
        {
            _originalUsername = Username;
            _originalBio = Bio;

            _isEditing = true;
            ToggleModes();
        }

        private async void SaveChanges()
        {
            if (Username == _originalUsername && Bio == _originalBio)
            {
                MessageBox.Show(Properties.Resources.Message_sameData,
                                Properties.Resources.Title_message, MessageBoxButton.OK, MessageBoxImage.Information);

                _isEditing = false;
                ToggleModes();
                return;
            }

            _isLoading = true;
            OnPropertyChanged(nameof(IsNotLoading));

            var updateRequest = new UpdateProfileRequestDto
            {
                UserId = SessionManager.Instance.CurrentUserId,
                Username = Username,
                Bio = Bio,
            };

            var response = await Task.Run(() => _userService.UpdateProfile(updateRequest));

            _isLoading = false;
            OnPropertyChanged(nameof(IsNotLoading));

            if (response.Success)
            {
                MessageBox.Show(Properties.Resources.Message_updateSuccess, Properties.Resources.Title_message, MessageBoxButton.OK, MessageBoxImage.Information);
                _isEditing = false;
                ToggleModes();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack()
        {
            Username = _originalUsername;
            Bio = _originalBio;

            _isEditing = false;
            ToggleModes();
        }

        private void ToggleModes()
        {
            OnPropertyChanged(nameof(ViewModeVisibility));
            OnPropertyChanged(nameof(EditModeVisibility));
            OnPropertyChanged(nameof(ModifyButtonsVisibility));
        }

        private void ToggleStats()
        {
            _isShowingStats = !_isShowingStats;
            OnPropertyChanged(nameof(StatsVisibility));
            OnPropertyChanged(nameof(ProfileDetailsVisibility));
            OnPropertyChanged(nameof(ModifyButtonsVisibility));
            OnPropertyChanged(nameof(ToggleStatsButtonText));
        }
    }
}