﻿using System.Collections.Generic;

namespace MakeConfig.Processor.DataType
{

    internal static class VirtualTypePool
    {

        private static readonly Dictionary<string, VirtualType> Data = new Dictionary<string, VirtualType>();

        public static VirtualType Get(string type)
        {
            type = type.Trim();

            if (Data.TryGetValue(type, out var vt))
            {
                return vt;
            }

            if (BuiltInTypePool.TryGetType(type, out var map))
            {
                return Add(type, new CLRType(map.Type));
            }

            if (ImportTypePool.TryGetType(type, out var clrType))
            {
                return Add(type, new CLRType(clrType));
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

    internal abstract class VirtualType
    {

        public abstract string Name { get; }

    }

    internal sealed class ArrayType : VirtualType
    {

        private readonly VirtualType baseType;

        public ArrayType(VirtualType baseType)
        {
            this.baseType = baseType;
        }

        public override string Name => baseType.Name + "[]";
    }
}
