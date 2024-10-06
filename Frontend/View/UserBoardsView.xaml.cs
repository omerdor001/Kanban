using Frontend.Model;
using Frontend.ViewModel;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for UserBoardsView.xaml
    /// </summary>
    public partial class UserBoardsView : Window
    {
        private BoardUsersViewModel viewModel;

        public UserBoardsView(UserLoginModel userLogin)
        {
            InitializeComponent();
            viewModel = new(userLogin.Email,userLogin.Controller);
            DataContext = viewModel;
        }

        /// <summary>
        /// This method shows a view of boards of user
        /// </summary>
        /// <returns>void function, unless an error occurs (see <see cref="UserBoardsModel"/>)</returns>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel.SelectedBoard == null)
            {
                return;
            }
            BoardTasksView boardTasksView = new(viewModel.SelectedBoard.Owner,viewModel.SelectedBoard.Name,viewModel.Controller);
            boardTasksView.Show();
            Close();
        }
    }
}
