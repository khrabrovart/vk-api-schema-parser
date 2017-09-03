namespace VKApiSchemaParser.Models
{
    public class ApiMethodResponse
    {
        public string Name { get; set; }
        public string ReferencePath { get; set; }
        public ApiResponse Reference { get; set; }
    }
}
