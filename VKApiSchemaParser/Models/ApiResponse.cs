namespace VKApiSchemaParser.Models
{
    public class ApiResponse
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ApiObject Object { get; set; }
        public bool AdditionalProperties { get; set; }
    }
}
