using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System.Reflection;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BoardController boardController;
        private readonly UserController userController;
        private readonly JsonConverter jsonConverter;

        public BoardService(UserController userController, BoardController boardController, JsonConverter jsonConverter)
        {
            this.userController = userController;
            this.boardController = boardController;
            this.jsonConverter = jsonConverter;
        }

        /// <summary>
        /// This method loads existing board and task data from the DB
        /// </summary>
        /// <returns>Load data from board and tasks tables, unless an error occurs (see <see cref="UserService"/>)</returns>
        public void LoadData()
        {
            boardController.LoadAllBoards();
        }

        /// <summary>
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string CreateBoard(string email, string boardName)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.CreateBoard(boardName, email);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} created board '{boardName}' successfully");
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in and an owner of the board.</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string DeleteBoard(string email, string boardName)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.DeleteBoard(boardName, email);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} deleted board '{boardName}' successfully");
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
        /// This method returns a column given it's ordinal
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                userController.IsLoggedUser(email);
                LinkedList<Task> tasks = boardController.GetColumn(email, boardName, columnOrdinal);
                Response<LinkedList<Task>> msg = new(null, tasks);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                userController.IsLoggedUser(email);
                string columnName = boardController.GetColumnName(email, boardName, columnOrdinal);
                Response<string> msg = new(null, columnName);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                userController.IsLoggedUser(email);
                int columnLimit = boardController.GetColumnLimit(email, boardName, columnOrdinal);
                ResponseInt msg = new(columnLimit);
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.LimitColumn(email, boardName, columnOrdinal, limit);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} limitted column '{columnOrdinal}', in board '{boardName}' successfully to {limit}");
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string AddUser(string email, int boardID)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.JoinBoard(boardID, email);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} joined board '{boardID}' successfully");
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string RemoveUser(string email, int boardID)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.LeaveBoard(boardID,email);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} left board '{boardID}' successfully");
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="BoardService"/>)</returns>
        public string SetOwner(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                userController.IsLoggedUser(currentOwnerEmail);
                boardController.SetOwner(currentOwnerEmail,newOwnerEmail, boardName);
                Response<string> msg = new(null, null);
                log.Info($"Email {currentOwnerEmail} transfered Ownership of the board to {newOwnerEmail}successfully");
                string json = jsonConverter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> error = new(e.Message, null);
                log.Error(e.Message);
                string json = jsonConverter.ToSerialize(error);
                return json;
            }
        }
    }
}