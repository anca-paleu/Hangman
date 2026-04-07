using Hangman.ViewModel;
using System.Collections.Generic;

namespace Hangman.Model
{
    public class SavedGame : ObservableObject
    {
        private string _saveName;
        public string SaveName
        {
            get { return _saveName; }
            set { _saveName = value; OnPropertyChanged(); }
        }

        public string SecretWord { get; set; }
        public string DisplayedWord { get; set; }
        public int Mistakes { get; set; }
        public int Level { get; set; }
        public int TimeLeft { get; set; }
        public string Category { get; set; }
        public List<string> GuessedLetters { get; set; }
        public bool IsCounted { get; set; }
    }
}