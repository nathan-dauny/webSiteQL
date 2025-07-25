namespace SliceQL.Core.appconsole
{
    public class CommandLineFileErrorException : Exception
    {
        public CommandLineFileErrorException()
        {

        }
        public CommandLineFileErrorException(string message)
            : base(message)
        {

        }
    }
}
