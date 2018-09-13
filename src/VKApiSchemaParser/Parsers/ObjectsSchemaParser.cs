using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    // TODO: Сделать абстрактный или базовый класс BaseObjectsParser, который будет содержать в себе методы, общие для ObjectsParser и ResponsesParser.
    // Остальное сделать абстрактными и имплементить здесь.
    internal class ObjectsSchemaParser : BaseSchemaParser<ApiObjectsSchema>
    {
        protected override string SchemaDownloadUrl => SchemaUrl.Objects;

        private JToken _definitions;
        private Dictionary<string, ApiObject> _apiObjects = new Dictionary<string, ApiObject>();

        protected override ApiObjectsSchema Parse(JSchema schema)
        {
            var _definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            foreach (var definition in _definitions)
            {
                if (!_apiObjects.ContainsKey(definition.Path))
                {
                    ParseObject(definition.First, ObjectParsingOptions.NamedAndRegistered);
                }
            }

            return new ApiObjectsSchema
            {
                SchemaVersion = schema.SchemaVersion,
                Title = schema.Title,
                Objects = _apiObjects.Values.OrderBy(obj => obj.Name)
            };
        }

        public ApiObject ParseObject(JToken token, ObjectParsingOptions options)
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

                /*
                 * Registered objects are stored in the _apiObjects dictionary.
                 * This is the main list of parsed objects and registration is 
                 * only needed for top-level objects.
                 */
                if (options == ObjectParsingOptions.NamedAndRegistered)
                {
                    _apiObjects.Add(name, obj);
                }
            }

            FillType(obj, token);
            FillProperties(obj, token);
            FillReference(obj, token);
            FillOther(obj, token);

            return obj;
        }

        private ApiObject ResolveReference(string referencePath)
        {
            referencePath = referencePath.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(referencePath))
            {
                throw new Exception($"Invalid reference \"{referencePath}\"");
            }

            return _apiObjects.ContainsKey(referencePath) ?
                _apiObjects[referencePath] :
                ParseObject(_definitions.First(d => d.Path == referencePath).First, ObjectParsingOptions.NamedAndRegistered);
        }

        private ApiObject ParseNestedObject(JToken token)
        {
            var referencePath = token.GetPropertyAsString(JsonStringConstants.Reference);

            if (!string.IsNullOrWhiteSpace(referencePath))
            {
                return ResolveReference(referencePath);
            }

            return ParseObject(token, ObjectParsingOptions.Unnamed);
        }

        private void FillType(ApiObject obj, JToken token)
        {
            var type = token.GetPropertyAsArray(JsonStringConstants.Type)?.Count() > 1 ?
                JsonStringConstants.Multiple :
                token.GetPropertyAsString(JsonStringConstants.Type);
            obj.Type = ObjectTypeMapper.Map(type);
            obj.OriginalTypeName = type;
        }

        private void FillProperties(ApiObject obj, JToken token)
        {
            var requiredProperties = token.GetPropertyAsArray(JsonStringConstants.Required);
            obj.Properties = token.SelectPropertyOrDefault(JsonStringConstants.Properties, t => t
                .Where(p => p.First != null)
                .Select(p =>
                {
                    var newObject = ParseObject(p.First, ObjectParsingOptions.Named);

                    if (requiredProperties != null)
                    {
                        newObject.IsRequired = requiredProperties.Contains(newObject.OriginalName);
                    }

                    return newObject;
                }));
        }

        private void FillReference(ApiObject obj, JToken token)
        {
            var reference = token.GetPropertyAsString(JsonStringConstants.Reference);
            obj.Reference = string.IsNullOrWhiteSpace(reference) ? null : ResolveReference(reference);
        }

        private void FillOther(ApiObject obj, JToken token)
        {
            obj.Description = token.GetPropertyAsString(JsonStringConstants.Description);
            obj.Minimum = token.GetPropertyAsInteger(JsonStringConstants.Minimum);
            obj.Enum = token.GetPropertyAsArray(JsonStringConstants.Enum);
            obj.EnumNames = token.GetPropertyAsArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify());
            obj.MinProperties = token.GetPropertyAsInteger(JsonStringConstants.MinProperties);
            obj.AdditionalProperties = token.GetPropertyAsBoolean(JsonStringConstants.AdditionalProperties) == true;
            obj.Items = token.SelectPropertyOrDefault(JsonStringConstants.Items, ParseNestedObject);
            obj.AllOf = token.SelectPropertyOrDefault(JsonStringConstants.AllOf, t => t.Select(ParseNestedObject));
            obj.OneOf = token.SelectPropertyOrDefault(JsonStringConstants.OneOf, t => t.Select(ParseNestedObject));
        }
    }
}