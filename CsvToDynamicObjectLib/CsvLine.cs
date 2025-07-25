using System.Collections;

namespace CsvToDynamicObjectLib
{
    /// <summary>
    /// This Object represent a Csv line as a Dictionnary
    /// (Key = field column, Value = field value)
    /// </summary>
    public class CsvLine : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Dictionary containing the key-value pairs representing the CSV fields.
        /// </summary>
        public Dictionary<string, object> Fields { get; }
        /// <summary>
        /// Initializes a new instance of <see cref="CsvLine"/> with a dictionary of fields.
        /// </summary>
        /// <param name="fields">The dictionary of column names and their values.</param>
        public CsvLine(Dictionary<string, object> fields)
        {
            Fields = fields;
        }
        /// <summary>
        /// Gets the value of a field by column name.
        /// </summary>
        /// <param name="columnName">The name of the column to look up.</param>
        /// <returns>The value associated with the column, or null if it does not exist.</returns>
        public object GetField(string columnName)
        {
            if (Fields.ContainsKey(columnName))
            {
                return Fields[columnName];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Returns a string representation of the dictionary (key=value pairs).
        /// </summary>
        /// <returns>A string listing the key=value pairs of the dictionary.</returns>
        public override string ToString()
        {
            return string.Join(", ", Fields.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => Fields.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
