using System.Data.SQLite;
using System.Text;
using Matrix = System.Collections.Generic.List<string?[]>;

namespace SliceQL.Core.appconsole
{
    /// <summary>
    /// Represents a database filled from the input.
    /// </summary>
    public class Database : IDisposable
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
        public Database(StreamReader data, string tableName)
        {
            connection = new SQLiteConnection(BDD_CONNECTION_STRING);
            connection.Open();

            foreach (SQLiteCommand command in BuildCommandQuery(data, tableName))
            {
                command.Connection = connection;
                using (command)
                {
                    command.ExecuteNonQuery();
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
        static IEnumerable<SQLiteCommand> BuildCommandQuery(StreamReader data, string tableName)
        {
            const string DATA_TYPE = "TEXT";
            const string DATA_NULLABLE = "NOT NULL";

            CsvParser csvParsed = new CsvParser(data);

            StringBuilder createTableQuery = new StringBuilder();
            createTableQuery.Append($"CREATE TABLE {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT,");
            string[] titleDataColumnArray = csvParsed.dataList[0];

            StringBuilder insertValueQuery = new StringBuilder();
            StringBuilder insertTitleQuery = new StringBuilder();
            for (int columnIndex = 0; columnIndex < titleDataColumnArray.Length; columnIndex++)
            {
                createTableQuery.Append($"{titleDataColumnArray[columnIndex]} {DATA_TYPE} {DATA_NULLABLE},");
                insertTitleQuery.Append($"{titleDataColumnArray[columnIndex]},");
                insertValueQuery.Append($"@Column{columnIndex},");
            }

            createTableQuery
                .Remove(createTableQuery.Length - 1, 1)
                .Append(')');
            insertTitleQuery.Remove(insertTitleQuery.Length - 1, 1);
            insertValueQuery.Remove(insertValueQuery.Length - 1, 1);
            string insertQuery = $"INSERT INTO {tableName} ( {insertTitleQuery} ) VALUES ( {insertValueQuery} )";

            yield return new SQLiteCommand(createTableQuery.ToString());

            yield return new SQLiteCommand($"DELETE FROM {tableName}");

            foreach (string[] lineArray in csvParsed.dataList.Skip(1)) // ← ignore la première ligne
            {
                SQLiteCommand insertCommand = new SQLiteCommand(insertQuery);
                for (int columnIndex = 0; columnIndex < lineArray.Length; columnIndex++)
                {
                    insertCommand.Parameters.AddWithValue("@Column" + columnIndex, lineArray[columnIndex]);
                }
                yield return insertCommand;
            }
        }

        public void ExecuteSqlQuery(Query sqlQUery)
        {
            using (SQLiteCommand userCommand = new SQLiteCommand(sqlQUery.asString, connection))
            {
                using (SQLiteDataReader readerResultQuery = userCommand.ExecuteReader())
                {
                    Matrix matrix = MatrixAdapter.ToMatrix(readerResultQuery);
                    string result = MatrixPrinter.Print(matrix);
                    Console.WriteLine(result);
                }
            }
        }

        public class MatrixAdapter
        {
            public static Matrix ToMatrix(SQLiteDataReader sqlReader)
            {
                return SqlReaderToMatrixAdapter(sqlReader);
            }
            static Matrix SqlReaderToMatrixAdapter(SQLiteDataReader sqlReader)
            {
                Matrix matrix = new Matrix();
                string[] valueTitle = new string[sqlReader.FieldCount - 1];
                for (int j = 0; j < sqlReader.FieldCount - 1; j++)
                {
                    valueTitle[j] = sqlReader.GetName(j + 1).ToString();
                }
                matrix.Add(valueTitle);
                //sqlReader.Read();
                while (sqlReader.Read())
                {
                    string?[] valueField = new string[sqlReader.FieldCount - 1];
                    StringBuilder displayLine = new StringBuilder();

                    for (int j = 0; j < sqlReader.FieldCount - 1; j++)
                    {
                        if (sqlReader.IsDBNull(j + 1))
                        {
                            valueField[j] = "";
                        }
                        else
                        {
                            valueField[j] = sqlReader.GetValue(j + 1).ToString();
                        }
                    }
                    matrix.Add(valueField);
                }
                return matrix;
            }
        }

    }
}
