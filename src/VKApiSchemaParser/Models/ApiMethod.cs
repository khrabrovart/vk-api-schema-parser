using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiMethod
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public IEnumerable<ApiAccessTokenType> AccessTokenTypes { get; set; }
        public IEnumerable<ApiObject> Parameters { get; set; }
        public IEnumerable<ApiObject> Responses { get; set; }
        public IEnumerable<ApiError> Errors { get; set; }
    }
}
