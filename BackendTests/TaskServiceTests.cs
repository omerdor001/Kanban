using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    public class TaskServiceTests
    {
        private readonly UserService UserService;
        private readonly BoardService BoardService;
        private readonly TaskService TaskService;

        public TaskServiceTests(UserService userService, BoardService boardService, TaskService taskService)
        {
            UserService = userService;
            BoardService = boardService;
            TaskService = taskService;
            userService.LoadData();
        }

        /// <summary>
        /// Run all tests related to task
        /// </summary>
        public void RunTaskServiceTests()
        {
            InitTests();
            RunAddTaskTests();
            RunMoveTaskTests();
            TaskUpdatingTests();
            AssignTaskTests();
        }

        /// <summary>
        /// Initialize objects for testing
        /// </summary>
        private void InitTests()
        {
            UserService.Register("guy@gmail.com", "sE23468");
            UserService.Register("adi@gmail.com", "dR14052022");
            UserService.Register("omer@gmail.com", "Aa123456");
            UserService.Register("rachel@gmail.com", "Bb123456");
            UserService.Login("guy@gmail.com", "sE23468");
            UserService.Login("omer@gmail.com", "Aa123456");
            UserService.Login("rachel@gmail.com", "Bb123456");
            BoardService.CreateBoard("guy@gmail.com", "Board");
            TaskService.AddTask("guy@gmail.com", "Board", "Laundry", "Make laundry", new DateTime(2022, 5, 1, 8, 30, 52));
            TaskService.AssignTask("guy@gmail.com", "Board", 0, 0, "guy@gmail.com");
        }

        /// <summary>
        /// Testing task creation function (Test requirment 12).
        /// </summary>
        private void RunAddTaskTests()
        {
            Console.WriteLine("Task creation tests");
            Console.WriteLine();
            DateTime dt1 = new(2022, 8, 7, 10, 30, 52);
            string res = TaskService.AddTask("adi@gmail.com", "MyBoard", "Laundry", "Make laundry", dt1); ////Test adding task for user not logged in
            Console.WriteLine(res);
            res = TaskService.AddTask(null, "MyBoard", "Laundry", "Make laundry", dt1); ////Test adding task for null email
            Console.WriteLine(res);
            res = TaskService.AddTask("martin@gmail.com", "MyBoard", "Laundry", "Make laundry", dt1); ////Test adding task for non existing user
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", null, "Laundry", "Make laundry", dt1); ////Test illegal adding task with null board name
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", "Board1", "Laundry", "Make laundry", dt1); ////Test illegal adding task with non existing board
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", "Board", null, "Make laundry", dt1); ////Test illegal adding task with null task title
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", "Board", "", "Make laundry", dt1); ////Test illegal adding task (empty task title)
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", "Board", "Making laundry for myself including all my suit for the meeting", "Make laundry", dt1); ////Test illegal title length when adding task
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", "Board", "Making laundry", null, dt1); ////Test illegal adding task (null description)
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", "Board", "Laundry", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc,", dt1); ////Test illegal description length when adding task
            Console.WriteLine(res);
            res = TaskService.AddTask("omer@gmail.com", "Board", "Laundry", "Make laundry", DateTime.Now); ////Test illegal adding task (illegal due date)
            Console.WriteLine(res);
            res = TaskService.AddTask("rachel@gmail.com", "Board", "Laundry", "Make laundry", dt1); ////Test illegal adding task (user is not in board members)
            Console.WriteLine(res);
            DateTime dt2 = new(2022, 7, 1, 8, 30, 52);
            res = TaskService.AddTask("omer@gmail.com", "Board", "Laundry", "Make laundry", dt2); ////Test legal adding task
            Console.WriteLine(res);
            Console.WriteLine();
        }

        /// <summary>
        /// Testing move task function (Test requirment 13).
        /// </summary>
        private void RunMoveTaskTests()
        {
            Console.WriteLine("Tasks advancing tests");
            Console.WriteLine();
            string res = TaskService.MoveTask("adi@gmail.com", "MyBoard", 0, 0); ////Test advancing task for user not logged in
            Console.WriteLine(res);
            res = TaskService.MoveTask("martin@gmail.com", "MyBoard", 0, 0); ////Test advancing task for non existing user
            Console.WriteLine(res);
            res = TaskService.MoveTask(null, "MyBoard", 0, 0); ////Test advancing task with null email
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", null, 0, 0); ////Test advance task with null board name
            Console.WriteLine(res);
            res = TaskService.MoveTask("omer@gmail.com","Board", 0, 0); ////Test advance task with user who is not the assignee
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", "Board1", 0, 0); ////Test advance task with non existing board
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", "Board", 0, 3); ////Test advance task which does not exist
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", "Board", 3, 0); ////Test advance task from non existing column
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", "Board", 1, 0); ////Test advance task to column without the task in it
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", "Board", 0, 0); ////Test advance task (from backlog to in progress)
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", "Board", 1, 0); ////Test advance task (from in progress to done)
            Console.WriteLine(res);
            res = TaskService.MoveTask("guy@gmail.com", "Board", 2, 0); ////Test advance task from done column
            Console.WriteLine(res);
            Console.WriteLine();
        }

        /// <summary>
        /// Testing task updating functions (Test requirment 14+15).
        /// </summary>
        private void TaskUpdatingTests()
        {
            Console.WriteLine("Task title update tests");
            Console.WriteLine();
            TaskService.AssignTask("omer@gmail.com", "Board", 0, 2, "guy@gmail.com");
            string res = TaskService.UpdateTaskTitle("adi@gmail.com", "MyBoard", 0, 0, "DoLaundry"); ////Test edit title task for user not logged in
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("martin@gmail.com", "MyBoard", 0, 0, "DoLaundry"); ////Test edit title task for non existing user
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle(null, "MyBoard", 0, 0, "DoLaundry"); ////Test edit title for null user
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", null, 0, 0, "DoLaundry"); ////Test edit title for null board email
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "board1", 0, 0, "DoLaundry"); ////Test edit title task for non existing board
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "Board", 6, 2, "Laundry suits"); ////Test edit title with illegal column ordinal
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "Board", 2, 0, "Laundry suits"); ////Test edit title of task in done column
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "Board", 0, 2, null); ////Test edit title of task with null title
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "Board", 0, 2, ""); ////Test edit title of task with empty title
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "Board", 0, 2, "Making laundry for myself including all my suit for the meeting"); ////Test edit title of task with illegal title
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "Board", 0, 4, "Laundry suits"); ////Test edit title of task which does not exist
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("omer@gmail.com", "Board", 0, 2, "Laundry suits"); ////Test edit title of task with member of board which is not the assignee
            Console.WriteLine(res);
            res = TaskService.UpdateTaskTitle("guy@gmail.com", "Board", 0, 2, "Laundry suits"); ////Test edit title of task with legal parameters
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Task description update tests");
            Console.WriteLine();
            res = TaskService.UpdateTaskDescription("adi@gmail.com", "MyBoard", 0, 2, "Make laundry suits");////Test edit description for user not logged in
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("martin@gmail.com", "MyBoard", 0, 2, "Make laundry suits");////Test edit description for non existing user
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription(null, "MyBoard", 0, 2, "Make laundry suits");////Test edit description for null email
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", null, 0, 2, "Make laundry suits"); ////Test edit description with null board name
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", "board1", 0, 2, "Make laundry suits"); ////Test edit description with non existing board
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", "Board", 6, 2, "Make laundry suits");////Test edit description with illegal column ordinal
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", "Board", 2, 0, "Make laundry suits");////Test edit description in done column
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", "Board", 0, 2, null); ////Test edit description of task with null description
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", "Board", 0, 2, "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc,");////Test edit description with illegal description
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", "Board", 0, 4, "Make laundry suits"); ////Test edit description of task which does not exist
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("omer@gmail.com", "Board", 0, 2, "Make laundry suits"); ////Test edit description of task with member of board which is not the assignee
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDescription("guy@gmail.com", "Board", 0, 2, "Make laundry suits"); ////Test edit description of task with legal parameters
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Task due date update tests");
            Console.WriteLine();
            DateTime dt3 = new(2022, 9, 2, 8, 00, 00);
            res = TaskService.UpdateTaskDueDate("adi@gmail.com", "MyBoard", 0, 2, dt3);////Test edit due date task for user not logged in
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("martin@gmail.com", "MyBoard", 0, 2, dt3);////Test edit due date for non existing user
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate(null, "MyBoard", 0, 2, dt3);////Test edit due date task for null email
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("guy@gmail.com", null, 0, 2, dt3); ////Test edit due date with null board name
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("guy@gmail.com", "Board1", 0, 2, dt3); ////Test edit due date with non existing board
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("guy@gmail.com", "Board", 6, 2, dt3);////Test edit due date with illegal column ordinal
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("guy@gmail.com", "Board", 0, 4, dt3); ////Test edit due date of a task which does not exist
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("guy@gmail.com", "Board", 0, 2, DateTime.Now); ////Test edit due date with illegal due date
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("omer@gmail.com", "Board", 0, 2, dt3); ////Test edit due date of task with member of board which is not the assignee
            Console.WriteLine(res);
            res = TaskService.UpdateTaskDueDate("guy@gmail.com", "Board", 0, 2, dt3); ////Test edit due date of task with legal parameters
            Console.WriteLine(res);
        }

        /// <summary>
        /// Testing task updating functions (Test requirment 23).
        /// </summary>
        private void AssignTaskTests()
        {
            Console.WriteLine("Assign Task tests");
            Console.WriteLine();
            string res = TaskService.AssignTask("adi@gmail.com", "Board", 0, 1, "omer@gmail.com"); ////Test Assign Task with user which is not in board
            Console.WriteLine(res);
            res = TaskService.AssignTask("martin@gmail.com", "Board", 0, 1, "omer@gmail.com"); ////Test Assign Task for not existed user 
            Console.WriteLine(res);
            res = TaskService.AssignTask(null, "Board", 0, 1, "omer@gmail.com"); ////Test Assign Task with null email
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", null, 0, 1, "omer@gmail.com"); ////Test Assign Task with null board
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", "board34", 0, 1, "omer@gmail.com"); ////Test Assign Task with not existed board
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", "Board", -9, 1, "omer@gmail.com"); ////Test Assign Task with illegal column ordinal
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", "Board", 0, -9, "omer@gmail.com"); ////Test Assign Task with not existed task ID
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", "Board", 0, 1, "adi@gmail.com"); ////Test Assign Task to user which is not in board
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", "Board", 0, 1, "martin@gmail.com"); ////Test Assign Task to not existed user
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", "Board", 0, 1, null); ////Test Assign Task to not null user
            Console.WriteLine(res);
            res = TaskService.AssignTask("guy@gmail.com", "Board", 0, 1, "omer@gmail.com"); ////Test legal test Assign Task
            Console.WriteLine(res);
        }
    }
}
