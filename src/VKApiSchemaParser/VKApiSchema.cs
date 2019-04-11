using System.Threading.Tasks;
using VKApiSchemaParser.Models.Schemas;
using VKApiSchemaParser.Parsers;

namespace VKApiSchemaParser
{
    public static class VKApiSchema
    {
        public static async Task<ApiSchema> ParseAsync()
        {
            var objects = await new ObjectsSchemaParser().ParseAsync().ConfigureAwait(false);
            var responses = await new ResponsesSchemaParser(objects).ParseAsync().ConfigureAwait(false);
            var methodsSchema = await new MethodsSchemaParser(objects, responses).ParseAsync().ConfigureAwait(false);

            return new ApiSchema
            {
                Objects = objects,
                Responses = responses,
                Methods = methodsSchema.Methods,
                Errors = methodsSchema.Errors
            };
        }
    }
}
