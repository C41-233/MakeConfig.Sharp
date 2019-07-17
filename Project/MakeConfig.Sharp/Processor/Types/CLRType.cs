using System;

namespace MakeConfig.Processor.Types
{
    internal sealed class CLRType : VirtualType
    {

        public Type Type { get; }

        public CLRType(string name, Type type)
        {
            Type = type;
            Name = name;
        }

        public CLRType(Type type)
        {
            Type = type;
            Name = type.FullName;
        }

        public override string Name { get; }

        public override void CheckImportField(string memberName, VirtualType memberType)
        {
            var field = Type.GetField(memberName);
            if (field == null)
            {
                throw MakeConfigException.FieldReferenceNotExist(memberName);
            }

            if (!(memberType is CLRType clrType))
            {
                throw new InnerException();
            }

            if (field.FieldType != clrType.Type)
            {
                throw MakeConfigException.FieldReferenceTypeNotMatch(memberName, field.FieldType.FullName, clrType.Type.FullName);
            }
        }
    }

}