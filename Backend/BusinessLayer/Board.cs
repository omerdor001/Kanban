using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Board
    {
        private int id;
        private int taskIdCounter = 0;
        public string Name { get; set; }
        private string owner;
        private LinkedList<string> users;
        private LinkedList<Task> blList;
        private LinkedList<Task> ipList;
        private LinkedList<Task> doneList;
        private int blLimit;
        private int ipLimit;
        private int doneLimit;
        private readonly int BACKLOG_COLUMN_ORDINAL = 0;
        private readonly int INPROGRESS_COLUMN_ORDINAL = 1;
        private readonly int DONE_COLUMN_ORDINAL = 2;
        private BoardMapper boardMapper;

        public Board(int _id, string name, string _owner, BoardMapper _boardMapper)
        {
            id = _id;
            Name = name;
            users = new();
            blList = new();
            ipList = new();
            doneList = new();
            blLimit = -1;
            ipLimit = -1;
            doneLimit = -1;
            owner = _owner;
            boardMapper = _boardMapper;
        }

        public Board(int boardID, string boardName, LinkedList<string> _users, LinkedList<Task> _blList, LinkedList<Task> _ipList, LinkedList<Task> _doneList, int _blLimit, int _ipLimit, int _doneLimit, string ownerEmail, int taskCounter, BoardMapper _boardDalMapper)
        {
            id = boardID;
            Name = boardName;
            users = _users; 
            blList = _blList;
            ipList = _ipList;
            doneList = _doneList;
            blLimit = _blLimit;
            ipLimit = _ipLimit;
            doneLimit = _doneLimit;
            owner = ownerEmail;
            taskIdCounter = taskCounter;
            boardMapper= _boardDalMapper;
        }

        [JsonIgnore]
        /// <summary>
        /// Board id getter.
        /// </summary>
        public int Id { get { return id; } }

        [JsonIgnore]
        /// <summary>
        /// Board task id counter getter and setter.
        /// </summary>
        public int TaskIdCounter { get { return taskIdCounter; } set { taskIdCounter = value; } }

        /// <summary>
        /// Board owner getter and setter.
        /// </summary>
        public string Owner { get { return owner; } set { owner = value; } }

        [JsonIgnore]
        /// <summary>
        /// Board backlog list getter and setter.
        /// </summary>
        public LinkedList<string> Users { get { return users; } set { users = value; } }

        [JsonIgnore]
        /// <summary>
        /// Board backlog list getter and setter.
        /// </summary>
        public LinkedList<Task> BlList { get { return blList; } set { blList = value; } }

        [JsonIgnore]
        /// <summary>
        /// Board in progress list getter and setter.
        /// </summary>
        public LinkedList<Task> IpList { get { return ipList; } set { ipList = value; } }

        [JsonIgnore]
        /// <summary>
        /// Board done list getter and setter.
        /// </summary>
        public LinkedList<Task> DoneList { get { return doneList; } set { doneList = value; } }

        [JsonIgnore]
        /// <summary>
        /// Board back log limit getter and setter.
        /// </summary>
        public int BlLimit { get { return blLimit; } set { blLimit = value; } }

        [JsonIgnore]
        /// <summary>
        /// Board in progress limit getter and setter.
        /// </summary>
        public int IpLimit { get { return ipLimit; } set { ipLimit = value; } }

        [JsonIgnore]
        /// <summary>
        /// Board done limit getter and setter.
        /// </summary>
        public int DoneLimit { get { return doneLimit; } set { doneLimit = value; } }

        /// <summary>
        /// This method returns a column given its ordinal
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="email">The user email, must join board first</param>
        /// <returns>Returns list of the column's tasks, unless an error occurs  (see <see cref="Board"/>)</returns>
        public LinkedList<Task> GetColumn(int columnOrdinal,string email)
        {
            if (!ContainsUser(email))
                throw new Exception("User that is not in a board can not get the board's columns");
            if (columnOrdinal == BACKLOG_COLUMN_ORDINAL)
                return BlList;
            else if (columnOrdinal == INPROGRESS_COLUMN_ORDINAL)
                return IpList;
            else if (columnOrdinal == DONE_COLUMN_ORDINAL)
                return DoneList;
            else
                throw new Exception("Column does not exist");
        }

        /// <summary>
        /// This method returns the name of a given column's ordinal
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="email">The user email, must join board first</param>
        /// <returns>Returns a string of the column name, unless an error occurs (see <see cref="Board"/>)</returns>
        public string GetColumnName(int columnOrdinal,string email)
        {
            if (!ContainsUser(email))
                throw new Exception("User that is not in a board can not get the board's columns");
            if (columnOrdinal == BACKLOG_COLUMN_ORDINAL)
                return "backlog";
            else if (columnOrdinal == INPROGRESS_COLUMN_ORDINAL)
                return "in progress";
            else if (columnOrdinal == DONE_COLUMN_ORDINAL)
                return "done";
            else
                throw new Exception("Column does not exist");
        }

        /// <summary>
        /// This method gets the limit of a specific column given its ordinal.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="email">The user email, must join board first</param>
        /// <returns>Returns the column limit value, unless an error occurs (see <see cref="Board"/>)</returns>
        public int GetColumnLimit(int columnOrdinal, string email)
        {
            if (!ContainsUser(email))
                throw new Exception("User that is not in a board can not get the board's columns");
            if (columnOrdinal == BACKLOG_COLUMN_ORDINAL)
                return BlLimit;
            else if (columnOrdinal == INPROGRESS_COLUMN_ORDINAL)
                return IpLimit;
            else if (columnOrdinal == DONE_COLUMN_ORDINAL)
                return DoneLimit;
            else
                throw new Exception("Column does not exist");
        }

        /// <summary>
        /// This method sets the limit of a specific column given its ordinal.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="Limit">The new limit for the column</param>
        /// <param name="email">The user email, must join board first</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        public void EditLimit(int columnOrdinal, int limit, string email)
        {
            if (!ContainsUser(email))
                throw new Exception("User that is not in a board can not get the board's columns");
            if (limit < -1)
                throw new Exception("Limit must be non-negative");
            if (columnOrdinal == BACKLOG_COLUMN_ORDINAL)
            {
                if (limit < BlList.Count & limit != -1)
                    throw new Exception("Backlog column already contains more tasks than requested limit");
                else
                {
                    if (boardMapper.Update(Id, "BlLimit", BlLimit))
                        BlLimit = limit;
                }
            }
            else if (columnOrdinal == INPROGRESS_COLUMN_ORDINAL)
            {
                if (limit < IpList.Count & limit != -1)
                    throw new Exception("In Progress column already contains more tasks than requested limit");
                else
                {
                    if (boardMapper.Update(Id, "IpLimit", IpLimit))
                        IpLimit = limit;
                }
            }
            else if (columnOrdinal == DONE_COLUMN_ORDINAL)
            {
                if (limit < DoneList.Count & limit != -1)
                    throw new Exception("Done column already contains more tasks than requested limit");
                else
                {
                    if (boardMapper.Update(Id, "DoneLimit", DoneLimit))
                        DoneLimit = limit;
                }
            }
            else
                throw new Exception("Column does not exist");
        }

        /// <summary>
        /// This method adds a user to a board.
        /// </summary>
        /// <param name="userEmail">Email to add</param>
        /// <returns>Void function, unless an error occurs(see <see cref="Board"/>)</returns>
        public void AddUser(string userEmail)
        {
            if (userEmail == null)
                throw new Exception("Email is null");
            else
            {
                userEmail = userEmail.ToLower();
                users.AddLast(userEmail);
            }
        }

        /// <summary>
        /// This method delete a user from a board.
        /// </summary>
        /// <param name="userEmail">Email to check if in board</param>
        /// <returns>Void function, unless an error occurs(see <see cref="Board"/>)</returns>
        public void RemoveUser(string userEmail)
        {
            if (userEmail == null)
                throw new Exception("Email is null");
            else
            {
                userEmail = userEmail.ToLower();
                users.Remove(userEmail);
            }
        }

        /// <summary>
        /// This method transfers board ownership.
        /// </summary>
        /// <param name="currentOwner">Email of the current owner. Must be logged in</param>
        /// <param name="newEmail">Email of the new owner</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        public void SetOwner(string currentOwner, string newEmail)
        {
            currentOwner = currentOwner.ToLower();
            if (!owner.Equals(currentOwner))
                throw new Exception("Only owner can set ownership");
            if (ContainsUser(currentOwner) && ContainsUser(newEmail))
            {
                if (!boardMapper.Update(id, "OwnerEmail", newEmail))
                    throw new Exception("Failed to edit board owner in DB");
                newEmail = newEmail.ToLower();
                owner = newEmail;
            }
            else
                throw new Exception("Current owner and New owner must both be in the board");
        }

        /// <summary>
        /// This method checks if a user is in the board, and if he is logged in.
        /// </summary>
        /// <param name="userEmail">Email to check if in boar.</param>
        /// <returns>Returns true if a user is in the board and false if not(see <see cref="Board"/>)</returns>
        public bool ContainsUser(string userEmail)
        {
            if (userEmail == null)
                throw new Exception("Email is null");
            userEmail = userEmail.ToLower();
            foreach (string email in users)
            {
                if (email.Equals(userEmail))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// This method adds a new task to the backlog column of a board.
        /// </summary>
        /// <param name="dueDate">The duaDate of the task</param>
        /// <param name="title">The task title</param>
        /// <param name="description">The task description</param>
        /// <param name="email">The user email</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        public void AddTask(DateTime dueDate, string title, string description, string email, TaskMapper taskMapper)
        {
            if (!ContainsUser(email))
                throw new Exception("User which is not in board cannot add tasks to this board");
            if (title == null)
                throw new Exception("Title is null");
            title = title.Trim();
            if (title.Length == 0)
                throw new Exception("Title is empty");
            else if (title.Length > 50)
                throw new Exception("Title is too long");
            else if (description == null)
                throw new Exception("Description is null");
            description = description.Trim();
            if (description.Length > 300)
                throw new Exception("Description is too long");
            if (BlList.Count >= BlLimit & BlLimit != -1)
                throw new Exception("No more space for tasks in backlog column");
            if (DateTime.Compare(DateTime.Now, dueDate) > 0)
                throw new Exception("Illegal due date");
            if (!boardMapper.Update(id, "BoardIdTaskCounter", taskIdCounter + 1))
                throw new Exception("Failed to update board in DB");
            Task task = new(title, description, dueDate, taskIdCounter, taskMapper);
            task.InsertTask(id);
            BlList.AddLast(task);
            taskIdCounter++;
        }

        /// <summary>
        /// This method searches for a task in a given column.
        /// </summary>
        /// <param name="taskID">An id of the task</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Returns a task, unless an error occurs (see <see cref="Board"/>)</returns>
        public Task GetTask(int taskID, int columnOrdinal)
        {
            if (columnOrdinal == BACKLOG_COLUMN_ORDINAL)
                return GetTask(taskID, BlList);
            else if (columnOrdinal == INPROGRESS_COLUMN_ORDINAL)
                return GetTask(taskID, IpList);
            else if (columnOrdinal == DONE_COLUMN_ORDINAL)
                return GetTask(taskID, DoneList);
            else
                throw new Exception("Invalid column ordinal");
        }

        /// <summary>
        /// This method searches for a task in a list of tasks.
        /// </summary>
        /// <param name="taskID">Id of the task</param>
        /// <param name="taskList">A list of tasks</param>
        /// <returns>Returns a task, unless an error occurs (see <see cref="Board"/>)</returns>
        private Task GetTask(int taskID, LinkedList<Task> taskList)
        {
            LinkedListNode<Task> task = taskList.First;
            while (task != null)
            {
                if (task.Value.Id == taskID)
                    return task.Value;
                task = task.Next;
            }
            throw new Exception("Task not found");
        }

        /// <summary>
        /// This method advances a task to the next column of the board.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskID">A task id to move to the next column</param>
        /// <param name="email">The user email, must join board first</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        public void MoveTask(int columnOrdinal, int taskID, string email)
        {
            email = email.ToLower();
            if (columnOrdinal == BACKLOG_COLUMN_ORDINAL)
            {
                if (IpList.Count >= IpLimit & IpLimit != -1)
                    throw new Exception("No more space for tasks in in progress column");
                Task task = GetTask(taskID, BlList);
                if ((task.Assignee == null) || (!task.Assignee.Equals(email)))
                    throw new Exception("Only the assignee can move tasks");
                task.MoveTask(id, INPROGRESS_COLUMN_ORDINAL);
                IpList.AddLast(task);
                BlList.Remove(task);
            }
            else if (columnOrdinal == INPROGRESS_COLUMN_ORDINAL)
            {
                if (DoneList.Count >= DoneLimit & DoneLimit != -1)
                    throw new Exception("No more space for tasks in done column");
                Task task = GetTask(taskID, IpList);
                if ((task.Assignee == null) || (!task.Assignee.Equals(email)))
                    throw new Exception("Only the assignee can move tasks");
                task.MoveTask(id, DONE_COLUMN_ORDINAL);
                DoneList.AddLast(task);
                IpList.Remove(task);
            }
            else if (columnOrdinal == DONE_COLUMN_ORDINAL)
                throw new Exception("Cannot move tasks from done column");
            else
                throw new Exception("Invalid column ordinal");
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary> 
        /// <param name="email">The user email. Must be logged in and join board</param>
        /// <param name="emailAssignee">Email of the user to asssign. Must join board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskID">An id of the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        public void AssignTask(string email, string emailAssignee, int columnOrdinal, int taskID)
        {
            if (columnOrdinal == DONE_COLUMN_ORDINAL)
                throw new Exception("Cannot edit done tasks");
            if (!ContainsUser(email) || !ContainsUser(emailAssignee))
                throw new Exception("Both users must be in the board to assign tasks");
            Task task = GetTask(taskID, columnOrdinal);
            if (task.Assignee != null && !task.Assignee.Equals(email))
                throw new Exception("User is not allowed to assign this task");
            emailAssignee = emailAssignee.ToLower();
            task.AssignTask(id, emailAssignee);
        }

        /// <summary>
        /// This method updates the title of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in and join board</param>
        /// <param name="newTitle">A new title for the task</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskID">The id of the task</param>
        /// <returns>Void function which update task title, unless an error occurs (see <see cref="Board"/>)</returns>
        public void EditTaskTitle(string email,string newTitle, int columnOrdinal, int taskID)
        {
            Task task = GetTask(taskID, columnOrdinal);
            CheckLegalColumnEdit(columnOrdinal);
            task.EditTaskTitle(email,id,newTitle);
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in and join board</param>
        /// <param name="newDescription">A new description for the task</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskID">The id of the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        public void EditTaskDescription(string email,string newDescription, int columnOrdinal, int taskID)
        {
            Task task = GetTask(taskID, columnOrdinal);
            CheckLegalColumnEdit(columnOrdinal);
            task.EditTaskDescription(email,id,newDescription);
        }

        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in and join board</param>
        /// <param name="newDueDate">A new due date for the task</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskID">The id of the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        public void EditTaskDueDate(string email,DateTime newDueDate, int columnOrdinal, int taskID)
        {
            Task task = GetTask(taskID, columnOrdinal);
            CheckLegalColumnEdit(columnOrdinal);
            task.EditTaskDueDate(email,id, newDueDate);
        }

        /// <summary>
        /// This method checks if the task is in a legal column in order to edit task information
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Void function, unless an error occurs (see <see cref="Board"/>)</returns>
        private void CheckLegalColumnEdit(int columnOrdinal)
        {
            if (columnOrdinal == DONE_COLUMN_ORDINAL)
                throw new Exception("Cannot edit done tasks");
            if (columnOrdinal < BACKLOG_COLUMN_ORDINAL || columnOrdinal > INPROGRESS_COLUMN_ORDINAL)
                throw new Exception("Illegal column ID");
        }
    }
}