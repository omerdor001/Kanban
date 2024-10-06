using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;

namespace Frontend.Model
{
    public class BackendController
    {
        private ServiceFactory serviceFactory { get; set; }
        public BackendController()
        {
            serviceFactory = new ServiceFactory();
            serviceFactory.LoadData();
        }

        /// <summary>
        /// This method registers a new user to the system
        /// </summary>
        /// <param name="userEmail">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BackendController"/>)</returns>
        public UserLoginModel Register(string userEmail, string password)
        {
            if (password == null)
                throw new Exception("password is null");
            if (userEmail == null)
                throw new Exception("Email is null");
            string user = serviceFactory.userService.Register(userEmail, password);
            Response<string> userResponse = serviceFactory.Converter.ToDeSerialize<Response<string>>(user);
            if (userResponse.ErrorMessage != null)
            {
                throw new Exception(userResponse.ErrorMessage);
            }
            return new UserLoginModel(this, userEmail);
        }

        /// <summary>
        /// This method logs in an existing user
        /// </summary>
        /// <param name="userEmail">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A user login model, with the user's email, unless an error occurs (see <see cref="BackendController"/>)</returns>
        public UserLoginModel Login(string userEmail, string password)
        {
            if (password == null)
                throw new Exception("password is null");
            if (userEmail == null)
                throw new Exception("Email is null");
            string user = serviceFactory.userService.Login(userEmail, password);
            Response<string> userResponse=serviceFactory.Converter.ToDeSerialize<Response<string>>(user);
            if (userResponse.ErrorMessage!=null)
            {
                throw new Exception(userResponse.ErrorMessage);
            }
            return new UserLoginModel(this, userEmail);
        }

        /// <summary>
        /// This method returns a list of all the user's boards
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>A list of all user's board models, unless an error occurs (see <see cref="BackendController"/>)</returns>
        public List<BoardModel> GetUserBoards(string userEmail)
        {
            string boards = serviceFactory.userService.GetUserBoards(userEmail);
            if (boards != null)
            {
                Response<List<BoardModel>> boardsResponse = serviceFactory.Converter.ToDeSerialize<Response<List<BoardModel>>>(boards);
                if (boardsResponse.ErrorMessage != null)
                {
                    throw new Exception(boardsResponse.ErrorMessage);
                }
                if (boardsResponse.ReturnValue != null)
                {
                    return boardsResponse.ReturnValue;
                }
                else return new List<BoardModel>();
            }
            return new List<BoardModel>();
        }

        /// <summary>
        /// This method returns a task in column given it's ordinal
        /// </summary>
        /// <param name="userEmail">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A list of the column's task models, unless an error occurs (see <see cref="BackendController"/>)</returns>
        public List<TaskModel> GetBoardTasks(string userEmail,string boardName, int columnOrdinal)
        {
            string tasks = serviceFactory.boardService.GetColumn(userEmail,boardName, columnOrdinal);
            if (tasks != null)
            {
                Response<List<TaskModel>> boardsResponse = serviceFactory.Converter.ToDeSerialize<Response<List<TaskModel>>>(tasks);
                if (boardsResponse.ErrorMessage != null)
                {
                    throw new Exception(boardsResponse.ErrorMessage);
                }
                if (boardsResponse.ReturnValue != null)
                {
                    return boardsResponse.ReturnValue;
                }
                else return new List<TaskModel>();
            }
            return new List<TaskModel>();
        }
    }
}
