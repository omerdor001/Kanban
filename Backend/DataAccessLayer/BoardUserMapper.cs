using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardUserMapper : Mapper
    {
        private const string TableName = "BoardUser";

        public BoardUserMapper() : base(TableName){}

        /// <summary>
        /// This method gets a list of all BoardUsers in the DB.
        /// </summary>
        /// <returns>List of BoardUserDTO, unless an error occurs (see <see cref="BoardUserMapper"/>)</returns>
        public List<BoardUserDTO> SelectAllBoardUsers()
        {
            List<BoardUserDTO> result = Select().Cast<BoardUserDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method gets a list of all BoardUsers in the DB with id of a board.
        /// </summary>
        /// <param name="id">The id of the board</param>
        /// <returns>List of BoardUserDTO, unless an error occurs (see <see cref="BoardUserMapper"/>)</returns>
        public List<BoardUserDTO> SelectAllBoardUsers(int id)
        {
            List<BoardUserDTO> result = Select(id).Cast<BoardUserDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method reads a row in the BoardUser table in the database.
        /// </summary>
        /// <param name="reader">The reader of the database</param>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="BoardUserMapper"/>)</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardUserDTO result = new(reader.GetString(0), reader.GetInt32(1));
            return result;
        }

        public bool InsertBoardUser(BoardUserDTO boardUser)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int result = -1;
                SQLiteCommand command = new(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({BoardUserDTO.EmailColumnName},{BoardUserDTO.IdColumnName}) " + $"VALUES (@emailVal,@idVal);";

                    SQLiteParameter emailParam = new(@"emailVal", boardUser.Email);
                    SQLiteParameter idParam = new("@idVal", boardUser.Id);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(idParam);
                    command.Prepare();

                    result = command.ExecuteNonQuery();
                    log.Error("Success in inserting boardUser to DB");
                }
                catch 
                {
                    log.Error("Failure inserting to DB BoardUsers table");
                    throw new Exception("Failed insertion into DB BoardUsers table");
                }
                finally 
                {
                    command.Dispose();
                    connection.Close();
                }
                return result > 0;
            }
        }
    }
}

