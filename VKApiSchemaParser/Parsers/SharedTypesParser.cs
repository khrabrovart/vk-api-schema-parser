using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class SharedTypesParser
    {
        public static ApiObjectType ParseType(string typeName)
        {
            switch (typeName)
            {
                case StringConstants.Integer:
                    return ApiObjectType.Integer;
                case StringConstants.String:
                    return ApiObjectType.String;
                case StringConstants.Boolean:
                    return ApiObjectType.Boolean;
                case StringConstants.Object:
                    return ApiObjectType.Object;
                case StringConstants.Array:
                    return ApiObjectType.Array;
                case StringConstants.Number:
                    return ApiObjectType.Number;
                default:
                    return ApiObjectType.Undefined;
            }   
        }
    }
}
