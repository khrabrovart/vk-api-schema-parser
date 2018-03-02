using System.Threading.Tasks;
using VKApiSchemaParser.Models;
using VKApiSchemaParser.Parsers;

namespace VKApiSchemaParser
{
    public class VKApiSchema
    {
        private readonly ObjectsSchemaParser _objectsSchemaParser;
        private readonly ResponsesSchemaParser _responsesSchemaParser;
        private readonly MethodsSchemaParser _methodsSchemaParser;

        public VKApiSchema()
        {
            _objectsSchemaParser = new ObjectsSchemaParser();
            _responsesSchemaParser = new ResponsesSchemaParser();
            _methodsSchemaParser = new MethodsSchemaParser();
        }

        public Task<ApiObjectsSchema> GetObjectsAsync()
        {
            return _objectsSchemaParser.ParseAsync();
        }

        public Task<ApiResponsesSchema> GetResponsesAsync()
        {
            return _responsesSchemaParser.ParseAsync();
        }

        public Task<ApiMethodsSchema> GetMethodsAsync()
        {
            return _methodsSchemaParser.ParseAsync();
        }
    }
}
