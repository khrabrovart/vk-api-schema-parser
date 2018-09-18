using System;
using System.Collections.Generic;

namespace VKApiSchemaParser.Models.Schemas
{
    public class ApiResponsesSchema : IApiSchema
    {
        public Uri SchemaVersion { get; set; }
        public string Title { get; set; }
        public IDictionary<string, ApiObject> Responses { get; set; }
    }
}
