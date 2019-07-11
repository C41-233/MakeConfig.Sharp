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

        public string Name { get; }

        public ConfigType(string name)
        {
            Name = name;
        }

        public void AddField(VirtualType type, string name)
        {
            fields.Add(new Field
            {
                Type = type,
                Name = name,
            });   
        }

        public void Write(IOutputWriter writer)
        {
            writer.WriteLine($"public class {Name}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                foreach (var field in fields)
                {
                    writer.WriteLine($"{field.Type.Name} {field.Name} {{get; private set;}}");
                }
            });
            writer.WriteLine("}");
        }

    }
}
