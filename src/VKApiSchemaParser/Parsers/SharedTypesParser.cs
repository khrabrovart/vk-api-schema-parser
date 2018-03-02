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

        public static ApiObjectType ParseObjectPropertyType(string typeName)
        {
            switch (typeName)
            {
                case JsonStringConstants.Integer:
                    return ApiObjectType.Integer;
                case JsonStringConstants.String:
                    return ApiObjectType.String;
                case JsonStringConstants.Boolean:
                    return ApiObjectType.Boolean;
                case JsonStringConstants.Array:
                    return ApiObjectType.Array;
                case JsonStringConstants.Number:
                    return ApiObjectType.Number;
                default:
                    return ApiObjectType.Undefined;
            }
        }
    }
}
