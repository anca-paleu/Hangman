using Hangman.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Hangman.ViewModel
{
    public class StatisticsLogic : ObservableObject
    {
        private ObservableCollection<User> _allUsers;

        public ObservableCollection<User> AllUsers
        {
            get { return _allUsers; }
            set { _allUsers = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; private set; }

        public StatisticsLogic(ObservableCollection<User> allUsers)
        {
            AllUsers = allUsers;

            foreach (var user in AllUsers)
            {
                if (user.Statistics == null || user.Statistics.Count == 0)
                    user.InitStatistics();
            }

            CloseCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is System.Windows.Window window)
                window.Close();
        }
    }
}