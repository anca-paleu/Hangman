using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Hangman.Model;

namespace Hangman.ViewModel
{
    public class GameLogic : ObservableObject
    {
        private string _userName;
        private string _userImagePath;
        private int _timeLeft;
        private int _level;
        private string _hangmanImage;
        private int _mistakes;
        private string _displayedWord;
        private string _secretWord;

        private DispatcherTimer _timer;
        private List<string> _allLines;
        private string _currentCategory;
        private List<string> _guessedLetters;

        private bool _isAllChecked;
        private bool _isAnimalsChecked;
        private bool _isCountriesChecked;
        private bool _isMoviesChecked;
        private bool _isFruitsChecked;
        private bool _isCarsChecked;

        public bool IsAllChecked
        {
            get { return _isAllChecked; }
            set { _isAllChecked = value; OnPropertyChanged("IsAllChecked"); }
        }

        public bool IsAnimalsChecked
        {
            get { return _isAnimalsChecked; }
            set { _isAnimalsChecked = value; OnPropertyChanged("IsAnimalsChecked"); }
        }

        public bool IsCountriesChecked
        {
            get { return _isCountriesChecked; }
            set { _isCountriesChecked = value; OnPropertyChanged("IsCountriesChecked"); }
        }

        public bool IsMoviesChecked
        {
            get { return _isMoviesChecked; }
            set { _isMoviesChecked = value; OnPropertyChanged("IsMoviesChecked"); }
        }

        public bool IsFruitsChecked
        {
            get { return _isFruitsChecked; }
            set { _isFruitsChecked = value; OnPropertyChanged("IsFruitsChecked"); }
        }

        public bool IsCarsChecked
        {
            get { return _isCarsChecked; }
            set { _isCarsChecked = value; OnPropertyChanged("IsCarsChecked"); }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        public string UserImagePath
        {
            get { return _userImagePath; }
            set { _userImagePath = value; OnPropertyChanged("UserImagePath"); }
        }

        public int TimeLeft
        {
            get { return _timeLeft; }
            set { _timeLeft = value; OnPropertyChanged("TimeLeft"); }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged("Level"); }
        }

        public string HangmanImage
        {
            get { return _hangmanImage; }
            set { _hangmanImage = value; OnPropertyChanged("HangmanImage"); }
        }

        public string DisplayedWord
        {
            get { return _displayedWord; }
            set { _displayedWord = value; OnPropertyChanged("DisplayedWord"); }
        }

        public int Mistakes
        {
            get { return _mistakes; }
            set
            {
                _mistakes = value;
                if (_mistakes >= 0 && _mistakes <= 6)
                {
                    HangmanImage = "Images/game" + _mistakes + ".jpg";
                }
                OnPropertyChanged("Mistakes");
            }
        }

        public ICommand GuessLetterCommand { get; private set; }
        public ICommand ChangeCategoryCommand { get; private set; }
        public ICommand NewGameCommand { get; private set; }

        public GameLogic(User currentUser)
        {
            if (currentUser != null)
            {
                UserName = currentUser.Name;
                UserImagePath = currentUser.ProfileImagePath;
            }

            Level = 1;
            _allLines = new List<string>();
            _guessedLetters = new List<string>();

            _currentCategory = "All Categories";
            IsAllChecked = true;

            GuessLetterCommand = new RelayCommand(GuessLetter, CanGuessLetter);
            ChangeCategoryCommand = new RelayCommand(ChangeCategory);
            NewGameCommand = new RelayCommand(NewGame);

            InitTimer();
            LoadWords();
            StartNewGame();
        }

        private void InitTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimeLeft = TimeLeft - 1;
            if (TimeLeft == 0)
            {
                _timer.Stop();
                MessageBox.Show("Time is up! Game Over.");
                StartNewGame();
            }
        }

        private void NewGame(object parameter)
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
            StartNewGame();
        }

        private void ChangeCategory(object parameter)
        {
            if (parameter != null)
            {
                _currentCategory = parameter.ToString();

                IsAllChecked = false;
                IsAnimalsChecked = false;
                IsCountriesChecked = false;
                IsMoviesChecked = false;
                IsFruitsChecked = false;
                IsCarsChecked = false;

                if (_currentCategory == "All Categories") { IsAllChecked = true; }
                if (_currentCategory == "Animals") { IsAnimalsChecked = true; }
                if (_currentCategory == "Countries") { IsCountriesChecked = true; }
                if (_currentCategory == "Movies") { IsMoviesChecked = true; }
                if (_currentCategory == "Fruits") { IsFruitsChecked = true; }
                if (_currentCategory == "Cars") { IsCarsChecked = true; }

                if (_timer != null)
                {
                    _timer.Stop();
                }

                StartNewGame();
            }
        }

        private void LoadWords()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "words.txt");
            if (File.Exists(path) == true)
            {
                string[] lines = File.ReadAllLines(path);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.Contains("|") == true)
                    {
                        _allLines.Add(line.ToUpper());
                    }
                }
            }
        }

        private void StartNewGame()
        {
            Mistakes = 0;
            TimeLeft = 30;

            _guessedLetters.Clear();
            CommandManager.InvalidateRequerySuggested();

            List<string> availableWords = new List<string>();

            for (int i = 0; i < _allLines.Count; i++)
            {
                string line = _allLines[i];
                string[] parts = line.Split('|');

                string categoryPart = parts[0];
                string wordPart = parts[1];

                if (_currentCategory == "All Categories")
                {
                    availableWords.Add(wordPart);
                }
                else
                {
                    if (categoryPart == _currentCategory.ToUpper())
                    {
                        availableWords.Add(wordPart);
                    }
                }
            }

            if (availableWords.Count > 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, availableWords.Count);
                _secretWord = availableWords[index];
            }
            else
            {
                _secretWord = "ERROR";
            }

            DisplayedWord = "";
            for (int i = 0; i < _secretWord.Length; i++)
            {
                DisplayedWord = DisplayedWord + "_ ";
            }

            if (_timer != null)
            {
                _timer.Start();
            }
        }

        private bool CanGuessLetter(object parameter)
        {
            if (parameter != null)
            {
                string letter = parameter.ToString().ToUpper();
                if (_guessedLetters.Contains(letter) == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void GuessLetter(object parameter)
        {
            if (parameter == null) return;

            string letter = parameter.ToString().ToUpper();
            bool found = false;

            _guessedLetters.Add(letter);
            CommandManager.InvalidateRequerySuggested();

            StringBuilder word = new StringBuilder(DisplayedWord);

            for (int i = 0; i < _secretWord.Length; i++)
            {
                if (_secretWord[i].ToString() == letter)
                {
                    found = true;
                    word[i * 2] = letter[0];
                }
            }

            if (found == true)
            {
                DisplayedWord = word.ToString();

                if (DisplayedWord.Contains("_") == false)
                {
                    _timer.Stop();
                    MessageBox.Show("You won! The word was: " + _secretWord);
                    StartNewGame();
                }
            }
            else
            {
                Mistakes = Mistakes + 1;
                if (Mistakes == 6)
                {
                    _timer.Stop();
                    MessageBox.Show("Game Over! The word was: " + _secretWord);
                    StartNewGame();
                }
            }
        }
    }
}