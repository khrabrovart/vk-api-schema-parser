using System.Collections.Generic;
using System.Linq;
using VKApiSchemaParser;
using VKApiSchemaParser.Models;

namespace CitrinaCodeGen
{
    public static class ResponsesProcessor
    {
        public static IEnumerable<CitrinaObject> Process()
        {
            var result = new List<CitrinaObject>();

            var apiSchema = new VKApiSchema();
            var responses = apiSchema.GetResponsesAsync().Result.Responses;

            foreach (var response in responses)
            {
                if ((response.Object.Type != ApiObjectType.Object || response.Object.Reference != null) && response.Object.Properties == null)
                {
                    continue;
                }

                var citrinaObject = new CitrinaObject
                {
                    Name = response.Name,
                    Properties = GetObjectProperties(response.Object)
                };

                if (response.Type == ApiObjectType.Object || response.Type == ApiObjectType.Undefined)
                {
                    result.Add(citrinaObject);
                }
            }

            return result;
        }

        private static IEnumerable<CitrinaObjectProperty> GetObjectProperties(ApiObject obj)
        {
            var properties = new List<ApiObjectProperty>();

            if (obj.AllOf != null)
            {
                if (obj.AllOf.Any(o => o.OneOf != null))
                {
                    var oneOfList = obj.AllOf.FirstOrDefault(o => o.OneOf != null)?.OneOf;

                    if (oneOfList != null)
                    {
                        foreach (var o in oneOfList)
                        {
                            var props = o.Reference.Properties;

                            if (props != null)
                            {
                                properties.AddRange(props);
                            }
                        }
                    }
                }
                else
                {
                    var props = obj.AllOf.FirstOrDefault(o => o.Reference == null && o.OneOf == null)?.Properties;

                    if (props != null)
                    {
                        properties.AddRange(props);
                    }
                }
            }
            else
            {
                if (obj.Properties != null)
                {
                    properties.AddRange(obj.Properties);
                }
            }

            return properties.Distinct(new ObjectPropertyEqualityComparer()).Select(p =>
            {
                var name = p.Name;
                var needJsonAttr = false;

                if (char.IsDigit(name.First()))
                {
                    name = '_' + name;
                    needJsonAttr = true;
                }

                return new CitrinaObjectProperty
                {
                    Name = name,
                    OriginalName = p.OriginalName,
                    Type = TypesResolver.ResolveType(p),
                    NeedJsonAttribute = needJsonAttr,
                    Summary = p.Description
                };
            }).ToArray();
        }
    }
}
