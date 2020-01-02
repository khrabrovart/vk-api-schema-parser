using System.Threading.Tasks;
using VKApiSchemaParser.Models.Schemas;
using VKApiSchemaParser.Parsers;

namespace VKApiSchemaParser
{
    /// <summary>
    /// Represents API schema parser entry point.
    /// </summary>
    public static class VKApiSchema
    {
        /// <summary>
        /// Parses API schema and returns all its content.
        /// </summary>
        /// <returns>API schema content.</returns>
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
