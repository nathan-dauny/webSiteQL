namespace CsvToDynamicObjectLib
{
    public class CsvTyped
    {
        public List<Dictionary<string,object>> GetFieldsTyped(Dictionary<string, Type> columnsType, List<Dictionary<string, string>> csvParsed)
        {
            var columnsName = csvParsed.First().Keys;
            var result = new List<Dictionary<string, object>>();
            foreach (var rawRow in csvParsed)
            {
                var typedFields = new Dictionary<string, object>();
                foreach (var col in columnsName)
                {
                    var val = rawRow[col];
                    var type = columnsType[col];
                    typedFields[col] = CsvTypeDetector.ConvertToType(val, type);
                }
                result.Add(typedFields);
            }
            return result;
        }
    }
}
