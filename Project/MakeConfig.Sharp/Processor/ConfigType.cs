using System.Collections.Generic;
using MakeConfig.Output;
using MakeConfig.Utils;

namespace MakeConfig.Processor
{
    internal class ConfigType
    {

        private struct Field
        {
            public VirtualType Type;
            public string Name;
        }

        private readonly List<Field> fields = new List<Field>();

        private Field keyField;

        public string Name { get; }

        public ConfigType(string name)
        {
            Name = name;
        }

        public void AddField(VirtualType type, string name)
        {
            var field = new Field
            {
                Type = type,
                Name = name,
            };
            if (name == "Id")
            {
                keyField = field;
            }
            else
            {
                fields.Add(field);   
            }
        }

        public void Write(IOutputWriter writer)
        {
            writer.WriteLine($"namespace {Config.Namespace}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                writer.WriteLine($"public sealed partial class {Name} : ConfigBase<{Name}, {keyField.Type.Name}>");
                writer.WriteLine("{");
                writer.Block(() =>
                {
                    foreach (var field in fields)
                    {
                        writer.WriteLine($"public {field.Type.Name} {field.Name} {{get; private set;}}");
                    }
                });
                writer.WriteLine("}");
            });
            writer.WriteLine("}");
        }

    }
}
