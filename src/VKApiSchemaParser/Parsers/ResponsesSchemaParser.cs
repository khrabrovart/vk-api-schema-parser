using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ResponsesSchemaParser : BaseSchemaParser<ApiResponsesSchema>
    {
        private const string ObjectsReference = "objects.json#/definitions/";

        protected override string SchemaDownloadUrl => SchemaUrl.Responses;

        private ApiObjectsSchema _objects;

        protected override ApiResponsesSchema Parse(JSchema schema)
        {
            _objects = new ObjectsSchemaParser().ParseAsync().Result;

            var definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            return new ApiResponsesSchema
            {
                SchemaVersion = schema.SchemaVersion,
                Title = schema.Title,
                Responses = definitions.Select(d => GetResponse(d.First, d.Path))
            };
        }

        private ApiResponse GetResponse(JToken token, string originalName)
        {
            return new ApiResponse
            {
                Name = originalName.Beautify(),
                OriginalName = originalName,
                Type = ObjectTypeMapper.Map(token.GetString(JsonStringConstants.Type)),
                OriginalTypeName = token.GetString(JsonStringConstants.Type),
                Object = GetResponseObject(token, originalName),
                AdditionalProperties = token.GetBoolean(JsonStringConstants.AdditionalProperties) == true
            };
        }

        private ApiObject GetResponseObject(JToken token, string originalName)
        {
            var objectSchema = token[JsonStringConstants.Properties][JsonStringConstants.Response];
            return objectSchema != null ? ParseObject(objectSchema, originalName) : null;
        }

        private ApiObject ParseObject(JToken token, string originalName = null)
        {
            var objectPropertyItems = token.UseValueOrDefault(JsonStringConstants.Items, ParseObjectProperty);

            var parsedObject = new ApiObject
            {
                Name = originalName?.Beautify(),
                OriginalName = originalName,
                Type = ObjectTypeMapper.Map(token.GetString(JsonStringConstants.Type)),
                AdditionalProperties = token.GetBoolean(JsonStringConstants.AdditionalProperties) == true,
                AllOf = token.UseValueOrDefault(JsonStringConstants.AllOf, t => t?.Select(ao => ParseObject(ao))),
                //ReferencePath = token.GetString(JsonStringConstants.Reference)
            };

            parsedObject.Properties = GetObjectProperties(token, token.GetArray(JsonStringConstants.Required)?.Select(p => p.Beautify()));

            //parsedObject.Reference = ResolveReference(parsedObject.ReferencePath);

            return parsedObject;
        }

        private IEnumerable<ApiObject> GetObjectProperties(JToken token, IEnumerable<string> requiredProperties)
        {
            var a = token.UseValueOrDefault(JsonStringConstants.Properties, t => t.Select(p =>
            {
                var property = ParseObjectProperty(p.First);

                if (requiredProperties != null)
                {
                    property.IsRequired = requiredProperties.Contains(property.Name);
                }

                return property;
            }));

            return a;
        }

        private ApiObject ResolveReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return null;
            }

            if (reference.StartsWith(ObjectsReference))
            {
                var referenceObjectName = reference.Substring(ObjectsReference.Length);
                return _objects.Objects.FirstOrDefault(o => o.OriginalName.Equals(referenceObjectName));
            }

            return null;
        }

        private ApiObject ParseObjectProperty(JToken token)
        {
            if (token == null)
            {
                return null;
            }

            var objectPropertyItems = token.UseValueOrDefault(JsonStringConstants.Items, ParseObjectProperty);
            var name = token.Path.Split('.').Last();

            var parsedObjectProperty = new ApiObject
            {
                Name = name.Beautify(),
                OriginalName = name,
                Description = token.GetString(JsonStringConstants.Description),
                Type = ObjectTypeMapper.Map(token.GetString(JsonStringConstants.Type)),
                Minimum = token.GetInteger(JsonStringConstants.Minimum),
                Enum = token.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify()),
                EnumNames = token.GetArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify()),
                Items = objectPropertyItems,
                //ReferencePath = token.GetString(JsonStringConstants.Reference)
            };

            //parsedObjectProperty.Reference = ResolveReference(parsedObjectProperty.ReferencePath);

            return parsedObjectProperty;
        }
    }
}
