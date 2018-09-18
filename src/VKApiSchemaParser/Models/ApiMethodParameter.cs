using System;
using System.Collections.Generic;
using System.Text;

namespace VKApiSchemaParser.Models
{
    public class ApiMethodParameter
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
        /// Typically it's the type of object.
        /// </summary>
        public ApiObject Reference { get; set; }

        /// <summary>
        /// Gets or sets object's properties list.
        /// </summary>
        public IEnumerable<ApiObject> Properties { get; set; }

        /// <summary>
        /// Gets or sets object's pattern properties list.
        /// Pattern properties are properties whose names are represented as a regular expressions.
        /// </summary>
        public IEnumerable<ApiObject> PatternProperties { get; set; }

        /// <summary>
        /// Gets or sets object's allOf list.
        /// Typically it has one reference as parent object and
        /// one object with additional properties.
        /// </summary>
        public IEnumerable<ApiObject> AllOf { get; set; }

        /// <summary>
        /// Gets or sets object's oneOf list.
        /// </summary>
        public IEnumerable<ApiObject> OneOf { get; set; }

        /// <summary>
        /// Gets or sets object's enum values.
        /// </summary>
        public IEnumerable<string> Enum { get; set; }

        /// <summary>
        /// Gets or sets object's enum names.
        /// </summary>
        public IEnumerable<string> EnumNames { get; set; }

        /// <summary>
        /// Gets or sets object's minimum number of properties.
        /// </summary>
        public int? MinProperties { get; set; }

        /// <summary>
        /// Gets or sets object's maximum number of properties.
        /// </summary>
        public int? MaxProperties { get; set; }

        /// <summary>
        /// Gets or sets object's additional properties value.
        /// </summary>
        public bool AdditionalProperties { get; set; }

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
