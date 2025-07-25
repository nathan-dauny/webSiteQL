namespace SliceQL.Core
{
    public class InvalidSqlQueryException : Exception
    {
        public InvalidSqlQueryException()
        {
        }

        // Constructor with message
        public InvalidSqlQueryException(string message)
            : base(message)
        {
        }

        // Constructor with message and cause
        public InvalidSqlQueryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
