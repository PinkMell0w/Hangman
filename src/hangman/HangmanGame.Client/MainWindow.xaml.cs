using HangmanGame.Client.Helpers;
using HangmanGame.Client.Views.SignUp;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;

namespace HangmanGame.Client
{
    /// <summary>
    /// navegabilidad entre paginas.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += Window_Closing;

            NavigationManager.Instance.Initialize(MainFrame, this);
            NavigationManager.Instance.Navigate(new SignUpPage());
        }

        private void Window_Closing(object sender, CancelEventArgs e) { /* TODO LogOut*/ }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //TODO
        }
    }
}
