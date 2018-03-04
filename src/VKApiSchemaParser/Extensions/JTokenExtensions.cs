using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace VKApiSchemaParser.Extensions
{
    internal static class JTokenExtensions
    {
        public static bool TryGetValue(this JToken token, string propertyName, out JToken value)
        {
            if (token[propertyName] != null)
            {
                value = token[propertyName];
                return true;
            }

            value = null;
            return false;
        }

        public static T UseValueOrDefault<T>(this JToken token, string propertyName, Func<JToken, T> function)
        {
            return function != null && TryGetValue(token, propertyName, out JToken resultToken) ? 
                function(resultToken) : 
                default(T);
        }

        public static string GetString(this JToken token, string propertyName)
        {
            return UseValueOrDefault(token, propertyName, t => t.ToString());
        }

        public static int? GetInteger(this JToken token, string propertyName)
        {
            return token.TryGetValue(propertyName, out JToken result) &&
                int.TryParse(result.ToString(), out int value) ? (int?)value : null;
        }

        public static bool? GetBoolean(this JToken token, string propertyName)
        {
            return token.TryGetValue(propertyName, out JToken result) &&
                   bool.TryParse(result.ToString(), out bool value) ? (bool?)value : null;
        }

        public static IEnumerable<string> GetArray(this JToken token, string propertyName)
        {
            return token.UseValueOrDefault(propertyName, t => t.Children().Select(c => c.ToString()));
        }
    }
}