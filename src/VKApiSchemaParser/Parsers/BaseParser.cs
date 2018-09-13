using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Threading.Tasks;

namespace VKApiSchemaParser.Parsers
{
    public abstract class BaseParser<T>
    {
        private bool _initialized;
        protected JSchema Schema { get; private set; }
        protected abstract string SchemaDownloadUrl { get; }

        public async Task<T> ParseAsync(JToken token)
        {
            if (token == null)
            {
                return default(T);
            }

            if (!_initialized)
            {
                Initialize(await GetSchemaAsync(SchemaDownloadUrl).ConfigureAwait(false));
                _initialized = true;
            }

            return Parse(token);
        }

        protected async Task<JSchema> GetSchemaAsync(string schemaUrl)
        {
            var rawSchema = await SchemaLoader.Instance.LoadAsync(schemaUrl).ConfigureAwait(false);
            return JSchema.Parse(rawSchema);
        }

        protected abstract void Initialize(JSchema schema);
        protected abstract T Parse(JToken token);
    }
}
