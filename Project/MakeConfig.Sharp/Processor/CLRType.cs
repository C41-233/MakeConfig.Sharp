using System;
using MakeConfig.Excel;

namespace MakeConfig.Processor
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
                if (BuiltInType.TryGetBuiltIn(type, out var map))
                {
                    return map.Output;
                }
                return type.Name;
            }
        }

    }

}