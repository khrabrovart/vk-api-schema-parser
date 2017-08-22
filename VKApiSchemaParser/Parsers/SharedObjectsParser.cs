using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal static class SharedObjectsParser
    {
        public static ApiObject ParseObject(JToken token, string originalName = null)
        {
            return new ApiObject
            {
                Name = originalName.FormatAsName(),
                OriginalName = originalName,
                Type = SharedTypesParser.ParseType(token.GetString(StringConstants.Type)),
                Properties = GetObjectProperties(token, token.GetArray(StringConstants.Required)?.Select(p => p.FormatAsName())),
                AdditionalProperties = token.GetBoolean(StringConstants.AdditionalProperties) == true,
                AllOf = token.UseValueOrDefault(StringConstants.AllOf, t => t?.Select(ao => ParseObject(ao))),
                OneOf = token.UseValueOrDefault(StringConstants.OneOf, t => t?.Select(oo => ParseObject(oo))),
                Reference = ParseReference(token.GetString(StringConstants.Reference))
            };
        }

        private static IEnumerable<ApiObjectProperty> GetObjectProperties(JToken token, IEnumerable<string> requiredProperties)
        {
            return token.UseValueOrDefault(StringConstants.Properties, t => t.Select(p =>
            {
                var property = ParseObjectProperty(p.First);

                if (requiredProperties != null)
                {
                    property.IsRequired = requiredProperties.Contains(property.Name);
                }

                return property;
            }));
        }

        private static ApiObjectProperty ParseObjectProperty(JToken token)
        {
            if (token == null)
            {
                return null;
            }

            var objectPropertyItems = token.UseValueOrDefault(StringConstants.Items, ParseObjectProperty);

            return new ApiObjectProperty
            {
                Name = token.Path.Split('.').Last().FormatAsName(),
                Description = token.GetString(StringConstants.Description),
                Type = SharedTypesParser.ParseType(token.GetString(StringConstants.Type)),
                Minimum = token.GetInteger(StringConstants.Minimum),
                Enum = token.GetArray(StringConstants.Enum)?.Select(item => item.FormatAsName()),
                EnumNames = token.GetArray(StringConstants.EnumNames)?.Select(item => item.FormatAsName()),
                Items = objectPropertyItems,
                Reference = ParseReference(token.GetString(StringConstants.Reference))
            };
        }

        private static string ParseReference(string referenceString)
        {
            return string.IsNullOrWhiteSpace(referenceString) ? null : referenceString.Split('/').LastOrDefault()?.FormatAsName();
        }
    }
}
