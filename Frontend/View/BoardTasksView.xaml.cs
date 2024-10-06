using Frontend.Model;
using Frontend.ViewModel;
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
    /// Interaction logic for BoardTasksView.xaml
    /// </summary>
    public partial class BoardTasksView : Window
    {
        private BoardTasksViewModel viewModel;
        public BoardTasksView(string _userEmail,string _boardName ,BackendController _controller)
        {
            InitializeComponent();
            viewModel = new(_userEmail, _boardName, _controller);
            DataContext = viewModel;
        }
    }
}
