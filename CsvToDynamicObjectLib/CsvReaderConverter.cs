using System.Globalization;

namespace CsvToDynamicObjectLib
{
    /// <summary>
    /// Responsible for reading CSV data from a stream
    /// and converting it into a list of dictionaries (string key-value pairs).
    /// Each dictionary represents one row of the CSV file.
    /// </summary>
    public class CsvReaderConverter
    {
        /// <summary>
        /// Reads CSV data from the given stream and parses it row by row.
        /// Each row is returned as a dictionary where:
        ///   - the key is the column name (header),
        ///   - the value is the raw string value from the CSV.
        /// </summary>
        /// <param name="csvStream">The input stream containing CSV data.</param>
        /// <returns>A list of dictionaries, each representing a CSV row.</returns>
        public List<Dictionary<string, string>> ReadCsv(Stream csvStream)
        {
            // Use StreamReader to read the CSV content from the stream
            using var reader = new StreamReader(csvStream);
            return ReadCsv(reader);
        }
        public List<Dictionary<string, string>> ReadCsv(StreamReader csvStream)
        {
            var rows = new List<Dictionary<string, string>>();

            // Create a CsvReader instance from the CsvHelper library with invariant culture (standard formatting)
            using (var csv = new CsvHelper.CsvReader(csvStream, CultureInfo.InvariantCulture))
            {
                // Read all records as dynamic objects
                var records = csv.GetRecords<dynamic>();
                foreach (var record in records)
                {
                    // Create a dictionary to hold the column-value pairs for this row
                    var dict = new Dictionary<string, string>();

                    // Cast the dynamic record to a dictionary of string -> object
                    var d = (IDictionary<string, object>)record;

                    // Convert each value to string and add to the dictionary
                    foreach (var kvp in d)
                    {
                        dict[kvp.Key] = kvp.Value?.ToString();
                    }

                    // Add the parsed row to the result list
                    rows.Add(dict);
                }
            }

            // Return all parsed rows
            return rows;
        }
    }
}
