using System.Collections.Generic;
using VKApiSchemaParser.Models;

namespace CitrinaCodeGen
{
    internal class ObjectPropertyEqualityComparer : EqualityComparer<ApiObjectProperty>
    {
        public override bool Equals(ApiObjectProperty x, ApiObjectProperty y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Name.Equals(y.Name);
        }

        public override int GetHashCode(ApiObjectProperty obj)
        {
            return obj?.Name.GetHashCode() ?? 0;
        }
    }
}
