using HangmanGame.Client.Commands;
using HangmanGame.Client.Helpers;
using HangmanGame.Client.MatchServiceReference;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame.Client.ViewModels
{
    internal class CreateMatchViewModel : BaseViewModel
    {
        private readonly WordServiceClient _wordService;
        private readonly MatchServiceClient _matchService;
        private ObservableCollection<Word> _words;

        private Word _selectedWord;
        private string _selectedCategory = "Animals";

        public ObservableCollection<Word> Words
        {
            get => _words;
            set { _words = value; OnPropertyChanged(); }
        }

        public Word SelectedWord
        {
            get => _selectedWord;
            set { _selectedWord = value; OnPropertyChanged(); }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        public ICommand NavigateToLobbyCommand { get; }
        public ICommand CreateWaitingRoomCommand { get; }
        public ICommand SelectWordCommand { get; }
        public ICommand SelectCategoryCommand { get; }

        public CreateMatchViewModel()
        {
            _wordService = new WordServiceClient();
            _matchService = new MatchServiceClient();
            NavigateToLobbyCommand = new RelayCommand(_ => NavigateToLobby());
            CreateWaitingRoomCommand = new RelayCommand(_ => ExecuteCreateMatch());
            SelectWordCommand = new RelayCommand(SelectWord);
            SelectCategoryCommand = new RelayCommand(SelectCategory);
            LoadWords();
        }

        private async void LoadWords()
        {
            try
            {
                var request = new WordRequestDto
                {
                    Language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName,
                    Category = SelectedCategory
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

        private async void ExecuteCreateMatch()
        {
            if (SelectedWord == null)
            {
                MessageBox.Show("Select a word first.");
                return;
            }

            var request = new CreateMatchRequestDto
            {
                HostId = SessionManager.Instance.CurrentUserId,
                WordId = SelectedWord.WordId
            };

            var response = await Task.Run(() => _matchService.CreateMatch(request));

            if (response.Success)
            {
                MessageBox.Show(
                    $"Match created: {response.MatchId}");
            }
            NavigationManager.Instance.Navigate(new MatchPage());
        }

        private void SelectWord(object parameter)
        {
            if (parameter is Word word)
            {
                SelectedWord = word;
            }
        }

        private void SelectCategory(object parameter)
        {
            if (parameter is string category)
            {
                SelectedCategory = category;
                LoadWords();
            }
        }
    }
}
