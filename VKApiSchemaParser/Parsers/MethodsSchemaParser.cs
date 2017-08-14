using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class MethodsSchemaParser : SchemaParser<ApiMethodsSchema>
    {
        protected override string CurrentSchemaUrl => SchemaUrl.Methods;

        protected override ApiMethodsSchema Parse()
        {
            var errors = RawSchema.ExtensionData[StringConstants.Errors];
            var methods = RawSchema.ExtensionData[StringConstants.Methods];

            return new ApiMethodsSchema
            {
                Errors = GetErrors(errors),
                Methods = GetMethods(methods)
            };
        }

        private IEnumerable<ApiError> GetErrors(JToken token)
        {
            return token.Children().Select(c => new ApiError
            {
                Name = c.GetString(StringConstants.Name),
                Code = c.GetInteger(StringConstants.Code),
                Description = c.GetString(StringConstants.Description)
            });
        }

        private IEnumerable<ApiMethod> GetMethods(JToken token)
        {
            return token.Children().Select(c => new ApiMethod
            {
                Name = c.GetString(StringConstants.Name).FormatAsName(),
                Description = c.GetString(StringConstants.Description),
                AccessTokenTypes = GetAccessTokenTypes(c),
                Parameters = GetMethodParameters(c)
            });
        }

        private IEnumerable<ApiAccessTokenType> GetAccessTokenTypes(JToken token)
        {
            return token.UseValueOrDefault(StringConstants.AccessTokenType, t => t.Children().Select(c =>
            {
                switch (c.ToString())
                {
                    case "user":
                        return ApiAccessTokenType.User;

                    case "open":
                        return ApiAccessTokenType.Open;

                    case "service":
                        return ApiAccessTokenType.Service;

                    case "group":
                        return ApiAccessTokenType.Group;

                    default:
                        return ApiAccessTokenType.Undefined;
                }
            }));
        }

        private ApiMethodParameterItems GetMethodParameterItems(JToken token)
        {
            return token.UseValueOrDefault(StringConstants.Items, t => new ApiMethodParameterItems
            {
                Type = token.GetString(StringConstants.Type),
                Minimum = t.GetInteger(StringConstants.Minimum),
                Enum = t.GetArray(StringConstants.Enum)?.Select(item => item.FormatAsName())
            });
        }

        private IEnumerable<ApiMethodParameter> GetMethodParameters(JToken token)
        {
            return token.UseValueOrDefault(StringConstants.Parameters, t => t.Select(p => new ApiMethodParameter
            {
                Name = p.GetString(StringConstants.Name).FormatAsName(),
                Description = p.GetString(StringConstants.Description),
                Type = p.GetString(StringConstants.Type),
                Minimum = p.GetInteger(StringConstants.Minimum),
                Maximum = p.GetInteger(StringConstants.Maximum),
                Default = p.GetInteger(StringConstants.Default),
                MaxItems = p.GetInteger(StringConstants.MaxItems),
                Required = p.GetBoolean(StringConstants.Required) == true,
                Enum = p.GetArray(StringConstants.Enum)?.Select(item => item.FormatAsName()),
                EnumNames = p.GetArray(StringConstants.EnumNames)?.Select(item => item.FormatAsName()),
                Items = GetMethodParameterItems(p)
            }));
        }
    }
}