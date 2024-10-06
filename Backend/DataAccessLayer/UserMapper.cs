using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserMapper : Mapper
    {
        private const string TableName = "Users";

        public UserMapper() : base(TableName) {}

        /// <summary>
        /// This method gets a list of all users in the database.
        /// </summary>
        /// <returns>List of UserDTO, unless an error occurs (see <see cref="UserMapper"/>)</returns>
        public List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method reads a row in the User table in the database.
        /// </summary>
        /// <param name="reader">The reader of the database</param>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="UserMapper"/>)</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new(reader.GetString(0), reader.GetString(1));
            return result;
        }

        /// <summary>
        /// This method inserts a new row into User table.
        /// </summary>
        /// <param name="user">The UserDTO containing all row data</param>
        /// <returns>Returns boolean if the row was successfully inserted, unless an error occurs (see <see cref="UserMapper"/>)</returns>
        public bool InsertUser(UserDTO user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int result = -1;
                SQLiteCommand command = new(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({UserDTO.EmailColumnName},{UserDTO.PasswordColumnName}) " + $"VALUES (@emailVal,@passwordVal);";

                    SQLiteParameter emailParam = new("@emailVal", user.Email);
                    SQLiteParameter passwordParam = new(@"passwordVal", user.Password);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();

                    result = command.ExecuteNonQuery();
                    log.Info("Successful insertion into DB Users table");
                }
                catch
                {
                    log.Error("Failed insertion into DB Users table");
                    throw new Exception("Cannot insert to DB Users table");
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
