
using System.Collections.Generic;
using MakeConfig.Excel;

namespace MakeConfig.Processor
{

    internal static class VirtualTypePool
    {

        private static readonly Dictionary<string, VirtualType> Data = new Dictionary<string, VirtualType>();

        public static VirtualType Get(string type)
        {
            if (Data.TryGetValue(type, out var vt))
            {
                return vt;
            }

            if (BuiltInType.TryGetBuiltIn(type, out var map))
            {
                vt = new CLRType(map.Type);
                Data.Add(type, vt);
                return vt;
            }

            return null;
        }

    }

    internal abstract class VirtualType
    {

        public abstract string Name { get; }

    }
}
