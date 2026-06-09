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
    /// Interaction logic for MatchSettingsPage.xaml
    /// </summary>
    public partial class MatchSettingsPage : Page
    {
        public MatchSettingsPage()
        {
            InitializeComponent();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnKickPlayer_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
