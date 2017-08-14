using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiMethodsSchema
    {
        public IEnumerable<ApiError> Errors { get; set; }
        public IEnumerable<ApiMethod> Methods { get; set; }
    }
}
