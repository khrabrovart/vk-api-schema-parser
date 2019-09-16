using System.Collections.Generic;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser
{
    internal static class ObjectTypeMapper
    {
        private static readonly IDictionary<string, ApiObjectType> _typesMapping = new Dictionary<string, ApiObjectType>
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
            return typeName == null || !_typesMapping.TryGetValue(typeName, out var objectType) 
                ? ApiObjectType.Undefined 
                : objectType; 
        }
    }
}
