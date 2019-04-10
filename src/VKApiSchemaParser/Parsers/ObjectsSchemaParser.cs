﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
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

            // to linq (where, select, todictionary)
            foreach (var definition in _definitions)
            {
                if (!_apiObjects.ContainsKey(definition.Path))
                {
                    ParseObject(definition.First, ObjectParsingOptions.NamedAndRegistered);
                }
            }

            return _apiObjects;
        }

        protected override ApiObject ResolveReference(string referencePath)
        {
            referencePath = referencePath.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(referencePath))
            {
                throw new Exception($"Invalid reference \"{referencePath}\"");
            }

            // to trygetvalue
            return _apiObjects.ContainsKey(referencePath) ?
                _apiObjects[referencePath] :
                ParseObject(_definitions.FirstOrDefault(d => d.Path == referencePath)?.First, ObjectParsingOptions.NamedAndRegistered);
        }

        protected override ApiObject ParseObject(JToken token, ObjectParsingOptions options)
        {
            if (token == null)
            {
                return null; // Throw exception later.
            }

            var obj = InitializeObject(token, options);

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

                // Registration is only needed for top-level objects.
                if (options == ObjectParsingOptions.NamedAndRegistered)
                {
                    _apiObjects.Add(name, obj);
                }
            }

            return obj;
        }
    }
}