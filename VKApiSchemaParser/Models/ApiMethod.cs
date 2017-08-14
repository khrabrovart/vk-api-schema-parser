using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiMethod
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ApiAccessTokenType> AccessTokenTypes { get; set; }
        public IEnumerable<ApiMethodParameter> Parameters { get; set; }
        public IEnumerable<ApiMethodResponse> Responses { get; set; }
        public IEnumerable<ApiError> Errors { get; set; }
    }
}
