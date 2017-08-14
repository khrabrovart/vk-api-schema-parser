using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiMethodParameterItems
    {
        public string Type { get; set; }
        public int? Minimum { get; set; }
        public IEnumerable<string> Enum { get; set; }
    }
}
