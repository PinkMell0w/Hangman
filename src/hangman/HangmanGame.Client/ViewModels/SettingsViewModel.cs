using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    internal class SettingsViewModel : BaseViewModel
    {
        private string _selectedLanguage;

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set { _selectedLanguage = value; OnPropertyChanged(); }
        }

        public ICommand GoBackCommand { get; }
        public ICommand SaveSettingsCommand { get; }

        public SettingsViewModel()
        {
            SelectedLanguage = Properties.Settings.Default.Language;
            SaveSettingsCommand = new RelayCommand(_ => SaveSettings());
            GoBackCommand = new RelayCommand(_ => NavigationManager.Instance.Navigate(new Views.LobbyPage()));
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Language = SelectedLanguage;
            Properties.Settings.Default.Save();

            LanguageManager.CurrentLanguage = SelectedLanguage;

            NavigationManager.Instance.Navigate(new Views.Settings.SettingsPage());
        }
    }
}
