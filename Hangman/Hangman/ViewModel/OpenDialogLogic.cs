using System.Collections.ObjectModel;
using System.Windows.Input;
using Hangman.Model;

namespace Hangman.ViewModel
{
    public class OpenDialogLogic : ObservableObject
    {
        private SavedGame _selectedGame;
        public ObservableCollection<SavedGame> SavedGames { get; set; }

        public SavedGame SelectedGame
        {
            get { return _selectedGame; }
            set { _selectedGame = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public bool IsConfirmed { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public OpenDialogLogic(ObservableCollection<SavedGame> savedGames)
        {
            SavedGames = savedGames;
            OpenCommand = new RelayCommand(Open, CanOpen);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanOpen(object parameter)
        {
            return SelectedGame != null;
        }

        private void Open(object parameter)
        {
            IsConfirmed = true;
            if (parameter is System.Windows.Window window) { window.Close(); }
        }

        private void Cancel(object parameter)
        {
            IsConfirmed = false;
            if (parameter is System.Windows.Window window) { window.Close(); }
        }
    }
}