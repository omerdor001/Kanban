using Frontend.Model;
using System.Collections.Generic;

namespace Frontend.ViewModel
{
    public class BoardUsersViewModel : NotifiableObject
    {
        private BackendController controller;
        public List<BoardModel> Boards { get; set; }
        public string Title { get; private set; }
        private BoardModel _selectedBoard;
        private bool _enableForward = false;

        public BoardUsersViewModel(string userEmail,BackendController _controller)
        {
            controller = _controller;
            Title = "Boards for " + userEmail;
            Boards = controller.GetUserBoards(userEmail);
        }

        /// <summary>
        /// Controller getter.
        /// </summary>
        public BackendController Controller { get { return controller; } }

        /// <summary>
        /// Selected board getter and setter.
        /// </summary>
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }

        /// <summary>
        /// EnableForward getter and setter.
        /// </summary>
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
    }
}
