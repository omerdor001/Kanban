namespace Frontend.Model
{
    public class UserBoardsModel : NotifiableModelObject
    {
        private string userEmail;

        public UserBoardsModel(BackendController controller, string _userEmail) : base(controller)
        {
            userEmail = _userEmail;
        }

        /// <summary>
        /// Email getter.
        /// </summary>
        public string UserEmail
        {
            get => userEmail;
        }

    }
}
