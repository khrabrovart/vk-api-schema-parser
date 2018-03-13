namespace VKApiSchemaParser.Models
{
    public class ApiMethodResponse
    {
        public string Name { get; set; }
        public string ReferencePath { get; set; }
        public ApiObject Reference { get; set; }
    }
}
