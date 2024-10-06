using Frontend.Model;
using Frontend.View;
using Frontend.ViewModel;
using System;
using System.Windows;


namespace Frontend
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            viewModel = (MainViewModel)DataContext;
        }

        /// <summary>
        /// This method logs a user
        /// </summary>
        /// <returns>void function, unless an error occurs (see <see cref="MainWindow"/>)</returns>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserLoginModel u = viewModel.Login();
            if (u != null)
            {
                UserBoardsView boardView = new UserBoardsView(u);
                boardView.Show();
                Close();
            }
        }

        /// <summary>
        /// This method register a user
        /// </summary>
        /// <returns>void function, unless an error occurs (see <see cref="MainWindow"/>)</returns>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserLoginModel u = viewModel.Register();
            if (u != null)
            {
                UserBoardsView boardView = new UserBoardsView(u);
                boardView.Show();
                Close();
            }
        }
    }
}
