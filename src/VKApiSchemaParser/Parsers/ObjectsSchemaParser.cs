using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ObjectsSchemaParser : BaseSchemaParser<IDictionary<string, ApiObject>>
    {
        private JToken _definitions;
        private readonly IDictionary<string, ApiObject> _apiObjects = new Dictionary<string, ApiObject>();

        protected override string SchemaUrl => SchemaUrls.Objects;

        protected override IDictionary<string, ApiObject> ParseSchema(JSchema schema)
        {
            _definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            foreach (var definition in _definitions)
            {
                if (!_apiObjects.ContainsKey(definition.Path))
                {
                    ParseObject(definition.First, ObjectParserOptions.NamedAndRegistered);
                }
            }

            return _apiObjects;
        }

        protected override ApiObject ResolveReference(string referencePath)
        {
            var referenceName = referencePath.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(referenceName))
            {
                throw new ArgumentException($"Invalid reference \"{referencePath}\"", nameof(referencePath));
            }

            if (!_apiObjects.TryGetValue(referenceName, out var referenceObject))
            {
                var referenceDefinition = _definitions.FirstOrDefault(d => d.Path == referenceName)?.First;

                if (referenceDefinition == null)
                {
                    throw new Exception($"Object \"{referencePath}\" in objects schema not found");
                }

                return ParseObject(referenceDefinition, ObjectParserOptions.NamedAndRegistered);
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

            // All registered objects have names. Objects without names cannot be registered.
            if (options >= ObjectParserOptions.Named)
            {
                var name = token.Path.Split('.').LastOrDefault();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception($"Invalid object's name \"{name}\"");
                }

                obj.Name = name;

                // Registration is only needed for top-level objects.
                if (options == ObjectParserOptions.NamedAndRegistered)
                {
                    _apiObjects.Add(name, obj);
                }
            }

            return obj;
        }
    }
}