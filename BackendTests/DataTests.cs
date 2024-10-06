using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.BackendTests
{
    public class DataTests
    {
        private readonly ServiceFactory serviceFactory;
        public DataTests(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        /// <summary>
        /// Run all tests related to load and delete data
        /// </summary>
        public void RunLoadandDeleteTests()
        {
            LoadDataTests();
            DeleteDataTests();
        }

        /// <summary>
        /// Run all Data tests
        /// </summary>
        public void LoadDataTests()
        {
            Console.WriteLine("Load Data Tests");
            Console.WriteLine();
            string res = serviceFactory.LoadData(); ////Test for legal load data
            Console.WriteLine(res);
            Console.WriteLine();
            Console.WriteLine("Activities Tests After Load Data");
            Console.WriteLine();
            res =serviceFactory.userService.Login("guy@gmail.com", "sE23468"); //Test for legal login after load data
            Console.WriteLine(res);
            res = serviceFactory.userService.InProgressTasks("guy@gmail.com"); //Test for legal get in progress column after load data
            Console.WriteLine(res);
            res = serviceFactory.userService.GetUserBoards("guy@gmail.com"); //Test for legal get user boards after load data
            Console.WriteLine(res);
            res = serviceFactory.boardService.GetColumnLimit("guy@gmail.com", "Board2", 1); //Test for legal get column limit after load data
            Console.WriteLine(res);
            res = serviceFactory.boardService.GetColumn("guy@gmail.com", "Board2", 2);//Test for legal get column after load data
            Console.WriteLine(res);
        }

        /// <summary>
        /// Delete all Data tests
        /// </summary>
        public void DeleteDataTests()
        {
            Console.WriteLine("Delete Data Tests");
            Console.WriteLine();
            string res = serviceFactory.DeleteData(); ////Test for legal delete data
            Console.WriteLine(res);
            Console.WriteLine();
            res = serviceFactory.userService.Login("guy@gmail.com", "sE23468"); //Test for legal login after delete data (suppose to do error)
            Console.WriteLine(res);
        }


    }
}
