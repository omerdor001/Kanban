using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDTO : DTO
    {

        public const string IdColumnName = "Id";
        private int _id;
        public const string BoardIdColumnName = "BoardID";
        private int _boardid;
        public const string CreationTimeColumnName = "CreationTime";
        private DateTime _creationtime;
        public const string TitleColumnName = "Title";
        private string _title;
        public const string DescriptionColumnName = "Description";
        private string _description;
        public const string DueDateColumnName = "DueDate";
        private DateTime _duedate;
        public const string AssigneeColumnName = "Assignee";
        private string _assignee;
        public const string ColumnPlaceColumnName = "Column";
        private int _column;

        /// <summary>
        /// Get for id of taskDTO.
        /// </summary>
        public int Id { get => _id;}

        /// <summary>
        /// Get for board-id of taskDTO.
        /// </summary>
        public int BoardId { get => _boardid;}

        /// <summary>
        /// Get for creation time of taskDTO.
        /// </summary>
        public DateTime CreationTime { get => _creationtime;}

        /// <summary>
        /// Get for title of taskDTO.
        /// </summary>
        public string Title { get => _title; set { _title = value; _mapper.Update(_boardid,_id, TitleColumnName, value); } }

        /// <summary>
        /// Get for description of taskDTO.
        /// </summary>
        public string Description { get => _description; set { _description = value; _mapper.Update(_boardid,_id, DescriptionColumnName, value); } }

        /// <summary>
        /// Get for due date of taskDTO.
        /// </summary>
        public DateTime DueDate { get => _duedate; set { _duedate = value; _mapper.Update(_boardid,_id, DueDateColumnName, value); } }

        /// <summary>
        /// Get for assignee of taskDTO.
        /// </summary>
        public string Assignee { get => _assignee; set { _assignee = value; _mapper.Update(_boardid,_id, AssigneeColumnName, value); } }

        /// <summary>
        /// Get for column of taskDTO.
        /// </summary>
        public int Column { get => _column; set { _column = value; _mapper.Update(_boardid,_id, ColumnPlaceColumnName, value); } }

        public TaskDTO(int id, int boardid, DateTime creationtime, string title, string description, DateTime dueDate, string assignee, int column) : base(new TaskMapper())
        {
            _id = id;
            _boardid = boardid;
            _creationtime = creationtime;
            _title = title;
            _description = description;
            _duedate = dueDate;
            _assignee = assignee;
            if (_assignee == null)
                _assignee = "0";
            _column = column;
        }
    }
}
