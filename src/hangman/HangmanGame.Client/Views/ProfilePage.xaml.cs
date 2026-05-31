using HangmanGame.Client.Helpers;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HangmanGame.Client.Views
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            DataContext = new ViewModels.ProfilePageViewModel();
            LoadProfilePicture();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.Navigate(new LobbyPage());
        }

        private void LoadProfilePicture()
        {
            try
            {
                var userId = SessionManager.Instance.CurrentUserId;
                var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

                var candidates = new[]
                {
                    Path.Combine(appDir, "images", $"{userId}.png"),
                    Path.Combine(appDir, "Resources", "Images", $"{userId}.png"),
                    Path.Combine(appDir, "images", "default-avatar.png"),
                    Path.Combine(appDir, "Resources", "Images", "default-avatar.png")
                };

                BitmapImage bmp = null;

                foreach (var path in candidates)
                {
                    if (File.Exists(path))
                    {
                        bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.UriSource = new Uri(path, UriKind.Absolute);
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.EndInit();
                        bmp.Freeze();
                        break;
                    }
                }

                if (bmp == null)
                {
                    try
                    {
                        bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/default-avatar.png", UriKind.Absolute));
                    }
                    catch
                    {
                        bmp = CreatePlaceholderImage();
                    }
                }

                profilePic.Source = bmp;
            }
            catch
            {
                profilePic.Source = CreatePlaceholderImage();
            }
        }

        private BitmapImage CreatePlaceholderImage()
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri("pack://application:,,,/", UriKind.Absolute);
            bmp.DecodePixelWidth = 1;
            bmp.DecodePixelHeight = 1;
            bmp.EndInit();
            return bmp;
        }
    }
}
