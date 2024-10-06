using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardTasksModel
    {
        public string userEmail { get; set; }
        public string boardName { get; set; }
        public BoardTasksModel(string _userEmail, string _boardName)
        {
            userEmail = _userEmail;
            boardName = _boardName;
        }
    }
}
