using System.Windows;
using Hangman.Model;

namespace Hangman.View
{
    public partial class NewUser : Window
    {
        public User CreatedUser { get; private set; }

        public NewUser()
        {
            InitializeComponent();

            ViewModel.NewUserLogic logic = new ViewModel.NewUserLogic();
            logic.CloseAction = () =>
            {
                if (logic.Saved == true)
                {
                    CreatedUser = logic.CreatedUser;
                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }
                this.Close();
            };

            this.DataContext = logic;
        }
    }
}