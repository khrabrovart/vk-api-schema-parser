using Newtonsoft.Json.Linq;

namespace VKApiSchemaParser.Parsers
{
    internal class ResponsesSchemaParser : ObjectsSchemaParser
    {
        // TODO: Create reference objects source, load object first or parse it on the go
        protected override string SchemaDownloadUrl => SchemaUrl.Responses;

        protected override JToken SelectObject(JToken definition)
        {
            return definition.First[JsonStringConstants.Properties][JsonStringConstants.Response];
        }
    }
}
