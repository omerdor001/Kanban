using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using System.Text.RegularExpressions;
using log4net.Config;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController
    {
        private readonly Dictionary<string, User> users;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly BoardController boardController;
        private UserMapper userMapper;
        public UserController(BoardController boardController)
        {
            this.boardController = boardController;
            users = new();
            userMapper = new();
        }

        /// <summary>
        /// This method loads all existing users from the DB to the users collection.
        /// </summary>
        /// <returns>Void Function, unless an error occurs (see <see cref="UserController"/>)</returns>
        public void LoadAllUsers()
        {
            List<UserDTO> users = userMapper.SelectAllUsers();
            foreach (UserDTO user in users)
            {
                LoadUser(user.Email, user.Password);
            }
        }

        /// <summary>
        /// This method loads a single user from the DB.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging into the system</param>
        /// <param name="password">The user password</param>
        /// <returns>Void function, unless an error occurs (see <see cref="UserController"/>)</returns>
        private void LoadUser(string email, string password)
        {
            User u = new(email, password, false, boardController);
            users[email] = u;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system</param>
        /// <param name="password">The user password</param>
        /// <returns>Void function, unless an error occurs (see <see cref="UserController"/>)</returns>
        public void CreateUser(string email, string password)
        {
            if (email == null)
            {
                log.Error("User with null email attempted register");
                throw new Exception("Email is null");
            }
            CheckEmail(email);
            email = email.ToLower();
            if (users.ContainsKey(email))
            {
                log.Error("User with the already existsting email attempted register");
                throw new Exception($"Email {email} already exists");
            }
            User u = new(email, password, boardController);
            if (!u.CheckPassword(password))
                throw new Exception("Password is illegal");
            if (!userMapper.InsertUser(new UserDTO(email, password)))
            {
                log.Error("Failure in insertion user to DB");
                throw new Exception("Failed to insert user into DB");
            }
            log.Info("Success in inserting user into DB");
            users[email] = u;
        }

        /// <summary>
        /// This method checks the validity of an email
        /// </summary>
        /// <param name="email">The user email address to check</param>
        /// <returns>Void function, unless an error occurs (see <see cref="UserController"/>)</returns>
        private void CheckEmail(string email)
        {
            if (email is null)
            {
                log.Error("User with null email attempted register");
                throw new Exception("Email is null");
            }
            var emailRegex1 = new Regex(@"^\w+([.-]?\w+)@\w+([.-]?\w+)(.\w{2,3})+$");
            var emailRegex2 = new Regex(@"^\S+@\S+\.\S+$");
            if (emailRegex1.Matches(email).Count != 1 || emailRegex2.Matches(email).Count != 1)
            {
                log.Error("User with invalid email attempted register");
                throw new Exception("Email is invalid");
            }
        }

        /// <summary>
        /// This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>Void function, unless an error occurs (see <see cref="UserController"/>)</returns>
        public void Login(string email, string password)
        {
            GetUser(email).Login(password);
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>Void function, unless an error occurs (see <see cref="UserController"/>)</returns>
        public void Logout(string email)
        {
            GetUser(email).Logout();
        }

        /// <summary>
        /// This method validates that a user is logged in.
        /// </summary>
        /// <param name="email">The user email address</param>
        /// <returns>Void functiom, unless an error occurs(see <see cref="UserController"/>)</returns>
        public void IsLoggedUser(string email)
        {
            User user = GetUser(email);
            if (!user.IsLogged)
                throw new Exception("User is not logged in");
        }

        /// <summary>
        /// This method gets a user from the users dictionary.
        /// </summary>
        /// <param name="email">The user email address</param>
        /// <returns>Returns a user, unless an error occurs (see <see cref="UserController"/>)</returns>
        public User GetUser(string email)
        {
            CheckEmail(email);
            email = email.ToLower();
            if (users.ContainsKey(email))
                return users[email];
            else
                throw new Exception("Email not found");
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A list of IDs of all user's boards, unless an error occurs (see <see cref="UserController"/>)</returns>
        public LinkedList<int> GetUserBoardsID(string email)
        {
            return GetUser(email).GetUserBoardsID();
        }

        /// <summary>
        /// This method returns a list of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A list of IDs of all user's boards, unless an error occurs (see <see cref="UserController"/>)</returns>
        public LinkedList<Board> GetUserBoards(string email)
        {
            return GetUser(email).GetUserBoards();
        }

        /// <summary>
        /// This method returns all in progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A list of 'in progress' tasks of the user, unless an error occurs (see <see cref="UserController"/>)</returns>
        public LinkedList<Task> GetInProgress(string email)
        {
            return GetUser(email).GetInProgress();
        }

        /// <summary>
        /// This method calls a method which deletes the users table data in the DB.
        /// </summary>
        /// <returns>Void Function, unless an error occurs (see <see cref="UserController"/>)</returns>
        public void DeleteData()
        {
            userMapper.DeleteData();
        }
    }
}