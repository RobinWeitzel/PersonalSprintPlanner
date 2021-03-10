using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Windows.Storage;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        const string DBName = "PersonalSprintPlanner.db";
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(DBName, CreationCollisionOption.ReplaceExisting);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = @"CREATE TABLE IF NOT
                    EXISTS Tasks (ID INTEGER  NOT NULL PRIMARY KEY, 
	                Title Text,
	                Description Text,
	                Position Text NOT NULL,
	                SprintPosition Text,
	                BoardID INTEGER NOT NULL,
	                Status Integer NOT NULL,
                    SprintRelevant Integer NOT NULL,
	                CreationDate Text NOT NULL,
	                DueDate Text,
	                Priority Integer NOT NULL);
                CREATE TABLE IF NOT EXISTS
                    Boards (ID INTEGER  NOT NULL PRIMARY KEY, 
                    Name Text NOT NULL,
                    Color INTEGER NOT NULL);";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                await createTable.ExecuteReaderAsync();

                db.Close();

                List<Models.Board> boards = (await GetBoards()).ToList();
                Models.Board defaultBoard = boards.Find(b => b.ID == 0);

                if (defaultBoard == null)
                {
                    await AddBoard(new Models.Board
                    {
                        Name = "Unassigned",
                        Color = Models.Color.Transparent
                    });
                }
            }
        }
        
        public static async Task<long> AddTask(Models.Task task)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"INSERT INTO Tasks VALUES (NULL, @Title, @Description, @Position, @SprintPosition, @BoardID, @Status, @SprintRelevant, @CreationDate, @DueDate, @Priority);";
                    if (task.Title == null)
                    {
                        insertCommand.Parameters.AddWithValue("@Title", DBNull.Value);
                    } else
                    {
                        insertCommand.Parameters.AddWithValue("@Title", task.Title);
                    }
                    if (task.Description == null)
                    {
                        insertCommand.Parameters.AddWithValue("@Description", DBNull.Value);

                    }
                    else
                    {
                        insertCommand.Parameters.AddWithValue("@Description", task.Description);
                    }
                    insertCommand.Parameters.AddWithValue("@Position", task.Position);
                    if (task.SprintPosition == null)
                    {
                        insertCommand.Parameters.AddWithValue("@SprintPosition", DBNull.Value);
                    }
                    else
                    {
                        insertCommand.Parameters.AddWithValue("@SprintPosition", task.SprintPosition);
                    }
                    insertCommand.Parameters.AddWithValue("@BoardID", task.BoardID);
                    insertCommand.Parameters.AddWithValue("@Status", task.Status);
                    insertCommand.Parameters.AddWithValue("@SprintRelevant", task.SprintRelevant);
                    insertCommand.Parameters.AddWithValue("@CreationDate", task.CreationDate);
                    if (task.DueDate == null)
                    {
                        insertCommand.Parameters.AddWithValue("@DueDate", DBNull.Value);
                    }
                    else
                    {
                        insertCommand.Parameters.AddWithValue("@DueDate", task.DueDate);
                    }
                    insertCommand.Parameters.AddWithValue("@Priority", task.Priority);

                    await insertCommand.ExecuteReaderAsync();

                    SqliteCommand selectCommand = new SqliteCommand
                            ("SELECT last_insert_rowid()", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    long id;

                    if (await query.ReadAsync())
                    {
                        id = query.GetInt32(0);
                    }
                    else
                    {
                        id = -1;
                    }

                    db.Close();

                    return id;
                }
            } catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");

                return -1;
            }
        }

        public static async void UpdateTaskSprintRelevance(Models.Task task)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"UPDATE Tasks SET
                        SprintRelevant = @SprintRelevant
                        WHERE ID = @ID;";

                    insertCommand.Parameters.AddWithValue("@SprintRelevant", task.SprintRelevant);
                    insertCommand.Parameters.AddWithValue("@ID", task.ID);

                    await insertCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async void UpdateTaskPosition(Models.Task task)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"UPDATE Tasks SET
                        Position = @Position
                        WHERE ID = @ID;";

                    insertCommand.Parameters.AddWithValue("@Position", task.Position);
                    insertCommand.Parameters.AddWithValue("@ID", task.ID);

                    await insertCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async void UpdateTaskSprintPosition(Models.Task task)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"UPDATE Tasks SET
                        SprintPosition = @SprintPosition
                        WHERE ID = @ID;";

                    insertCommand.Parameters.AddWithValue("@SprintRelevant", task.SprintPosition);
                    insertCommand.Parameters.AddWithValue("@ID", task.ID);

                    await insertCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async Task DeleteTask(Models.Task task)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"DELETE FROM Tasks 
                        WHERE ID = @ID;";
                    
                    insertCommand.Parameters.AddWithValue("@ID", task.ID);

                    await insertCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async Task<bool> DeleteBoard(Models.Board board)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                            ("SELECT COUNT(ID) FROM Tasks WHERE Tasks.BoardID = @BoardID", db);

                    selectCommand.Parameters.AddWithValue("@BoardID", board.ID);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    long count = 0;

                    if (await query.ReadAsync())
                    {
                        count = query.GetInt32(0);
                    }
                    
                    if(count > 0)
                    {
                        throw new Exception("Cant delete board because it is still in use");
                    }

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"DELETE FROM Boards 
                        WHERE ID = @ID;";

                    insertCommand.Parameters.AddWithValue("@ID", board.ID);

                    await insertCommand.ExecuteReaderAsync();
                    return true;
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                return false;
            }
        }

        public static async void UpdateTask(Models.Task task)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"UPDATE Tasks SET
                        Title = @Title, 
                        Description = @Description,
                        Position = @Position,
                        SprintPosition = @SprintPosition,
                        BoardID = @BoardID,
                        Status = @Status,
                        SprintRelevant = @SprintRelevant,
                        CreationDate = @CreationDate,
                        DueDate = @DueDate,
                        Priority = @Priority
                        WHERE ID = @ID;";
                    if (task.Title == null)
                    {
                        insertCommand.Parameters.AddWithValue("@Title", DBNull.Value);
                    }
                    else
                    {
                        insertCommand.Parameters.AddWithValue("@Title", task.Title);
                    }
                    if (task.Description == null)
                    {
                        insertCommand.Parameters.AddWithValue("@Description", DBNull.Value);

                    }
                    else
                    {
                        insertCommand.Parameters.AddWithValue("@Description", task.Description);
                    }
                    insertCommand.Parameters.AddWithValue("@Position", task.Position);
                    if (task.SprintPosition == null)
                    {
                        insertCommand.Parameters.AddWithValue("@SprintPosition", DBNull.Value);
                    }
                    else
                    {
                        insertCommand.Parameters.AddWithValue("@SprintPosition", task.SprintPosition);
                    }
                    insertCommand.Parameters.AddWithValue("@BoardID", task.BoardID);
                    insertCommand.Parameters.AddWithValue("@Status", task.Status);
                    insertCommand.Parameters.AddWithValue("@SprintRelevant", task.SprintRelevant);
                    insertCommand.Parameters.AddWithValue("@CreationDate", task.CreationDate);
                    if (task.DueDate == null)
                    {
                        insertCommand.Parameters.AddWithValue("@DueDate", DBNull.Value);
                    }
                    else
                    {
                        insertCommand.Parameters.AddWithValue("@DueDate", task.DueDate);
                    }
                    insertCommand.Parameters.AddWithValue("@Priority", task.Priority);
                    insertCommand.Parameters.AddWithValue("@ID", task.ID);

                    await insertCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async Task<long> AddBoard(Models.Board board)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"INSERT INTO Boards VALUES (NULL, @Name, @Color);";
                    insertCommand.Parameters.AddWithValue("@Name", board.Name);
                    insertCommand.Parameters.AddWithValue("@Color", board.Color);

                    await insertCommand.ExecuteReaderAsync();

                    SqliteCommand selectCommand = new SqliteCommand
                            ("SELECT last_insert_rowid()", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    long id;

                    if (await query.ReadAsync())
                    {
                        id = query.GetInt32(0);
                    }
                    else
                    {
                        id = -1;
                    }

                    db.Close();

                    return id;
                } 
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
                return -1;
            }

        }

        public static async Task ClearSprint()
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"UPDATE Tasks SET
                        SprintRelevant = @SprintRelevant;";
                    insertCommand.Parameters.AddWithValue("@SprintRelevant", false);

                    await insertCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async void UpdateBoard(Models.Board board)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = @"UPDATE Boards SET
                        Name = @Name, 
                        Color = @Color
                        WHERE ID = @ID;";
                    insertCommand.Parameters.AddWithValue("@Name", board.Name);
                    insertCommand.Parameters.AddWithValue("@Color", board.Color);
                    insertCommand.Parameters.AddWithValue("@ID", board.ID);

                    await insertCommand.ExecuteReaderAsync();
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
        }

        public static async Task<IEnumerable<Models.Board>> GetBoards()
        {
            string getDataQuery = @"
            SELECT Boards.ID,
                Boards.Name,
                Boards.Color
            FROM Boards;";

            List<Models.Board> entries = new List<Models.Board>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);

            try
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        (getDataQuery, db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (await query.ReadAsync())
                    {
                        Models.Board board = new Models.Board()
                        {
                            ID = query.GetInt32(0),
                            Name = query.GetString(1),
                            Color = (Models.Color)System.Enum.Parse(typeof(Models.Color), query.GetString(2))
                        };

                        entries.Add(board);
                    }

                    db.Close();
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return entries;
        }

        public static async Task<IEnumerable<Models.Task>> GetTasks(bool backlog)
        {
            string getDataQuery = @"
            SELECT Tasks.ID,
                Tasks.Title,
                Tasks.Description,
                Tasks.Position,
                Tasks.SprintPosition,
                Tasks.BoardID,
                Tasks.Status,
                Tasks.SprintRelevant,
                Tasks.CreationDate,
                Tasks.DueDate,
                Tasks.Priority
            FROM Tasks;
            ";

            if (backlog)
            {
                getDataQuery += "ORDER BY Tasks.Position;";
            }
            else
            {
                getDataQuery += "ORDER BY Tasks.SprintPosition;";
            }

            List<Models.Task> entries = new List<Models.Task>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);

            try
            {
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        (getDataQuery, db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (await query.ReadAsync())
                    {
                        Models.Task task = new Models.Task()
                        {
                            ID = query.GetInt32(0),
                            Title = query.IsDBNull(1) ? null : query.GetString(1),
                            Description = query.IsDBNull(2) ? null : query.GetString(2),
                            Position = query.GetString(3),
                            SprintPosition = query.IsDBNull(4) ? null : query.GetString(4),
                            BoardID = query.GetInt32(5),
                            Status = (Models.Status)System.Enum.Parse(typeof(Models.Status), query.GetString(6)),
                            SprintRelevant = query.GetBoolean(7),
                            CreationDate = !query.IsDBNull(8) ? query.GetDateTime(8) : default,
                            Priority = (Models.Priority)System.Enum.Parse(typeof(Models.Priority), query.GetString(10))
                        };

                        if(!query.IsDBNull(9))
                        {
                            task.DueDate = query.GetDateTime(9);
                        } 

                        entries.Add(task);
                    }

                    db.Close();
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }
            
            return entries;
        }
    }
}