using System;

namespace MakeConfig.Processor.DataType
{
    internal sealed class CLRType : VirtualType
    {

        private readonly Type type;

        public CLRType(Type type)
        {
            this.type = type;
        }

        public override string Name
        {
            get
            {
                if (BuiltInTypePool.TryGetBuiltIn(type, out var map))
                {
                    return map.Output;
                }
                return type.Name;
            }
        }

    }

}