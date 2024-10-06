using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class Mapper
    {
        protected readonly string _connectionString;
        private readonly string _tableName;
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Mapper(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            _tableName = tableName;
        }

        /// <summary>
        /// This method gets a list of DTO by ids from the database.
        /// </summary>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="Mapper"/>)</returns>
        protected List<DTO> Select(int id)
        {
            List<DTO> results = new();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new(null, connection) { CommandText = $"select * from {_tableName} where Id={id};" };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.Add(ConvertReaderToObject(dataReader));
                    log.Info("Successful Select");
                }
                catch
                {
                    log.Error("Failed Select");
                    throw new Exception("Cannot perform Select on DB table");
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
        /// This method gets a list of DTO from the database.
        /// </summary>
        /// <returns>Returns DTO list, unless an error occurs (see <see cref="Mapper"/>)</returns>
        protected List<DTO> Select()
        {
            List<DTO> results = new();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new(null, connection) { CommandText = $"select * from {_tableName};" };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.Add(ConvertReaderToObject(dataReader));
                    log.Info("Successful Select");
                }
                catch
                {
                    log.Error("Failed Select");
                    throw new Exception("Cannot perform Select on DB table");
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
        /// Abstracts method to override.
        /// </summary>
        /// <param name="reader">The reader of the database</param>
        /// <returns>Returns DTO, unless an error occurs (see <see cref="Mapper"/>)</returns>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

        /// <summary>
        /// This method updates a row in the database.
        /// </summary>
        /// <param name="id">The id of the row</param>
        /// <param name="attributeName">The name of the cell to update</param>
        /// <param name="attributeValue">The value to update</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Update(long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where Id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed update in {_tableName} table");
                    throw new Exception($"Cannot insert to {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method updates a row in the database.
        /// </summary>
        /// <param name="emailId">The id of the row</param>
        /// <param name="attributeName">The name of the cell to update</param>
        /// <param name="attributeValue">The value to update</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Update(string emailId, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where Email='{emailId}'"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed insertion into {_tableName} table");
                    throw new Exception($"Cannot insert to {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// This method updates a row in the database.
        /// </summary>
        /// <param name="id">The id of the row</param>
        /// <param name="attributeName">The name of the cell to update</param>
        /// <param name="attributeValue">The value to update</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Update(long id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where Id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed to update {_tableName} table");
                    throw new Exception($"Cannot insert to {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method updates a row in the database.
        /// </summary>
        /// <param name="emailId">The id of the row</param>
        /// <param name="attributeName">The name of the cell to update</param>
        /// <param name="attributeValue">The value to update</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Update(string emailId, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where Email='{emailId}'"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed to update in {_tableName} table");
                    throw new Exception($"Cannot insert to {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method updates a row in the database.
        /// </summary>
        /// <param name="boardId">The id of the board</param>
        /// <param name="id">The id of the row</param>
        /// <param name="attributeName">The name of the cell to update</param>
        /// <param name="attributeValue">The value to update</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Update(long boardId, long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where Id={id} and boardID={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed to update {_tableName} table");
                    throw new Exception($"Cannot update {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method updates a row in the database.
        /// </summary>
        /// <param name="boardId">The id of the board</param>
        /// <param name="id">The id of the row</param>
        /// <param name="attributeName">The name of the cell to update</param>
        /// <param name="attributeValue">The value to update</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Update(long boardId, long id, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where Id={id} and boardID={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed to update {_tableName} table");
                    throw new Exception($"Cannot update {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method updates a row in the database.
        /// </summary>
        /// <param name="boardId">The id of the board</param>
        /// <param name="id">The id of the row</param>
        /// <param name="attributeName">The name of the cell to update</param>
        /// <param name="attributeValue">The value to update</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Update(long boardId, long id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where Id={id} and BoardID={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed to update {_tableName} table");
                    throw new Exception($"Cannot update {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method deletes of DTO from the database.
        /// </summary>
        /// <param name="id">The id of the row</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Delete(long id)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where Id={id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Deletion successful from DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {_tableName} where Id={id}");
                    throw new Exception($"Cannot delete from {_tableName} where Id = {id}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();   
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method deletes a row from the database.
        /// </summary>
        /// <param name="id">The id of the row</param>
        /// <param name="boardID">The id of the board</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Delete(long id, long boardID)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where Id={id} and BoardID={boardID}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Deletionrow successful from DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {_tableName} where Id={id} and boardID={boardID}");
                    throw new Exception($"Cannot delete from {_tableName} where Id = {id} and boardID={boardID}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();   
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method deletes a row from the database.
        /// </summary>
        /// <param name="emailId">The id of the row</param>
        /// <param name="boardID">The id of the board</param>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool Delete(string emailId, long boardID)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where Email='{emailId}' and Id={boardID}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Deleted row successfully in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {_tableName} where Id={emailId} and boardID={boardID}");
                    throw new Exception($"Cannot delete from {_tableName} where Id = {emailId} and boardID={boardID}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();  
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method deletes a table data.
        /// </summary>
        /// <returns>Returns boolean if successful, unless an error occurs (see <see cref="Mapper"/>)</returns>
        public bool DeleteData()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"{_tableName} data deleted from DB");
                }
                catch
                {
                    log.Error($"Failed deleting {_tableName} data from DB");
                    throw new Exception($"Cannot delete data of {_tableName}");
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