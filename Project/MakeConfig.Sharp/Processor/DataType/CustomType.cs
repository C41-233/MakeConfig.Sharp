using System.Collections.Generic;
using MakeConfig.Output;
using MakeConfig.Utils;

namespace MakeConfig.Processor.DataType
{

    internal abstract class CustomType : VirtualType
    {

        public override string Name { get; }

        protected CustomType(string name)
        {
            Name = name;
        }

        public abstract void Write(IOutputWriter writer);
    }

    internal sealed class CustomStructType : CustomType
    {

        private struct Field
        {
            public VirtualType Type;
            public string Name;
        }

        private readonly List<Field> fields = new List<Field>();
        private readonly List<CustomType> innerTypes = new List<CustomType>();

        public CustomStructType(string name) : base(name)
        {
        }

        public override void Write(IOutputWriter writer)
        {
            writer.WriteLine($"public struct {Name}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                foreach (var field in fields)
                {
                    writer.WriteLine($"public {field.Type.Name} {field.Name};");
                    writer.WriteLine();
                }

                foreach (var type in innerTypes)
                {
                    type.Write(writer);
                    writer.WriteLine();
                }
            });
            writer.WriteLine("}");
            writer.WriteLine();
        }

        public void AddField(string field, VirtualType type)
        {
            fields.Add(new Field
            {
                Name = field,
                Type = type,
            });
            if (type is CustomType customType)
            {
                innerTypes.Add(customType);
            }
        }
    }

    internal sealed class CustomEnumType : CustomType
    {
        private readonly Dictionary<string, string> fields = new Dictionary<string, string>();

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

        public override void Write(IOutputWriter writer)
        {
            writer.WriteLine($"public enum {Name}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                foreach (var field in fields)
                {
                    if (field.Value != null)
                    {
                        writer.WriteLine($"{field.Key} = {field.Value},");
                    }
                    else
                    {
                        writer.WriteLine($"{field.Key},");
                    }
                }
            });
            writer.WriteLine("}");
            writer.WriteLine();
        }
    }

}