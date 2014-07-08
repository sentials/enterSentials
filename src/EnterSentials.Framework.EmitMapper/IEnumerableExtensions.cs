using System.Collections.Generic;
using System.Text;

namespace EnterSentials.Framework.EmitMapper
{
    // Borrowed or adapted from: EmitMapper source code
    public static class IEnumerableExtensions
    {
        public static string ToCSV<T>(this IEnumerable<T> collection, string delim)
        {
            if (collection == null)
            {
                return "";
            }

            StringBuilder result = new StringBuilder();
            foreach (T value in collection)
            {
                result.Append(value);
                result.Append(delim);
            }
            if (result.Length > 0)
            {
                result.Length -= delim.Length;
            }
            return result.ToString();
        }
    }
}
