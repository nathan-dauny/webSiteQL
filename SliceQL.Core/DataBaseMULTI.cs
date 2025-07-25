using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using CsvHelper;
using CsvToDynamicObjectLib;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Matrix = System.Collections.Generic.List<string?[]>;
using MatrixDYNAMIC = System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>;

namespace SliceQL.Core
{
    public class DatabaseMULTI : IDisposable
    {
        /// <summary>
        /// String to define the database.
        ///Data Source: to specify the database name and the location of the SQLite database.
        ///Use the special data source filename :memory: to create an in-memory database. 
        ///using :memory:, each connection creates its own database.
        ///Mode: Defines the access mode for the database.
        ///Cache: Controls the caching behavior for the database connection. Shared: make the database shareable between multiple connections

        /// </summary>
        public const string BDD_CONNECTION_STRING = $"Data Source=:memory:;Cache=Shared;";
        /// <summary>
        /// Gets a a connection to communicate with the database
        /// </summary>
        private readonly SQLiteConnection connection;

        /// <summary>
        /// Write the Query to create the table and the queries to insert data
        /// <param name="data">Data in the file</param>
        /// /// <param name="tableName">name of the table provided from the name of the File input</param>
        public DatabaseMULTI(Dictionary<StreamReader,string> CsvInputs)
        {
            connection = new SQLiteConnection(BDD_CONNECTION_STRING);
            connection.Open();
            foreach(var kvp in CsvInputs)
            {
                (CsvFinalObject csvObject, Dictionary<string, Type> dicoType) = ReturnObject.GetObjectsCSV(kvp.Key);
                IEnumerable<SQLiteCommand> queriesToSetupDatabase = BuildCommandQuery(csvObject, dicoType, kvp.Value);
                foreach (SQLiteCommand command in queriesToSetupDatabase)
                {
                    command.Connection = connection;
                    using (command)
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            
        }
        /// <summary>
        /// Call the dispose method of SQLite connection to release the resource. 
        /// Then Close the connection.
        void IDisposable.Dispose()
        {
            connection.Dispose();
        }
        /// <summary>
        /// Build the commands queries needed to provide the SQL table.
        /// CREATE TABLE
        /// DELETE 
        /// INSERT for each line
        /// <param name="data">Data in the file</param>
        static IEnumerable<SQLiteCommand> BuildCommandQuery(CsvFinalObject data,
            Dictionary<string, Type> columnstypeDico, string tableName)
        {
            const string DATA_TYPE = "TEXT";
            const string DATA_NULLABLE = "NOT NULL";

            //CsvParser csvParsed = new CsvParser(data);

            StringBuilder createTableQuery = new StringBuilder();
            createTableQuery.Append($"CREATE TABLE {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT,");

            int index = 0;
            StringBuilder insertValueQuery = new StringBuilder();
            StringBuilder insertTitleQuery = new StringBuilder();
            var test = data.First();
            foreach (var kvp in test)
            {
                createTableQuery.Append($"{kvp.Key} {TypeMapper.GetSQLType(columnstypeDico[kvp.Key])} ,");
                insertTitleQuery.Append($"{kvp.Key},");
                insertValueQuery.Append($"@Column{index},");
                index++;
            }
            createTableQuery
                .Remove(createTableQuery.Length - 1, 1)
                .Append(')');
            insertTitleQuery.Remove(insertTitleQuery.Length - 1, 1);
            insertValueQuery.Remove(insertValueQuery.Length - 1, 1);
            string insertQuery = $"INSERT INTO {tableName} ( {insertTitleQuery} ) VALUES ( {insertValueQuery} )";

            yield return new SQLiteCommand(createTableQuery.ToString());

            yield return new SQLiteCommand($"DELETE FROM {tableName}");

            foreach (var csvLine in data)
            {
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery);
                int indexfe = 0;
                foreach (var kvp in csvLine)
                {
                    insertCommand.Parameters.AddWithValue("@Column" + indexfe, kvp.Value);
                    indexfe++;
                }
                yield return insertCommand;
            }
        }

        public MatrixDYNAMIC ExecuteSqlQuery(Query sqlQUery)
        {
            using (SQLiteCommand userCommand = new SQLiteCommand(sqlQUery.asString, connection))
            {
                using (SQLiteDataReader readerResultQuery = userCommand.ExecuteReader())
                {
                    MatrixDYNAMIC matrix = MatrixAdapter.ToMatrix(readerResultQuery);
                    return matrix;
                }
            }
        }

        public class MatrixAdapter
        {
            public static MatrixDYNAMIC ToMatrix(SQLiteDataReader sqlReader)
            {
                return SqlReaderToMatrixAdapter(sqlReader);
            }
            static MatrixDYNAMIC SqlReaderToMatrixAdapter(SQLiteDataReader sqlReader)
            {
                MatrixDYNAMIC matrix = new MatrixDYNAMIC();
                string[] valueTitle = new string[sqlReader.FieldCount];
                for (int j = 0; j < sqlReader.FieldCount; j++)
                {
                    valueTitle[j] = sqlReader.GetName(j).ToString();
                }

                while (sqlReader.Read())
                {
                    var lineResult = new Dictionary<string, object>();
                    for (int j = 0; j < sqlReader.FieldCount; j++)
                    {
                        if (sqlReader.IsDBNull(j))
                        {
                            lineResult[valueTitle[j]] = null;
                        }
                        else
                        {
                            lineResult[valueTitle[j]] = sqlReader.GetValue(j);
                        }
                    }
                    matrix.Add(lineResult);
                }
                return matrix;
            }
        }
    }
}
