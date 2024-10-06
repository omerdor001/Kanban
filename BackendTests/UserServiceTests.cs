using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    public class UserServiceTests
    {
        private readonly UserService UserService;
        private readonly BoardService BoardService;
        private readonly TaskService TaskService;

        public UserServiceTests(UserService userService, BoardService boardService, TaskService taskService)
        {
            UserService = userService;
            BoardService = boardService;
            TaskService = taskService;
            userService.LoadData();
        }

        /// <summary>
        /// Run all tests related to user
        /// </summary>
        public void RunUserServiceTests()
        {
            RunRegistrationTests();
            UserService.Logout("guy@gmail.com");
            RunLoginAndLogoutTests();
            RunCreateAndDeleteBoardTests();
            RunInProgressTests();
            UserService.Register("omer@gmail.com", "Aa123456");
            UserService.Login("omer@gmail.com", "Aa123456");
            GetUserBoardsTests();
        }

        /// <summary>
        /// Testing registration function (Test requirment 7).
        /// </summary>
        private void RunRegistrationTests()
        {
            Console.WriteLine("Registration tests");
            Console.WriteLine();
            string res = UserService.Register("guy@gmail.com", null); ////Test illegal password (null)
            Console.WriteLine(res);
            res = UserService.Register("guy@gmail.com", "1235"); ////Test illegal password (length)
            Console.WriteLine(res);
            res = UserService.Register("guy@gmail.com", "12356u"); ////Test illegal password (Upper letter)
            Console.WriteLine(res);
            res = UserService.Register("guy@gmail.com", "TY6789"); ////Test illegal password (lower letter)
            Console.WriteLine(res);
            res = UserService.Register("guy@gmail.com", "ballkra"); ////Test illegal password (numbers)
            Console.WriteLine(res);
            res = UserService.Register("guy@gmail.com", "sE23468"); ////Test legal password
            Console.WriteLine(res);
            res = UserService.Register("guy@gmail.com", "sE86867"); ////Test same email
            Console.WriteLine(res);
            res = UserService.Register("fdsa", "sE86867"); ////Test invalid email (1)
            Console.WriteLine(res);
            res = UserService.Register("fdsa@", "sE86867"); ////Test invalid email (2)
            Console.WriteLine(res);
            res = UserService.Register("fdsa@fdsaasd", "sE86867"); ////Test invalid email (3)
            Console.WriteLine(res);
            res = UserService.Register("fdsa@fdsa.", "sE86867"); ////Test invalid email (4)
            Console.WriteLine(res);
            res = UserService.Register("@fdsa.com", "sE86867"); ////Test invalid email (5)
            Console.WriteLine(res);
            res = UserService.Register(null, "1235"); ////Test null email
            Console.WriteLine(res);
            Console.WriteLine();
        }

        /// <summary>
        /// Testing login and logout functions (Test requirment 8).
        /// </summary>
        private void RunLoginAndLogoutTests()
        {
            Console.WriteLine("Login tests");
            UserService.Logout("guy@gmail.com");
            Console.WriteLine();
            string res = UserService.Login("guy@gmail.com", "1235"); ////Test login with wrong password
            Console.WriteLine(res);
            res = UserService.Login("guy@gma", "sE23468"); ////Test login with invalid email
            Console.WriteLine(res);
            res = UserService.Login("guy@gmail.com", null); ////Test login with null password
            Console.WriteLine(res);
            res = UserService.Login(null, "1235"); ////Test login with null email
            Console.WriteLine(res);
            res = UserService.Login("martin@gmail.com", "ft75673"); ////Test login with unregistered user
            Console.WriteLine(res);
            res = UserService.Login("guy@gmail.com", "sE23468"); ////Test correct login
            Console.WriteLine(res);
            res = UserService.Login("guy@gmail.com", "sE23468"); ////Test login for logged in user
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Logout tests");
            Console.WriteLine();
            UserService.Register("adi@gmail.com", "dR14052022"); ////Register for further tests
            UserService.Logout("adi@gmail.com");
            res = UserService.Logout("adi@gmail.com");////Test logout for user not logged
            Console.WriteLine(res);
            res = UserService.Logout(null);////Test logout for null user
            Console.WriteLine(res);
            res = UserService.Logout("guy@gmail.com");////Test logout
            Console.WriteLine(res);
            res = UserService.Logout("guy@gmail.com");////Test already logged out user
            Console.WriteLine(res);
            res = UserService.Logout("guy@gmail.co");////Test logout wrong email
            Console.WriteLine(res);
            Console.WriteLine();
        }

        /// <summary>
        /// Testing create and delete board functions (Test requirment 9).
        /// </summary>
        private void RunCreateAndDeleteBoardTests()
        {
            UserService.Login("guy@gmail.com", "sE23468"); ////Login for further tests
            Console.WriteLine("Board creation tests");
            Console.WriteLine();
            string res = BoardService.CreateBoard("adi@gmail.com", "MyBoard");////Test create board for user not logged in
            Console.WriteLine(res);
            res = BoardService.CreateBoard("martin@gmail.com", "MyBoard");////Test create board for non existing user
            Console.WriteLine(res);
            res = BoardService.CreateBoard(null, "MyBoard");////Test create board for null user
            Console.WriteLine(res);
            res = BoardService.CreateBoard("guy@gmail.com", null);////Test create board for null board name
            Console.WriteLine(res);
            res = BoardService.CreateBoard("guy@gmail.com", "Board");////Test create board
            Console.WriteLine(res);
            res = BoardService.CreateBoard("guy@gmail.com", "Board");////Test create board with the same name
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Board deletion tests");
            Console.WriteLine();
            res = BoardService.DeleteBoard("adi@gmail.com", "MyBoard");////Test delete board for user not logged in
            Console.WriteLine(res);
            res = BoardService.DeleteBoard(null, "MyBoard");////Test delete board for null user
            Console.WriteLine(res);
            res = BoardService.DeleteBoard("martin@gmail.com", "MyBoard");////Test delete board for non existing user
            Console.WriteLine(res);
            res = BoardService.DeleteBoard("guy@gmail.com", null);////Test delete board null board name
            Console.WriteLine(res);
            res = BoardService.DeleteBoard("guy@gmail.com", "ThisBoard");////Test deleting non existent board
            Console.WriteLine(res);
            res = BoardService.DeleteBoard("guy@gmail.com", "Board");////Test delete board
            Console.WriteLine(res);
            res = BoardService.DeleteBoard("guy@gmail.com", "Board");////Test delete board after it was deleted
            Console.WriteLine(res);
            Console.WriteLine();
        }

        /// <summary>
        /// Testing get in progress function (Test requirment 16).
        /// </summary>
        private void RunInProgressTests()
        {
            Console.WriteLine("Get in progress list tests");
            Console.WriteLine();
            DateTime dt4 = new(2022, 8, 7, 10, 30, 52);
            string res = UserService.InProgressTasks("adi@gmail.com"); ////Test get in progress list for user not logged in
            Console.WriteLine(res);
            res = UserService.InProgressTasks("martin@gmail.com"); ////Test get in progress list for non existing user
            Console.WriteLine(res);
            res = UserService.InProgressTasks(null); ////Test get in progress list for null user
            Console.WriteLine(res);
            BoardService.CreateBoard("guy@gmail.com", "Board2");
            TaskService.AddTask("guy@gmail.com", "Board2", "Lunch", "Make lunch", dt4);
            TaskService.AddTask("guy@gmail.com", "Board2", "Lunch", "Homework", dt4);
            TaskService.AssignTask("guy@gmail.com", "Board2", 0, 0, "guy@gmail.com");
            TaskService.MoveTask("guy@gmail.com", "Board2", 0, 0);
            UserService.InProgressTasks("guy@gmail.com"); //Test get in progress list (Non empty column)
            Console.WriteLine(res);
            TaskService.MoveTask("guy@gmail.com", "Board2", 1, 0);
            res = UserService.InProgressTasks("guy@gmail.com"); //Test get in progress list (empty column)
            Console.WriteLine(res);
            Console.WriteLine();
        }

        /// <summary>
        /// Testing get in progress function (Test requirment 9).
        /// </summary>
        private void GetUserBoardsTests()
        {
            Console.WriteLine("Get User Boards tests");
            Console.WriteLine();
            DateTime dt4 = new(2022, 8, 7, 10, 30, 52);
            string res = UserService.GetUserBoards("adi@gmail.com"); ////Test get boards list for user not logged in
            Console.WriteLine(res);
            res = UserService.GetUserBoards("martin@gmail.com"); ////Test get boards list for non existing user
            Console.WriteLine(res);
            res = UserService.GetUserBoards(null); ////Test get boards list for null user
            Console.WriteLine(res);
            res = UserService.GetUserBoards("omer@gmail.com"); //Test get boards list (empty column)
            Console.WriteLine(res);
            BoardService.AddUser("omer@gmail.com", 1);
            res = UserService.GetUserBoards("omer@gmail.com"); //Test get boards list (Non empty column)
            Console.WriteLine(res);
            Console.WriteLine();
        }
    }
}
