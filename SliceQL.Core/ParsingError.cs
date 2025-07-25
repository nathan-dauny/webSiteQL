namespace SliceQL.Core
{
    public class ParsingError
    {
        public enum codeError
        {
            NoError = 0,
            InputError = 1
        }
        public codeError code { get; set; }
        public string ?message { get; set; }
        public bool error
        {
            get { return code != codeError.NoError; }
        }
        public ParsingError()
        {
            code = codeError.NoError;
            message = null;
        }
    }
}
