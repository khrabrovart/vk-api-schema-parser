using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class SharedTypesParser
    {
        public static ApiObjectType ParseType(string typeName)
        {
            switch (typeName)
            {
                case JsonStringConstants.Integer:
                    return ApiObjectType.Integer;
                case JsonStringConstants.String:
                    return ApiObjectType.String;
                case JsonStringConstants.Boolean:
                    return ApiObjectType.Boolean;
                case JsonStringConstants.Object:
                    return ApiObjectType.Object;
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
