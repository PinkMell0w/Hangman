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
            DataContext = new HangmanGame.Client.ViewModels.SignUpViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm) vm.Password = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm) vm.ConfirmPassword = ((PasswordBox)sender).Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(TextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
