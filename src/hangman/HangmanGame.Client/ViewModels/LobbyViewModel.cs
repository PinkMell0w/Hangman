using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.Views;
using HangmanGame.Client.Views.CreateMatch;
using HangmanGame.Client.Views.MatchesList;
using HangmanGame.Client.Views.Settings;
using HangmanGame.Client.Views.SignUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    public class LobbyViewModel : BaseViewModel
    {
        private string _username;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public ICommand CreateMatchCommand { get; }
        public ICommand JoinMatchCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand FriendsCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand LogOutCommand { get; }

        public LobbyViewModel()
        {
            Username = SessionManager.Instance.Username;

            CreateMatchCommand = new RelayCommand(_ => CreateMatch());
            JoinMatchCommand = new RelayCommand(_ => JoinMatch());

            ProfileCommand = new RelayCommand(_ => OpenProfile());
            SettingsCommand = new RelayCommand(_ => OpenSettings());

            LogOutCommand = new RelayCommand(_ => LogOut());
        }

        private void CreateMatch()
        {
            NavigationManager.Instance.Navigate(new CreateMatchPage());
        }

        private void JoinMatch()
        {
            NavigationManager.Instance.Navigate(new MatchesListPage());
        }

        private void OpenProfile()
        {
            NavigationManager.Instance.Navigate(new ProfilePage());
        }

        private void OpenSettings()
        {
            NavigationManager.Instance.Navigate(new SettingsPage());
        }

        private void LogOut()
        {
            SessionManager.Instance.ClearSession();
            NavigationManager.Instance.Navigate(new LogInPage());
        }
    }
}
