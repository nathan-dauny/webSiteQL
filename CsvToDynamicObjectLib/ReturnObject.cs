using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToDynamicObjectLib
{
    public static class ReturnObject
    {
        public static (CsvFinalObject, Dictionary<string, Type>) GetObjectsCSV(StreamReader data)
        {
            var reader = new CsvReaderConverter();
            var csvRead = reader.ReadCsv(data);
            //var headers = csvRead.First().Select(kvp => kvp.Key).ToList();

            var columnsTypeMULTITHREADING = new ColumnsTypeMULTITHREADING();
            var columnstypeDico = columnsTypeMULTITHREADING.GetAllColumnsTypesMULTITHREADING(csvRead);
            var csvTyped = new CsvTyped();
            var csvTypedFields = csvTyped.GetFieldsTyped(columnstypeDico, csvRead);
            return (new CsvFinalObject(csvTypedFields), columnstypeDico);
        }
    }
}
