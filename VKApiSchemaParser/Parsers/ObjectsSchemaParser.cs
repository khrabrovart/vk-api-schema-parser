using System.Linq;
using VKApiSchemaParser.Extensions;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Parsers
{
    internal class ObjectsSchemaParser : SchemaParser<ApiObjectsSchema>
    {
        protected override string CurrentSchemaUrl => SchemaUrl.Objects;

        protected override ApiObjectsSchema Parse()
        {
            var definitions = RawSchema.ExtensionData[StringConstants.Definitions];

            return new ApiObjectsSchema
            {
                SchemaVersion = RawSchema.SchemaVersion,
                Title = RawSchema.Title,
                Objects = definitions.Select(d => SharedObjectsParser.ParseObject(d.First, d.Path.FormatAsName()))
            };
        }
    }
}
