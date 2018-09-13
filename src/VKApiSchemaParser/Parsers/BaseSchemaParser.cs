using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace VKApiSchemaParser.Parsers
{
    internal abstract class BaseSchemaParser<T>
    {
        protected abstract string SchemaDownloadUrl { get; }

        public async Task<T> ParseAsync()
        {
            var schema = await InitializeAsync(SchemaDownloadUrl).ConfigureAwait(false);
            return Parse(schema);
        } 

        protected async Task<JSchema> InitializeAsync(string schemaUrl)
        {
            var rawSchema = await SchemaLoader.Instance.LoadAsync(schemaUrl).ConfigureAwait(false);
            return JSchema.Parse(rawSchema);
        }

        protected abstract T Parse(JSchema schema);
    }
}
