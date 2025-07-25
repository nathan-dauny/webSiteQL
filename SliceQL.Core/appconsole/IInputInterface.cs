namespace SliceQL.Core.appconsole
{
    public interface IInputInterface
    {
        /// <summary>
        /// Gets the reader to read the content of the file input specified in args.
        /// </summary>
        /// <value>the content of the file input specified in args.</value>
        StreamReader data { get; }
        /// <summary>
        /// Gets Sql query specified in args.
        /// </summary>
        /// <value>Sql query specified in args.</value>
        string querySql { get; }
        public string tableName { get; }
    }
}
