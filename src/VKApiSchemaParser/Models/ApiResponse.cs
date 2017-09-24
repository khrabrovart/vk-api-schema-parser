namespace VKApiSchemaParser.Models
{
    public class ApiResponse
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public ApiObjectType Type { get; set; }
        public string OriginalTypeName { get; set; }
        public ApiObject Object { get; set; }
        public bool AdditionalProperties { get; set; }
    }
}
