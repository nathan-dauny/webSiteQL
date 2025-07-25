namespace CsvToDynamicObjectLib
{
    /// <summary>
    /// Responsible for detecting the data types of columns in a parsed CSV input.
    /// </summary>
    public class ColumnsType
    {
        /// <summary>
        /// Analyzes the parsed CSV rows and determines the most appropriate .NET type for each column.
        /// </summary>
        /// <param name="csvParsed">A list of dictionaries representing CSV rows (each row is a dictionary of column name to raw string value).</param>
        /// <returns>A dictionary mapping each column name to its inferred data type.</returns>
        public Dictionary<string, Type> GetAllColumnsTypes(List<Dictionary<string, string>> csvParsed)
        {
            Dictionary<string, Type> ColumnTypesDico = new Dictionary<string, Type>();
            var columnsName = csvParsed.First().Keys;

            // For each column, detect the most likely type based on the column's values
            foreach (var col in columnsName)
            {
                // Extract all values for the current column
                var colValues = csvParsed.Select(r => r[col]);

                var detectedType = CsvTypeDetector.DetectColumnType(colValues);

                // Store the detected type for the column
                ColumnTypesDico[col] = detectedType;
            }
            return ColumnTypesDico;
        }
    }
}
