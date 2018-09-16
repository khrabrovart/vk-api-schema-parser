using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ResponsesSchemaParser : BaseSchemaParser<ApiObjectsSchema>
    {
        private static List<string> p = new List<string>();

        private JToken _definitions;
        private IDictionary<string, ApiObject> _apiResponses = new Dictionary<string, ApiObject>();
        private ApiObjectsSchema _objectsSchema;

        public ResponsesSchemaParser(ApiObjectsSchema objectsSchema)
        {
            _objectsSchema = objectsSchema;
        }

        protected override string SchemaDownloadUrl => SchemaUrl.Responses;

        protected override ApiObjectsSchema Parse(JSchema schema)
        {
            p = p.Distinct().OrderBy(o => o).ToList();
            _definitions = schema.ExtensionData[JsonStringConstants.Definitions];

            foreach (var definition in _definitions)
            {
                if (!_apiResponses.ContainsKey(definition.Path))
                {
                    ParseObject(definition.First, ObjectParsingOptions.NamedAndRegistered);
                }
            }

            return new ApiObjectsSchema
            {
                SchemaVersion = schema.SchemaVersion,
                Title = schema.Title,
                Objects = _apiResponses.Values
                    .OrderBy(obj => obj.Name)
                    .ToDictionary(obj => obj.OriginalName, obj => obj)
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
            return _objectsSchema.Objects.ContainsKey(referencePath) ?
                _objectsSchema.Objects[referencePath] : null; // Replace NULL with Exception later
        }

        protected override ApiObject ParseObject(JToken token, ObjectParsingOptions options)
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
                    _apiResponses.Add(name, obj);
                }
            }

            p.AddRange(token.Children().Select(t => t.Path.Split('.').Last()));

            FillType(obj, token);
            FillProperties(obj, token);
            FillReference(obj, token);
            FillOther(obj, token);

            return obj;
        }
    }
}
