using HangmanGame.Client.Helpers;
using HangmanGame.Client.Views.CreateMatch;
using HangmanGame.Client.Views.FriendsList;
using HangmanGame.Client.Views.MatchesList;
using HangmanGame.Client.Views.Profile;
using HangmanGame.Client.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HangmanGame.Client.Views
{
    /// <summary>
    /// Lógica de interacción para LobbyPage.xaml
    /// </summary>
    public partial class LobbyPage : Page
    {
        public LobbyPage()
        {
            InitializeComponent();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            //TODO logout logic 
            NavigationService.Navigate(new SignInPage());
        }

        private void BtnCreateMatch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateMatchPage());
        }

        private void BtnJoinMatch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MatchesListPage());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage());
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FriendsListPage());
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
