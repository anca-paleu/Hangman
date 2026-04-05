using Hangman.ViewModel;
using Hangman.ViewModel;

namespace Hangman.Model
{
    public class User : ObservableObject
    {
        private string _name;
        private string _profileImagePath;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string ProfileImagePath
        {
            get { return _profileImagePath; }
            set
            {
                _profileImagePath = value;
                OnPropertyChanged();
            }
        }

        public User(string name, string profileImagePath)
        {
            Name = name;
            ProfileImagePath = profileImagePath;
        }
    }
}