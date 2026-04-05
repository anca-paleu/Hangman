using System.Windows;
using Hangman.ViewModel;

namespace Hangman
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new SignIn();
        }
    }
}