using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ResponsesParser : BaseParser<ApiObject>
    {
        private JToken _definitions;
        private Dictionary<string, ApiObject> _apiObjects = new Dictionary<string, ApiObject>();

        protected override string SchemaDownloadUrl => SchemaUrl.Responses;

        protected override void Initialize(JSchema schema)
        {
            _definitions = schema.ExtensionData[JsonStringConstants.Definitions];
        }

        protected override ApiObject Parse(JToken token)
        {
            throw new NotImplementedException();
        }
    }
}
