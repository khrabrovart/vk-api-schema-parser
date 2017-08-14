using System;
using System.Collections.Generic;
using System.Text;

namespace VKApiSchemaParser.Models
{
    public class ApiObjectProperty
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? Minimum { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public IEnumerable<string> EnumNames { get; set; }
        public ApiObjectProperty Items { get; set; }
        public string Reference { get; set; }
        public bool IsRequired { get; set; }
    }
}
