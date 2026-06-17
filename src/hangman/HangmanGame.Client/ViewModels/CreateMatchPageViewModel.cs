using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.Views;
using HangmanGame.Client.Views.Game;
using HangmanGame.Client.WordServiceReference;
using HangmanGame.Core.Core.Domain;
using HangmanGame.Core.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    internal class CreateMatchPageViewModel : BaseViewModel
    {
        private readonly WordServiceClient _wordService;
        private ObservableCollection<Word> _words;

        private string _selectedWord;

        public ObservableCollection<Word> Words
        {
            get => _words;
            set { _words = value; OnPropertyChanged(); }
        }

        public string SelectedWord
        {
            get => _selectedWord;
            set { _selectedWord = value; OnPropertyChanged(); }
        }

        public ICommand NavigateToLobbyCommand { get; }
        public ICommand CreateWaitingRoomCommand { get; }

        public CreateMatchPageViewModel()
        {
            _wordService = new WordServiceClient();
            NavigateToLobbyCommand = new RelayCommand(_ => NavigateToLobby());
            CreateWaitingRoomCommand = new RelayCommand(_ => CreateWaitingRoom());
            LoadWords();
        }

        private async void LoadWords()
        {
            try
            {
                var request = new WordRequestDto
                {
                    Language = "es"
                };

                var response = await Task.Run(() =>
                    _wordService.GetWords(request));

                if (response.Success)
                {
                    Words = new ObservableCollection<Word>(
                        response.Words);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NavigateToLobby()
        {
            // TODO Ask confirmation to close room
            NavigationManager.Instance.Navigate(new LobbyPage());
        }

        private void CreateWaitingRoom()
        {
            // TODO Transfer data to MatchPage
            NavigationManager.Instance.Navigate(new MatchPage());
        }
    }
}
