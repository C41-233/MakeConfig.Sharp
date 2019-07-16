namespace MakeConfig.Processor.Types
{

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

