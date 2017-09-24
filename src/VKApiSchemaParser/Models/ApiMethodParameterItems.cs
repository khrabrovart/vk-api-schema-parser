using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiMethodParameterItems
    {
        public ApiObjectType Type { get; set; }
        public int? Minimum { get; set; }
        public IEnumerable<string> Enum { get; set; }
    }
}
