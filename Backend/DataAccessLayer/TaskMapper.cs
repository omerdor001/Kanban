using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskMapper : Mapper
    {
        private const string TableName = "Tasks";

        public TaskMapper() : base(TableName){}

        /// <summary>
        /// This method gets a list of all tasks in the database.
        /// </summary>
        /// <returns>List of TaskDTO, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        public List<TaskDTO> SelectAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method gets a list of all tasks in the database, according to their board.
        /// </summary>
        /// <param name="id">The board's id</param>
        /// <returns>List of TaskDTO, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        public List<TaskDTO> SelectAllTasks(int id)
        {
            List<TaskDTO> result = SelectTasks(id).Cast<TaskDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method gets a list of all tasks in the database, according to their board and the assignee.
        /// </summary>
        /// <param name="id">The board's id</param>
        /// <param name="email">The email of the assignee</param>
        /// <returns>List of TaskDTO, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        public List<TaskDTO> SelectAllAssigneeTasks(int id, string email)
        {
            List<TaskDTO> result = SelectTasksByAssignee(id,email).Cast<TaskDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method gets a list of DTOs by board id and assignee from Tasks table.
        /// </summary>
        /// <param name="id">The board's id</param>
        /// <param name="email">The email of the assignee</param>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        protected List<DTO> SelectTasksByAssignee(int id, string email)
        {
            List<DTO> results = new();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new(null, connection) { CommandText = $"select * from {TableName} where BoardID={id} and Assignee='{email}';" };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.Add(ConvertReaderToObject(dataReader));
                    log.Info($"Successful select in {TableName} table");
                }
                catch
                {
                    log.Error($"Failed select in {TableName} table");
                    throw new Exception($"Cannot perform select on {TableName} table");
                }
                finally
                {
                    if (dataReader != null) dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }

        /// <summary>
        /// This method gets a list of DTOs by board id from the Tasks table.
        /// </summary>
        /// <param name="id">The board's id</param>
        /// <returns>Returns list of DTOs, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        protected List<DTO> SelectTasks(int id)
        {
            List<DTO> results = new();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new(null, connection) { CommandText = $"select * from {TableName} where BoardID={id};" };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.Add(ConvertReaderToObject(dataReader));
                    log.Info($"Successful select in {TableName} table");
                }
                catch
                {
                    log.Error($"Failed select on {TableName} table");
                    throw new Exception($"Cannot perform select on {TableName} table");
                }
                finally
                {
                    if (dataReader != null) dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }

        /// <summary>
        /// This method reads a row in the Task table in the database.
        /// </summary>
        /// <param name="reader">The reader of the database</param>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new(reader.GetInt32(0), reader.GetInt32(1), (DateTime)reader.GetDateTime(2), reader.GetString(3), reader.GetString(4), (DateTime)reader.GetDateTime(5), reader.GetString(6), reader.GetInt32(7));
            return result;
        }

        /// <summary>
        /// This method adds TaskDTO to Task table.
        /// </summary>
        /// <param name="task">The new TaskDTO to be added</param>
        /// <returns>Returns true if TableDTO added to Task table successfully, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        public bool InsertTask(TaskDTO task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int result = -1;
                SQLiteCommand command = new(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({TaskDTO.IdColumnName},{TaskDTO.BoardIdColumnName},{TaskDTO.CreationTimeColumnName},{TaskDTO.TitleColumnName},{TaskDTO.DescriptionColumnName},{TaskDTO.DueDateColumnName},{TaskDTO.AssigneeColumnName},{TaskDTO.ColumnPlaceColumnName}) " +
                        $"VALUES (@idVal,@boardidVal,@creationtimeVal,@titleVal,@descriptionVal,@duedateVal,@assigneeVal,@columnVal);";

                    SQLiteParameter idParam = new("@idVal", task.Id);
                    SQLiteParameter boardidParam = new("@boardidVal", task.BoardId);
                    SQLiteParameter creationtimeParam = new("@creationtimeVal", task.CreationTime);
                    SQLiteParameter titleParam = new(@"titleVal", task.Title);
                    SQLiteParameter descriptionParam = new(@"descriptionVal", task.Description);
                    SQLiteParameter duedateParam = new(@"duedateVal", task.DueDate);
                    SQLiteParameter assigneeParam = new(@"assigneeVal", task.Assignee);
                    SQLiteParameter columnParam = new(@"columnVal", task.Column);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardidParam);
                    command.Parameters.Add(creationtimeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(duedateParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(columnParam);

                    command.Prepare();
                    result = command.ExecuteNonQuery();

                    log.Info($"Successful insert in {TableName} table");
                }
                catch
                {
                    log.Error($"Failed insertion on {TableName} table");
                    throw new Exception($"Cannot perform insert on {TableName} table");
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
        /// This method updates the assignee in table row in the database.
        /// </summary>
        /// <param name="id">The id of the row</param>
        /// <param name="email">The email of the earliest assignee</param>
        /// <param name="assignee">The email of the new assignee</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        public bool UpdateTasksAssignee(int id, string email, string assignee)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int result = -1;
                SQLiteCommand command = new(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"update {TableName} set Assignee='{assignee}' where BoardID={id} and Assignee='{email}' and (Column=0 or Column=1)";
                    command.Prepare();
                    result = command.ExecuteNonQuery();
                    log.Info("Suceessful update in DB Tasks table");
                }
                catch
                {
                    log.Error($"Failed update on {TableName} table");
                    throw new Exception($"Cannot perform update on {TableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return result >= 0;
            }
        }

        /// <summary>
        /// This method deletes DTOs from the database according to its board.
        /// </summary>
        /// <param name="id">The board's id</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="TaskMapper"/>)</returns>
        public bool DeleteTasksByBoardID(long id)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TableName} where BoardID={id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Deletion successful from DB {TableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {TableName} where Id={id}");
                    throw new Exception($"Cannot delete from {TableName} where Id = {id}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
    }
}
