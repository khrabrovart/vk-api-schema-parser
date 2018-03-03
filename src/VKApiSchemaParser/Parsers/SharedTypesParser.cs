using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class SharedTypesParser
    {
        public static ApiObjectType ParseObjectType(string typeName)
        {
            switch (typeName)
            {
                case JsonStringConstants.Multiple:
                    return ApiObjectType.Multiple;
                case JsonStringConstants.Object:
                    return ApiObjectType.Object;
                case JsonStringConstants.Integer:
                    return ApiObjectType.Integer;
                case JsonStringConstants.String:
                    return ApiObjectType.String;
                case JsonStringConstants.Array:
                    return ApiObjectType.Array;
                case JsonStringConstants.Number:
                    return ApiObjectType.Number;
                case JsonStringConstants.Boolean:
                    return ApiObjectType.Boolean;
                default:
                    return ApiObjectType.Undefined;
            }   
        }
    }
}
