using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace VKApiSchemaParser.Parsers
{
    internal abstract class BaseSchemaParser<T>
    {
        protected abstract string SchemaDownloadUrl { get; }

        private static T _parsedSchema;

        public async Task<T> ParseAsync()
        {
            if (_parsedSchema == null)
            {
                var schema = await InitializeAsync(SchemaDownloadUrl).ConfigureAwait(false);
                _parsedSchema = Parse(schema);
            }

            return _parsedSchema;
        } 

        protected async Task<JSchema> InitializeAsync(string schemaUrl)
        {
            var rawSchema = await SchemaLoader.Instance.LoadAsync(schemaUrl).ConfigureAwait(false);
            return JSchema.Parse(rawSchema);
        }

        protected abstract T Parse(JSchema schema);
    }
}
