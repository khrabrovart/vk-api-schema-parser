using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class MethodsSchemaParser : BaseSchemaParser<ApiMethodsSchema>
    {
        private const string ResponsesReference = "responses.json#/definitions/";

        protected override string SchemaDownloadUrl => SchemaUrl.Methods;

        private ApiObjectsSchema _responses;

        protected override ApiMethodsSchema Parse(JSchema schema)
        {
            _responses = new ResponsesSchemaParser(null).ParseAsync().Result;

            var errors = schema.ExtensionData[JsonStringConstants.Errors];
            var methods = schema.ExtensionData[JsonStringConstants.Methods];

            return new ApiMethodsSchema
            {
                Errors = GetErrors(errors),
                Methods = GetMethods(methods)
            };
        }

        private IEnumerable<ApiError> GetErrors(JToken token)
        {
            return token?.Children().Select(c => new ApiError
            {
                Name = c.GetPropertyAsString(JsonStringConstants.Name),
                Code = c.GetPropertyAsInteger(JsonStringConstants.Code),
                Description = c.GetPropertyAsString(JsonStringConstants.Description)
            });
        }

        private IEnumerable<ApiMethod> GetMethods(JToken token)
        {
            return token?.Children()
                .Select(c => new ApiMethod
                {
                    MethodGroup = GetMethodGroup(c.GetPropertyAsString(JsonStringConstants.Name)),
                    Name = GetMethodName(c.GetPropertyAsString(JsonStringConstants.Name)),
                    OriginalName = c.GetPropertyAsString(JsonStringConstants.Name),
                    Description = c.GetPropertyAsString(JsonStringConstants.Description),
                    AccessTokenTypes = GetAccessTokenTypes(c),
                    Parameters = GetMethodParameters(c),
                    Errors = c.SelectPropertyOrDefault(JsonStringConstants.Errors, GetErrors),
                    Responses = c.SelectPropertyOrDefault(JsonStringConstants.Responses, GetResponses)
                });
        }

        private string GetMethodGroup(string methodName)
        {
            return methodName.Split('.')[0].Beautify();
        }

        private string GetMethodName(string methodName)
        {
            return methodName.Split('.')[1].Beautify();
        }

        private IEnumerable<ApiMethodResponse> GetResponses(JToken token)
        {
            return token.Children().Select(r => new ApiMethodResponse
            {
                Name = r.Path.Split('.').Last().Beautify(),
                ReferencePath = r.First.GetPropertyAsString(JsonStringConstants.Reference),
                Reference = ResolveReference(r.First.GetPropertyAsString(JsonStringConstants.Reference))
            });
        }

        private IEnumerable<ApiAccessTokenType> GetAccessTokenTypes(JToken token)
        {
            return token.SelectPropertyOrDefault(JsonStringConstants.AccessTokenType, t => t.Children().Select(c =>
            {
                switch (c.ToString())
                {
                    case JsonStringConstants.User:
                        return ApiAccessTokenType.User;

                    case JsonStringConstants.Open:
                        return ApiAccessTokenType.Open;

                    case JsonStringConstants.Service:
                        return ApiAccessTokenType.Service;

                    case JsonStringConstants.Group:
                        return ApiAccessTokenType.Group;

                    default:
                        return ApiAccessTokenType.Undefined;
                }
            }));
        }

        private ApiMethodParameterItems GetMethodParameterItems(JToken token)
        {
            return token.SelectPropertyOrDefault(JsonStringConstants.Items, t => new ApiMethodParameterItems
            {
                Type = ObjectTypeMapper.Map(t.GetPropertyAsString(JsonStringConstants.Type)),
                Minimum = t.GetPropertyAsInteger(JsonStringConstants.Minimum),
                Enum = t.GetPropertyAsArray(JsonStringConstants.Enum)?.Select(item => item.Beautify())
            });
        }

        private IEnumerable<ApiMethodParameter> GetMethodParameters(JToken token)
        {
            return token.SelectPropertyOrDefault(JsonStringConstants.Parameters, t => t?.Select(p => new ApiMethodParameter
            {
                Name = p.GetPropertyAsString(JsonStringConstants.Name).Beautify(),
                OriginalName = p.GetPropertyAsString(JsonStringConstants.Name),
                Description = p.GetPropertyAsString(JsonStringConstants.Description),
                Type = ObjectTypeMapper.Map(p.GetPropertyAsString(JsonStringConstants.Type)),
                Minimum = p.GetPropertyAsInteger(JsonStringConstants.Minimum),
                Maximum = p.GetPropertyAsInteger(JsonStringConstants.Maximum),
                Default = p.GetPropertyAsInteger(JsonStringConstants.Default),
                MaxItems = p.GetPropertyAsInteger(JsonStringConstants.MaxItems),
                Required = p.GetPropertyAsBoolean(JsonStringConstants.Required) == true,
                Enum = p.GetPropertyAsArray(JsonStringConstants.Enum)?.Select(item => item.Beautify()),
                EnumNames = p.GetPropertyAsArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify()),
                Items = GetMethodParameterItems(p)
            }));
        }

        private ApiObject ResolveReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return null;
            }

            if (reference.StartsWith(ResponsesReference))
            {
                var referenceObjectName = reference.Substring(ResponsesReference.Length);
                return _responses.Objects[referenceObjectName];
            }

            return null;
        }
    }
}