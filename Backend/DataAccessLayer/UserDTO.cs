namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDTO : DTO
    {
        public const string EmailColumnName = "Email";
        private string _email;
        public const string PasswordColumnName = "Password";
        private string _password;

        /// <summary>
        /// Get for email of userDTO.
        /// </summary>
        public string Email { get => _email;}

        /// <summary>
        /// Get and Set for password of userDTO.
        /// </summary>
        public string Password { get => _password; set { _password = value; _mapper.Update(_email, PasswordColumnName, value); } }

        public UserDTO(string email,string password) : base(new UserMapper())
        {
            _email = email;
            _password = password;
        }
    }
}
