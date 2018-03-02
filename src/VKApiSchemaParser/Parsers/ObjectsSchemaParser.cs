using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ObjectsSchemaParser : SchemaParser<ApiObjectsSchema>
    {
        private const string SelfReference = "#/definitions/";

        protected override string SchemaDownloadUrl => SchemaUrl.Objects;

        private JToken _definitions;
        private Dictionary<string, ApiObject> _parsed;

        protected override ApiObjectsSchema Parse(JSchema schema)
        {
            _definitions = schema.ExtensionData[JsonStringConstants.Definitions];
            _parsed = new Dictionary<string, ApiObject>();

            foreach (var d in _definitions)
            {
                GetParsedObject(d);
            }

            return new ApiObjectsSchema
            {
                SchemaVersion = schema.SchemaVersion,
                Title = schema.Title,
                Objects = _parsed.Values.OrderBy(obj => obj.OriginalName)
            };
        }

        private ApiObject GetParsedObject(JToken token)
        {
            if (_parsed.ContainsKey(token.Path))
            {
                return _parsed[token.Path];
            }

            var parsedObject = ParseObject(token.First, token.Path);

            if (!_parsed.ContainsKey(token.Path))
            {
                _parsed.Add(token.Path, parsedObject);
            }

            return parsedObject;
        }

        private ApiObject ResolveReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return null;
            }

            if (reference.StartsWith(SelfReference))
            {
                var referenceObjectName = reference.Substring(SelfReference.Length);

                if (_parsed.ContainsKey(referenceObjectName))
                {
                    return _parsed[referenceObjectName];
                }

                var foundObject = _definitions.FirstOrDefault(d => d.Path.Equals(referenceObjectName));

                if (foundObject != null)
                {
                    return GetParsedObject(foundObject);
                }
            }

            return null;
        }

        private bool IsRecursiveReference(string objectOriginalName, string reference)
        {
            if (string.IsNullOrWhiteSpace(reference) || string.IsNullOrWhiteSpace(objectOriginalName))
            {
                return false;
            }

            return reference.StartsWith(SelfReference) && objectOriginalName.Equals(reference.Substring(14));
        }

        private ApiObject ParseObject(JToken token, string originalName = null)
        {
            var parsedObject = new ApiObject
            {
                Name = originalName?.Beautify(),
                OriginalName = originalName,
                Type = SharedTypesParser.ParseObjectType(token.GetString(JsonStringConstants.Type)),
                OriginalTypeName = token.GetString(JsonStringConstants.Type),
                AdditionalProperties = token.GetBoolean(JsonStringConstants.AdditionalProperties) == true,
                AllOf = token.UseValueOrDefault(JsonStringConstants.AllOf, t => t?.Select(ao => ParseObject(ao))),
                ReferencePath = token.GetString(JsonStringConstants.Reference)
            };

            parsedObject.Properties = GetObjectProperties(token, token.GetArray(JsonStringConstants.Required)?.Select(p => p.Beautify()), parsedObject);

            parsedObject.Reference = IsRecursiveReference(parsedObject.OriginalName, parsedObject.ReferencePath) ? 
                parsedObject : 
                ResolveReference(parsedObject.ReferencePath);

            return parsedObject;
        }

        private IEnumerable<ApiObjectProperty> GetObjectProperties(JToken token, IEnumerable<string> requiredProperties, ApiObject ownerObject)
        {
            return token.UseValueOrDefault(JsonStringConstants.Properties, t => t.Select(p =>
            {
                var property = ParseObjectProperty(p.First, ownerObject);

                if (requiredProperties != null)
                {
                    property.IsRequired = requiredProperties.Contains(property.Name);
                }

                return property;
            }));
        }

        private ApiObjectProperty ParseObjectProperty(JToken token, ApiObject ownerObject)
        {
            if (token == null)
            {
                return null;
            }

            var objectPropertyItems = token.UseValueOrDefault(JsonStringConstants.Items, t => ParseObjectProperty(t, ownerObject));
            var name = token.Path.Split('.').Last();

            var parsedObjectProperty = new ApiObjectProperty
            {
                Name = name.Beautify(),
                OriginalName = name,
                Description = token.GetString(JsonStringConstants.Description),
                Type = SharedTypesParser.ParseObjectPropertyType(token.GetString(JsonStringConstants.Type)),
                OriginalTypeName = token.GetString(JsonStringConstants.Type),
                Minimum = token.GetInteger(JsonStringConstants.Minimum),
                Enum = token.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify()),
                EnumNames = token.GetArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify()),
                Items = objectPropertyItems,
                ReferencePath = token.GetString(JsonStringConstants.Reference)
            };

            parsedObjectProperty.Reference = IsRecursiveReference(ownerObject.OriginalName, parsedObjectProperty.ReferencePath) ? 
                ownerObject : 
                ResolveReference(parsedObjectProperty.ReferencePath);

            return parsedObjectProperty;
        }
    }
}
