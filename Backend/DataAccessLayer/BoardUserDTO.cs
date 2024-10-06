namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardUserDTO : DTO
    {
        public const string EmailColumnName = "Email";
        private string _email;
        public const string IdColumnName = "Id";
        private int _id;

        /// <summary>
        /// Get for email of boardUserDTO.
        /// </summary>
        public string Email { get => _email; }

        /// <summary>
        /// Get for id of boardUserDTO.
        /// </summary>
        public int Id { get => _id; }

        public BoardUserDTO(string email,int id) : base(new BoardUserMapper())
        {
            _email = email;
            _id = id;
        }
    }
}
