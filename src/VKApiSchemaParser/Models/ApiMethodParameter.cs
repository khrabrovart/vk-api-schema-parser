using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    /// <summary>
    /// Represents an API method parameter.
    /// </summary>
    public class ApiMethodParameter : IApiEntity
    {
        /// <summary>
        /// Gets or sets object's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets object's type as enumerable.
        /// </summary>
        public ApiObjectType Type { get; set; }

        /// <summary>
        /// Gets or sets object's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets object's enum values.
        /// </summary>
        public IEnumerable<string> Enum { get; set; }

        /// <summary>
        /// Gets or sets object's enum names.
        /// </summary>
        public IEnumerable<string> EnumNames { get; set; }

        /// <summary>
        /// Gets or sets parameter's minimum length.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets parameter's maximum length.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets minimum value.
        /// </summary>
        public int? Minimum { get; set; }

        /// <summary>
        /// Gets or sets maximum value.
        /// </summary>
        public int? Maximum { get; set; }

        /// <summary>
        /// Gets or sets default value.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Gets or sets maximum number of items in array.
        /// </summary>
        public int? MaxItems { get; set; }

        /// <summary>
        /// Gets or sets items type if object's type is array.
        /// </summary>
        public ApiObject Items { get; set; }

        /// <summary>
        /// Gets or sets flag whether (object) property is required or not.
        /// </summary>
        public bool IsRequired { get; set; }
    }
}
