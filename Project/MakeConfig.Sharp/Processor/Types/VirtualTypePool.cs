using System;
using System.Collections.Generic;
using System.Reflection;

namespace MakeConfig.Processor.Types
{
    internal static class VirtualTypePool
    {

        private static readonly Dictionary<string, VirtualType> Data = new Dictionary<string, VirtualType>();

        public static void Load(Assembly assembly)
        {
            AddCLRType("string", typeof(string));
            AddCLRType("char", typeof(char));
            AddCLRType("byte", typeof(byte));
            AddCLRType("sbyte", typeof(sbyte));
            AddCLRType("short", typeof(short));
            AddCLRType("ushort", typeof(ushort));
            AddCLRType("int", typeof(int));
            AddCLRType("uint", typeof(uint));
            AddCLRType("long", typeof(long));
            AddCLRType("ulong", typeof(ulong));
            AddCLRType("float", typeof(float));
            AddCLRType("double", typeof(double));
            AddCLRType("bool", typeof(bool));

            foreach (var type in assembly.ExportedTypes)
            {
                AddCLRType(type);
            }
        }

        private static void AddCLRType(string name, Type type)
        {
            var clrType = new CLRType(name, type);
            Add(type.Name, clrType);
            Add(name, clrType);
        }

        private static void AddCLRType(Type type)
        {
            Add(type.Name, new CLRType(type));
        }

        public static VirtualType Get(string type)
        {
            type = type.Trim();

            if (Data.TryGetValue(type, out var vt))
            {
                return vt;
            }

            //array
            if (type.EndsWith("[]"))
            {
                var baseType = Get(type.Substring(0, type.Length - 2));
                if (baseType == null)
                {
                    return null;
                }

                return Add(type, new ArrayType(baseType));
            }

            return null;
        }

        private static VirtualType Add(string type, VirtualType vt)
        {
            Data.Add(type, vt);
            return vt;
        }

    }
}