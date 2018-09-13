using System.Collections.Generic;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser
{
    internal static class ObjectTypeMapper
    {
        private static IDictionary<string, ApiObjectType> TypesMapping = new Dictionary<string, ApiObjectType>
        {
            { JsonStringConstants.Multiple, ApiObjectType.Multiple },
            { JsonStringConstants.Object, ApiObjectType.Object },
            { JsonStringConstants.Integer, ApiObjectType.Integer },
            { JsonStringConstants.String, ApiObjectType.String },
            { JsonStringConstants.Array, ApiObjectType.Array },
            { JsonStringConstants.Number, ApiObjectType.Number },
            { JsonStringConstants.Boolean, ApiObjectType.Boolean }
        };

        public static ApiObjectType Map(string typeName)
        {
            return TypesMapping.TryGetValue(typeName, out var objectType) ? objectType : ApiObjectType.Undefined; 
        }
    }
}
