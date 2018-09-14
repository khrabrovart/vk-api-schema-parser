using System;
using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObjectsSchema
    {
        public Uri SchemaVersion { get; set; }
        public string Title { get; set; }
        public IDictionary<string, ApiObject> Objects { get; set; }
    }
}
