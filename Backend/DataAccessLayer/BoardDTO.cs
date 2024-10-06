namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDTO : DTO
    {
        public const string IdColumnName = "Id";
        private int _id;
        public const string NameColumnName = "Name";
        private string _name;
        public const string BlLimitColumnName = "BlLimit";
        private int _blLimit;
        public const string IpLimitColumnName = "IpLimit";
        private int _ipLimit;
        public const string DoneLimitColumnName = "DoneLimit";
        private int _doneLimit;
        public const string OwnerEmailColumnName = "OwnerEmail";
        private string _owneremail;
        public const string BoardIdTaskCounterColumnName = "BoardIdTaskCounter";
        private int _boardIdTaskCounter;

        /// <summary>
        /// Id getter.
        /// </summary>
        public int Id { get => _id; }

        /// <summary>
        /// Name getter and setter.
        /// </summary>
        public string Name { get => _name; set { _name = value; _mapper.Update(_id, NameColumnName, value); } }

        /// <summary>
        /// BlLimit getter and setter.
        /// </summary>
        public int BlLimit { get => _blLimit; set { _blLimit = value; _mapper.Update(_id, BlLimitColumnName, value); } }

        /// <summary>
        /// IpLimit getter and setter.
        /// </summary>
        public int IpLimit { get => _ipLimit; set { _ipLimit = value; _mapper.Update(_id, IpLimitColumnName, value); } }

        /// <summary>
        /// DoneLimit getter and setter.
        /// </summary>
        public int DoneLimit { get => _doneLimit; set { _doneLimit = value; _mapper.Update(_id, DoneLimitColumnName, value); } }

        /// <summary>
        /// OwnerEmail getter and setter.
        /// </summary>
        public string OwnerEmail { get => _owneremail; set { _owneremail = value; _mapper.Update(_id, OwnerEmailColumnName, value); } }

        /// <summary>
        /// BoardIdTaskCounter getter and setter.
        /// </summary>
        public int BoardIdTaskCounter { get => _boardIdTaskCounter; set { _boardIdTaskCounter = value; _mapper.Update(_id, BoardIdTaskCounterColumnName, value); } }

        public BoardDTO(int id, string name, int blLimit, int ipLimit, int doneLimit, string owneremail, int boardIdTaskCounter) : base(new BoardMapper())
        {
            _id = id;
            _name = name;
            _blLimit = blLimit;
            _ipLimit = ipLimit;
            _doneLimit = doneLimit;
            _owneremail = owneremail;
            _boardIdTaskCounter = boardIdTaskCounter;
        }
    }
}