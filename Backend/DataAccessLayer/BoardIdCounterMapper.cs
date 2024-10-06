using System;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardIdCounterMapper : Mapper
    {
        private const string TableName = "BoardIdCounter";

        public BoardIdCounterMapper() : base(TableName){}

        /// <summary>
        /// This method gets the BoardIdCounter cell in the database.
        /// </summary>
        /// <returns>BoardIdCounterDTO, unless an error occurs (see <see cref="BoardIdCounterMapper"/>)</returns>
        public BoardIdCounterDTO SelectBoardCounterId()
        {
            BoardIdCounterDTO result = Select().Cast<BoardIdCounterDTO>().Single();
            return result;
        }

        /// <summary>
        /// This method reads a row in the boardIdCounter table in the database.
        /// </summary>
        /// <param name="reader">The reader of the database</param>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="BoardIdCounterMapper"/>)</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardIdCounterDTO result = new(reader.GetInt32(0));
            return result;
        }

        /// <summary>
        /// This method inserts a new row into boardIdCounter table.
        /// </summary>
        /// <param name="idCounter">The BoardDTO containing all row data</param>
        /// <returns>Boolean if insertion was successful, unless an error occurs (see <see cref="BoardIdCounterMapper"/>)</returns>
        public bool InsertBoardIdCounter(BoardIdCounterDTO idCounter)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int result = -1;
                SQLiteCommand command = new(null, connection);
                try 
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({BoardIdCounterDTO.NextIDColumnName}) " + $"VALUES (@nextIDVal);";
                    SQLiteParameter nextIDParam = new("@nextIDVal", idCounter.NextID);

                    command.Parameters.Add(nextIDParam);
                    command.Prepare();

                    result = command.ExecuteNonQuery();
                    log.Info("Success in inserting boardIdCounter to DB");
                }
                catch 
                {
                    log.Error("Failed insertion to DB BoardIdCounter table");
                    throw new Exception("Cannot insert to DB BoardIdCounter table");
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
