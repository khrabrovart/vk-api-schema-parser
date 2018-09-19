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

            return new ApiMethodsSchema
            {
                Errors = JsonConvert.DeserializeObject<IEnumerable<ApiError>>(errorDefinitions.ToString()),
                Methods = methodDefinitions.Select(ParseMethod)
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
            method.Group = splittedName[0].Beautify();
            method.Name = splittedName[1].Beautify();
            method.Description = token.GetPropertyAsString(JsonStringConstants.Description);

            var accessTokenTypesString = token.GetPropertyAsString(JsonStringConstants.AccessTokenType);
            if (!string.IsNullOrWhiteSpace(accessTokenTypesString))
            {
                method.AccessTokenTypes = JsonConvert.DeserializeObject<IEnumerable<ApiAccessTokenType>>(accessTokenTypesString);
            }

            var errorsString = token.GetPropertyAsString(JsonStringConstants.Errors);
            if (!string.IsNullOrWhiteSpace(errorsString))
            {
                method.Errors = JsonConvert.DeserializeObject<IEnumerable<ApiError>>(errorsString);
            }

            method.Parameters = token.SelectPropertyOrDefault(JsonStringConstants.Parameters, t => t.Select(ParseMethodParameter));
            method.Responses = token.SelectPropertyOrDefault(JsonStringConstants.Responses, t => t.Select(tc => ParseObject(tc.First, ObjectParsingOptions.Named)));

            return method;
        }

        private ApiMethodParameter ParseMethodParameter(JToken token)
        {
            var parameter = new ApiMethodParameter();

            var name = token.GetPropertyAsString(JsonStringConstants.Name);
            parameter.Name = name.Beautify();
            parameter.OriginalName = name;

            var type = token.GetPropertyAsArray(JsonStringConstants.Type)?.Count() > 1 ?
                JsonStringConstants.Multiple :
                token.GetPropertyAsString(JsonStringConstants.Type);
            parameter.Type = ObjectTypeMapper.Map(type);
            parameter.OriginalTypeName = type;

            parameter.Description = token.GetPropertyAsString(JsonStringConstants.Description);
            parameter.Enum = token.GetPropertyAsArray(JsonStringConstants.Enum);
            parameter.EnumNames = token.GetPropertyAsArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify());
            parameter.Minimum = token.GetPropertyAsInteger(JsonStringConstants.Minimum);
            parameter.Maximum = token.GetPropertyAsInteger(JsonStringConstants.Maximum);
            parameter.Default = token.GetPropertyAsString(JsonStringConstants.Default);
            parameter.MinLength = token.GetPropertyAsInteger(JsonStringConstants.MinLength);
            parameter.MaxLength = token.GetPropertyAsInteger(JsonStringConstants.MaxLength);
            parameter.MaxItems = token.GetPropertyAsInteger(JsonStringConstants.MaxItems);
            parameter.Items = token.SelectPropertyOrDefault(JsonStringConstants.Items, ParseNestedObject);
            parameter.IsRequired = token.GetPropertyAsBoolean(JsonStringConstants.Required) == true;

            return parameter;
        }
    }
}