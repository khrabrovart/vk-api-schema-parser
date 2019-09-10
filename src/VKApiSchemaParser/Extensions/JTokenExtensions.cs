using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace VKApiSchemaParser.Extensions
{
    internal static class JTokenExtensions
    {
        public static bool TryGetProperty(this JToken token, string propertyName, out JToken value)
        {
            value = token[propertyName];
            return value != null;
        }

        public static T SelectPropertyValueOrDefault<T>(this JToken token, string propertyName, Func<JToken, T> selector) => 
            selector != null && TryGetProperty(token, propertyName, out var resultToken) ? selector(resultToken) : default;

        public static string GetPropertyAsString(this JToken token, string propertyName) => 
            SelectPropertyValueOrDefault(token, propertyName, t => t.ToString());

        public static int? GetPropertyAsInteger(this JToken token, string propertyName) => 
            token.TryGetProperty(propertyName, out var result) && int.TryParse(result.ToString(), out int value) ? (int?)value : null;

        public static bool? GetPropertyAsBoolean(this JToken token, string propertyName) => 
            token.TryGetProperty(propertyName, out var result) && bool.TryParse(result.ToString(), out bool value) ? (bool?)value : null;

        public static IEnumerable<string> GetPropertyAsArray(this JToken token, string propertyName) =>
            token.SelectPropertyValueOrDefault(propertyName, t => t.Children().Select(c => c.ToString()));
    }
}