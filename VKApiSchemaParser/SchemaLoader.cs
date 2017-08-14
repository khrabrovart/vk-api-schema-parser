using System.Net.Http;
using System.Threading.Tasks;

namespace VKApiSchemaParser
{
    internal class SchemaLoader
    {
        private static SchemaLoader _instance;
        private readonly HttpClient _client;

        public static SchemaLoader Instance => _instance ?? (_instance = new SchemaLoader());

        private SchemaLoader()
        {
            _client = new HttpClient();
        }

        public async Task<string> LoadAsync(string url)
        {
            var response = await _client.GetAsync(url).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
