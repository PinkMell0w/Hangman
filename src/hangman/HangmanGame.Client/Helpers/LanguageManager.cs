using System.Globalization;
using System.Threading;

namespace HangmanGame.Client.Helpers
{
    public static class LanguageManager
    {
        private static string _currentLanguage = Properties.Settings.Default.Language;

        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                ApplyCulture();
            }
        }

        public static void ApplyCulture()
        {
            var lang = new CultureInfo(_currentLanguage);
            Thread.CurrentThread.CurrentCulture = lang;
            Thread.CurrentThread.CurrentUICulture = lang;

            Properties.Resources.Culture = _currentLanguage == "es-MX" ? lang: null;
        }
    }
}
