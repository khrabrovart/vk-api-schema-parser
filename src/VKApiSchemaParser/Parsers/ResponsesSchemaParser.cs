using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;
using VKApiSchemaParser.Models.Schemas;

namespace VKApiSchemaParser.Parsers
{
    internal class ResponsesSchemaParser : BaseSchemaParser<ApiResponsesSchema>
    {
        private ApiObjectsSchema _objectsSchema;

        public ResponsesSchemaParser(ApiObjectsSchema objectsSchema)
        {
            _objectsSchema = objectsSchema;
        }

        protected override string SchemaUrl => SchemaUrls.Responses;

        protected override ApiResponsesSchema Parse(JSchema schema)
        {
            var definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            return new ApiResponsesSchema
            {
                SchemaVersion = schema.SchemaVersion,
                Title = schema.Title,
                Responses = definitions
                    .Select(d => ParseObject(d.First, ObjectParsingOptions.NamedAndRegistered))
                    .OrderBy(obj => obj.Name)
                    .ToDictionary(obj => obj.OriginalName, obj => obj)
            };
        }

        protected override ApiObject ResolveReference(string referencePath)
        {
            referencePath = referencePath.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(referencePath))
            {
                throw new Exception($"Invalid reference \"{referencePath}\"");
            }

            // Object database_street is missing, issue https://github.com/VKCOM/vk-api-schema/issues/44
            return _objectsSchema.Objects.ContainsKey(referencePath) ?
                _objectsSchema.Objects[referencePath] : null; // Replace NULL with Exception later
        }

        protected override ApiObject ParseObject(JToken token, ObjectParsingOptions options)
        {
            if (token == null)
            {
                return null; // Throw exception later.
            }

            var obj = InitializeObject(token, options);

            // Replacing actual response object with its 'response' property.
            // Only for top-level objects.
            if (options == ObjectParsingOptions.NamedAndRegistered)
            {
                token = token[JsonStringConstants.Properties]["response"];
            }

            FillType(obj, token);
            FillProperties(obj, token);
            FillReference(obj, token);
            FillOther(obj, token);

            return obj;
        }

        private ApiObject InitializeObject(JToken token, ObjectParsingOptions options)
        {
            var obj = new ApiObject();

            // All registered objects have names. Objects without names cannot be registered.
            if (options >= ObjectParsingOptions.Named)
            {
                var name = token.Path.Split('.').LastOrDefault();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception($"Invalid name \"{name}\"");
                }

                obj.Name = name?.Beautify();
                obj.OriginalName = name;
            }

            return obj;
        }
    }
}
