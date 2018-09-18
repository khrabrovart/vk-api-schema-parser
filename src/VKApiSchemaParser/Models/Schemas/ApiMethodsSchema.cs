using System.Collections.Generic;

namespace VKApiSchemaParser.Models.Schemas
{
    public class ApiMethodsSchema : IApiSchema
    {
        public IEnumerable<ApiError> Errors { get; set; }
        public IEnumerable<ApiMethod> Methods { get; set; }
    }
}
