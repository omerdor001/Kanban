using Frontend.Model;
using System;

namespace Frontend.ViewModel
{
    public class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        private string _email;
        private string _password;
        private string _message;

        public MainViewModel()
        {
            Controller = new BackendController();
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

        /// <summary>
        /// Password getter and setter.
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Message getter and setter.
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// This method registers a new user to the system
        /// </summary>
        /// <returns>Void function, unless an error occurs (see <see cref="MainViewModel"/>)</returns>
        public UserLoginModel Register()
        {
            Message = "";
            try
            {
                UserLoginModel u = Controller.Register(Email, Password);
                return u;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// This method logs in an existing user
        /// </summary>
        /// <returns>A user login model, with the user's email, unless an error occurs (see <see cref="MainViewModel"/>)</returns>
        public UserLoginModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Email, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        
    }
}

