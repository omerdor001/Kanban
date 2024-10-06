using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardMapper : Mapper
    {
        private const string TableName = "Boards";
        private BoardUserMapper boardUserDalMapper;

        public BoardMapper() : base(TableName) {
            boardUserDalMapper = new();
          }

        /// <summary>
        /// This method gets a list of all boards in the database.
        /// </summary>
        /// <returns>Returns list of BoardDTO, unless an error occurs (see <see cref="BoardMapper"/>)</returns>
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method gets a list of all users' emails from BoardUser table, according to id of board.
        /// </summary>
        /// <param name="id">The id of the board</param>
        /// <returns>Returns list of users' emails, unless an error occurs (see <see cref="BoardMapper"/>)</returns>
        public LinkedList<string> SelectAllUsers(int id)
        {
            List<BoardUserDTO> selectAllUsers = boardUserDalMapper.SelectAllBoardUsers(id);
            LinkedList<string> selectAllUsersEmails = new();
            foreach (BoardUserDTO boardUserDTO in selectAllUsers)
            {
                selectAllUsersEmails.AddLast(boardUserDTO.Email);
            }
            return selectAllUsersEmails;
        }

        /// <summary>
        /// This method reads a row in the boards table in the database.
        /// </summary>
        /// <param name="reader">The reader of the database</param>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="BoardMapper"/>)</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5), reader.GetInt32(6));
            return result;
        }

        /// <summary>
        /// This method inserts a new row into boards table.
        /// </summary>
        /// <param name="board">The BoardDTO containing all row data</param>
        /// <returns>Boolean if insertion was successful, unless an error occurs (see <see cref="BoardMapper"/>)</returns>
        public bool InsertBoard(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int result = -1;
                SQLiteCommand command = new(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({BoardDTO.IdColumnName},{BoardDTO.NameColumnName},{BoardDTO.BlLimitColumnName},{BoardDTO.IpLimitColumnName},{BoardDTO.DoneLimitColumnName},{BoardDTO.OwnerEmailColumnName},{BoardDTO.BoardIdTaskCounterColumnName}) " + $"VALUES (@idVal,@nameVal,@blLimitVal,@ipLimitVal,@doneLimitVal,@owneremailVal,@boardIdTaskCounterVal);";
                    SQLiteParameter idParam = new("@idVal", board.Id);
                    SQLiteParameter nameParam = new(@"nameVal", board.Name);
                    SQLiteParameter blLimitParam = new(@"blLimitVal", board.BlLimit);
                    SQLiteParameter ipLimitParam = new(@"ipLimitVal", board.IpLimit);
                    SQLiteParameter doneLimitParam = new(@"doneLimitVal", board.DoneLimit);
                    SQLiteParameter ownerEmailParam = new(@"owneremailVal", board.OwnerEmail);
                    SQLiteParameter boardIdTaskCounterParam = new(@"boardIdTaskCounterVal", board.BoardIdTaskCounter);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(blLimitParam);
                    command.Parameters.Add(ipLimitParam);
                    command.Parameters.Add(doneLimitParam);
                    command.Parameters.Add(ownerEmailParam);
                    command.Parameters.Add(boardIdTaskCounterParam);
                    command.Prepare();

                    result = command.ExecuteNonQuery();
                    log.Info("Success in inserting board to DB Boards table");
                }
                catch
                {
                    log.Error("Failed insertion to DB Boards table");
                    throw new Exception("Cannot insert to DB Boards table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return result > 0;
            }
        }

        /// <summary>
        /// This method creates new BoardUserDTO and adds it to BoardsUser table
        /// </summary>
        /// <param name="id">The id of the new board</param>
        /// <param name="email">The user email</param>
        /// <returns>Boolean if insertion was successful, unless an error occurs (see <see cref="BoardMapper"/>)</returns> 
        public bool InsertUser(string email, int id)
        {
            BoardUserDTO boardUserDTO = new(email, id);
            return boardUserDalMapper.InsertBoardUser(boardUserDTO);
        }

        /// <summary>
        /// This method delete BoardUserDTO from BoardsUser table
        /// </summary>
        /// <param name="id">The id of the board</param>
        /// <param name="email">The user email</param>
        /// <returns>Boolean if deletion was successful, unless an error occurs (see <see cref="BoardMapper"/>)</returns> 
        public bool DeleteUser(string email, int id)
        {
            return boardUserDalMapper.Delete(email,id);
        }

        /// <summary>
        /// This method delete BoardUserDTO from BoardsUser table
        /// </summary>
        /// <param name="id">The id of the board</param>
        /// <returns>Boolean if deletion was successful, unless an error occurs (see <see cref="BoardMapper"/>)</returns> 
        public bool DeleteBoard(int id)
        {
            return boardUserDalMapper.Delete(id);
        }

        /// <summary>
        /// This method deletes the boardUser table data.
        /// </summary>
        /// <returns>Boolean if deletion was successful, unless an error occurs (see <see cref="BoardMapper"/>)</returns>
        public bool DeleteBoardUsers()
        {
            return boardUserDalMapper.DeleteData();  
        }
    }
}