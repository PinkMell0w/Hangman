using HangmanGame.Client.Views.FriendsList;
using HangmanGame.Client.Views.Game;
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

namespace HangmanGame.Client.Views.MatchesList
{
    /// <summary>
    /// Interaction logic for MatchesListPage.xaml
    /// </summary>
    public partial class MatchesListPage : Page
    {
        public MatchesListPage()
        {
            InitializeComponent();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FriendsListPage());
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MatchPage());
        }
    }
}
