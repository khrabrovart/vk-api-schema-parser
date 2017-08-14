using System.Linq;

namespace VKApiSchemaParser.Extensions
{
    internal static class StringExtensions
    {
        public static string FormatAsName(this string src)
        {
            var nameParts = src.Split('_', '.');

            return nameParts.Aggregate("", (r, p) =>
            {
                return r + p[0].ToString().ToUpper() + p.Substring(1);
            });
        }
    }
}
