namespace SliceQL.Core
{
    public static class TypeMapper
    {
        private static readonly Dictionary<Type, string> _map = new()
        {
            { typeof(string), "TEXT" },
            { typeof(int), "INTEGER" },
            { typeof(long), "BIGINT" },
            { typeof(double), "DOUBLE PRECISION" },
            { typeof(float), "REAL" },
            { typeof(bool), "BOOLEAN" },
            { typeof(DateTime), "TIMESTAMP" },
        };
        public static string GetSQLType(Type type)
        {
            return _map[type];
        }
    }
}
