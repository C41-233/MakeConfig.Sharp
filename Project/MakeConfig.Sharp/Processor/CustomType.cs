using System.Collections.Generic;
using MakeConfig.Output;
using MakeConfig.Utils;

namespace MakeConfig.Processor
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
        }
    }

}