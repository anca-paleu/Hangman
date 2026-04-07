using Hangman.Model;
using System.Windows.Input;

namespace Hangman.ViewModel
{
    public class StatisticsLogic : ObservableObject
    {
        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; private set; }

        public StatisticsLogic(User user)
        {
            CurrentUser = user;

            if (CurrentUser.Statistics == null || CurrentUser.Statistics.Count == 0)
            {
                CurrentUser.InitStatistics();
            }

            CloseCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is System.Windows.Window window)
            {
                window.Close();
            }
        }
    }
}