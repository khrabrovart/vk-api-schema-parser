using System;
using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiResponsesSchema
    {
        public Uri SchemaVersion { get; set; }
        public string Title { get; set; }
        public IEnumerable<ApiResponse> Responses { get; set; }
    }
}
