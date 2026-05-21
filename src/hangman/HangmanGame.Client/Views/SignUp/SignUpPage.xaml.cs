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

namespace HangmanGame.Client.Views.SignUp
{
    /// <summary>
    /// Lógica de interacción para SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void btnLoginPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.Navigate(new LoginPage());
        }

        private void btnRegister_click(object sender, RoutedEventArgs e)
        {
            // TODO
            NavigationManager.Instance.Navigate(new LobbyPage());
        }
    }
}
