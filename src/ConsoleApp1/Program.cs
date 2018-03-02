using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using VKApiSchemaParser;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var vkapi = new VKApiSchema();
            var a = vkapi.GetObjectsAsync().Result;
            a.Objects = a.Objects.Skip(0).Take(20);
            var j = JsonConvert.SerializeObject(a, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });

            File.WriteAllText($"D:\\json-{Guid.NewGuid()}.json", j);
            Console.WriteLine();
        }
    }
}
