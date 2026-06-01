using HangmanGame.Client.Helpers;
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

        private void BtnCreateMatch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnJoinMatch_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.Navigate(new ProfilePage());
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            //TODO logout logic 
            NavigationManager.Instance.Navigate(new SignInPage());
        }
    }
}
