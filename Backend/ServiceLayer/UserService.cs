using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Reflection;
using log4net;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private readonly UserController userController;
        private readonly JsonConverter jsonConverter;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserService(UserController userController, JsonConverter jsonConverter)
        {
            this.userController = userController;
            this.jsonConverter = jsonConverter;
        }

        /// <summary>
        /// This method loads registered users from the DB
        /// </summary>
        /// <returns>Load data from users table, unless an error occurs (see <see cref="UserService"/>)</returns>
        public void LoadData()
        {
                userController.LoadAllUsers();
        }

        /// <summary>
        /// This method registers a new user to the system
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="UserService"/>)</returns>
        public string Register(string email, string password)
        {
            try
            {
                userController.CreateUser(email, password);
                Response<string> msg = new(null, null);
                log.Debug(msg.ReturnValue);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
        }

        /// <summary>
        /// This method logs in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="UserService"/>)</returns>
        public string Login(string email, string password)
        {
            try
            {
                userController.Login(email, password);
                Response<string> msg = new(null, email);
                log.Info($"User '{email}' logged in successfully");
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
        }

        /// <summary>
        /// This method logs out a logged in user 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="UserService"/>)</returns>
        public string Logout(string email)
        {
            try
            {
                userController.Logout(email);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} logged in successfully");
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
        }

        /// <summary>
        /// This method returns a list of IDs of all the user's boards
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="UserService"/>)</returns>
        public string GetUserBoardsID(string email)
        {
            try
            {
                userController.IsLoggedUser(email);
                LinkedList<int> boards = userController.GetUserBoardsID(email);
                Response<LinkedList<int>> msg = new(null, boards);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
        }

        /// <summary>
        /// This method returns a list of all the user's boards
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of all user's boards, unless an error occurs (see <see cref="UserService"/>)</returns>
        public string GetUserBoards(string email)
        {
            try
            {
                userController.IsLoggedUser(email);
                LinkedList<Board> boards = userController.GetUserBoards(email);
                Response<LinkedList<Board>> msg = new(null, boards);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
        }

        /// <summary>
        /// This method returns all in-progress tasks of a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="UserService"/>)</returns>
        public string InProgressTasks(string email)
        {
            try
            {
                userController.IsLoggedUser(email);
                LinkedList<Task> tasks = userController.GetInProgress(email);
                Response<LinkedList<Task>> msg = new(null, tasks);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
        }

    }
}