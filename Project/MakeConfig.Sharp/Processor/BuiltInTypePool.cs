using System;
using System.Collections.Generic;
using MakeConfig.Utils;

namespace MakeConfig.Processor
{

    internal struct BuiltInTypeMap
    {
        public string Input;
        public Type Type;
        public string Output;
    }

    internal static class BuiltInTypePool
    {

        private static readonly List<BuiltInTypeMap> InputToMap = new List<BuiltInTypeMap>();

        static BuiltInTypePool()
        {
            AddMap("string", typeof(string));
            AddMap("char", typeof(char));
            AddMap("byte", typeof(byte));
            AddMap("sbyte", typeof(sbyte));
            AddMap("short", typeof(short));
            AddMap("ushort", typeof(ushort));
            AddMap("int", typeof(int));
            AddMap("uint", typeof(uint));
            AddMap("long", typeof(long));
            AddMap("ulong", typeof(ulong));
            AddMap("float", typeof(float));
            AddMap("double", typeof(double));
            AddMap("bool", typeof(bool));
        }

        public static bool TryGetBuiltIn(string input, out BuiltInTypeMap map)
        {
            return InputToMap.TryGetValue(v => v.Input == input, out map);
        }

        public static bool TryGetBuiltIn(Type type, out BuiltInTypeMap map)
        {
            return InputToMap.TryGetValue(v => v.Type == type, out map);
        }

        private static void AddMap(string input, Type type)
        {
            AddMap(input, type, input);
        }

        private static void AddMap(string input, Type type, string output)
        {
            InputToMap.Add(new BuiltInTypeMap
            {
                Input = input,
                Type = type,
                Output = output,
            });
        }

    }
}
