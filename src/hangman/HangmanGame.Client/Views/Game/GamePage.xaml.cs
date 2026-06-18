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

namespace HangmanGame.Client.Views.Game
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
        }

        public GamePage(int matchId)
        {
            InitializeComponent();
        }

        private void BtnLeaveMatch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LobbyPage());
        }

        private void BtnKickPlayer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LobbyPage());
        }

        private void BtnLetter_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }
    }
}
