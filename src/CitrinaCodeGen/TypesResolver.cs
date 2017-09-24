using System.Collections.Generic;
using VKApiSchemaParser.Models;

namespace CitrinaCodeGen
{
    public static class TypesResolver
    {
        public static string MapType(ApiObjectType type)
        {
            switch (type)
            {
                case ApiObjectType.Integer:
                    return "int?";
                case ApiObjectType.String:
                    return "string";
                case ApiObjectType.Boolean:
                    return "bool?";
                case ApiObjectType.Number:
                    return "double?";
                case ApiObjectType.Object:
                    return "object";
                case ApiObjectType.Undefined:
                    return "string";
                default:
                    return "type_parse_error";
            }
        }

        public static string ResolveType(ApiObjectProperty property)
        {
            if (property.Type == ApiObjectType.Undefined)
            {
                if (property.Reference != null)
                {
                    return ResolveReferenceType(property.Reference);
                }

                if (property.AllOf != null)
                {
                    return MapType(ApiObjectType.String);
                }
            }

            if (property.Type != ApiObjectType.Array)
            {
                return ResolveDate(property.Name, MapType(property.Type), property.Description);
            }

            var enumerableGenericType = property.Items.Type != ApiObjectType.Undefined ? 
                ResolveDate(property.Name, MapType(property.Items.Type), property.Description) : 
                ResolveReferenceType(property.Items.Reference);

            if (property.Type == ApiObjectType.Array && property.Items.Type == ApiObjectType.Array)
            {
                enumerableGenericType = MapType(property.Items.Items.Type);
                return $"IEnumerable<IEnumerable<{enumerableGenericType ?? "string"}>>";
            }

            return $"IEnumerable<{enumerableGenericType ?? "string"}>";
        }

        public static string ResolveType(ApiMethodParameter parameter)
        {
            if (parameter.Type == ApiObjectType.Array)
            {
                return $"IEnumerable<{MapType(parameter.Items.Type) ?? "string"}>";
            }

            if (parameter.Type == ApiObjectType.String && JsonArrayParameters.Contains(parameter.Name))
            {
                return "JsonArray";
            }

            return ResolveDate(parameter.Name, MapType(parameter.Type), parameter.Description);
        }

        public static string ResolveType(ApiResponse response)
        {
            if (CustomTypeMap.TryGetValue(response.Name, out string remappedType))
            {
                return remappedType;
            }

            if (response.Object.Type == ApiObjectType.Array)
            {
                return $"IEnumerable<{ResolveType(response.Object.Items) ?? "string"}>";
            }

            if (response.Object.Type != ApiObjectType.Object && response.Object.Type != ApiObjectType.Undefined)
            {
                return MapType(response.Object.Type);
            }

            if (response.Object.Reference != null)
            {
                return CustomTypeMap.TryGetValue(response.Object.Reference.Name, out string remappedType2) ? remappedType2 : response.Object.Reference.Name;
            }

            return response.Object.Name;
        }

        private static string ResolveReferenceType(ApiObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.Type != ApiObjectType.Object && obj.Type != ApiObjectType.Undefined)
            {
                return CustomTypeMap.TryGetValue(obj.Name, out string remappedType) ? 
                    remappedType :
                    ResolveDate(obj.Name, MapType(obj.Type), null);
            }

            return obj.Name;
        }

        /// <summary>
        /// Magic for dates in Unixtime.
        /// </summary>
        private static string ResolveDate(string name, string resolvedType, string description)
        {
            if (!resolvedType.Equals("int?"))
            {
                return resolvedType;
            }

            description = description?.ToLower();

            return (!string.IsNullOrWhiteSpace(name) && (name.EndsWith("Date") || name.EndsWith("Timestamp") || name.EndsWith("DisabledUntil"))) ||
                
                (!string.IsNullOrWhiteSpace(description) && (description.Contains("unixtime") || description.Contains("timestamp") || description.Contains("unix time") || description.Contains("unix-time"))) ?

                "DateTime?" : 
                resolvedType;
        }

        private static readonly IDictionary<string, string> CustomTypeMap = new Dictionary<string, string>
        {
            ["BaseBoolInt"] = MapType(ApiObjectType.Boolean),
            ["BaseOkResponse"] = MapType(ApiObjectType.Boolean),
            ["OkResponse"] = MapType(ApiObjectType.Boolean),
            ["BasePropertyExists"] = MapType(ApiObjectType.Boolean)
        };

        private static readonly IList<string> JsonArrayParameters = new List<string>
        {
            "AddAnswers", "DeleteAnswers", "EditAnswers"
        };
    }
}
