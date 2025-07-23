using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SliceQL.Core;
using SliceQL.web.Models;

namespace SliceQL.web.Controllers
{
    public class QueryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new QueryInputViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(QueryInputViewModel model)
        {
            if (model.CsvFiles == null || model.CsvFiles.Count == 0 || string.IsNullOrWhiteSpace(model.SqlQuery))
            {
                ModelState.AddModelError("", "Please provide at least one CSV file and an SQL query.");
                return View(model);
            }

            try
            {
                var csvStreams = new Dictionary<StreamReader, string>();

                foreach (var csvFile in model.CsvFiles)
                {
                    var stream = csvFile.OpenReadStream();
                    var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: false);
                    string tableName = Path.GetFileNameWithoutExtension(csvFile.FileName).Replace(" ", "_");
                    csvStreams.Add(reader, tableName);
                }
                using var db = new DatabaseMULTI(csvStreams);

                var query = new Query(model.SqlQuery);

                var result = db.ExecuteSqlQuery(query);

                if (result.Count > 0)
                    model.Columns = result.First().Keys.ToList();

                model.Result = result;

                foreach (var reader in csvStreams.Keys)
                {
                    reader.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", "Erreur : " + ex.Message);
            }
            return View(model);
        }
    }
}
