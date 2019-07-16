using MakeConfig.Processor.Types;

namespace MakeConfig.Processor.Constraints
{
    internal sealed class ImportTypeConstraint : IConstraint
    {

        public CLRType Type { get; }

        public ImportTypeConstraint(CLRType type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is ImportTypeConstraint other)
            {
                return Equals(other);
            }

            return false;
        }

        private bool Equals(ImportTypeConstraint other)
        {
            return Type == other.Type;
        }

        public override int GetHashCode()
        {
            return Type?.GetHashCode() ?? 0;
        }

        public void CheckFieldReference(string field, VirtualType type)
        {
            var fieldInfo = Type.Type.GetField(field);
            if (fieldInfo == null)
            {
                throw MakeConfigException.FieldReferenceNotExist(field);
            }

            if (!(type is CLRType clrType))
            {
                throw MakeConfigException.FieldReferenceNotExist(field);
            }

            if (fieldInfo.FieldType != clrType.Type)
            {
                throw MakeConfigException.FieldReferenceTypeNotMatch(field, fieldInfo.FieldType.Name, clrType.Name);
            }
        }
    }
}