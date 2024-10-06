using System;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        private int id;
        private DateTime creationTime;
        private string title;
        private string description;
        private DateTime dueDate;
        private string assignee;
        private readonly int TITLE_LENGTH = 50;
        private readonly int DESCRIPTION_LENGTH = 300;
        private TaskMapper taskMapper;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get and set for id of task.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Get and set for creation time of task.
        /// </summary>
        public DateTime CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }

        /// <summary>
        /// Get and set for title of task.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Get and set for description of task.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Get and set for due date of task.
        /// </summary>
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        /// <summary>
        /// Get and set for assignee of task.
        /// </summary>
        public string Assignee
        {
            get { return assignee; }
            set { assignee = value; }
        }

        public Task(string title, string description, DateTime dueDate, int id, TaskMapper _taskDalMapper)
        {
            CreationTime = DateTime.Now;
            DueDate = dueDate;
            Title = title;
            Description = description;
            Id = id;
            assignee = null;
            taskMapper = _taskDalMapper;
        }

        public Task(string title, string description, DateTime dueDate, int id, string _assigee, TaskMapper _taskDalMapper, DateTime creationTime)
        {
            CreationTime = creationTime;
            DueDate = dueDate;
            Title = title;
            Description = description;
            Id = id;
            assignee = _assigee;
            taskMapper = _taskDalMapper;
        }


        /// <summary>
        /// This method adds a new task to the board.
        /// </summary>
        /// <param name="boardID">The board's ID</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Task"/>)</returns>
        public void InsertTask(int boardID)
        {
            if (!taskMapper.InsertTask(new(id, boardID, creationTime, title, description, dueDate, Assignee, 0)))
                throw new Exception("Failed to insert task to DB");
            log.Info("Success in inserting task into DB");
        }

        /// <summary>
        /// This method assigns a task to a user.
        /// </summary> 
        /// <param name="boardID">The board's ID</param>
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Task"/>)</returns>
        public void AssignTask(int boardID, string emailAssignee)
        {
            if (taskMapper.Update(boardID, id, "Assignee", emailAssignee))
            {
                emailAssignee = emailAssignee.ToLower();
                Assignee = emailAssignee;
                log.Info("Success in updating assignee in DB");
            }
            else
            {
                log.Error("Failure in assigning task in DB");
                throw new Exception("Failed to assign task in DB");
            }
        }

        /// <summary>
        /// This method updates the title of a task.
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <param name="boardID">The board's ID</param>
        /// <param name="newTitle">A new title for the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Task"/>)</returns>
        public void EditTaskTitle(string email,int boardID,string newTitle)
        {
            email = email.ToLower();
            if (!email.Equals(assignee))
                throw new Exception("Only the assignee can edit task");
            if (newTitle == null)
                throw new Exception("New title is null");
            newTitle = newTitle.Trim();
            if (newTitle.Length == 0 || newTitle.Length > TITLE_LENGTH)
                throw new Exception("Invalid title");
            if (taskMapper.Update(boardID, id, "Title", newTitle)){
                Title = newTitle;
                log.Info("Success in updating title in DB");
            }
            else{
                log.Error("Failure in edition title in DB");
                throw new Exception("Failed to edit title in DB");
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <param name="boardID">The board's ID</param>
        /// <param name="newDescription">A new description for the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Task"/>)</returns>
        public void EditTaskDescription(string email,int boardID,string newDescription)
        {
            email = email.ToLower();
            if (!email.Equals(assignee))
                throw new Exception("Only the assignee can edit task");
            if (newDescription == null)
                throw new Exception("New description is null");
            newDescription = newDescription.Trim();
            if (newDescription.Length > DESCRIPTION_LENGTH)
                throw new Exception("Invalid Description");
            if (taskMapper.Update(boardID, id, "Description", newDescription))
            {
                Description = newDescription;
                log.Info("Success in updating description in DB");
            }
            else{
                log.Error("Failure in edition description in DB");
                throw new Exception("Failed to edit description in DB");
            }
        }

        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <param name="boardID">The board's ID</param>
        /// <param name="newDueDate">A new due date for the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Task"/>)</returns>
        public void EditTaskDueDate(string email,int boardID, DateTime newDueDate)
        {
            email=email.ToLower();
            if (!email.Equals(assignee))
                throw new Exception("Only the assignee can edit task");
            if (newDueDate.CompareTo(DateTime.Now) <= 0){
                throw new Exception("Not a valid due date");
            }
            if (taskMapper.Update(boardID, id, "DueDate", newDueDate))
            {
                log.Info("Success in updating due date in DB");
                DueDate = newDueDate;
            }
            else {
                log.Error("Failure in edition due date in DB");
                throw new Exception("Failed to edit due date in DB");
            }
        }

        /// <summary>
        /// This method advances a task to the next column.
        /// </summary>
        /// <param name="boardID">The board's ID</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Task"/>)</returns>
        public void MoveTask(int boardID, int columnOrdinal)
        {
            if (!taskMapper.Update(boardID, id, "Column", columnOrdinal)){
                log.Error("Failure in moving task in DB");
                throw new Exception("Failed to move task in DB");
            }
            log.Info("Success moving task in DB");
        }
    }
}