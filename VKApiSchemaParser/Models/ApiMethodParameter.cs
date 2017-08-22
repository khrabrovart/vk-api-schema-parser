using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiMethodParameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ApiObjectType Type { get; set; }
        public int? Minimum { get; set; }
        public int? Default { get; set; }
        public int? Maximum { get; set; }
        public int? MaxItems { get; set; }
        public bool Required { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public IEnumerable<string> EnumNames { get; set; }
        public ApiMethodParameterItems Items { get; set; }
    }
}
