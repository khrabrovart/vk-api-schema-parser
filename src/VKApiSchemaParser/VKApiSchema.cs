using System.Threading.Tasks;
using VKApiSchemaParser.Models;
using VKApiSchemaParser.Parsers;

namespace VKApiSchemaParser
{
    public class VKApiSchema
    {
        public Task<ApiObjectsSchema> GetObjectsAsync()
        {
            return new ObjectsSchemaParser().ParseAsync();
        }

        public async Task<ApiObjectsSchema> GetResponsesAsync()
        {
            var objectsSchema = await GetObjectsAsync().ConfigureAwait(false);
            return await new ResponsesSchemaParser(objectsSchema).ParseAsync().ConfigureAwait(false);
        }
    }
}
