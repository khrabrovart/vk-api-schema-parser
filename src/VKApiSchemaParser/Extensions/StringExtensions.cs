using System.Linq;

namespace VKApiSchemaParser.Extensions
{
    internal static class StringExtensions
    {
        public static string Beautify(this string str)
        {
            return str
                .Trim()
                .Split('_', ' ')
                .Aggregate(string.Empty, (r, p) => r + p[0].ToString().ToUpper() + p.Substring(1));
        }
    }
}
