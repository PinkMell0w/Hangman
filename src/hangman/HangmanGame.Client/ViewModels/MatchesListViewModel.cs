using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.MatchServiceReference;
using HangmanGame.Client.UserServiceReference;
using HangmanGame.Client.Views;
using HangmanGame.Client.Views.Game;
using HangmanGame.Client.Views.Settings;
using HangmanGame.Core.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    internal class MatchesListViewModel : BaseViewModel
    {
        private readonly MatchServiceClient _matchService;
        private ObservableCollection<MatchDto> _matches;

        private string _username;

        public ObservableCollection<MatchDto> Matches
        {
            get => _matches;
            set { _matches = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public ICommand JoinMatchCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand GoToSettingsCommand { get; }
        public ICommand ViewProfileCommand { get; }
        public ICommand RefreshListCommand { get; }

        public MatchesListViewModel()
        {
            _matchService = new MatchServiceClient();
            JoinMatchCommand = new RelayCommand(JoinMatch);
            GoBackCommand = new RelayCommand(_ => GoToLobby());
            GoToSettingsCommand = new RelayCommand(_ => GoToSettings());
            ViewProfileCommand = new RelayCommand(_ => ViewProfile());
            RefreshListCommand = new RelayCommand(_ => RefreshList());
            LoadMatches();
            LoadUsername();
        }

        private void JoinMatch(object parameter)
        {
            try
            {
                if (parameter == null)
                    return;

                int matchId = (int)parameter;

                var request = new JoinMatchRequestDto
                {
                    MatchId = matchId,
                    UserId = SessionManager.Instance.CurrentUserId
                };

                var response = _matchService.JoinMatch(request);

                if (!response.Success)
                {
                    MessageBox.Show(response.Message);
                    return;
                }

                NavigationManager.Instance.Navigate(new MatchPage(matchId, false));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoToLobby()
        {
            NavigationManager.Instance.Navigate(new LobbyPage());
        }

        private void GoToSettings()
        {
            NavigationManager.Instance.Navigate(new SettingsPage());
        }

        private void ViewProfile()
        {
            NavigationManager.Instance.Navigate(new ProfilePage());
        }

        private void RefreshList()
        {
            LoadMatches();
        }

        private void LoadUsername()
        {
            Username = SessionManager.Instance.Username;
        }

        private async void LoadMatches()
        {
            try
            {
                var request = new GetAvailableMatchesRequestDto
                    {
                        UserId = SessionManager.Instance.CurrentUserId
                    };

                var response = await Task.Run(() => _matchService.GetAvailableMatches(request));

                if (response.Success)
                {
                    Matches = new ObservableCollection<MatchDto>(response.AvailableMatches);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Properties.Resources.Error_serverTimeout, Properties.Resources.Title_message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
