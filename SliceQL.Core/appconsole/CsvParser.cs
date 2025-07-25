using System.Threading.Channels;
using System;
using Matrix = System.Collections.Generic.List<string[]>;

namespace SliceQL.Core.appconsole
{
    /// <summary>
    /// Represent the List<string[]> dataList of the file's reader
    /// </summary>
    /// 
    internal class CsvParser
    {
        private const string CSV_SEPARATOR = ";";
        /// <summary>
        /// Gets the data as a list of array for each line
        /// </summary>
        public Matrix dataList = new Matrix();
        public CsvParser(StreamReader data)
        {
            using (data)
            {
                string? line;
                line = data.ReadLine();
                string CsvSeparator = GetCsvSeparator(line);
                dataList.Add(line.Split(CsvSeparator));
                while ((line = data.ReadLine()) != null)
                {
                    dataList.Add(line.Split(CsvSeparator));
                }
            }
        }
        string GetCsvSeparator(string? line)
        {
            Dictionary<string, int> ocurenceBySeparator = new Dictionary<string, int>()
            {
                { ";", 0 },
                { ",", 0 },
                { "\t", 0 },
                { " ", 0 },
                { "|", 0 }
            };
            foreach (var item in ocurenceBySeparator)
            {
                int indexThroughLine = 0;
                int countOcurenceItem = 0;
                while ((indexThroughLine = line.IndexOf(item.Key, indexThroughLine)) != -1)
                {
                    countOcurenceItem++;
                    indexThroughLine += item.Key.Length;
                }
                ocurenceBySeparator[item.Key] = countOcurenceItem;
            }
            return ocurenceBySeparator.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }
    }
}
