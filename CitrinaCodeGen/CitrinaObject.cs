using System.Collections.Generic;

namespace CitrinaCodeGen
{
    public class CitrinaObject
    {
        public string Name { get; set; }
        public IEnumerable<CitrinaObjectProperty> Properties { get; set; }
        public string ParentType { get; set; }
    }
}
