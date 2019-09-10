using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class MethodsSchemaParser : BaseSchemaParser<MethodsSchema>
    {
        private readonly IDictionary<string, ApiObject> _objects;
        private readonly IDictionary<string, ApiObject> _responses;

        public MethodsSchemaParser(IDictionary<string, ApiObject> objects, IDictionary<string, ApiObject> responses)
        {
            _objects = objects;
            _responses = responses;
        }

        protected override string SchemaUrl => SchemaUrls.Methods;

        protected override MethodsSchema ParseSchema(JSchema schema)
        {
            var errorDefinitions = schema.ExtensionData[JsonStringConstants.Errors];
            var methodDefinitions = schema.ExtensionData[JsonStringConstants.Methods];

            var methodsSchema = new MethodsSchema
            {
                Errors = JsonConvert.DeserializeObject<IEnumerable<ApiError>>(errorDefinitions.ToString()).ToDictionary(err => err.Name),
                Methods = methodDefinitions.Select(ParseMethod).ToDictionary(method => method.FullName)
            };

            return methodsSchema;
        }

        protected override ApiObject ResolveReference(string referencePath)
        {
            var referenceSchema = referencePath.Split('.').FirstOrDefault();
            var referenceName = referencePath.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(referenceName) || string.IsNullOrWhiteSpace(referenceSchema))
            {
                throw new ArgumentException($"Invalid reference \"{referencePath}\" in \"{referenceSchema}\" schema");
            }

            var referenceDictionary = referenceSchema == "objects" ? _objects : _responses;

            if (!referenceDictionary.TryGetValue(referenceName, out var referenceObject))
            {
                throw new Exception($"Object \"{referencePath}\" in {referenceSchema} schema not found");
            }

            return referenceObject;
        }

        protected override ApiObject ParseObject(JToken token, ObjectParserOptions options)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var obj = InitializeObject(token, options);
            FillObject(obj, token);
            return obj;
        }

        private ApiObject InitializeObject(JToken token, ObjectParserOptions options)
        {
            var obj = new ApiObject();

            if (options >= ObjectParserOptions.Named)
            {
                var name = token.Path.Split('.').LastOrDefault();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception($"Invalid object's name \"{name}\"");
                }

                obj.Name = name;
            }

            return obj;
        }

        private ApiMethod ParseMethod(JToken token)
        {
            var method = new ApiMethod();

            var name = token.GetPropertyAsString(JsonStringConstants.Name);
            var splittedName = name.Split('.');

            method.FullName = name;
            method.Group = splittedName[0];
            method.Name = splittedName[1];
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

            method.Parameters = token
                .SelectPropertyValueOrDefault(JsonStringConstants.Parameters, t => t.Select(ParseMethodParameter));

            method.Responses = token
                .SelectPropertyValueOrDefault(JsonStringConstants.Responses, t => t.Select(tc => ParseObject(tc.First, ObjectParserOptions.Named)));

            return method;
        }

        private ApiMethodParameter ParseMethodParameter(JToken token)
        {
            var parameter = new ApiMethodParameter();

            var name = token.GetPropertyAsString(JsonStringConstants.Name);

            parameter.Name = name;

            var type = token.GetPropertyAsArray(JsonStringConstants.Type)?.Count() > 1 ?
                JsonStringConstants.Multiple :
                token.GetPropertyAsString(JsonStringConstants.Type);

            parameter.Type = ObjectTypeMapper.Map(type);

            parameter.Description = token.GetPropertyAsString(JsonStringConstants.Description);
            parameter.Enum = token.GetPropertyAsArray(JsonStringConstants.Enum);
            parameter.EnumNames = token.GetPropertyAsArray(JsonStringConstants.EnumNames);
            parameter.Minimum = token.GetPropertyAsInteger(JsonStringConstants.Minimum);
            parameter.Maximum = token.GetPropertyAsInteger(JsonStringConstants.Maximum);
            parameter.Default = token.GetPropertyAsString(JsonStringConstants.Default);
            parameter.MinLength = token.GetPropertyAsInteger(JsonStringConstants.MinLength);
            parameter.MaxLength = token.GetPropertyAsInteger(JsonStringConstants.MaxLength);
            parameter.MaxItems = token.GetPropertyAsInteger(JsonStringConstants.MaxItems);
            parameter.Items = token.SelectPropertyValueOrDefault(JsonStringConstants.Items, ParseNestedObject);
            parameter.IsRequired = token.GetPropertyAsBoolean(JsonStringConstants.Required) == true;

            return parameter;
        }
    }
}