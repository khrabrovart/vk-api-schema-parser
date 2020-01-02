using System.Collections.Generic;

namespace VKApiSchemaParser.Models
{
    /// <summary>
    /// Represents API method.
    /// </summary>
    public class ApiMethod
    {
        /// <summary>
        /// Gets or sets method's category name.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets method's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets method's original full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets method's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets method's token types.
        /// </summary>
        public IEnumerable<ApiAccessTokenType> AccessTokenTypes { get; set; }

        /// <summary>
        /// Gets or sets method's parameters.
        /// </summary>
        public IEnumerable<ApiMethodParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets method's responses.
        /// </summary>
        public IEnumerable<ApiObject> Responses { get; set; }

        /// <summary>
        /// Gets or sets method's possible errors.
        /// </summary>
        public IEnumerable<ApiError> Errors { get; set; }
    }
}
