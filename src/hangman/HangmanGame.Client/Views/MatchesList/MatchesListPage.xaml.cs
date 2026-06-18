using HangmanGame.Client.ViewModels;
using HangmanGame.Client.Views.Game;
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
            DataContext = new MatchesListViewModel();
        }
    }
}
