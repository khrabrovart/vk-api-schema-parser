using System.Collections.Generic;

namespace VKApiSchemaParser.Models.Schemas
{
    public class ApiMethodsSchema : IApiSchema
    {
        /// <summary>
        /// Gets or sets errors.
        /// </summary>
        public IEnumerable<ApiError> Errors { get; set; }

        /// <summary>
        /// Gets or sets methods.
        /// </summary>
        public IEnumerable<ApiMethod> Methods { get; set; }
    }
}
