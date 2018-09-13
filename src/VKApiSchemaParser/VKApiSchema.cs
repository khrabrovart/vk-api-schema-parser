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

        public Task<ApiObjectsSchema> GetResponsesAsync()
        {
            return new ResponsesSchemaParser().ParseAsync();
        }
    }
}
