using System.Collections.ObjectModel;
using System.Windows.Input;
using Hangman.Model;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Hangman.ViewModel
{
    public class SignIn : ObservableObject
    {
        private ObservableCollection<User> _users;
        private User _selectedUser;

        private ICommand _newUserCommand;
        private ICommand _deleteUserCommand;
        private ICommand _playCommand;
        private ICommand _cancelCommand;

        private readonly string _filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "users.json");

        public SignIn()
        {
            Users = new ObservableCollection<User>();

            LoadUsers();
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged("Users"); }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand NewUserCommand
        {
            get { return _newUserCommand ?? (_newUserCommand = new RelayCommand(CreateNewUser)); }
        }

        public ICommand DeleteUserCommand
        {
            get { return _deleteUserCommand ?? (_deleteUserCommand = new RelayCommand(DeleteUser, CanDeleteOrPlay)); }
        }

        public ICommand PlayCommand
        {
            get { return _playCommand ?? (_playCommand = new RelayCommand(Play, CanDeleteOrPlay)); }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }

        private bool CanDeleteOrPlay(object parameter)
        {
            if (SelectedUser != null) { return true; }
            else { return false; }
        }

        private void CreateNewUser(object parameter)
        {
            var newUserWindow = new View.NewUser();

            if (newUserWindow.ShowDialog() == true)
            {
                Users.Add(newUserWindow.CreatedUser);

                SaveUsers();
            }
        }

        private void DeleteUser(object parameter)
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;

                SaveUsers();
            }
        }

        private void Play(object parameter)
        {
            if (SelectedUser != null)
            {
                View.GameWindow gameWindow = new View.GameWindow(SelectedUser);
                gameWindow.Show();

                System.Windows.Window mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (mainWindow != null)
                {
                    mainWindow.Close();
                }
            }
        }

        private void Cancel(object parameter)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveUsers()
        {
            string jsonString = JsonSerializer.Serialize(Users);

            File.WriteAllText(_filePath, jsonString);
        }

        private void LoadUsers()
        {
            if (File.Exists(_filePath))
            {
                string jsonString = File.ReadAllText(_filePath);

                var loadedUsers = JsonSerializer.Deserialize<ObservableCollection<User>>(jsonString);

                if (loadedUsers != null)
                {
                    Users = loadedUsers;
                }
            }
        }
    }
}