namespace SliceQL.Core
{
    public class Query
    {
        /// <summary>
        /// Represents the query specified in commandline.
        /// </summary>
        public string asString { get; private set; }
        public Query(string SqlQuery)
        {
            if (!IsValidSqlQuery(SqlQuery))
            {
                throw new InvalidSqlQueryException("Query is invalid because it contains a dangerous key word");
            }
            asString = SqlQuery;
        }
        /// <summary>
        /// Check the Sql Query.
        /// <param name="SqlQuery">Sql Query specified in commandline</param>
        static bool IsValidSqlQuery(string SqlQuery)
        {
            string[] riskyKeyWords = { "UPDATE", "DELETE", "INSERT", "DROP", "ALTER" };
            foreach (var keyWords in riskyKeyWords)
            {
                bool contains = SqlQuery.IndexOf(keyWords, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
