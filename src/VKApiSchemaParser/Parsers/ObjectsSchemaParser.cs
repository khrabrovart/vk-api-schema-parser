﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ObjectsSchemaParser : BaseSchemaParser<ApiObjectsSchema>
    {
        private const string SelfReference = "#/definitions/";

        protected override string SchemaDownloadUrl => SchemaUrl.Objects;

        private JToken _definitions;
        private Dictionary<string, ApiObject> _apiObjects = new Dictionary<string, ApiObject>();

        protected override ApiObjectsSchema Parse(JSchema schema)
        {
            _definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            foreach (var definition in _definitions)
            {
                if (!_apiObjects.ContainsKey(definition.Path))
                {
                    ParseObject(definition.First, true);
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
                ParseObject(_definitions.First(d => d.Path == referencePath).First, true);
        }

        private ApiObject ParseNestedObject(JToken token)
        {
            var referencePath = token.GetString(JsonStringConstants.Reference);

            if (!string.IsNullOrWhiteSpace(referencePath))
            {
                return ResolveReference(referencePath);
            }

            return ParseObject(token, false);
        }

        private ApiObject ParseObject(JToken token, bool needRegistration)
        {
            // Name
            var name = token.Path.Split('.').Last();
            var obj = new ApiObject
            {
                Name = name?.Beautify(),
                OriginalName = name
            };

            /*
             * Registered objects are stored in the _apiObjects dictionary.
             * This is the main list of parsed objects and registration is 
             * only needed for top-level objects.
             */
            if (needRegistration)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException("Invalid name.", nameof(name));
                }

                _apiObjects.Add(name, obj);
            }

            // Type
            var type = token.GetArray(JsonStringConstants.Type)?.Count() > 1 ?
                JsonStringConstants.Multiple :
                token.GetString(JsonStringConstants.Type);
            obj.Type = ObjectTypeMapper.Map(type);
            obj.OriginalTypeName = type;

            // Properties
            var requiredProperties = token.GetArray(JsonStringConstants.Required);
            obj.Properties = token.UseValueOrDefault(JsonStringConstants.Properties, t => t
                .Where(p => p.First != null)
                .Select(p =>
                {
                    var newObject = ParseObject(p.First, false);

                    if (requiredProperties != null)
                    {
                        newObject.IsRequired = requiredProperties.Contains(newObject.OriginalName);
                    }

                    return newObject;
                }));

            // Reference
            var reference = token.GetString(JsonStringConstants.Reference);
            obj.Reference = string.IsNullOrWhiteSpace(reference) ? null : ResolveReference(reference);

            // Other
            obj.Description = token.GetString(JsonStringConstants.Description);
            obj.Minimum = token.GetInteger(JsonStringConstants.Minimum);
            obj.Enum = token.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify());
            obj.EnumNames = token.GetArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify());
            obj.MinProperties = token.GetInteger(JsonStringConstants.MinProperties);
            obj.AdditionalProperties = token.GetBoolean(JsonStringConstants.AdditionalProperties) == true;
            obj.Items = token.UseValueOrDefault(JsonStringConstants.Items, ParseNestedObject);
            obj.AllOf = token.UseValueOrDefault(JsonStringConstants.AllOf, t => t?.Select(ParseNestedObject));
            obj.OneOf = token.UseValueOrDefault(JsonStringConstants.OneOf, t => t?.Select(ParseNestedObject));

            return obj;
        }
    }
}