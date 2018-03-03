using System;
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

        // TODO: Deal with loop references.
        protected override ApiObjectsSchema Parse(JSchema schema)
        {
            _definitions = schema.ExtensionData[JsonStringConstants.Definitions];

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
            // Checking for self reference, issue https://github.com/VKCOM/vk-api-schema/issues/35
            if (referencePath.StartsWith(SelfReference))
            {
                referencePath = referencePath.Substring(SelfReference.Length);
            }

            return _apiObjects.ContainsKey(referencePath) ?
                _apiObjects[referencePath] :
                ParseObject(_definitions.First(d => d.Path == referencePath));
        }

        private ApiObject ParseObject(JToken token)
        {
            // Some magic just for looped references. Creating and pushing new object
            // to apiObjects list in case if this object will suddenly need it inside 
            // itself. Chain of property references can lead to the object itself.
            //
            // Example: messages_message object has fwd_messages property that contains
            // reference to the messages_message object itself.
            var newObject = new ApiObject
            {
                Name = token.Path.Beautify(),
                OriginalName = token.Path
            };

            _apiObjects.Add(newObject.OriginalName, newObject);
            FillObjectWithData(newObject, token.First);

            return newObject;
        }

        private void FillObjectWithData(ApiObject obj, JToken token)
        {
            var type = token.GetArray(JsonStringConstants.Type)?.Count() > 1 ? 
                JsonStringConstants.Multiple : 
                token.GetString(JsonStringConstants.Type);

            obj.Type = SharedTypesParser.ParseObjectType(type);
            obj.OriginalTypeName = type;

            obj.Description = token.GetString(JsonStringConstants.Description);
            obj.Minimum = token.GetInteger(JsonStringConstants.Minimum);

            var requiredProperties = token.GetArray(JsonStringConstants.Required);
            obj.Properties = token.UseValueOrDefault(JsonStringConstants.Properties, t => t
                .Where(p => p.First != null)
                .Select(p =>
                {
                    var name = p.First.Path.Split('.').Last();
                    var newObject = new ApiObject
                    {
                        Name = name.Beautify(),
                        OriginalName = name
                    };

                    FillObjectWithData(newObject, p.First);

                    if (requiredProperties != null)
                    {
                        newObject.IsRequired = requiredProperties.Contains(newObject.OriginalName);
                    }

                    return newObject;
                }));

            obj.Enum = token.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify());
            obj.EnumNames = token.GetArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify());
            obj.MinProperties = token.GetInteger(JsonStringConstants.MinProperties);
            obj.AdditionalProperties = token.GetBoolean(JsonStringConstants.AdditionalProperties) == true;
            obj.Items = token.UseValueOrDefault(JsonStringConstants.Items, ParseAsNestedObject);
            obj.AllOf = token.UseValueOrDefault(JsonStringConstants.AllOf, t => t?.Select(ParseAsNestedObject));
            obj.OneOf = token.UseValueOrDefault(JsonStringConstants.OneOf, t => t?.Select(ParseAsNestedObject));

            var reference = token.GetString(JsonStringConstants.Reference);
            obj.Reference = string.IsNullOrWhiteSpace(reference) ? null : ResolveReference(reference);
        }

        private ApiObject ParseAsNestedObject(JToken token)
        {
            var referencePath = token.GetString(JsonStringConstants.Reference);

            if (!string.IsNullOrWhiteSpace(referencePath))
            {
                return ResolveReference(referencePath);
            }

            var newObject = new ApiObject();
            FillObjectWithData(newObject, token);
            return newObject;
        }
    }
}