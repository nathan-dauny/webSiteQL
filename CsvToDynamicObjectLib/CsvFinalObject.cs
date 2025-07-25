using System.Collections;

namespace CsvToDynamicObjectLib
{
    public class CsvFinalObject : IEnumerable<CsvLine>
    {
        public List<CsvLine> csvObject { get; private set; } = new List<CsvLine>();
        public CsvFinalObject(List<Dictionary<string, object>> csvTyped)
        {
            csvObject=csvTyped.Select(kvp=> new CsvLine(kvp)).ToList();
        }
        public IEnumerator<CsvLine> GetEnumerator() => csvObject.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
