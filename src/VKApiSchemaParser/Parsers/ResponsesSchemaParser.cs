﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return schema
                .ExtensionData[JsonStringConstants.Definitions]
                .Select(d => ParseObject(d.First, ObjectParserOptions.NamedAndRegistered))
                .ToDictionary(obj => obj.Name);
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

        protected override ApiObject ParseObject(JToken token, ObjectParserOptions options)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var obj = InitializeObject(token, options);

            // Replacing actual response object with its 'response' property.
            // Only for top-level objects.
            if (options == ObjectParserOptions.NamedAndRegistered)
            {
                token = token[JsonStringConstants.Properties]["response"];
            }

            FillObject(obj, token);
            return obj;
        }

        private ApiObject InitializeObject(JToken token, ObjectParserOptions options)
        {
            var obj = new ApiObject();

            // All registered objects have names. Objects without names cannot be registered.
            if (options >= ObjectParserOptions.Named)
            {
                // TODO: Really shitty behaviour caused by "patternProperties".
                // Token path contains "['...']" instead of "." in case if token name is not a regular string.
                // Need refactor someday!
                var name = token.Path.Contains("['") 
                    ? GetTokenNameFromAbnormalPath(token.Path)
                    : token.Path.Split('.').LastOrDefault();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception($"Invalid name \"{name}\"");
                }

                obj.Name = name;
            }

            return obj;
        }

        private string GetTokenNameFromAbnormalPath(string path)
        {
            var firstApostropheIndex = path.IndexOf('\'');
            var lastApostropheIndex = path.LastIndexOf('\'');

            return path.Substring(firstApostropheIndex + 1, lastApostropheIndex - (firstApostropheIndex + 1));
        }
    }
}
