using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BoardController boardController;
        private readonly UserController userController;
        private readonly JsonConverter jsonConverter;

        public TaskService(UserController userController, BoardController boardController, JsonConverter jsonConverter)
        {
            this.userController = userController;
            this.boardController = boardController;
            this.jsonConverter = jsonConverter;
        }

        /// <summary>
        /// This method adds a new task
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="TaskService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.AddTask(email, boardName, title , description,dueDate);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} added task '{title}', '{description}', '{dueDate}', to board '{boardName}', successfully");
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
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="TaskService"/>)</returns>
        public string MoveTask(string email, string boardName, int columnOrdinal, int taskID)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.MoveTask(email,boardName,columnOrdinal,taskID);
                Response<string> msg = new(null, null);
                log.Info($"Email {email} moved task '{taskID}' ,from column '{columnOrdinal}' successfully in board '{boardName}'");
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
        /// This method updates the title of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newTitle">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="TaskService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string newTitle)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.EditTaskTitle(email,boardName,columnOrdinal, taskId, newTitle);
                Response<string> msg = new(null, null);
                log.Info($"user '{email}' updated title of task '{taskId}', in board '{boardName}', in column '{columnOrdinal}' to '{newTitle}'");
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
        /// This method updates the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newDescription">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="TaskService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string newDescription)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.EditTaskDescription(email,boardName,columnOrdinal,taskId,newDescription);
                Response<string> msg = new(null, null);
                log.Info($"user '{email}' updated description of task '{taskId}', in board '{boardName}', in column '{columnOrdinal}' to '{newDescription}'");
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
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newDueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="TaskService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            try
            {
                userController.IsLoggedUser(email);
                boardController.EditTaskDueDate(email,boardName,columnOrdinal,taskId,newDueDate);
                Response<string> msg = new(null, null);
                log.Info($"user '{email}' updated due date of task '{taskId}', in board '{boardName}', in column '{columnOrdinal}' to '{newDueDate}' ");
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
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="TaskService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try {
                userController.IsLoggedUser(email);
                boardController.AssignTask(email,boardName,columnOrdinal,taskID,emailAssignee);
                Response<string> msg = new(null, null);
                log.Info($"user '{email}' assigned '{taskID}', in the board, in column '{columnOrdinal}' to '{emailAssignee}' ");
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
