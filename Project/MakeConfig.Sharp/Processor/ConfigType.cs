using System;
using System.Collections.Generic;
using MakeConfig.Configs;
using MakeConfig.Processor.Types;

namespace MakeConfig.Processor
{
    internal class ConfigType
    {

        public struct Field
        {
            public VirtualType Type;
            public string Name;
            public string Description;
        }

        private readonly List<Field> fields = new List<Field>();

        public Field IdField { get; private set; }

        public string Name { get; }

        public ConfigType(string name)
        {
            Name = name;
        }

        private readonly List<CustomType> innerTypes = new List<CustomType>();

        public IEnumerable<CustomType> InnerTypes => innerTypes;

        public void SetIdField(VirtualType type, string description)
        {
            IdField = NewField(type, Config.IdName, description);
        }

        public void AddField(VirtualType type, string name, string description)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            fields.Add(NewField(type, name, description));   
        }

        public IEnumerable<Field> Fields => fields;

        private Field NewField(VirtualType type, string name, string description)
        {
            var field = new Field
            {
                Type = type,
                Name = name,
                Description = description,
            };
            if (type is CustomType customType)
            {
                innerTypes.Add(customType);
            }
            return field;
        }

    }
}
