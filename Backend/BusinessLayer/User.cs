using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        private readonly BoardController boardController;
        public string Email { get; set; }
        private string password;
        public bool IsLogged { get; set; }
        private readonly int MIN_PASS_LENGTH = 6;
        private readonly int MAX_PASS_LENGTH = 20;

        public User(string email, string _password, BoardController _boardController)
        {
            password = _password;
            Email = email;
            IsLogged = true;
            boardController = _boardController;
        }

        public User(string email, string _password,bool _islogged, BoardController _boardController)
        {
            password = _password;
            Email = email;
            IsLogged = _islogged;
            boardController = _boardController;
        }

        /// <summary>
        /// Password getter and setter.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// This method logs in an existing user.
        /// </summary>
        /// <param name="password">The password of the user to login</param>
        /// <returns>Void function, unless an error occured (see <see cref="User"/>)</returns>
        public void Login(string password)
        {
            if (IsLogged)
                throw new Exception("User already logged in");
            if (password == null)
                throw new Exception("Password is null");
            if (password.Equals(Password))
                IsLogged = true;
            else
                throw new Exception("Password does not match");
        }

        /// <summary>
        /// This method logs out a logged in user.
        /// </summary>
        /// <returns>Void function, unless an error occurs (see <see cref="User"/>)</returns>
        public void Logout()
        {
            if (IsLogged)
                IsLogged = false;
            else
                throw new Exception("User is already logged out");
        }

        /// <summary>
        /// This method checks if a password meets certain conditions.
        /// </summary>
        /// <param name="password">A password </param>
        /// <returns>Returns boolean if the password meets the conditions, unless an error occurs (see <see cref="User"/>)</returns>
        public bool CheckPassword(string password)
        {
            if (password == null)
                throw new Exception("Password is null");
            if (password.Length < MIN_PASS_LENGTH && password.Length > MAX_PASS_LENGTH)
                return false;
            bool isUpperCase = false;
            bool isLowerCase = false;
            bool isDigit = false;
            for (int i = 0; i < password.Length; i++)
            {
                char c = password[i];
                if (char.IsUpper(c))
                    isUpperCase = true;
                if (char.IsLower(c))
                    isLowerCase = true;
                if (char.IsNumber(c))
                    isDigit = true;
            }
            return (isDigit && isUpperCase && isLowerCase);
        }

        /// <summary>
        /// This method returns a list of all boards the user joined, by their id.
        /// </summary>
        /// <returns>Returns a list of all user's board ID's, unless an error occurs (see <see cref="User"/>)</returns>
        public LinkedList<int> GetUserBoardsID()
        {
            LinkedList<int> boardsID = new();
            foreach (Board board in boardController.Boards.Values)
            {
                if (board.ContainsUser(Email))
                    boardsID.AddLast(board.Id);
            }
            return boardsID;
        }

        /// <summary>
        /// This method returns a list of all boards the user joined
        /// </summary>
        /// <returns>Returns a list of all user's board ID's, unless an error occurs (see <see cref="User"/>)</returns>
        public LinkedList<Board> GetUserBoards()
        {
            LinkedList<Board> boardsID = new();
            foreach (Board board in boardController.Boards.Values)
            {
                if (board.ContainsUser(Email))
                    boardsID.AddLast(board);
            }
            return boardsID;
        }

        /// <summary>
        /// This method returns all the 'in progress' tasks of a user.
        /// </summary>
        /// <returns>Returns a list of all 'in progress' tasks, unless an error occurs (see <see cref="User"/>)</returns>
        public LinkedList<Task> GetInProgress()
        {
            LinkedList<Task> tasks = new();
            foreach (Board board in boardController.Boards.Values)
            {
                if (board.ContainsUser(Email))
                {
                    foreach (Task task in board.IpList)
                        if (task.Assignee.Equals(Email))
                        {
                            tasks.AddLast(task);
                        }
                }
            }
            return tasks;
        }
    }
}