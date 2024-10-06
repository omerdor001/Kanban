using Frontend.Model;
using System.Collections.Generic;

namespace Frontend.ViewModel
{
    public class BoardTasksViewModel:NotifiableObject
    {
        public List<TaskModel> BackLogTasks { get; set; }
        public List<TaskModel> InProgressTasks { get; set; }
        public List<TaskModel> DoneTasks { get; set; }
        public string userEmail { get; set; }
        public string boardName { get; set; }
        public string Title { get; set; }
        private BackendController controller;

        public BoardTasksViewModel(string _userEmail,string _boardName, BackendController _controller)
        {
            Title = "Tasks for " + _boardName;
            controller = _controller;
            userEmail = _userEmail;
            boardName = _boardName;
            BackLogTasks = controller.GetBoardTasks(_userEmail,_boardName,0);
            InProgressTasks = controller.GetBoardTasks(_userEmail,_boardName,1);
            DoneTasks = controller.GetBoardTasks(_userEmail,_boardName,2);
        }
 
    }
}
