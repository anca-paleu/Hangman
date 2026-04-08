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
        private string _secretWordCategory;
        private string _currentSaveName;

        private DispatcherTimer _timer;
        private List<string> _allLines;
        private string _currentCategory;
        private List<string> _guessedLetters;
        private bool _isCurrentGameCounted;

        private bool _isAllChecked;
        private bool _isAnimalsChecked;
        private bool _isCountriesChecked;
        private bool _isMoviesChecked;
        private bool _isFruitsChecked;
        private bool _isCarsChecked;


        private User _currentUserObj;

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
        public ICommand AboutCommand { get; private set; }
        public ICommand ShowStatisticsCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }
        public ICommand OpenGameCommand { get; private set; }

        public GameLogic(User currentUser)
        {
            if (currentUser != null)
            {
                UserName = currentUser.Name;
                UserImagePath = currentUser.ProfileImagePath;
                _currentUserObj = currentUser;
            }

            Level = 1;
            _allLines = new List<string>();
            _guessedLetters = new List<string>();

            _currentCategory = "All Categories";
            IsAllChecked = true;

            GuessLetterCommand = new RelayCommand(GuessLetter, CanGuessLetter);
            ChangeCategoryCommand = new RelayCommand(ChangeCategory);
            NewGameCommand = new RelayCommand(NewGame);
            AboutCommand = new RelayCommand(ShowAbout);
            ShowStatisticsCommand = new RelayCommand(ShowStatistics);
            CancelCommand = new RelayCommand(CancelGame);
            SaveGameCommand = new RelayCommand(SaveGame);
            OpenGameCommand = new RelayCommand(OpenGame);

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
            Level = 1;
            StartNewGame();
        }
        private void ShowAbout(object parameter)
        {
            MessageBox.Show("Paleu Anca-Nicoleta 10LF243 Informatica", "About", MessageBoxButton.OK, MessageBoxImage.Information);
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

                Level = 1;
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

        private void ShowStatistics(object parameter)
        {
            bool wasTimerRunning = false;
            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
                wasTimerRunning = true;
            }

            View.StatisticsWindow statsWindow = new View.StatisticsWindow();
            statsWindow.DataContext = new StatisticsLogic(_currentUserObj);
            statsWindow.ShowDialog();

            if (wasTimerRunning)
            {
                _timer.Start();
            }
        }

        private void StartNewGame()
        {
            Mistakes = 0;
            TimeLeft = 30;

            _guessedLetters.Clear();
            CommandManager.InvalidateRequerySuggested();

            List<string> availableLines = new List<string>();

            for (int i = 0; i < _allLines.Count; i++)
            {
                string line = _allLines[i];
                string[] parts = line.Split('|');

                string categoryPart = parts[0];

                if (_currentCategory == "All Categories")
                {
                    availableLines.Add(line);
                }
                else if (categoryPart == _currentCategory.ToUpper())
                {
                    availableLines.Add(line);
                }
            }

            if (availableLines.Count > 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, availableLines.Count);
                string[] selectedParts = availableLines[index].Split('|');

                _secretWordCategory = selectedParts[0];
                _secretWord = selectedParts[1];
            }
            else
            {
                _secretWordCategory = "UNKNOWN";
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

            _isCurrentGameCounted = false;
        }
        private void SaveDataToFile()
        {
            if (_currentUserObj == null) return;

            try
            {
                string filePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "users.json");
                if (System.IO.File.Exists(filePath))
                {
                    string json = System.IO.File.ReadAllText(filePath);
                    var users = System.Text.Json.JsonSerializer.Deserialize<System.Collections.ObjectModel.ObservableCollection<User>>(json);

                    if (users != null)
                    {
                        var userToUpdate = users.FirstOrDefault(u => u.Name == _currentUserObj.Name);
                        if (userToUpdate != null)
                        {
                            userToUpdate.Statistics = _currentUserObj.Statistics;

                            userToUpdate.SavedGames = _currentUserObj.SavedGames;

                            string newJson = System.Text.Json.JsonSerializer.Serialize(users);
                            System.IO.File.WriteAllText(filePath, newJson);
                        }
                    }
                }
            }
            catch { }
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

                    if (Level == 3)
                    {
                        if (_currentUserObj != null && _currentUserObj.Statistics != null)
                        {
                            for (int i = 0; i < _currentUserObj.Statistics.Count; i++)
                            {
                                if (_currentUserObj.Statistics[i].CategoryName.ToUpper() == _secretWordCategory.ToUpper())
                                {
                                    _currentUserObj.Statistics[i].GamesWon++;
                                    _currentUserObj.Statistics[i].GamesPlayed++;
                                }
                            }
                            SaveDataToFile();
                        }

                        MessageBox.Show("You won the game!", "Victory");
                        Level = 1;
                    }
                    else
                    {
                        Level++;
                        MessageBox.Show("You guessed the word! Proceeding to level " + Level + ".");
                    }

                    StartNewGame();
                }
            }
            else
            {
                Mistakes = Mistakes + 1;
                if (Mistakes == 6)
                {
                    _timer.Stop();

                    if (_currentUserObj != null && _currentUserObj.Statistics != null)
                    {
                        for (int i = 0; i < _currentUserObj.Statistics.Count; i++)
                        {
                            if (_currentUserObj.Statistics[i].CategoryName.ToUpper() == _secretWordCategory.ToUpper())
                            {
                                _currentUserObj.Statistics[i].GamesPlayed++;
                            }
                        }
                        SaveDataToFile();
                    }

                    MessageBox.Show("Game Over! The word was: " + _secretWord, "Defeat");
                    Level = 1;
                    StartNewGame();
                }
            }
        }

        private void CancelGame(object parameter)
        {
            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            System.Windows.Window gameWindow = System.Windows.Application.Current.Windows.OfType<View.GameWindow>().FirstOrDefault();
            if (gameWindow != null)
            {
                gameWindow.Close();
            }
        }
        private void SaveGame(object parameter)
        {
            bool wasRunning = _timer != null && _timer.IsEnabled;
            if (wasRunning) _timer.Stop();

            View.SaveGameWindow saveWindow = new View.SaveGameWindow();
            SaveDialogLogic saveLogic = new SaveDialogLogic();

            saveWindow.DataContext = saveLogic;
            saveWindow.ShowDialog();

            if (saveLogic.IsConfirmed)
            {
                if (_currentUserObj.SavedGames == null)
                    _currentUserObj.SavedGames = new System.Collections.ObjectModel.ObservableCollection<SavedGame>();

                var existingSave = _currentUserObj.SavedGames.FirstOrDefault(s => s.SaveName.ToLower() == saveLogic.SaveName.ToLower());

                if (existingSave != null)
                {
                    MessageBox.Show("A saved game with this name already exists!\nPlease choose a different name.", "Duplicate Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    SavedGame newSave = new SavedGame
                    {
                        SaveName = saveLogic.SaveName,
                        SecretWord = _secretWord,
                        DisplayedWord = DisplayedWord,
                        Mistakes = Mistakes,
                        Level = Level,
                        TimeLeft = TimeLeft,
                        Category = _secretWordCategory,
                        GuessedLetters = new List<string>(_guessedLetters),
                        IsCounted = _isCurrentGameCounted
                    };

                    _currentUserObj.SavedGames.Add(newSave);
                    SaveDataToFile();
                    MessageBox.Show("Game saved successfully!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            if (wasRunning) _timer.Start();
        }
        private void OpenGame(object parameter)
        {
            bool wasRunning = _timer != null && _timer.IsEnabled;
            if (wasRunning) _timer.Stop();

            if (_currentUserObj.SavedGames == null || _currentUserObj.SavedGames.Count == 0)
            {
                MessageBox.Show("You have no saved games.", "Open Game", MessageBoxButton.OK, MessageBoxImage.Information);
                if (wasRunning) _timer.Start();
                return;
            }

            View.OpenGameWindow openWindow = new View.OpenGameWindow();
            OpenDialogLogic openLogic = new OpenDialogLogic(_currentUserObj.SavedGames);
            openWindow.DataContext = openLogic;
            openWindow.ShowDialog();

            if (openLogic.IsConfirmed && openLogic.SelectedGame != null)
            {
                LoadGameState(openLogic.SelectedGame);
            }
            else
            {
                if (wasRunning) _timer.Start();
            }
        }

        private void LoadGameState(SavedGame saved)
        {
            if (_timer != null) _timer.Stop();

            _secretWord = saved.SecretWord;
            DisplayedWord = saved.DisplayedWord;
            Mistakes = saved.Mistakes;
            Level = saved.Level;
            TimeLeft = saved.TimeLeft;
            _secretWordCategory = saved.Category;
            _guessedLetters = new List<string>(saved.GuessedLetters);
            _isCurrentGameCounted = saved.IsCounted;

            _currentCategory = _secretWordCategory == "UNKNOWN" ? "All Categories" : _secretWordCategory;
            IsAllChecked = _currentCategory == "All Categories";
            IsAnimalsChecked = _currentCategory == "ANIMALS";
            IsCountriesChecked = _currentCategory == "COUNTRIES";
            IsMoviesChecked = _currentCategory == "MOVIES";
            IsFruitsChecked = _currentCategory == "FRUITS";
            IsCarsChecked = _currentCategory == "CARS";

            CommandManager.InvalidateRequerySuggested();

            if (_timer != null) _timer.Start();
            _currentSaveName = saved.SaveName;
        }
    }
}