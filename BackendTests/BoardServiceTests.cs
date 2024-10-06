using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    class BoardServiceTests
    {
        private readonly BoardService BoardService;
        private readonly UserService UserService;
        private readonly TaskService TaskService;
        public BoardServiceTests(UserService userService, BoardService boardService, TaskService taskService)
        {
            BoardService = boardService;
            UserService = userService;
            userService.LoadData();
            TaskService = taskService;
        }

        /// <summary>
        /// Run all tests related to board
        /// </summary>
        public void RunBoardServiceTests()
        {
            InitTests();
            RunColumnFunctionsTests();
            RunJoinLeaveBoardTests();
            RunSetOwnerTests();
        }

        /// <summary>
        /// Initialize objects for testing
        /// </summary>
        private void InitTests()
        {
            UserService.Register("guy@gmail.com", "sE23468");
            UserService.Register("adi@gmail.com", "dR14052022");
            UserService.Register("omer@gmail.com", "oM6586868");
            UserService.Register("rachel@gmail.com", "Bb123456");
            UserService.Login("guy@gmail.com", "sE23468");
            UserService.Login("omer@gmail.com", "Aa123456");
            UserService.Login("rachel@gmail.com", "Bb123456");
            BoardService.CreateBoard("guy@gmail.com", "Board");
        }

        /// <summary>
        /// Testing column functions (get column/column name/column limit, set limit) (Test requirment 10+11).
        /// </summary>
        private void RunColumnFunctionsTests()
        {
            Console.WriteLine("Column limiting tests");
            Console.WriteLine();
            string res = BoardService.LimitColumn("martin@gmail.com", "MyBoard", 0, 20);////Test limit column for non existing user
            Console.WriteLine(res);
            res = BoardService.LimitColumn(null, "MyBoard", 0, 20);////Test limit column for null email
            Console.WriteLine(res);
            res = BoardService.LimitColumn("adi@gmail.com", "MyBoard", 0, 20);////Test limit column for user not logged in
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board1", 0, 13);////Test limiting column with not existed board name
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", null, 0, 13);////Test limiting column with null board name
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 6, 13);////Test limiting backlog column with illegal column ordinal
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 0, -13);////Test limiting backlog column with illegal limit
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 0, 1);////limiting backlog column
            Console.WriteLine(res);
            DateTime dt = new(2022, 8, 2, 7, 30, 52);
            res = TaskService.AddTask("guy@gmail.com", "Board", "Laundry", "Make laundry", dt); ////adding task to backlog column
            res = TaskService.AddTask("guy@gmail.com", "Board", "Homework", "Make Homework", dt); ////adding task to backlog column with no space in it
            Console.WriteLine(res);
            BoardService.LimitColumn("guy@gmail.com", "Board", 0, 2);////limiting backlog column
            TaskService.AddTask("guy@gmail.com", "Board", "Homework", "Make Homework", dt); ////adding task to backlog column
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 0, 1);////limiting backlog column after adding to column more than the new limit
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Column get limit tests");
            Console.WriteLine();
            res = BoardService.GetColumnLimit("adi@gmail.com", "MyBoard", 0);////Test get limit column for user not logged in
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit("martin@gmail.com", "MyBoard", 0);////Test limit column for non existing used
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit(null, "MyBoard", 0);////Test limit column for null email
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit("guy@gmail.com", "Board1", 0);////Test limiting column with not existing board name
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit("guy@gmail.com", null, 0);////Test limiting column with null board name
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit("guy@gmail.com", "Board", 6);////Test get column limit with illegal column ordinal
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Column get column name tests");
            Console.WriteLine();
            res = BoardService.GetColumnName("adi@gmail.com", "MyBoard", 0);////Test get column name for user not logged in
            Console.WriteLine(res);
            res = BoardService.GetColumnName("martin@gmail.com", "MyBoard", 0);////Test get column name for non existing user
            Console.WriteLine(res);
            res = BoardService.GetColumnName(null, "MyBoard", 0);////Test get column name for null email
            Console.WriteLine(res);
            res = BoardService.GetColumnName("guy@gmail.com", "Board1", 0);////Test get column name with non existing board name
            Console.WriteLine(res);
            res = BoardService.GetColumnName("guy@gmail.com", null, 0);////Test get column name with null board name
            Console.WriteLine(res);
            res = BoardService.GetColumnName("guy@gmail.com", "Board", 6);////Test get column name with illegal column ordinal
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Column set and get limit combined tests");
            Console.WriteLine();
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 0, 13);////Test limiting backlog column
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit("guy@gmail.com", "Board", 0);////Test get column limit
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 0, -1);////Test removing limit on backlog column
            Console.WriteLine(res);
            res = BoardService.GetColumnName("guy@gmail.com", "Board", 0);////Test get column name
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 1, 40);////Test limiting in progress column
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit("guy@gmail.com", "Board", 1);////Test get column limit
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 1, -1);////Test removing limit on in progress column
            Console.WriteLine(res);
            res = BoardService.GetColumnName("guy@gmail.com", "Board", 1);////Test get column name
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 2, 100);////Test limiting done column
            Console.WriteLine(res);
            res = BoardService.GetColumnLimit("guy@gmail.com", "Board", 2);////Test get column limit
            Console.WriteLine(res);
            res = BoardService.LimitColumn("guy@gmail.com", "Board", 2, -1);////Test removing limit on done column
            Console.WriteLine(res);
            res = BoardService.GetColumnName("guy@gmail.com", "Board", 2);////Test get column name
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Get column tests");
            Console.WriteLine();
            res = BoardService.GetColumn("adi@gmail.com", "MyBoard", 0); ////Test get colomn for user not logged in
            Console.WriteLine(res);
            res = BoardService.GetColumn(null, "MyBoard", 0); ////Test get colomn for null email
            Console.WriteLine(res);
            res = BoardService.GetColumn("martin@gmail.com", "MyBoard", 0); ////Test get colomn for non existing user
            Console.WriteLine(res);
            res = BoardService.GetColumn("guy@gmail.com", "Board1", 1); //Test get column with not existing board
            Console.WriteLine(res);
            res = BoardService.GetColumn("guy@gmail.com", null, 1); //Test get column with null board name
            Console.WriteLine(res);
            res = BoardService.GetColumn("guy@gmail.com", "Board", 3); //Test get column with illegal column ordinal
            Console.WriteLine(res);
            res = BoardService.GetColumn("guy@gmail.com", "Board", 0); //Test get column (non empty column)
            Console.WriteLine(res);
            res = BoardService.GetColumn("guy@gmail.com", "Board", 2); //Test get column (empty column)
            Console.WriteLine(res);
            Console.WriteLine();
        }

        /// <summary>
        /// Testing Join and leave board functions (Test requirment 12).
        /// </summary>
        private void RunJoinLeaveBoardTests()
        {

            Console.WriteLine("Join Boards tests");
            Console.WriteLine();
            string res = BoardService.AddUser("adi@gmail.com", 1); ////Test Join Board for user not logged in
            Console.WriteLine(res);
            res = BoardService.AddUser("martin@gmail.com", 1); ////Test Join Board for non existing user
            Console.WriteLine(res);
            res = BoardService.AddUser(null, 1); ////Test Join Board with null email
            Console.WriteLine(res);
            res = BoardService.AddUser("guy@gmail.com", -1); ////Test Join Board with no existing board
            Console.WriteLine(res);
            res = BoardService.AddUser("guy@gmail.com", 1); ////Test Join Board which user is already in user boards
            Console.WriteLine(res);
            res = BoardService.AddUser("rachel@gmail.com", 1); ////Test legal Join Board
            Console.WriteLine(res);
            Console.WriteLine("Leave Boards tests");
            Console.WriteLine();
            res = BoardService.RemoveUser("adi@gmail.com", 1); ////Test Leave Board for user not logged in
            Console.WriteLine(res);
            res = BoardService.RemoveUser("martin@gmail.com", 1); ////Test Leave Board for non existing user
            Console.WriteLine(res);
            res = BoardService.RemoveUser(null, 1); ////Test Leave Board with null email
            Console.WriteLine(res);
            res = BoardService.RemoveUser("guy@gmail.com", -1); ////Test Leave Board with no existing board
            Console.WriteLine(res);
            res = BoardService.RemoveUser("omer@gmail.com", 2); ////Test Leave Board which user is not in it
            Console.WriteLine(res);
            res = BoardService.RemoveUser("guy@gmail.com", 2); ////Test Leave Board to the owner
            Console.WriteLine(res);
            res = BoardService.RemoveUser("omer@gmail.com", 1); ////Test legal Leave Board
            Console.WriteLine(res);
        }

        /// <summary>
        /// Testing Join and leave board functions (Test requirment 13).
        /// </summary>
        private void RunSetOwnerTests()
        {
            Console.WriteLine("Set Owner tests");
            Console.WriteLine();
            string res = BoardService.SetOwner("adi@gmail.com", "omer@gmail.com", "Board"); ////Test Set Owner with user not logged in
            Console.WriteLine(res);
            res = BoardService.SetOwner("martin@gmail.com", "omer@gmail.com", "Board"); ////Test Set Owner with user with non existing user
            Console.WriteLine(res);
            res = BoardService.SetOwner(null, "omer@gmail.com", "Board"); ////Test Set Owner with null user
            Console.WriteLine(res);
            BoardService.AddUser("omer@gmail.com", 2);
            res = BoardService.SetOwner("omer@gmail.com", "guy@gmail.com", "Board"); ////Test Set Owner with user who is not the owner
            Console.WriteLine(res);
            res = BoardService.SetOwner("guy@gmail.com", "guy@gmail.com", "Board"); ////Test Set Owner to himself
            Console.WriteLine(res);
            res = BoardService.SetOwner("guy@gmail.com", "adi@gmail.com", "Board"); ////Test Set Owner to user which is not in board
            Console.WriteLine(res);
            res = BoardService.SetOwner("guy@gmail.com", "martin@gmail.com", "Board"); ////Test Set Owner to non existing user
            Console.WriteLine(res);
            res = BoardService.SetOwner("guy@gmail.com", null, "Board"); ////Test Set Owner to null user
            Console.WriteLine(res);
            res = BoardService.SetOwner("guy@gmail.com", "omer@gmail.com", "Board34"); ////Test Set Owner with to existed board
            Console.WriteLine(res);
            res = BoardService.SetOwner("guy@gmail.com", "omer@gmail.com", null); ////Test Set Owner with null board
            Console.WriteLine(res);
            res = BoardService.SetOwner("guy@gmail.com", "omer@gmail.com", "Board"); ////Legal test Set Owner
            Console.WriteLine(res);

        }
    }
}
