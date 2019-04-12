using System.Collections.Generic;

namespace VKApiSchemaParser.Models.Schemas
{
    /// <summary>
    /// Represents API schema content.
    /// </summary>
    public class ApiSchema
    {
        /// <summary>
        /// Gets or sets objects dictionary.
        /// </summary>
        public IDictionary<string, ApiObject> Objects { get; set; }

        /// <summary>
        /// Gets or sets responses dictionary.
        /// </summary>
        public IDictionary<string, ApiObject> Responses { get; set; }

        /// <summary>
        /// Gets or sets methods dictionary.
        /// </summary>
        public IDictionary<string, ApiMethod> Methods { get; set; }

        /// <summary>
        /// Gets or sets errors dictionary.
        /// </summary>
        public IDictionary<string, ApiError> Errors { get; set; }
    }
}
