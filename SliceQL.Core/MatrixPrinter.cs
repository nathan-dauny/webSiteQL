using System.Text;
using Matrix = System.Collections.Generic.List<string?[]>;

namespace SliceQL.Core
{
    internal class MatrixPrinter
    {
        /// <summary>
        /// Represents a proper display of a matrice in the console
        /// </summary>
        public static string Print(Matrix matrix)
        {
            int[] getMaxCharByColumn = GetMaxCharByColumn(matrix);
            IEnumerable<string> buildedStringLines = StringBuildResult(matrix, getMaxCharByColumn);
            return string.Join("\n", buildedStringLines);
        }
        static int[] GetMaxCharByColumn(Matrix matrix)
        {
            int columnCount = matrix.First().Length;
            int[] maxLengths = new int[columnCount];
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                maxLengths[columnIndex] = matrix.Select(row => (row[columnIndex] ?? "").Length).Max();
            }
            return maxLengths;
        }
        static IEnumerable<string> StringBuildResult(Matrix matrix, int[] numberCharMaxByColumn)
        {
            const char columnDivider = '|';
            const char cornerDivider = '+';
            const char rowDivider = '-';

            List<StringBuilder> displayLineList2 = new List<StringBuilder>();

            StringBuilder displayTitleLine = new StringBuilder(columnDivider);
            StringBuilder displaySpacerLine = new StringBuilder(cornerDivider);
            for (int columnIndex = 0; columnIndex < numberCharMaxByColumn.Length; columnIndex++)
            {
                string dashes = new string(rowDivider, numberCharMaxByColumn[columnIndex]);
                displayTitleLine.Append($"{(matrix.First()[columnIndex] ?? "").PadRight(numberCharMaxByColumn[columnIndex])}{columnDivider}");
                displaySpacerLine.Append($"{dashes}{cornerDivider}");
            }
            yield return displayTitleLine.ToString();
            yield return displaySpacerLine.ToString();

            foreach (string?[] row in matrix.Skip(1))
            {
                StringBuilder displayLine = new StringBuilder(columnDivider);
                for (int j = 0; j < numberCharMaxByColumn.Length; j++)
                {
                    displayLine.Append($"{(row[j] ?? "").PadRight(numberCharMaxByColumn[j])}{columnDivider}");
                }
                yield return displayLine.ToString();
            }
            yield return displaySpacerLine.ToString();
        }
    }
}
