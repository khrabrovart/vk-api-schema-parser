using System.Linq;

namespace VKApiSchemaParser.Extensions
{
    internal static class StringExtensions
    {
        public static string Beautify(this string str)
        {
            return str
                .Replace(' ', '_')
                .Split('_')
                .Aggregate("", (r, p) => r + p[0].ToString().ToUpper() + p.Substring(1));
        }
    }
}
