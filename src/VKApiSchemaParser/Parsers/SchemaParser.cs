using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace VKApiSchemaParser.Parsers
{
    internal abstract class SchemaParser<T>
    {
        protected abstract string CurrentSchemaUrl { get; }
        protected JSchema RawSchema { get; private set; }

        private static T _parsedSchema;

        public async Task<T> GetAsync()
        {
            if (_parsedSchema == null)
            {
                RawSchema = await InitializeAsync(CurrentSchemaUrl).ConfigureAwait(false);
                _parsedSchema = Parse();
            }

            return _parsedSchema;
        } 

        protected async Task<JSchema> InitializeAsync(string schemaUrl)
        {
            var rawSchema = await SchemaLoader.Instance.LoadAsync(schemaUrl).ConfigureAwait(false);
            return JSchema.Parse(rawSchema);
        }

        protected abstract T Parse();
    }
}
