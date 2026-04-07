using System.Windows.Input;
using Hangman.Model;

namespace Hangman.ViewModel
{
    public class SaveDialogLogic : ObservableObject
    {
        private string _saveName;
        public string SaveName
        {
            get { return _saveName; }
            set { _saveName = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public bool IsConfirmed { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public SaveDialogLogic()
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(SaveName);
        }

        private void Save(object parameter)
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