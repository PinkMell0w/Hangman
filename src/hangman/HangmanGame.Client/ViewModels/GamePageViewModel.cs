using HangmanGame.Client.Commands;
using HangmanGame.Client.MatchServiceReference;
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
        private string _gameState = "PLAYING"; // "PLAYING", "WON", "LOST"
        private bool _isMyTurn;
        private string _turnMessage;

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
            { _isHost = value; OnPropertyChanged(); }
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
            set { _isMyTurn = value; OnPropertyChanged(); }
        }

        public string TurnMessage
        {
            get => _turnMessage;
            set { _turnMessage = value; OnPropertyChanged(); }
        }

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
        public ICommand HostCorrectCommand { get; private set; }
        public ICommand HostIncorrectCommand { get; private set; }

        public GamePageViewModel(int matchId, bool isHost)
        {
            _matchId = matchId;
            _isHost = isHost;
            _matchService = new MatchServiceClient();

            var alphabet = Properties.Resources.Keyboard_alphabet ?? "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            KeyboardLetters = new ObservableCollection<char>(alphabet.ToCharArray());

            GuessLetterCommand = new RelayCommand(param => GuessLetter((char)param));

            HostCorrectCommand = new RelayCommand(_ => SubmitHostValidation(true));
            HostIncorrectCommand = new RelayCommand(_ => SubmitHostValidation(false));

            DisplayedWord = "";

            InitializeWordDisplay();
            StartSyncTimer();
        }

        private void GuessLetter(char letter)
        {
            if (!IsMyTurn || IsHost) return;

            KeyboardLetters.Remove(letter);

            try
            {
                // TODO: Send letter to WCF server, which flags the turn state to "Awaiting Host Validation"
                // _matchService.SubmitGuesserLetter(_matchId, letter);
                IsMyTurn = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending guess: {ex.Message}");
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
                    string word = response.Match.WordName;
                    int wordLength = word.Length;
                    DisplayedWord = string.Join(" ", Enumerable.Repeat("_", wordLength));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing word: {ex.Message}");
                DisplayedWord = "";
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
                // TODO: Fetch current game state row from server
                // var state = _matchService.GetGameState(_matchId);
                // 1. Update DisplayedWord with letters filled in so far
                // 2. Update HangmanStage (0 to 6)
                // 3. Update GameState ("PLAYING", "WON", "LOST")
                // 4. Update IsMyTurn depending on whether the server is waiting for a Guess or a Host validation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sync Error: {ex.Message}");
            }
        }

        private void SubmitHostValidation(bool wasCorrect)
        {
            if (!IsMyTurn || !IsHost) return;

            try
            {
                // TODO: Tell WCF server if guess was correct.
                // If false: Server increments HangmanStage.
                // If true: Server updates the masked word string with the guessed letter.
                // Then, server sets Turn state back to the Guesser.
                // _matchService.SubmitHostValidation(_matchId, wasCorrect);
                IsMyTurn = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving host action: {ex.Message}");
            }
        }

        public void Cleanup()
        {
            _syncTimer?.Stop();
        }

    }
}
