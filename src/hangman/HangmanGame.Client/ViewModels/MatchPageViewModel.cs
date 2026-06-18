using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    internal class MatchPageViewModel : BaseViewModel
    {
        private bool _isHost { get; set; }
        private string _opponentName { get; set; }
        private int _matchId { get; set; }
        public Visibility HostControlsVisibility => _isHost ? Visibility.Visible : Visibility.Collapsed;

        public bool IsNotHost => !_isHost;

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

        public ICommand ConfigureMatchCommand { get; }
        public ICommand LeaveMatchCommand { get; }
        public ICommand StartMatchCommand { get; }
        public ICommand SendMessageCommand { get; }

        public MatchPageViewModel()
        {
            _isHost = true;
        }

        public MatchPageViewModel(int matchId)
        {

            LoadMatch(matchId);
        }

        private void ConfigureMatch()
        {
            // TODO
        }

        private void LeaveMatch()
        {
            // TODO
        }

        private void StartMatch()
        {
            // TODO
        }

        private void SendMessage()
        {
            // TODO
        }

        private void LoadMatch(int matchId)
        {
            // TODO
        }

        private void HandleOpponentName()
        {
            // TODO
        }
    }
}
