using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObject
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public ApiObjectType Type { get; set; }
        public string OriginalTypeName { get; set; }
        public string Description { get; set; }

        // Object
        public IEnumerable<ApiObject> Properties { get; set; }
        public IEnumerable<ApiObject> AllOf { get; set; }
        public IEnumerable<ApiObject> OneOf { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public IEnumerable<string> EnumNames { get; set; }
        public int? MinProperties { get; set; }
        public bool AdditionalProperties { get; set; }

        // Property
        public int? Minimum { get; set; }
        public ApiObject Items { get; set; }     
        public bool IsRequired { get; set; }
    }
}
