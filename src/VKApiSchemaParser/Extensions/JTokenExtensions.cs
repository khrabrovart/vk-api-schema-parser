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
            if (token[propertyName] != null)
            {
                value = token[propertyName];
                return true;
            }

            value = null;
            return false;
        }

        public static T SelectPropertyOrDefault<T>(this JToken token, string propertyName, Func<JToken, T> selector)
        {
            return selector != null && TryGetProperty(token, propertyName, out JToken resultToken) ? 
                selector(resultToken) : 
                default(T);
        }

        public static string GetPropertyAsString(this JToken token, string propertyName)
        {
            return SelectPropertyOrDefault(token, propertyName, t => t.ToString());
        }

        public static int? GetPropertyAsInteger(this JToken token, string propertyName)
        {
            return token.TryGetProperty(propertyName, out JToken result) &&
                int.TryParse(result.ToString(), out int value) ? (int?)value : null;
        }

        public static bool? GetPropertyAsBoolean(this JToken token, string propertyName)
        {
            return token.TryGetProperty(propertyName, out JToken result) &&
                   bool.TryParse(result.ToString(), out bool value) ? (bool?)value : null;
        }

        public static IEnumerable<string> GetPropertyAsArray(this JToken token, string propertyName)
        {
            return token.SelectPropertyOrDefault(propertyName, t => t.Children().Select(c => c.ToString()));
        }
    }
}