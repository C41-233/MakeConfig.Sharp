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
            public string Description;
        }

        private readonly List<Field> fields = new List<Field>();

        private Field idField;

        public string Name { get; }

        public ConfigType(string name)
        {
            Name = name;
        }

        private readonly List<CustomType> innerTypes = new List<CustomType>();

        public void SetIdField(VirtualType type, string description)
        {
            idField = NewField(type, Config.IdName, description);
        }

        public void AddField(VirtualType type, string name, string description)
        {
            fields.Add(NewField(type, name, description));   
        }

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

        public void Write(IOutputWriter writer)
        {
            writer.WriteLine($"namespace {Config.Namespace}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                writer.WriteLine($"public sealed partial class {Name} : ConfigBase<{Name}, {idField.Type.Name}>");
                writer.WriteLine("{");
                writer.Block(() =>
                {
                    //fields
                    foreach (var field in fields)
                    {
                        WriteField(writer, field);
                        writer.WriteLine();
                    }

                    //inner class
                    foreach (var type in innerTypes)
                    {
                        type.Write(writer);
                    }
                });
                writer.WriteLine("}");
            });
            writer.WriteLine("}");
        }

        private static void WriteField(IOutputWriter writer, Field field)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine($"/// {field.Description}");
            writer.WriteLine("/// </summary>");
            writer.WriteLine($"public {field.Type.Name} {field.Name};");
        }

    }
}
