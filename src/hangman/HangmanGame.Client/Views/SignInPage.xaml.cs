using HangmanGame.Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HangmanGame.Client.Views
{
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
            DataContext = new SignInViewModel();
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignInViewModel vm) vm.Password = ((PasswordBox)sender).Password;
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }
    }
}
