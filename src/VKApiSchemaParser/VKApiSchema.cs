using System.Threading.Tasks;
using VKApiSchemaParser.Models.Schemas;
using VKApiSchemaParser.Parsers;

namespace VKApiSchemaParser
{
    public class VKApiSchema
    {
        public Task<ApiObjectsSchema> GetObjectsAsync()
        {
            return new ObjectsSchemaParser().ParseAsync();
        }

        public async Task<ApiResponsesSchema> GetResponsesAsync()
        {
            var objectsSchema = await GetObjectsAsync().ConfigureAwait(false);
            return await new ResponsesSchemaParser(objectsSchema).ParseAsync().ConfigureAwait(false);
        }

        public async Task<ApiMethodsSchema> GetMethodsAsync()
        {
            var responsesSchema = await GetResponsesAsync().ConfigureAwait(false);
            return await new MethodsSchemaParser(responsesSchema).ParseAsync().ConfigureAwait(false);
        }
    }
}
