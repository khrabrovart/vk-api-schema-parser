using System;
using System.Collections.Generic;
using System.Linq;
using VKApiSchemaParser;
using VKApiSchemaParser.Models;

namespace CitrinaCodeGen
{
    public static class MethodsProcessor
    {
        public static IDictionary<string, IList<CitrinaMethod>> Process()
        {
            var result = new Dictionary<string, IList<CitrinaMethod>>();

            var apiSchema = new VKApiSchema();
            var methods = apiSchema.GetMethodsAsync().Result.Methods;

            foreach (var method in methods)
            {
                if (method.AccessTokenTypes == null)
                {
                    method.AccessTokenTypes = new[] {ApiAccessTokenType.Open};
                }

                foreach (var accessTokenType in method.AccessTokenTypes)
                {
                    var excludedParameters = method.Responses.Select(r => r.Name.Replace("Response", "")).ToArray();

                    foreach (var response in method.Responses)
                    {
                        if (response.Reference == null)
                        {
                            // Waiting for fixes to come...
                            continue;
                        }

                        var methodNamePostfix = response.Name.Replace("Response", "");

                        var methodParameters = new List<string>();
                        var mappingParameters = new List<string>();
                        var needAccessToken = true;

                        switch (accessTokenType)
                        {
                            case ApiAccessTokenType.User:
                                methodParameters.Add("UserAccessToken accessToken");
                                mappingParameters.Add("[\"access_token\"] = accessToken?.Value");
                                break;

                            case ApiAccessTokenType.Group:
                                methodParameters.Add("GroupAccessToken accessToken");
                                mappingParameters.Add("[\"access_token\"] = accessToken?.Value");
                                break;

                            case ApiAccessTokenType.Service:
                                methodParameters.Add("ServiceAccessToken accessToken");
                                mappingParameters.Add("[\"access_token\"] = accessToken?.Value");
                                break;

                            default:
                                needAccessToken = false;
                                break;
                        }

                        if (method.Parameters != null)
                        {
                            foreach (var parameter in method.Parameters)
                            {
                                var parameterName = parameter.Name;
                                var originalParameterName = parameter.OriginalName;
                                var parameterType = TypesResolver.ResolveType(parameter);

                                if (!parameterName.Equals(methodNamePostfix) && excludedParameters.Contains(parameterName))
                                {
                                    continue;
                                }

                                parameterName = parameterName[0].ToString().ToLower() + string.Join("", parameterName.Skip(1));

                                if (InternalNames.Contains(parameterName))
                                {
                                    parameterName = "@" + parameterName;
                                    originalParameterName = "@" + originalParameterName;
                                }

                                methodParameters.Add($"{parameterType} {parameterName} = null");

                                mappingParameters.Add(GetDictionaryParameter(parameterType, originalParameterName, parameterName));
                            }
                        }

                        var citrinaMethod = new CitrinaMethod
                        {
                            Name = $"{method.Name}{methodNamePostfix}",
                            OriginalName = method.OriginalName,
                            Description = method.Description,
                            InlineParameters = string.Join(", ", methodParameters),
                            MappingParameters = mappingParameters,
                            ReturnType = TypesResolver.ResolveType(response.Reference),
                            NeedAccessToken = needAccessToken
                        };

                        if (result.ContainsKey(method.MethodGroup))
                        {
                            result[method.MethodGroup].Add(citrinaMethod);
                        }
                        else
                        {
                            result.Add(method.MethodGroup, new List<CitrinaMethod> {citrinaMethod});
                        }
                    }
                }
            }

            return result;
        }

        private static IList<string> InternalNames = new List<string>
        {
            "out", "long", "object", "private"
        };

        private static string GetDictionaryParameter(string parameterType, string originalParameterName, string parameterName)
        {
            if (parameterType.Equals("bool?"))
            {
                return $"[\"{originalParameterName}\"] = RequestHelpers.ParseBoolean({parameterName})";
            }

            if (parameterType.Contains("IEnumerable"))
            {
                return $"[\"{originalParameterName}\"] = RequestHelpers.ParseEnumerable({parameterName})";
            }

            if (parameterType.Equals("JsonArray"))
            {
                return $"[\"{originalParameterName}\"] = {parameterName}?.JsonValue";
            }

            if (parameterType.Equals("DateTime?"))
            {
                return $"[\"{originalParameterName}\"] = RequestHelpers.ParseDateTime({parameterName})";
            }

            if (parameterType.Equals("string"))
            {
                return $"[\"{originalParameterName}\"] = {parameterName}";
            }

            return $"[\"{originalParameterName}\"] = {parameterName}?.ToString()";
        }
    }
}
