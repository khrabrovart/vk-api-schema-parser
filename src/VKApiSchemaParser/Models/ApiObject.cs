using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    /// <summary>
    /// Represents an Object in the API schema.
    /// </summary>
    public class ApiObject
    {
        /// <summary>
        /// Gets or sets object's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets object's type.
        /// </summary>
        public ApiObjectType Type { get; set; }

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
        /// Gets or sets object's "additional properties" value.
        /// </summary>
        public bool AdditionalProperties { get; set; }

        /// <summary>
        /// Gets or sets object's "with setters" value.
        /// </summary>
        public bool WithSetters { get; set; }

        /// <summary>
        /// Gets or sets object's "without refs" value.
        /// </summary>
        public bool WithoutRefs { get; set; }

        /// <summary>
        /// Gets or sets minimum value.
        /// </summary>
        public int? Minimum { get; set; }

        /// <summary>
        /// Gets or sets items type if object's type is array.
        /// </summary>
        public ApiObject Items { get; set; } 
        
        /// <summary>
        /// Gets or sets flag whether (object) property is required or not.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets object's (only strings) format.
        /// </summary>
        public ApiStringFormat? Format { get; set; }
    }
}