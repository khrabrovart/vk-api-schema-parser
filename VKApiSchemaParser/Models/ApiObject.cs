using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObject
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public ApiObjectType Type { get; set; }
        public string OriginalTypeName { get; set; }
        public IEnumerable<ApiObjectProperty> Properties { get; set; }
        public IEnumerable<ApiObject> AllOf { get; set; }
        public IEnumerable<ApiObject> OneOf { get; set; }
        public string ReferencePath { get; set; }
        public ApiObject Reference { get; set; }
        public bool AdditionalProperties { get; set; }
        public ApiObjectProperty Items { get; set; }
    }
}
