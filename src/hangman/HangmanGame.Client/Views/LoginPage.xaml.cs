using HangmanGame.Client.Resources;
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
    /// Lógica de interacción para LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private static string LoginPlaceholderText = Strings.TxtUsername;
        private bool isPlaceholderActive = true;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void IniciarSesion()
        {
            string nombreUsuario = TxtUsername.Text.Trim();
            if (string.IsNullOrEmpty(nombreUsuario) || isPlaceholderActive)
            {
                EmptyUser();
                return;
            }
        }

        private void EmptyUser()
        {
            TxtErrorLogin.Content = Strings.TxtEmptyUser;
            TxtUsername.Clear();
            TxtUsername.Text = LoginPlaceholderText;
            TxtUsername.Foreground = Brushes.Gray;
            isPlaceholderActive = true;
        }

        private void TxtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtUsername.Text == LoginPlaceholderText)
            {
                TxtUsername.Clear();
                TxtUsername.Foreground = Brushes.Black;
                isPlaceholderActive = false;
            }
        }

        private void TxtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtUsername.Text))
            {
                TxtUsername.Text = LoginPlaceholderText;
                TxtUsername.Foreground = Brushes.Gray;
                isPlaceholderActive = true;
            }
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            IniciarSesion();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUp.SignUpPage());
        }
    }
}
