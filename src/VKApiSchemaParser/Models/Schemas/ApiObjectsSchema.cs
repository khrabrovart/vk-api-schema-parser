using System;
using System.Collections.Generic;

namespace VKApiSchemaParser.Models.Schemas
{
    public class ApiObjectsSchema : IApiSchema
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
        /// Gets or sets objects as dictionary.
        /// </summary>
        public IDictionary<string, ApiObject> ObjectsDictionary { get; set; }

        /// <summary>
        /// Gets objects collection.
        /// </summary>
        public IEnumerable<ApiObject> Objects => ObjectsDictionary.Values;
    }
}
