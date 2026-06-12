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
            string language = _currentLanguage;

            if (string.IsNullOrWhiteSpace(language))
            {
                language = CultureInfo.CurrentUICulture.Name;
            }

            if (language == "system")
            {
                language = CultureInfo.CurrentUICulture.Name;
            }

            var lang = new CultureInfo(language);

            Thread.CurrentThread.CurrentCulture = lang;
            Thread.CurrentThread.CurrentUICulture = lang;

            Properties.Resources.Culture = lang;
        }
    }
}
