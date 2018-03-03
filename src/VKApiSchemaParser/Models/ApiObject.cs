using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    public class ApiObject
    {
        /// <summary>
        /// Gets or sets beautified object's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets original object's name.
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Gets or sets object's type as enumerable.
        /// </summary>
        public ApiObjectType Type { get; set; }

        /// <summary>
        /// Gets or sets original object's type.
        /// </summary>
        public string OriginalTypeName { get; set; }

        /// <summary>
        /// Gets or sets object's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets object's reference object.
        /// Typically it's regarded as object's type.
        /// </summary>
        public ApiObject Reference { get; set; }


        public IEnumerable<ApiObject> Properties { get; set; }
        public IEnumerable<ApiObject> AllOf { get; set; }
        public IEnumerable<ApiObject> OneOf { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public IEnumerable<string> EnumNames { get; set; }
        public int? MinProperties { get; set; }
        public bool AdditionalProperties { get; set; }

        public int? Minimum { get; set; }
        public ApiObject Items { get; set; }     
        public bool IsRequired { get; set; }
    }
}