using HangmanGame.Client.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace HangmanGame.Client.Views.SignUp
{
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
            DataContext = new SignUpViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm) vm.Password = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm) vm.ConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}
