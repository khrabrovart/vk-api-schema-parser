using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObjectProperty
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public ApiObjectPropertyType Type { get; set; }
        public string OriginalTypeName { get; set; }
        public int? Minimum { get; set; }
        public string ReferencePath { get; set; }
        public ApiObject Reference { get; set; }
        public ApiObjectProperty Items { get; set; }     

        // Sometimes it's Int, sometimes it's String.
        public IEnumerable<string> Enum { get; set; }
        public IEnumerable<string> EnumNames { get; set; }
        
        // Actually this property not declared in the object property definition
        // but is replaces object's Required property.
        public bool IsRequired { get; set; }

        
           
    }
}
