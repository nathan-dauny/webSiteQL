namespace SliceQL.web.Models
{
    public class InsertGeneratorViewModel
    {
        public IFormFile? CsvFile { get; set; }

        public string? TableName { get; set; }

        public List<string>? InsertStatements { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
