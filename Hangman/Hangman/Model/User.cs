using System.Collections.ObjectModel;
using Hangman.ViewModel;

namespace Hangman.Model
{
    public class User : ObservableObject
    {
        private string _name;
        private string _profileImagePath;
        private ObservableCollection<Statistics> _statistics;
        private ObservableCollection<SavedGame> _savedGames;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string ProfileImagePath
        {
            get { return _profileImagePath; }
            set { _profileImagePath = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Statistics> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; OnPropertyChanged(); }
        }
        public ObservableCollection<SavedGame> SavedGames
        {
            get { return _savedGames; }
            set { _savedGames = value; OnPropertyChanged(); }
        }

        public User()
        {
        }

        public User(string name, string profileImagePath)
        {
            Name = name;
            ProfileImagePath = profileImagePath;
            InitStatistics();
        }

        public void InitStatistics()
        {
            Statistics = new ObservableCollection<Statistics>
            {
                new Statistics { CategoryName = "Animals", GamesPlayed = 0, GamesWon = 0 },
                new Statistics { CategoryName = "Countries", GamesPlayed = 0, GamesWon = 0 },
                new Statistics { CategoryName = "Movies", GamesPlayed = 0, GamesWon = 0 },
                new Statistics { CategoryName = "Fruits", GamesPlayed = 0, GamesWon = 0 },
                new Statistics { CategoryName = "Cars", GamesPlayed = 0, GamesWon = 0 }
            };
        }
    }
}