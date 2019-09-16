using System.Collections.Generic;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser
{
    internal static class StringFormatMapper
    {
        private static readonly IDictionary<string, ApiStringFormat> _formatsMapping = new Dictionary<string, ApiStringFormat>
        {
            { JsonStringConstants.Uri, ApiStringFormat.Uri }
        };

        public static ApiStringFormat? Map(string formatName)
        {
            return formatName == null || !_formatsMapping.TryGetValue(formatName, out var stringFormat)
                ? null
                : (ApiStringFormat?)stringFormat;
        }
    }
}
