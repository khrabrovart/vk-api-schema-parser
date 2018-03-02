using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObject
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public ApiObjectType Type { get; set; }
        public string OriginalTypeName { get; set; }
        public bool AdditionalProperties { get; set; }
        public IEnumerable<ApiObjectProperty> Properties { get; set; }
        public IEnumerable<ApiObject> AllOf { get; set; }

        // Actually these properties not declared in the object definition
        // but AllOf arrays always consist of one or more objects with reference.
        public string ReferencePath { get; set; }
        public ApiObject Reference { get; set; }
        
        // Moved directly to ApiObjectProperty class as IsRequired.
        // public IEnumerable<ApiObjectProperty> Required { get; set; }

        // TODO: Fill these properties.
        public string Description { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public IEnumerable<string> EnumNames { get; set; }
        public int? MinProperties { get; set; }
    }
}
