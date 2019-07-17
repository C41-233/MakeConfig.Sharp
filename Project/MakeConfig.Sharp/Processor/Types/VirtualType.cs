namespace MakeConfig.Processor.Types
{

    internal abstract class VirtualType
    {

        public abstract string Name { get; }

        //检查指定字段是否符合导入类型
        public virtual void CheckImportField(string memberName, VirtualType memberType)
        {
            throw new InnerException();
        }
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

