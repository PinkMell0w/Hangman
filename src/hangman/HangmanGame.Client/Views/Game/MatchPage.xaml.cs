using HangmanGame.Client.ViewModels;
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
    /// Interaction logic for MatchPage.xaml
    /// </summary>
    public partial class MatchPage : Page
    {
        public MatchPage()
        {
            InitializeComponent();
            DataContext = new MatchPageViewModel();
        }

        private void BtnConfigureMatch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MatchSettingsPage());
        }

        private void BtnStartMatch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GamePage());
        }

        private void BtnLeaveMatch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LobbyPage());
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }
    }
}
