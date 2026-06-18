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

namespace HangmanGame.Client.ViewModels
{
    internal class GamePageViewModel : BaseViewModel
    {
        private readonly MatchServiceClient _matchService;
        private readonly WordServiceClient _wordService;

        private string _displayedWord;
        private int _hangmanStage = 0;
        private bool _isHost;
        private int _matchId;

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
            }
        }

        public Visibility HeadVisibility => HangmanStage >= 1 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility TorsoVisibility => HangmanStage >= 2 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Arm1Visibility => HangmanStage >= 3 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Arm2Visibility => HangmanStage >= 4 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Leg1Visibility => HangmanStage >= 5 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility StickmanVisibility => HangmanStage >= 6 ? Visibility.Visible : Visibility.Collapsed;

        public ICommand GuessLetterCommand { get; private set; }

        public GamePageViewModel(int matchId, bool isHost)
        {
            _isHost = isHost;

            var alphabet = Properties.Resources.Keyboard_alphabet ?? "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            KeyboardLetters = new ObservableCollection<char>(alphabet.ToCharArray());

            GuessLetterCommand = new RelayCommand(param => GuessLetter((char)param));

            DisplayedWord = "";
        }

        private void GuessLetter(char letter)
        {
            KeyboardLetters.Remove(letter);

            // TODO: Send guess to backend
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
                System.Diagnostics.Debug.WriteLine($"Error initializing word: {ex.Message}");
                DisplayedWord = "";
            }
        }

    }
}
