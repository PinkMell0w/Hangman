using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HangmanGame.Client.Helpers
{
    public class NavigationManager
    {
        private static NavigationManager _instance = new NavigationManager();
        private Frame mainFrame;
        private MainWindow mainWindow;

        private NavigationManager() { }

        public static NavigationManager Instance
        {
            get
            {
                if (_instance == null) { _instance = new NavigationManager(); }

                return _instance;
            }
        }

        public void Initialize(Frame mainFrame, MainWindow mainWindow)
        {
            this.mainFrame = mainFrame;
            this.mainWindow = mainWindow;
        }

        public void Navigate(Page page)
        {
            if (mainFrame != null)
            {
                mainFrame.NavigationService.RemoveBackEntry();
                mainFrame.Content = page;
            }
        }

        public Page currentPage => mainFrame?.Content as Page;
    }
}
