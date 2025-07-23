namespace SliceQL.web.Models
{
    public class QueryInputViewModel
    {
        public List<IFormFile> CsvFiles { get; set; }
        public string SqlQuery { get; set; }

        public List<string> Columns { get; set; } = new();
        public List<Dictionary<string, object>> Result { get; set; } = new();
    }
}
