using System.Linq;

namespace VKApiSchemaParser.Extensions
{
    internal static class StringExtensions
    {
        public static string Beautify(this string str)
        {
            var trimmed = str?.Trim();

            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var splitted = trimmed.Split('_', ' ');

            if (splitted.Length == 1)
            {
                return trimmed[0].ToString().ToUpper() + trimmed.Substring(1);
            }
            
            return splitted.Aggregate(string.Empty, (r, p) => r + p[0].ToString().ToUpper() + p.Substring(1));
        }
    }
}
