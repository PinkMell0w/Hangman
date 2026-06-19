using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.MatchServiceReference;
using HangmanGame.Client.Views;
using HangmanGame.Client.Views.MatchesList;
using HangmanGame.Client.WordServiceReference;
using HangmanGame.Core.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HangmanGame.Client.ViewModels
{
    internal class GamePageViewModel : BaseViewModel
    {
        private readonly MatchServiceClient _matchService;
        private readonly WordServiceClient _wordService;
        private DispatcherTimer _syncTimer;

        private string _displayedWord;
        private int _hangmanStage = 0;
        private bool _isHost;
        private int _matchId;
        private string _gameState = "PLAYING";
        private bool _isMyTurn;
        private string _turnMessage;
        private char _lastActiveGuessedLetter = ' ';

        public ObservableCollection<char> KeyboardLetters { get; set; }

        public string DisplayedWord
        {
            get => _displayedWord;
            set { _displayedWord = value; OnPropertyChanged(); }
        }

        public int HangmanStage
        {
            get => _hangmanStage;
            set
            {
                _hangmanStage = value;
                OnPropertyChanged();
                UpdateHangmanVisibility();
            }
        }

        public bool IsHost
        {
            get => _isHost;
            set
            {
                _isHost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuesserUiVisibility));
                OnPropertyChanged(nameof(HostUiVisibility));
            }
        }

        public string GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FaceOkVisibility));
                OnPropertyChanged(nameof(FaceWinVisibility));
                OnPropertyChanged(nameof(FaceLoseVisibility));
            }
        }

        public bool IsMyTurn
        {
            get => _isMyTurn;
            set
            {
                _isMyTurn = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GuesserUiVisibility));
                OnPropertyChanged(nameof(HostUiVisibility));
            }
        }

        public string TurnMessage
        {
            get => _turnMessage;
            set
            {
                _turnMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValidationPromptText));
            }
        }

        public string ValidationPromptText => TurnMessage;
        public Visibility GuesserUiVisibility => (!IsHost) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HostUiVisibility => (IsHost && IsMyTurn) ? Visibility.Visible : Visibility.Collapsed;

        public Visibility HeadVisibility => HangmanStage >= 1 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility TorsoVisibility => HangmanStage >= 2 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Arm1Visibility => HangmanStage >= 3 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Arm2Visibility => HangmanStage >= 4 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Leg1Visibility => HangmanStage >= 5 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility StickmanVisibility => HangmanStage >= 6 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility FaceOkVisibility => (GameState == "PLAYING" && HangmanStage >= 1) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility FaceWinVisibility => GameState == "WON" ? Visibility.Visible : Visibility.Collapsed;
        public Visibility FaceLoseVisibility => GameState == "LOST" ? Visibility.Visible : Visibility.Collapsed;

        public ICommand GuessLetterCommand { get; private set; }
        public ICommand ValidateGuessCommand { get; private set; }
        public ICommand LeaveMatchCommand { get; private set; }
        public ICommand KickPlayerCommand { get; private set; }
        public ICommand SendMessageCommand { get; private set; }

        public GamePageViewModel(int matchId, bool isHost)
        {
            _matchId = matchId;
            _isHost = isHost;
            _matchService = new MatchServiceClient();
            _wordService = new WordServiceClient();

            var alphabet = Properties.Resources.Keyboard_alphabet ?? "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            KeyboardLetters = new ObservableCollection<char>(alphabet.ToCharArray());

            GuessLetterCommand = new RelayCommand(param => GuessLetter((char)param));

            ValidateGuessCommand = new RelayCommand(param => {
                if (param != null && bool.TryParse(param.ToString(), out bool wasCorrect))
                {
                    SubmitHostValidation(wasCorrect);
                }
            });

            LeaveMatchCommand = new RelayCommand(_ => { /* Handle Leave Match */ });
            KickPlayerCommand = new RelayCommand(_ => { /* Handle Kick */ });
            SendMessageCommand = new RelayCommand(_ => { /* Handle Chat */ });

            DisplayedWord = "";

            InitializeWordDisplay();
            StartSyncTimer();
        }

        private void GuessLetter(char letter)
        {
            if (!IsMyTurn || IsHost) return;

            try
            {
                IsMyTurn = false;
                TurnMessage = "Submitting guess...";

                _matchService.SubmitGuesserLetter(_matchId, letter);

                if (KeyboardLetters.Contains(letter))
                {
                    KeyboardLetters.Remove(letter);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error sending guess: {ex.Message}");
            }
        }

        private void SubmitHostValidation(bool wasCorrect)
        {
            if (!IsMyTurn || !IsHost) return;

            try
            {
                IsMyTurn = false;

                _matchService.SubmitHostValidation(_matchId, wasCorrect);
            }
            catch (Exception ex)
            {
                IsMyTurn = true;
                System.Diagnostics.Debug.WriteLine($"Error sending validation: {ex.Message}");
            }
        }

        private void UpdateHangmanVisibility()
        {
            OnPropertyChanged(nameof(HeadVisibility));
            OnPropertyChanged(nameof(TorsoVisibility));
            OnPropertyChanged(nameof(Arm1Visibility));
            OnPropertyChanged(nameof(Arm2Visibility));
            OnPropertyChanged(nameof(Leg1Visibility));
            OnPropertyChanged(nameof(StickmanVisibility));
        }

        private void InitializeWordDisplay()
        {
            try
            {
                var response = _matchService.GetMatchById(new GetMatchDetailsRequestDto { MatchId = _matchId });
                if (response != null && response.Success && response.Match != null)
                {
                    // Sets the initial masked string display
                    DisplayedWord = response.Match.WordName ?? "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing word display: {ex.Message}");
            }
        }

        private void StartSyncTimer()
        {
            _syncTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _syncTimer.Tick += OnSyncTick;
            _syncTimer.Start();
        }

        private void OnSyncTick(object sender, EventArgs e)
        {
            try
            {
                var state = _matchService.GetLiveGameLoopState(_matchId);
                if (state != null)
                {
                    DisplayedWord = state.MaskedWord;
                    HangmanStage = state.HangmanStage;
                    GameState = state.MatchStatus;
                    _lastActiveGuessedLetter = state.LastGuessedLetter;

                    if (state.CurrentTurn == "GUESSER" && state.LastGuessedLetter != ' ')
                    {
                        if (KeyboardLetters.Contains(state.LastGuessedLetter))
                        {
                            KeyboardLetters.Remove(state.LastGuessedLetter);
                        }
                    }

                    if (GameState == "WON" || GameState == "LOST")
                    {
                        _syncTimer.Stop();
                        string message = GameState == "WON"
                            ? Properties.Resources.Message_wordGuessed
                            : Properties.Resources.Message_wordNotGuessed;

                        MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);

                        Cleanup();
                        App.Current.Dispatcher.Invoke(() => {
                            if (IsHost) NavigationManager.Instance.Navigate(new LobbyPage());
                            else NavigationManager.Instance.Navigate(new MatchesListPage());
                        });
                        return;
                    }

                    if (IsHost)
                    {
                        IsMyTurn = (state.CurrentTurn == "HOST_VALIDATION");
                        TurnMessage = IsMyTurn
                            ? $"Opponent guessed '{_lastActiveGuessedLetter}'. Accept or reject this move?"
                            : "Waiting for player to make a move...";
                    }
                    else
                    {
                        IsMyTurn = (state.CurrentTurn == "GUESSER");
                        TurnMessage = IsMyTurn
                            ? "Your turn! Pick a letter."
                            : "Waiting for host validation...";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sync Error: {ex.Message}");
            }
        }

        public void Cleanup()
        {
            _syncTimer?.Stop();
        }
    }
}