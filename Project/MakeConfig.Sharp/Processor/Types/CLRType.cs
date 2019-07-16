using System;

namespace MakeConfig.Processor.Types
{
    internal sealed class CLRType : VirtualType
    {

        public Type Type { get; }

        public CLRType(string name, Type type)
        {
            Type = type;
            this.Name = name;
        }

        public CLRType(Type type)
        {
            Type = type;
            Name = type.Name;
        }

        public override string Name { get; }
    }

}