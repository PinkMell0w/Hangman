using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.MatchServiceReference;
using HangmanGame.Client.Views;
using HangmanGame.Client.Views.Game;
using HangmanGame.Client.WordServiceReference;
using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace HangmanGame.Client.ViewModels
{
    internal class MatchPageViewModel : BaseViewModel
    {
        private readonly MatchServiceClient _matchService;
        private readonly WordServiceClient _wordService;
        private ObservableCollection<Word> _words;

        private Word _selectedWord;
        private string _selectedCategory;

        private bool _isHost { get; set; }
        private string _opponentName { get; set; }
        private int _matchId { get; set; }
        private DispatcherTimer _lobbyTimer { get; set; }
        private bool _isShowingSettings { get; set; }

        public ObservableCollection<Word> Words
        {
            get => _words;
            set { _words = value; OnPropertyChanged(); }
        }

        public Word SelectedWord
        {
            get => _selectedWord;
            set { _selectedWord = value; OnPropertyChanged(); }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        public bool IsHost
        {
            get => _isHost;
            set
            {
                _isHost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HostControlsVisibility));
                OnPropertyChanged(nameof(IsNotHost));
            }
        }

        public string OpponentName
        {
            get => _opponentName;
            set { _opponentName = value; OnPropertyChanged(); }
        }

        public int MatchId
        {
            get => _matchId;
            set { _matchId = value; OnPropertyChanged(); }
        }

        public bool IsShowingSettings
        {
            get => _isShowingSettings;
            set
            {
                _isShowingSettings = value;
                OnPropertyChanged(nameof(LobbyControlsVisibility));
                OnPropertyChanged(nameof(SettingsControlsVisibility));
            }
        }

        public Visibility HostControlsVisibility => _isHost ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LobbyControlsVisibility => _isShowingSettings ? Visibility.Collapsed : Visibility.Visible;
        public Visibility SettingsControlsVisibility => _isShowingSettings ? Visibility.Visible : Visibility.Collapsed;
        public bool IsNotHost => !_isHost;

        public ICommand ConfigureMatchCommand { get; private set; }
        public ICommand StartMatchCommand { get; private set; }
        public ICommand LeaveMatchCommand { get; private set; }
        public ICommand SendMessageCommand { get; private set; }
        public ICommand SelectWordCommand { get; private set; }
        public ICommand SelectCategoryCommand { get; private set; }
        public ICommand GoBackCommand { get; private set; }
        public ICommand KickPlayerCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }

        public MatchPageViewModel()
        {
            InitializeCommands();
            _matchService = new MatchServiceClient();

            IsHost = true;
            OpponentName = Properties.Resources.Text_awaitingOpponent;
        }

        public MatchPageViewModel(int matchId)
        {
            InitializeCommands();
            _matchService = new MatchServiceClient();

            LoadMatch(matchId);
        }

        private void InitializeCommands()
        {
            ConfigureMatchCommand = new RelayCommand(_ => ConfigureMatch());
            StartMatchCommand = new RelayCommand(_ => StartMatch());
            LeaveMatchCommand = new RelayCommand(_ => LeaveMatch());
            SendMessageCommand = new RelayCommand(_ => SendMessage());
            SelectWordCommand = new RelayCommand(SelectWord);
            SelectCategoryCommand = new RelayCommand(SelectCategory);
            GoBackCommand = new RelayCommand(_ => GoBack());
        }

        private void StartLobbyPolling(int matchId)
        {
            MatchId = matchId;

            _lobbyTimer = new DispatcherTimer();
            _lobbyTimer.Interval = TimeSpan.FromSeconds(2);
            _lobbyTimer.Tick += OnLobbyTimerTick;
            _lobbyTimer.Start();
        }

        private void OnLobbyTimerTick(object sender, EventArgs e)
        {
            try
            {
                var request = new GetMatchDetailsRequestDto { MatchId = this.MatchId };

                var response = _matchService.GetMatchById(request);

                if (response != null && response.Success && response.Match != null)
                {
                    if (IsHost)
                    {
                        if (!string.IsNullOrEmpty(response.Match.OpponentName))
                        {
                            OpponentName = response.Match.OpponentName;
                        }
                        else
                        {
                            OpponentName = Properties.Resources.Text_awaitingOpponent;
                        }
                    }
                    else
                    {
                        if (response.Match.Status == "IN_PROGRESS")
                        {
                            _lobbyTimer.Stop();
                            NavigationManager.Instance.Navigate(new GamePage(MatchId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lobby tick failed: {ex.Message}");
            }
        }

        private void ConfigureMatch()
        {
            IsShowingSettings = true;

            if (Words == null)
            {
                LoadWords();
            }
        }

        private void StartMatch()
        {
            // TODO
        }

        private void LeaveMatch()
        {
            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.Message_confirmLeave,
                Properties.Resources.Button_leaveMatch,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.No)
            {
                return;
            }

            _lobbyTimer?.Stop();

            try
            {
                var request = new CancelMatchRequestDto { MatchId = this.MatchId };

                var response = _matchService.CancelMatch(request);

                if (response.Success)
                {
                    MessageBox.Show("Match cancelled successfully. Returning to main menu.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error canceling match: {ex.Message}");
            }

            NavigationManager.Instance.Navigate(new LobbyPage());
        }

        private void SendMessage()
        {
            // TODO
        }

        private void LoadMatch(int matchId)
        {
            MatchId = matchId;
            StartLobbyPolling(matchId);
        }

        private void SelectWord(object parameter)
        {
            if (parameter is Word word)
            {
                SelectedWord = word;
            }
        }

        private void SelectCategory(object parameter)
        {
            if (parameter is string category)
            {
                SelectedCategory = category;
                LoadWords();
            }
        }

        private async void LoadWords()
        {
            try
            {
                var request = new WordRequestDto
                {
                    Language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName,
                    Category = SelectedCategory
                };

                var response = await Task.Run(() =>
                    _wordService.GetWords(request));

                if (response.Success)
                {
                    Words = new ObservableCollection<Word>(
                        response.Words);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoBack()
        {
            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.Message_backNoSave,
                "",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                IsShowingSettings = false;
            }
        }

        private void SaveChanges()
        {
            if (SelectedWord == null)
            {
                MessageBox.Show(Properties.Resources.Message_wordNeeded);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.Message_wordChange,
                "",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.No)
                return;

            /*
            try
            {
                var response = _matchService.UpdateMatchWord(this.MatchId, SelectedWord.WordId);

                if (response.Success)
                {
                    IsShowingSettings = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving match settings: {ex.Message}");
            }
            */
        }

        private void KickPlayer()
        {
            if (OpponentName == Properties.Resources.Text_awaitingOpponent)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                Properties.Resources.Message_confirmKick,
                "",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.No) return;

            /*
            try
            {
                var response = _matchService.KickPlayer(this.MatchId);

                if (response.Success)
                {
                    OpponentName = Properties.Resources.Text_awaitingOpponent;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error kicking player: {ex.Message}");
            }
            */
        }
    }
}
