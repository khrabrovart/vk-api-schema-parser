using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ResponsesSchemaParser : BaseSchemaParser<IDictionary<string, ApiObject>>
    {
        private readonly IDictionary<string, ApiObject> _objects;

        public ResponsesSchemaParser(IDictionary<string, ApiObject> objects)
        {
            _objects = objects;
        }

        protected override string SchemaUrl => SchemaUrls.Responses;

        protected override IDictionary<string, ApiObject> ParseSchema(JSchema schema)
        {
            var definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            var objectsDictionary = definitions
                .Select(d => ParseObject(d.First, ObjectParsingOptions.NamedAndRegistered))
                .ToDictionary(obj => obj.OriginalName);

            return objectsDictionary;
        }

        protected override ApiObject ResolveReference(string referencePath)
        {
            var referenceName = referencePath.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(referenceName))
            {
                throw new ArgumentException($"Invalid reference \"{referencePath}\"", nameof(referencePath));
            }

            if (!_objects.TryGetValue(referenceName, out var referenceObject))
            {
                throw new Exception($"Object \"{referencePath}\" in objects schema not found");
            }

            return referenceObject;
        }

        protected override ApiObject ParseObject(JToken token, ObjectParsingOptions options)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var obj = InitializeObject(token, options);

            // Replacing actual response object with its 'response' property.
            // Only for top-level objects.
            if (options == ObjectParsingOptions.NamedAndRegistered)
            {
                token = token[JsonStringConstants.Properties]["response"];
            }

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

                obj.Name = name.Beautify();
                obj.OriginalName = name;
            }

            return obj;
        }
    }
}
