using HangmanGame.Client.Helpers;
using HangmanGame.Client.ViewModels;
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
    /// Lógica de interacción para LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        public LogInPage()
        {
            InitializeComponent();
            DataContext = new LogInViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LogInViewModel vm) vm.Password = ((PasswordBox)sender).Password;
        }
    }
}
