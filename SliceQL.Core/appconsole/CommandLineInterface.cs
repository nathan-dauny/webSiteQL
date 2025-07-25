using System.CommandLine;
using System.IO;

namespace SliceQL.Core.appconsole
{
    /// <summary>
    /// Represents a commandline an option: the file input.
    /// Implements the <see cref="IInputInterface"/> interface.
    /// </summary>
    public class CommandLineInterface : IInputInterface
    {
        private RootCommand command = new RootCommand();
        public StreamReader data { get; private set; } = null!;
        public string querySql { get; private set; } = null!;

        private ParsingError parsingError = new ParsingError();
        public string tableName { get; private set; } = "tableName";

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineInterface"/> class.
        /// </summary>
        /// <param name="args">The args of the commandline.</param>
        public CommandLineInterface(string[] args)
        {
            //querySql = "";
            //data=new StreamReader("");
            // Precondition: args has to be defined like this: {"XLQL","-f";"pathfile";"s";"sql query"} || {"XLQL","--data-file";"pathfile";"--data-sql";"sql query"}
            var fileOptionInput = new Option<string>(
                name: "--data-file",
                description: "Data file to read from."
            );
            var sqlOptionInput = new Option<string>(
                name: "--sql",
                description: "SQL query to execute."
            );

            fileOptionInput.AddAlias("-f");
            sqlOptionInput.AddAlias("-s");

            command.AddOption(fileOptionInput);
            command.AddOption(sqlOptionInput);

            //command.SetHandler(HandleCommand, fileOptionInput, sqlOptionInput);
            //command.Invoke(args);

            // Utilise Parse au lieu d’Invoke
            var result = command.Parse(args);

            var fileInput = result.GetValueForOption(fileOptionInput);
            var sqlinput = result.GetValueForOption(sqlOptionInput);

            HandleCommand(fileInput, sqlinput);


            if (parsingError.error)
            {
                Console.WriteLine($"Input Error: {parsingError.message} (Code: {parsingError.code})");
                Environment.Exit((int)parsingError.code);
            }
        }

        public CommandLineInterface(StreamReader reader, string sqlQuery, string tableName = "table")
        {
            data = reader;
            querySql = sqlQuery;
            this.tableName = tableName;
        }



        /// <summary>
        /// Store content of the file in "Data" and the sql query in "QuerySql"
        /// <param name="fileInput">The file specified in args.</param>
        /// /// <param name="sqlinput">The Sql query specified in args.</param>
        private void HandleCommand(string? fileInput, string? sqlinput)
        {
            if (string.IsNullOrEmpty(fileInput))
            {
                parsingError.code = ParsingError.codeError.InputError;
                parsingError.message = "no file provided";
                return;
            }
            if (!File.Exists(fileInput))
            {
                parsingError.code = ParsingError.codeError.InputError;
                parsingError.message = "file doesn't exist";
                return;
            }
            if (string.IsNullOrEmpty(sqlinput))
            {
                parsingError.code = ParsingError.codeError.InputError;
                parsingError.message = "no sql query provided";
                return;
            }
            data = new StreamReader(fileInput);
            tableName = Path.GetFileNameWithoutExtension(fileInput);
            querySql = sqlinput;
        }
    }
}
