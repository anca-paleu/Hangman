using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Hangman.Model;

namespace Hangman.ViewModel
{
    public class NewUserLogic : ObservableObject
    {
        private List<string> _availableAvatars;
        private int _currentAvatarIndex = 0;
        private string _username;
        private BitmapImage _currentAvatar;

        public User CreatedUser { get; private set; }
        public bool Saved { get; private set; }
        public Action CloseAction { get; set; }

        private ICommand _prevCommand;
        private ICommand _nextCommand;
        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        public BitmapImage CurrentAvatar
        {
            get { return _currentAvatar; }
            set { _currentAvatar = value; OnPropertyChanged(); }
        }

        public ICommand PrevCommand
        {
            get
            {
                if (_prevCommand == null)
                {
                    _prevCommand = new RelayCommand(Prev);
                }
                return _prevCommand;
            }
        }

        public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(Next);
                }
                return _nextCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(Save);
                }
                return _saveCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(Cancel);
                }
                return _cancelCommand;
            }
        }

        public NewUserLogic()
        {
            _availableAvatars = new List<string>();
            Saved = false;
            LoadAvailableAvatars();
            UpdateAvatarDisplay();
        }

        private void LoadAvailableAvatars()
        {
            if (_availableAvatars != null)
            {
                _availableAvatars.Clear();
            }

            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string imagesPath = Path.Combine(baseFolder, "Images");

            for (int i = 0; i <= 9; i++)
            {
                string fullPath = Path.Combine(imagesPath, "avatar" + i + ".jpg");

                if (File.Exists(fullPath))
                {
                    _availableAvatars.Add(fullPath);
                }
            }

            if (_availableAvatars.Count > 0)
            {
                _currentAvatarIndex = 0;
                User newUser = new User(Username, _availableAvatars[_currentAvatarIndex]);
            }
        }

        private void UpdateAvatarDisplay()
        {
            if (_availableAvatars.Count > 0)
            {
                string currentPath = _availableAvatars[_currentAvatarIndex];
                CurrentAvatar = new BitmapImage(new Uri(currentPath, UriKind.Absolute));
            }
        }

        private void Prev(object parameter)
        {
            if (_availableAvatars.Count > 0)
            {
                _currentAvatarIndex--;
                if (_currentAvatarIndex < 0)
                {
                    _currentAvatarIndex = _availableAvatars.Count - 1;
                }
                UpdateAvatarDisplay();
            }
        }

        private void Next(object parameter)
        {
            if (_availableAvatars.Count > 0)
            {
                _currentAvatarIndex++;
                if (_currentAvatarIndex >= _availableAvatars.Count)
                {
                    _currentAvatarIndex = 0;
                }
                UpdateAvatarDisplay();
            }
        }

        private void Save(object parameter)
        {
            if (string.IsNullOrEmpty(Username))
            {
                System.Windows.MessageBox.Show("Please enter a username!");
                return;
            }
            else if (Username.Contains(" "))
            {
                System.Windows.MessageBox.Show("Username must be a single word!");
                return;
            }
            else if (_availableAvatars.Count == 0)
            {
                System.Windows.MessageBox.Show("Cannot save without an avatar.");
                return;
            }
            else
            {
                string selectedPath = _availableAvatars[_currentAvatarIndex];
                string relativePath = Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, selectedPath);
                CreatedUser = new User(Username, relativePath);
                Saved = true;
                if (CloseAction != null)
                {
                    CloseAction();
                }
            }
        }

        private void Cancel(object parameter)
        {
            Saved = false;
            if (CloseAction != null)
            {
                CloseAction();
            }
        }
    }
}