using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VKApiSchemaParser.Models;

namespace VKApiSchemaParser.Tests
{
    public class Program
    {
        private const string OutputDirectory = "parsed";

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };

        public static async Task Main()
        {
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            try
            {
                var schema = await VKApiSchema.ParseAsync();

                Console.WriteLine("Schema parsed");

                await SerializeAndSave(schema.Objects, "objects");
                await SerializeAndSave(schema.Responses, "responses");
                await SerializeAndSave(schema.Methods, "methods");

                Console.WriteLine($"\nComplete! Check \"{OutputDirectory}\" directory for output files.\nPress any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        public static Task SerializeAndSave<T>(IDictionary<string, T> obj, string name) where T : IApiEntity
        {
            Console.WriteLine($"Processing {name}");
            return SaveToFileAsync(JsonConvert.SerializeObject(obj, SerializerSettings), name);
        }

        private static Task SaveToFileAsync(string data, string prefix) => File.WriteAllTextAsync($"{OutputDirectory}\\{prefix}.json", data);
    }
}
