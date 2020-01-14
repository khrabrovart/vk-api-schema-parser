using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    /// <summary>
    /// Represents an enum entity.
    /// </summary>
    public interface IApiEnumEntity : IApiEntity
    {
        /// <summary>
        /// Gets API entity's enum values.
        /// </summary>
        IEnumerable<string> Enum { get; }

        /// <summary>
        /// Gets API entity's enum names.
        /// </summary>
        IEnumerable<string> EnumNames { get; }
    }
}
