using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<ApiObjectProperty> Properties { get; set; }
        public IEnumerable<ApiObject> AllOf { get; set; }
        public IEnumerable<ApiObject> OneOf { get; set; }
        public string Reference { get; set; }
        public bool AdditionalProperties { get; set; }
    }
}
