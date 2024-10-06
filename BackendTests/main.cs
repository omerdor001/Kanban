using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    internal class main
    {
        public static void Main()
        {
            ServiceFactory serviceFactory = new();
            /*serviceFactory.DeleteData();
            UserServiceTests userServiceTests = new(serviceFactory.userService, serviceFactory.boardService, serviceFactory.taskService);
            BoardServiceTests boardServiceTests = new(serviceFactory.userService, serviceFactory.boardService, serviceFactory.taskService);
            TaskServiceTests taskServiceTests = new(serviceFactory.userService, serviceFactory.boardService, serviceFactory.taskService);
            userServiceTests.RunUserServiceTests();
            boardServiceTests.RunBoardServiceTests();
            taskServiceTests.RunTaskServiceTests();
            ServiceFactory serviceFactory2 = new();
            DataTests dataTests = new(serviceFactory2);
            dataTests.RunLoadandDeleteTests();*/
            serviceFactory.userService.Register("mail@mail.com", "Password1");
            serviceFactory.boardService.CreateBoard("mail@mail.com", "board1");
            serviceFactory.boardService.CreateBoard("mail@mail.com", "board2");
            serviceFactory.taskService.AddTask("mail@mail.com", "board1", "task1", "description1", new DateTime(2023,1,1,11,11,11));
            serviceFactory.taskService.AddTask("mail@mail.com", "board1", "task2", "", new DateTime(2023,1,1,11,11,11));
            serviceFactory.taskService.AddTask("mail@mail.com", "board1", "task3", "description3", new DateTime(2023,1,1,11,11,11));
        }
    }
}