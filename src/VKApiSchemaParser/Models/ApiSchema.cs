using System.Collections.Generic;

namespace VKApiSchemaParser.Models.Schemas
{
    public class ApiSchema
    {
        public IDictionary<string, ApiObject> Objects { get; set; }

        public IDictionary<string, ApiObject> Responses { get; set; }

        public IDictionary<string, ApiMethod> Methods { get; set; }

        public IDictionary<string, ApiError> Errors { get; set; }
    }
}
