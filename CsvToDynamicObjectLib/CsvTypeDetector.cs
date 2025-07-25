using System.Globalization;

namespace CsvToDynamicObjectLib
{
    /// <summary>
    /// Provides methods to detect the data type of CSV columns and convert string values accordingly.
    /// </summary>
    public static class CsvTypeDetector
    {
        private static readonly Type[] CandidateTypes = new Type[]
        {
            typeof(int),
            typeof(double),
            typeof(bool),
            typeof(DateTime),
            typeof(string)
        };

        /// <summary>
        /// Detects the most appropriate data type for a CSV column based on its values.
        /// </summary>
        /// <param name="valuesOfAColumn">The collection of string values from the column.</param>
        /// <returns>The detected <see cref="Type"/> for the column, defaulting to <see cref="string"/> if none match.</returns>
        public static Type DetectColumnType(IEnumerable<string> valuesOfAColumn)
        {
            foreach (var type in CandidateTypes)
            {
                if (type == typeof(string))
                    continue;

                if (valuesOfAColumn.All(v => string.IsNullOrWhiteSpace(v) || TryConvert(v, type)))
                    return type;
            }
            return typeof(string);
        }

        /// <summary>
        /// Attempts to convert a string value to the specified type.
        /// </summary>
        /// <param name="valueField">The string value to convert.</param>
        /// <param name="type">The target <see cref="Type"/> to convert to.</param>
        /// <returns>True if conversion is successful; otherwise, false.</returns>
        private static bool TryConvert(string valueField, Type type)
        {
            try
            {
                if (type == typeof(int))
                    int.Parse(valueField, NumberStyles.Integer, CultureInfo.InvariantCulture);
                else if (type == typeof(double))
                    double.Parse(valueField, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);
                else if (type == typeof(bool))
                    bool.Parse(valueField);
                else if (type == typeof(DateTime))
                    DateTime.Parse(valueField, CultureInfo.InvariantCulture);
                else

                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a string value to the specified type, returning the original string if conversion fails.
        /// </summary>
        /// <param name="val">The string value to convert.</param>
        /// <param name="type">The target <see cref="Type"/> to convert to.</param>
        /// <returns>The converted value as an <see cref="object"/>, or the original string if conversion fails.</returns>
        public static object ConvertToType(string val, Type type)
        {
            if (string.IsNullOrWhiteSpace(val))
                return null;

            try
            {
                if (type == typeof(int))
                    return int.Parse(val, NumberStyles.Integer, CultureInfo.InvariantCulture);
                else if (type == typeof(double))
                    return double.Parse(val, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);
                else if (type == typeof(bool))
                    return bool.Parse(val);
                else if (type == typeof(DateTime))
                    return DateTime.Parse(val, CultureInfo.InvariantCulture);
                else
                    return val;
            }
            catch
            {
                return val;
            }
        }
    }
}
