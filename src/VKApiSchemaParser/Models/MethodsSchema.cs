using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    internal class MethodsSchema
    {
        public IDictionary<string, ApiMethod> Methods { get; set; }

        public IDictionary<string, ApiError> Errors { get; set; }
    }
}
