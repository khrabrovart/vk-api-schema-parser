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
            _responses = new ResponsesSchemaParser().ParseAsync().Result;

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
                Name = c.GetString(JsonStringConstants.Name),
                Code = c.GetInteger(JsonStringConstants.Code),
                Description = c.GetString(JsonStringConstants.Description)
            });
        }

        private IEnumerable<ApiMethod> GetMethods(JToken token)
        {
            return token?.Children()
                .Select(c => new ApiMethod
                {
                    MethodGroup = GetMethodGroup(c.GetString(JsonStringConstants.Name)),
                    Name = GetMethodName(c.GetString(JsonStringConstants.Name)),
                    OriginalName = c.GetString(JsonStringConstants.Name),
                    Description = c.GetString(JsonStringConstants.Description),
                    AccessTokenTypes = GetAccessTokenTypes(c),
                    Parameters = GetMethodParameters(c),
                    Errors = c.UseValueOrDefault(JsonStringConstants.Errors, GetErrors),
                    Responses = c.UseValueOrDefault(JsonStringConstants.Responses, GetResponses)
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
                ReferencePath = r.First.GetString(JsonStringConstants.Reference),
                Reference = ResolveReference(r.First.GetString(JsonStringConstants.Reference))
            });
        }

        private IEnumerable<ApiAccessTokenType> GetAccessTokenTypes(JToken token)
        {
            return token.UseValueOrDefault(JsonStringConstants.AccessTokenType, t => t.Children().Select(c =>
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
            return token.UseValueOrDefault(JsonStringConstants.Items, t => new ApiMethodParameterItems
            {
                Type = ObjectTypeMapper.Map(t.GetString(JsonStringConstants.Type)),
                Minimum = t.GetInteger(JsonStringConstants.Minimum),
                Enum = t.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify())
            });
        }

        private IEnumerable<ApiMethodParameter> GetMethodParameters(JToken token)
        {
            return token.UseValueOrDefault(JsonStringConstants.Parameters, t => t?.Select(p => new ApiMethodParameter
            {
                Name = p.GetString(JsonStringConstants.Name).Beautify(),
                OriginalName = p.GetString(JsonStringConstants.Name),
                Description = p.GetString(JsonStringConstants.Description),
                Type = ObjectTypeMapper.Map(p.GetString(JsonStringConstants.Type)),
                Minimum = p.GetInteger(JsonStringConstants.Minimum),
                Maximum = p.GetInteger(JsonStringConstants.Maximum),
                Default = p.GetInteger(JsonStringConstants.Default),
                MaxItems = p.GetInteger(JsonStringConstants.MaxItems),
                Required = p.GetBoolean(JsonStringConstants.Required) == true,
                Enum = p.GetArray(JsonStringConstants.Enum)?.Select(item => item.Beautify()),
                EnumNames = p.GetArray(JsonStringConstants.EnumNames)?.Select(item => item.Beautify()),
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
                return _responses.Objects.FirstOrDefault(o => o.OriginalName.Equals(referenceObjectName));
            }

            return null;
        }
    }
}