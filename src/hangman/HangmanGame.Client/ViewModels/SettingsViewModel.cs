using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangmanGame.Client.Helpers;

namespace HangmanGame.Client.ViewModels
{
    internal class SettingsViewModel : BaseViewModel
    {
        private string _selectedLanguage;

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            SelectedLanguage = Properties.Settings.Default.Language;
        }
    }
}
