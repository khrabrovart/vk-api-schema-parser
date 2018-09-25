using System;
using System.Collections.Generic;

namespace VKApiSchemaParser.Models.Schemas
{
    public class ApiResponsesSchema : IApiSchema
    {
        /// <summary>
        /// Gets or sets schema version.
        /// </summary>
        public Uri SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets schema title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets responses as dictionary.
        /// </summary>
        public IDictionary<string, ApiObject> ResponsesDictionary { get; set; }

        /// <summary>
        /// Gets responses collection.
        /// </summary>
        public IEnumerable<ApiObject> Responses => ResponsesDictionary.Values;
    }
}
