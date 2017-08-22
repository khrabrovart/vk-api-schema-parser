namespace VKApiSchemaParser.Models
{
    public class ApiResponse
    {
        public string Name { get; set; }
        public ApiObjectType Type { get; set; }
        public ApiObject Object { get; set; }
        public bool AdditionalProperties { get; set; }
    }
}
