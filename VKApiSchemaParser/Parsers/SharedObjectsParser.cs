using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal static class SharedObjectsParser
    {
        public static ApiObject ParseObject(JToken token, string name = null)
        {
            return new ApiObject
            {
                Name = name,
                Type = token.GetString(StringConstants.Type),
                Properties = GetObjectProperties(token, token.GetArray(StringConstants.Required)?.Select(p => p.FormatAsName())),
                AdditionalProperties = token.GetBoolean(StringConstants.AdditionalProperties) == true,
                AllOf = token.UseValueOrDefault(StringConstants.AllOf, t => t?.Select(ao => ParseObject(ao))),
                OneOf = token.UseValueOrDefault(StringConstants.OneOf, t => t?.Select(ao => ParseObject(ao))),
                Reference = token.GetString(StringConstants.Reference)
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
            var objectPropertyItems = token.UseValueOrDefault(StringConstants.Items, ParseObjectProperty);

            string objectPropertyReference = token.UseValueOrDefault(StringConstants.Reference,
                t => objectPropertyReference = t.ToString().Split('/').LastOrDefault()?.FormatAsName());

            return new ApiObjectProperty
            {
                Name = token.Path.Split('.').Last().FormatAsName(),
                Description = token.GetString(StringConstants.Description),
                Type = token.GetString(StringConstants.Type),
                Minimum = token.GetInteger(StringConstants.Minimum),
                Enum = token.GetArray(StringConstants.Enum)?.Select(item => item.FormatAsName()),
                EnumNames = token.GetArray(StringConstants.EnumNames)?.Select(item => item.FormatAsName()),
                Items = objectPropertyItems,
                Reference = objectPropertyReference
            };
        }
    }
}
