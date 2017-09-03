using System.Linq;

namespace VKApiSchemaParser.Extensions
{
    internal static class StringExtensions
    {
        public static string Beautify(this string src)
        {
            return src
                .Split('_')
                .Aggregate("", (r, p) => r + p[0].ToString().ToUpper() + p.Substring(1));
        }
    }
}
