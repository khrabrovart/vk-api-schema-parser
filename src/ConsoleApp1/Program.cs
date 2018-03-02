using System;
using System.IO;
using VKApiSchemaParser;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var vkapi = new VKApiSchema();
            var a = vkapi.GetObjectsHierarchyAsync().Result;

            File.WriteAllLines("D:\\hier.txt", a);
        }
    }
}
