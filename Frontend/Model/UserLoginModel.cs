
namespace Frontend.Model
{
    public class UserLoginModel : NotifiableModelObject
    {
        private string _email;

        public UserLoginModel(BackendController controller, string email) : base(controller)
        {
            Email = email;
        }

        /// <summary>
        /// Email getter and setter.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
    }
}
