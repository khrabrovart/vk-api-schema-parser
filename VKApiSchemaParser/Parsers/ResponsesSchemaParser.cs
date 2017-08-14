using System.Linq;
using Newtonsoft.Json.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ResponsesSchemaParser : SchemaParser<ApiResponsesSchema>
    {
        protected override string CurrentSchemaUrl => SchemaUrl.Responses;

        protected override ApiResponsesSchema Parse()
        {
            var definitions = RawSchema.ExtensionData[StringConstants.Definitions];

            return new ApiResponsesSchema
            {
                SchemaVersion = RawSchema.SchemaVersion,
                Title = RawSchema.Title,
                Responses = definitions.Select(d => GetResponse(d.First, d.Path.FormatAsName()))
            };
        }

        private ApiResponse GetResponse(JToken token, string name)
        {
            return new ApiResponse
            {
                Name = name,
                Type = token.GetString(StringConstants.Type),
                Object = GetResponseObject(token, name),
                AdditionalProperties = token.GetBoolean(StringConstants.AdditionalProperties) == true
            };
        }

        private ApiObject GetResponseObject(JToken token, string name)
        {
            var objectSchema = token[StringConstants.Properties][StringConstants.Response];
            return objectSchema != null ? SharedObjectsParser.ParseObject(objectSchema, name) : null;
        }
    }
}
