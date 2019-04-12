namespace VKApiSchemaParser.Models
{
    /// <summary>
    /// Represents API error object.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Gets or sets error's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets error's code.
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// Gets or sets error's description.
        /// </summary>
        public string Description { get; set; }
    }
}
