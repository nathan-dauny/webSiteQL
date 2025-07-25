using System.Collections.Concurrent;
using CsvHelper;

namespace CsvToDynamicObjectLib
{
    public class ColumnsTypeMULTITHREADING
    {
        public Dictionary<string, Type> GetAllColumnsTypesMULTITHREADING(List<Dictionary<string, string>> csvParsed)
        {
            var headers = csvParsed.First().Select(kvp => kvp.Key).ToList();

            ConcurrentDictionary<string, Type> columnTypes = new();

            Parallel.ForEach(headers, header =>
            {
                var columnData = csvParsed.Select(kvp => kvp[header]);
                Type type = CsvTypeDetector.DetectColumnType(columnData);
                columnTypes[header] = type;
            });
            return columnTypes.ToDictionary();
        }

    }
}
