using System;
using System.Collections.Generic;
using System.Reflection;

namespace MakeConfig.Processor.Types
{
    internal static class ImportTypePool
    {

        private static readonly Dictionary<string, Type> types = new Dictionary<string, Type>();

        public static void Load(Assembly assembly)
        {
            foreach (var type in assembly.ExportedTypes)
            {
                types.Add(type.Name, type);
            }
        }

        public static bool TryGetType(string type, out Type clrType)
        {
            return types.TryGetValue(type, out clrType);
        }
    }
}
