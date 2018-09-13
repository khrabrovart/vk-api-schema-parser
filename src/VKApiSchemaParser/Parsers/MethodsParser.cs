using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    public class MethodsParser : BaseParser<ApiObject>
    {
        private JToken _definitions;
        private Dictionary<string, ApiObject> _apiObjects = new Dictionary<string, ApiObject>();

        protected override string SchemaDownloadUrl => SchemaUrl.Methods;

        protected override void Initialize(JSchema schema)
        {
            _definitions = schema.ExtensionData[JsonStringConstants.Methods];
        }

        protected override ApiObject Parse(JToken token)
        {
            var a = new List<string>();

            foreach (var d in _definitions)
            {
                var s = d.Children().Select(c => c.Path.Split('.').LastOrDefault());
                a.AddRange(s);
            }

            a = a.Distinct().ToList();

            foreach (var i in a)
            {
                Console.WriteLine(i);
            }

            return null;
        }
    }
}
