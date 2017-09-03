using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObjectProperty
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public ApiObjectType Type { get; set; }
        public string OriginalTypeName { get; set; }
        public int? Minimum { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public IEnumerable<string> EnumNames { get; set; }
        public ApiObjectProperty Items { get; set; }
        public string ReferencePath { get; set; }
        public ApiObject Reference { get; set; }
        public IEnumerable<ApiObject> AllOf { get; set; }
        public IEnumerable<ApiObject> OneOf { get; set; }
        public bool IsRequired { get; set; }
    }
}
