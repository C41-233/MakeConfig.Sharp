using System;

namespace MakeConfig.Processor.Types
{
    internal sealed class CLRType : VirtualType
    {

        public Type Type { get; }

        public CLRType(Type type)
        {
            Type = type;
        }

        public override string Name
        {
            get
            {
                if (BuiltInTypePool.TryGetType(Type, out var map))
                {
                    return map.Output;
                }
                return Type.Name;
            }
        }

    }

}