using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;
using VKApiSchemaParser.Models.Schemas;

namespace VKApiSchemaParser.Parsers
{
    internal class MethodsSchemaParser : BaseSchemaParser<ApiMethodsSchema>
    {
        private IDictionary<string, ApiObject> _apiMethods = new Dictionary<string, ApiObject>();
        private ApiResponsesSchema _responsesSchema;

        public MethodsSchemaParser(ApiResponsesSchema responsesSchema)
        {
            _responsesSchema = responsesSchema;
        }

        protected override string SchemaDownloadUrl => SchemaUrl.Methods;

        protected override ApiMethodsSchema Parse(JSchema schema)
        {
            var errorDefinitions = schema.ExtensionData[JsonStringConstants.Errors];
            var methodDefinitions = schema.ExtensionData[JsonStringConstants.Methods];

            foreach (var methodDefinition in methodDefinitions)
            {
                ParseMethod(methodDefinition);
            }

            return new ApiMethodsSchema
            {
                Errors = JsonConvert.DeserializeObject<IEnumerable<ApiError>>(errorDefinitions.ToString()),
                Methods = null
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
            return _responsesSchema.Responses.ContainsKey(referencePath) ?
                _responsesSchema.Responses[referencePath] : null; // Replace NULL with Exception later
        }

        protected override ApiObject ParseObject(JToken token, ObjectParsingOptions options)
        {
            if (token == null)
            {
                return null; // Throw exception later.
            }

            var obj = InitializeObject(token, options);

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

                // Registration is only needed for top-level objects.
                if (options == ObjectParsingOptions.NamedAndRegistered)
                {
                    _apiMethods.Add(name, obj);
                }
            }

            return obj;
        }

        private ApiMethod ParseMethod(JToken token)
        {
            var method = new ApiMethod();

            var name = token.GetPropertyAsString(JsonStringConstants.Name);
            var splittedName = name.Split('.');

            method.OriginalName = name;
            method.Group = splittedName[0];
            method.Name = splittedName[1];

            var accessTokenTypesString = token.GetPropertyAsString(JsonStringConstants.AccessTokenType);
            method.AccessTokenTypes = JsonConvert.DeserializeObject<IEnumerable<ApiAccessTokenType>>(accessTokenTypesString);

            var parametersString = token.GetPropertyAsString(JsonStringConstants.Parameters);
            method.Parameters = JsonConvert.DeserializeObject<IEnumerable<ApiMethodParameter>>(parametersString);

            return method;
        }
    }
}