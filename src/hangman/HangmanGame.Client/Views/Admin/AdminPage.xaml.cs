using HangmanGame.Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HangmanGame.Client.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            DataContext = new SignInViewModel();
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignInViewModel vm) vm.Password = ((PasswordBox)sender).Password;
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
