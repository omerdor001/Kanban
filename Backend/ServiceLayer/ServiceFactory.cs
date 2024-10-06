using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System.Reflection;
using System.IO;
using log4net.Config;
using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private BoardController boardController;
        private UserController userController;
        private JsonConverter converter;
        public UserService userService;
        public BoardService boardService;
        public TaskService taskService;

        public ServiceFactory()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.Config"));
            log.Info("Starting log!");
            converter = new JsonConverter();
            boardController = new BoardController();
            userController = new UserController(boardController);
            userService = new UserService(userController, converter);
            boardService = new BoardService(userController, boardController, converter);
            taskService = new TaskService(userController, boardController, converter);
        }
        public JsonConverter Converter{ get{ return converter; } }


        /// <summary>
        /// This method loads all persisted data.
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="ServiceFactory"/>)</returns>
        public string LoadData()
        {
            try
            {
                userService.LoadData();
                boardService.LoadData();
                Response<string> msg = new(null, null);
                log.Info($"Data loaded successfully!");
                string json = converter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = converter.ToSerialize(msg);
                return json;
            }
        }

        /// <summary>
        /// This method deletes all persisted data.
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="ServiceFactory"/>)</returns>
        public string DeleteData()
        {
            try
            {
                boardController.DeleteData();
                userController.DeleteData();
                converter = new JsonConverter();
                boardController = new BoardController();
                userController = new UserController(boardController);
                userService = new UserService(userController, converter);
                boardService = new BoardService(userController, boardController, converter);
                taskService = new TaskService(userController, boardController, converter);
                Response<string> msg = new(null, null);
                log.Info($"Data deleted successfully!");
                string json = converter.ToSerialize(msg);
                return json;
            }
            catch (Exception e)
            {
                Response<string> msg = new(e.Message, null);
                log.Error(e.Message);
                string json = converter.ToSerialize(msg);
                return json;
            }
        }
    }
}
