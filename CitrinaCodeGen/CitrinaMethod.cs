using System.Collections.Generic;

namespace CitrinaCodeGen
{
    public class CitrinaMethod
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string ReturnType { get; set; }
        public string InlineParameters { get; set; }
        public IEnumerable<string> MappingParameters { get; set; }
        public string Description { get; set; }
        public bool NeedAccessToken { get; set; }
    }
}
