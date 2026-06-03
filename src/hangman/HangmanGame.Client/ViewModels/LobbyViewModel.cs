using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateMatchCommand { get; }
        public ICommand JoinMatchCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand FriendsCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand SignOutCommand { get; }

        public LobbyViewModel()
        {
            Username = SessionManager.Instance.Username;

            CreateMatchCommand = new RelayCommand(_ => CreateMatch());
            JoinMatchCommand = new RelayCommand(_ => JoinMatch());

            ProfileCommand = new RelayCommand(_ => OpenProfile());
            FriendsCommand = new RelayCommand(_ => OpenFriends());
            SettingsCommand = new RelayCommand(_ => OpenSettings());

            SignOutCommand = new RelayCommand(_ => SignOut());
        }

        private void CreateMatch()
        {
            // NavigationManager.Instance.Navigate(new CreateMatchPage());
            MessageBox.Show("Create Match not implemented yet");
        }

        private void JoinMatch()
        {
            // NavigationManager.Instance.Navigate(new JoinMatchPage());
            MessageBox.Show("Join Match not implemented yet");
        }

        private void OpenProfile()
        {
            NavigationManager.Instance.Navigate(new ProfilePage());
        }

        private void OpenFriends()
        {
            // NavigationManager.Instance.Navigate(new FriendsPage());
            MessageBox.Show("Friends not implemented yet");
        }

        private void OpenSettings()
        {
            // NavigationManager.Instance.Navigate(new SettingsPage());
            MessageBox.Show("Settings not implemented yet");
        }

        private void SignOut()
        {
            SessionManager.Instance.ClearSession();
            NavigationManager.Instance.Navigate(new SignInPage());
        }
    }
}
