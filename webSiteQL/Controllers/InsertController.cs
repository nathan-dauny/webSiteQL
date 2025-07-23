using CsvToDynamicObjectLib;
using CsvToSqlInsert;
using Microsoft.AspNetCore.Mvc;
using SliceQL.web.Models;

namespace SliceQL.web.Controllers
{
    public class InsertController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new InsertGeneratorViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(InsertGeneratorViewModel model)
        {
            if (model.CsvFile == null || string.IsNullOrWhiteSpace(model.TableName))
            {
                model.ErrorMessage = "Please provide both a CSV file and a table name.";
                return View(model);
            }

            try
            {
                using var stream = model.CsvFile.OpenReadStream();
                using var reader = new StreamReader(stream);

                var (csvObject, columnTypes) = ReturnObject.GetObjectsCSV(reader);

                var generator = new CsvToSqlInsertGenerator(new SqlInsertStatementBuilder());

                model.InsertStatements = generator.Generate(model.TableName, csvObject, columnTypes).ToList();
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "Error: " + ex.Message;
            }

            return View(model);
        }
    }
}
