using System.Windows;
using Hangman.Model;

namespace Hangman.View
{
    public partial class GameWindow : Window
    {
        public GameWindow(User currentUser)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.GameLogic(currentUser);
        }
    }
}