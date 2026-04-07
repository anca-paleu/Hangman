using Hangman.ViewModel;

namespace Hangman.Model
{
    public class Statistics : ObservableObject
    {
        private string _categoryName;
        private int _gamesPlayed;
        private int _gamesWon;

        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; OnPropertyChanged(); }
        }

        public int GamesPlayed
        {
            get { return _gamesPlayed; }
            set { _gamesPlayed = value; OnPropertyChanged(); }
        }

        public int GamesWon
        {
            get { return _gamesWon; }
            set { _gamesWon = value; OnPropertyChanged(); }
        }
    }
}