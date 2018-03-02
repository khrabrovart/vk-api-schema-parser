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
        private Dictionary<string, ApiObject> _apiObjects = new Dictionary<string, ApiObject>();

        protected override ApiObjectsSchema Parse(JSchema schema)
        {
            _definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            // TODO: Try use Parallel
            foreach (var definition in _definitions)
            {
                if (!_apiObjects.ContainsKey(definition.Path))
                {
                    ParseObject(definition);
                }
            }

            return new ApiObjectsSchema
            {
                SchemaVersion = schema.SchemaVersion,
                Title = schema.Title,
                Objects = _apiObjects.Values.OrderBy(obj => obj.Name)
            };
        }

        private ApiObject ResolveReference(string referencePath)
        {
            // Checking for self refeerence, issue https://github.com/VKCOM/vk-api-schema/issues/35
            if (referencePath.StartsWith(SelfReference))
            {
                referencePath = referencePath.Substring(SelfReference.Length);
            }

            return _apiObjects.ContainsKey(referencePath) ? 
                _apiObjects[referencePath] :
                ParseObject(_definitions.First(d => d.Path == referencePath));
        }

        #region Object Parsing

        private ApiObject ParseObject(JToken token)
        {
            // Some magic just for looped references. Creating and pushing new object
            // to apiObjects list in case if this object will suddenly need it inside 
            // itself. Chain of property references can lead to the object itself.
            var newObject = new ApiObject
            {
                Name = token.Path.Beautify(),
                OriginalName = token.Path
            };

            _apiObjects.Add(token.Path, newObject);
            FillObjectWithData(newObject, token.First);

            return newObject;
        }

        private void FillObjectWithData(ApiObject obj, JToken token)
        {
            obj.Type = SharedTypesParser.ParseObjectType(token.GetString(JsonStringConstants.Type));
            obj.OriginalTypeName = token.GetString(JsonStringConstants.Type);
            obj.Description = token.GetString(JsonStringConstants.Description);
            obj.Properties = GetObjectProperties(token, token.GetArray(JsonStringConstants.Required));
            obj.Enum = token.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify());
            obj.EnumNames = token.GetArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify());
            obj.MinProperties = token.GetInteger(JsonStringConstants.MinProperties);
            obj.AdditionalProperties = token.GetBoolean(JsonStringConstants.AdditionalProperties) == true;
            obj.AllOf = token.UseValueOrDefault(JsonStringConstants.AllOf, t => t?.Select(ao =>
            {
                var referencePath = ao.GetString(JsonStringConstants.Reference);

                if (referencePath != null)
                {
                    return ResolveReference(referencePath);
                }

                var newObject = new ApiObject();
                FillObjectWithData(newObject, ao);

                return newObject;
            }));
        }

        #endregion

        #region Property Parsing

        private IEnumerable<ApiObject> GetObjectProperties(JToken token, IEnumerable<string> requiredProperties)
        {
            return token.UseValueOrDefault(JsonStringConstants.Properties, t => t
                .Where(p => p.First != null)
                .Select(p =>
                {
                    var property = ParseObjectProperty(p.First);

                    if (requiredProperties != null)
                    {
                        property.IsRequired = requiredProperties.Contains(property.OriginalName);
                    }

                    return property;
                }));
        }

        private ApiObject ParseObjectProperty(JToken token)
        {
            var referencePath = token.GetString(JsonStringConstants.Reference);

            if (referencePath != null)
            {
                return ResolveReference(referencePath);
            }

            var name = token.Path.Split('.').Last();

            var newObjectProperty = new ApiObject
            {
                Name = name.Beautify(),
                OriginalName = name,

                // Drops on reference
                Description = token.GetString(JsonStringConstants.Description),
                Type = SharedTypesParser.ParseObjectPropertyType(token.GetString(JsonStringConstants.Type)),
                OriginalTypeName = token.GetString(JsonStringConstants.Type),
                Minimum = token.GetInteger(JsonStringConstants.Minimum),
                Enum = token.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify()),
                EnumNames = token.GetArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify()),
                Items = token.UseValueOrDefault(JsonStringConstants.Items, t => ParseObjectProperty(t))
            };

            return newObjectProperty;
        }

        #endregion
    }
}
