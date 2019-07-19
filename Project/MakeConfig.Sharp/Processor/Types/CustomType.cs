using System.Collections.Generic;
using MakeConfig.Output;
using MakeConfig.Utils;

namespace MakeConfig.Processor.Types
{

    internal abstract class CustomType : VirtualType
    {

        public override string Name { get; }

        protected CustomType(string name)
        {
            Name = name;
        }
    }

    internal sealed class CustomStructType : CustomType
    {

        public struct Field
        {
            public VirtualType Type;
            public string Name;
            public string Description;
        }

        private readonly List<Field> fields = new List<Field>();
        private readonly List<CustomType> innerTypes = new List<CustomType>();

        public readonly bool IsStruct;

        public IEnumerable<Field> Fields => fields;

        public IEnumerable<CustomType> InnerTypes => innerTypes;

        public CustomStructType(string name, bool isStruct = false) : base(name)
        {
            IsStruct = isStruct;
        }

        public void AddField(string field, VirtualType type, string description)
        {
            fields.Add(new Field
            {
                Name = field,
                Type = type,
                Description = description,
            });
            if (type is CustomType customType)
            {
                innerTypes.Add(customType);
            }
        }

        public override void CheckImportField(string memberName, VirtualType memberType)
        {
            throw new InnerException();
        }
    }

    internal sealed class CustomEnumType : CustomType
    {
        private readonly Dictionary<string, string> fields = new Dictionary<string, string>();

        public IEnumerable<KeyValuePair<string, string>> Fields => fields;

        public CustomEnumType(string name) : base(name)
        {
        }

        public void Add(string field)
        {
            fields.Add(field, null);
        }

        public void Add(string field, string value)
        {
            fields.Add(field, value);
        }

    }

}