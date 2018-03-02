using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class SharedTypesParser
    {
        public static ApiObjectType ParseObjectType(string typeName)
        {
            switch (typeName)
            {
                case JsonStringConstants.Integer:
                    return ApiObjectType.Integer;
                case JsonStringConstants.String:
                    return ApiObjectType.String;
                case JsonStringConstants.Object:
                    return ApiObjectType.Object;
                default:
                    return ApiObjectType.Undefined;
            }   
        }

        public static ApiObjectPropertyType ParseObjectPropertyType(string typeName)
        {
            switch (typeName)
            {
                case JsonStringConstants.Integer:
                    return ApiObjectPropertyType.Integer;
                case JsonStringConstants.String:
                    return ApiObjectPropertyType.String;
                case JsonStringConstants.Boolean:
                    return ApiObjectPropertyType.Boolean;
                case JsonStringConstants.Array:
                    return ApiObjectPropertyType.Array;
                case JsonStringConstants.Number:
                    return ApiObjectPropertyType.Number;
                default:
                    return ApiObjectPropertyType.Undefined;
            }
        }
    }
}
