using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HangmanGame.Client.ViewModels
{
    internal class MatchPageViewModel : BaseViewModel
    {
        public bool IsHost { get; set; }
        public Visibility HostControlsVisibility => IsHost ? Visibility.Visible : Visibility.Collapsed;
    }
}
