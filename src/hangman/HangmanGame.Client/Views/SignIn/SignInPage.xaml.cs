using HangmanGame.Client.Helpers;
using HangmanGame.Client.Views.SignUp;
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
    /// Lógica de interacción para SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void TxtUsername_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TxtUsername_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void BtnForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LobbyPage());
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUpPage());
        }
    }
}
