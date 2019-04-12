namespace VKApiSchemaParser.Models
{
    /// <summary>
    /// Represents API object type.
    /// </summary>
    public enum ApiObjectType
    {
        /// <summary>
        /// Undefined object type.
        /// </summary>
        Undefined,

        /// <summary>
        /// Variable object type.
        /// </summary>
        Multiple,

        /// <summary>
        /// Object.
        /// </summary>
        Object,

        /// <summary>
        /// Integer.
        /// </summary>
        Integer,

        /// <summary>
        /// String.
        /// </summary>
        String,

        /// <summary>
        /// Array.
        /// </summary>
        Array,

        /// <summary>
        /// Number (mostly float).
        /// </summary>
        Number,

        /// <summary>
        /// Boolean.
        /// </summary>
        Boolean
    }
}
