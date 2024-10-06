namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardIdCounterDTO : DTO
    {
        public const string NextIDColumnName = "Id";
        private int _nextID;

        /// <summary>
        /// NextID getter and setter.
        /// </summary>
        public int NextID { get => _nextID; set { _nextID = value; _mapper.Update(_nextID - 1, NextIDColumnName, value); } }

        public BoardIdCounterDTO(int nextID) : base(new BoardIdCounterMapper())
        {
            _nextID = nextID;
        }
    }
}
