using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardController
    {
        private int _boardId = 0;
        private readonly Dictionary<int, Board> boards;
        private BoardMapper boardMapper;
        private BoardIdCounterMapper boardIdCounterMapper;
        private TaskMapper taskMapper;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController()
        {
            boards = new();
            boardMapper = new();
            boardIdCounterMapper = new();
            taskMapper = new();
        }

        /// <summary>
        /// Boards collection getter.
        /// </summary>
        public Dictionary<int, Board> Boards
        {
            get { return boards; }
        }

        /// <summary>
        /// This method loads existing boards from DB to the boards collection.
        /// </summary>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void LoadAllBoards()
        {
            BoardIdCounterDTO boardIdCounterDTO = boardIdCounterMapper.SelectBoardCounterId();
            _boardId = boardIdCounterDTO.NextID;
            List<BoardDTO> boards = boardMapper.SelectAllBoards();
            foreach (BoardDTO board in boards)
            {
                LinkedList<Task> backlog = new LinkedList<Task>();
                LinkedList<Task> inProgress = new LinkedList<Task>();
                LinkedList<Task> done = new LinkedList<Task>();
                List<TaskDTO> tasks = taskMapper.SelectAllTasks(board.Id);
                LinkedList<string> users = boardMapper.SelectAllUsers(board.Id);
                foreach (TaskDTO task in tasks)
                {
                    string assigee = task.Assignee;
                    if (task.Assignee.Equals("0"))
                        assigee = null;
                    if (task.Column == 0)
                        backlog.AddLast(new Task(task.Title, task.Description, task.DueDate, task.Id, assigee, taskMapper, task.CreationTime));
                    else if (task.Column == 1)
                        inProgress.AddLast(new Task(task.Title, task.Description, task.DueDate, task.Id, assigee, taskMapper, task.CreationTime));
                    else
                        done.AddLast(new Task(task.Title, task.Description, task.DueDate, task.Id, assigee, taskMapper, task.CreationTime));
                }
                LoadBoard(board.Id, board.Name, users, backlog, inProgress, done, board.BlLimit, board.IpLimit, board.DoneLimit, board.OwnerEmail, board.BoardIdTaskCounter);
            }
        }

        /// <summary>
        /// This method constructs a single board from the DTO.
        /// </summary>
        /// <param name="boardID">The board's ID</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="_users">The list of users who belong to the board</param>
        /// <param name="_blList">The list of tasks belonging to back log column</param>
        /// <param name="_ipList">The list of tasks belonging to in progress column</param>
        /// <param name="_doneList">The list of tasks belonging to in progress column</param>
        /// <param name="_blLimit">The limit of back log column</param>
        /// <param name="_ipLimit">The limit of in progress column</param>
        /// <param name="_ipLimit">The limit of done column</param>
        /// <param name="ownerEmail">The board owner's email</param>
        /// <param name="taskCounter">The counter of the next task id</param>
        /// <returns>Void fuction, unless an error occurs (see <see cref="BoardController"/>)</returns>
        private void LoadBoard(int boardID, string boardName, LinkedList<string> _users, LinkedList<Task> _blList, LinkedList<Task> _ipList, LinkedList<Task> _doneList, int _blLimit, int _ipLimit, int _doneLimit, string ownerEmail, int taskCounter)
        {
            Board newBoard = new(boardID, boardName, _users, _blList, _ipList, _doneList, _blLimit, _ipLimit, _doneLimit, ownerEmail, taskCounter, boardMapper);
            boards.Add(_boardId, newBoard);
            _boardId++;
        }

        /// <summary>
        /// This method creates a new board.
        /// </summary>
        /// <param name="boardName">The name of the new board</param>
        /// <param name="email">The user email, must be logged in</param>
        /// <returns>Void fuction, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void CreateBoard(string boardName, string email)
        {
            if (boardName == null)
            {
                throw new Exception("Board name is null");
            }
            boardName = boardName.Trim();
            if (boardName.Length == 0)
            {
                throw new Exception("Board name cannot be empty");
            }
            foreach (Board board in boards.Values)
            {
                if (board.ContainsUser(email))
                {
                    if (boardName.Equals(board.Name))
                    {
                        log.Error("Board name already exists");
                        throw new Exception("Board with this name exists already");
                    }
                        
                }
            }
            email = email.ToLower();
            Board newBoard = new(_boardId, boardName, email, boardMapper);
            if (!boardMapper.InsertBoard(new BoardDTO(newBoard.Id, boardName, newBoard.BlLimit, newBoard.IpLimit, newBoard.DoneLimit, email, 0))) {
                log.Error("Failure in insertion board into DB");
                throw new Exception("Failed to insert board into DB");
            }
            log.Info("Success in inserting board into Boards table in DB");  
            if (!boardMapper.InsertUser(email, _boardId))
            {
                boardMapper.Delete(_boardId);
                log.Error("Failure in insertion boardUser into DB");
                throw new Exception("Failed to insert into boardUser");
            }
            log.Info("Success in inserting user into BoardUsers table in DB");
            if (newBoard.Id == 0)
            {
                if (!boardIdCounterMapper.InsertBoardIdCounter(new BoardIdCounterDTO(_boardId)))
                {
                    boardMapper.Delete(_boardId);
                    boardMapper.DeleteUser(email, _boardId);
                    log.Error("Failure in insertion boardIDCounter into DB");
                    throw new Exception("Failed to insert into boardIDCounter");
                }
                log.Info("Success in inserting user into BoardIdCounter in DB");
            }
            else if (!boardIdCounterMapper.Update(_boardId - 1, "Id", _boardId))
            {
                boardMapper.Delete(_boardId);
                boardMapper.DeleteUser(email, _boardId);
                log.Error("Failure in updating boardIDCounter");
                throw new Exception("Failed to update boardIDCounter");
            }
            log.Info("Success in updating boardIDCounter");
            newBoard.AddUser(email);
            boards.Add(_boardId, newBoard);
            _boardId++;
        }

        /// <summary>
        /// This method deletes a board from the user's boards
        /// </summary>
        /// <param name="boardName">The name of the board to delete</param>
        /// <param name="email">The user email. Must be logged in, join board and be the owner of the board</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void DeleteBoard(string boardName, string email)
        {
            email = email.ToLower();
            if (boardName == null)
            {
                throw new Exception("Board name is null");
            }
            Board board = GetBoard(boardName, email);
            List<TaskDTO> tasks = taskMapper.SelectAllTasks(_boardId);
            LinkedList<string> boardUsers = boardMapper.SelectAllUsers(board.Id);
            if (boardMapper.DeleteBoard(board.Id))
            {
                if (taskMapper.DeleteTasksByBoardID(board.Id))
                {
                    if (!boardMapper.Delete(board.Id))
                    {
                        foreach (TaskDTO taskDTO in tasks)
                            taskMapper.InsertTask(taskDTO);
                        foreach (string boardUser in boardUsers)
                            boardMapper.InsertUser(boardUser, _boardId);
                        log.Error("Failure in boardUser rows DB deletion");
                        throw new Exception("Failed to delete boardUser rows DB");
                    }
                    boards.Remove(board.Id);
                    log.Info("Board deleted successfully");
                    log.Info("Tsks from board deleted successfully");
                }
                else
                {
                    foreach (string boardUser in boardUsers)
                        boardMapper.InsertUser(boardUser, _boardId);
                    log.Error("Failure in task rows DB deletion");
                    throw new Exception("Failed to delete task rows DB");
                }
                log.Info("Success in deleting board from BoardUser table");
            }
            else{
                log.Error("Failure in updating DB");
                throw new Exception("Failed to update DB");
            }
        }

        /// <summary>
        /// This method returns by its name and user email.
        /// </summary>
        /// <param name="boardName">The name of the board</param>
        /// <param name="email">The email of the user</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        private Board GetBoard(string boardName, string email)
        {
            email = email.ToLower();
            foreach (Board board in boards.Values)
            {
                if (board.Name.Equals(boardName) && board.ContainsUser(email))
                    return board;
            }
            throw new Exception("Board does not exist for this user");
        }

        /// <summary>
        /// This method returns a board by its id.
        /// </summary>
        /// <param name="id">The id of the board to return</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        private Board GetBoard(int id)
        {
            if (boards.ContainsKey(id)){
                return boards[id];
            }
            else{
                throw new Exception("Board does not exist");
            }    
        }

        /// <summary>
        /// This method adds a user to an existing board.
        /// </summary>
        /// <param name="boardID">The board's ID</param>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void JoinBoard(int boardID, string email)
        {
            email=email.ToLower();
            Board boardToJoin = GetBoard(boardID);
            if (boardToJoin.ContainsUser(email)){
                throw new Exception("User already joined board");
            }    
            foreach (Board board in boards.Values)
            {
                if (board.Name.Equals(boardToJoin.Name))
                {
                    if (board.ContainsUser(email)){
                        throw new Exception("Board with this name already exists for user");
                    }

                    if (boardMapper.InsertUser(email, boardID))
                        board.AddUser(email);
                    else {
                        log.Error("Failure in insertion board into DB");
                        throw new Exception("Failed to insert board into DB");
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// This method removes a user from a board.
        /// </summary>
        /// <param name="boardID">The board's ID</param>
        /// <param name="email">The email of the user that leaves the board. Must be logged in and join board</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void LeaveBoard(int boardID, string email)
        {
            email = email.ToLower();
            Board board = GetBoard(boardID);
            if (board.Owner.Equals(email)){
                throw new Exception("Board owner cannot leave board");
            }  
            if (!board.ContainsUser(email)){
                throw new Exception("User is not in the board");
            }
            if (boardMapper.DeleteUser(email, boardID))
            {
                if (taskMapper.UpdateTasksAssignee(boardID, email, "0"))
                {
                    foreach (Task task in board.BlList)
                    {
                        if ((task.Assignee == null) || (!task.Assignee.Equals(email)))
                            task.Assignee = null;
                    }
                    foreach (Task task in board.IpList)
                    {
                        if (task.Assignee.Equals(email))
                            task.Assignee = null;
                    }
                    board.RemoveUser(email);
                    log.Info("Success in update assignee to task");
                }
                else
                {
                    boardMapper.InsertUser(email, boardID);
                    log.Error("Failure in updating assignee to task in DB");
                    throw new Exception("Failed to update assignee to task in DB");
                }
                log.Info("Success in deleting user from BoardUser table in DB");
            }
            else{
                log.Error("Failure in deletion user from BoardUser table in DB");
                throw new Exception("Failed to delete user from BoardUser table in DB");
            }
        }

        /// <summary>
        /// This method returns a board's column given it's ordinal.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and join board</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A list of the column's tasks, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public LinkedList<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {
            return GetBoard(boardName, email).GetColumn(columnOrdinal, email);
        }

        /// <summary>
        /// This method gets the name of a column given it's ordinal.
        /// </summary>
        /// <param name="email">The email address of the user. Must be logged in and join board</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A column's name, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            return GetBoard(boardName, email).GetColumnName(columnOrdinal, email);
        }

        /// <summary>
        /// This method gets the limit of a column given it's ordinal.
        /// </summary>
        /// <param name="email">The email address of the user. Must be logged in and join board</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The column's limit, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            return GetBoard(boardName, email).GetColumnLimit(columnOrdinal, email);
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user. Must be logged in and join board</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            GetBoard(boardName, email).EditLimit(columnOrdinal, limit, email);
        }

        /// <summary>
        /// This method transfers board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in and join board</param>
        /// <param name="newOwnerEmail">Email of the new owner. Must join board</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void SetOwner(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            GetBoard(boardName, currentOwnerEmail).SetOwner(currentOwnerEmail, newOwnerEmail);
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and join board</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            GetBoard(boardName, email).AddTask(dueDate, title, description, email, taskMapper);
        }

        /// <summary>
        /// This method advances a task to the next column.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in, join board and be the task assignee</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void MoveTask(string email, string boardName, int columnOrdinal, int taskID)
        {
            GetBoard(boardName, email).MoveTask(columnOrdinal, taskID, email);
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in and join board</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newTitle">New title for the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void EditTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string newTitle)
        {
            GetBoard(boardName, email).EditTaskTitle(email,newTitle, columnOrdinal, taskId);
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in, join board and be the task assignee</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newDescription">New description for the task</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void EditTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string newDescription)
        {
            GetBoard(boardName, email).EditTaskDescription(email,newDescription, columnOrdinal, taskId);
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in, join board and be the task assignee</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified by ID</param>
        /// <param name="newDueDate">The new due date of the column</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void EditTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            GetBoard(boardName, email).EditTaskDueDate(email,newDueDate, columnOrdinal, taskId);
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in, join board and be the task assignee</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified by ID</param>        
        /// <param name="emailAssignee">Email of the user to assign. Must join board first.</param>
        /// <returns>Void function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            GetBoard(boardName, email).AssignTask(email, emailAssignee, columnOrdinal, taskID);

        }

        /// <summary>
        /// This method calls a method which deletes boards and tasks tables data in the DB.
        /// </summary>
        /// <returns>Void Function, unless an error occurs (see <see cref="BoardController"/>)</returns>
        public void DeleteData()
        {
            boardMapper.DeleteBoardUsers();
            boardMapper.DeleteData();
            boardIdCounterMapper.DeleteData();
            taskMapper.DeleteData();
        }
    }
}