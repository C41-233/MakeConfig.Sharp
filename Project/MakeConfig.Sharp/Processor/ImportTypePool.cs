using System;
using System.Reflection;

namespace MakeConfig.Processor
{
    internal static class ImportTypePool
    {
        public static void Load(Assembly assembly)
        {
            foreach (var type in assembly.ExportedTypes)
            {
                Console.WriteLine(type);
            }
        }
    }
}
