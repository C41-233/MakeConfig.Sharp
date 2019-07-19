using System.Collections.Generic;
using MakeConfig.Configs;
using MakeConfig.Excel;
using MakeConfig.Processor;
using MakeConfig.Processor.Types;
using MakeConfig.Utils;

namespace MakeConfig.Output
{
    internal static class CSharpOutput
    {

        public static void Output(ConfigType type, List<VirtualDataTable> tables)
        {
            var table = tables[0];
            using (var writer = new FileWriter($"{Config.OutputFolder}/{table.TableName + Config.GenerateClassSuffix}.cs"))
            {
                Write(writer, type);
            }
        }

        private static void Write(IOutputWriter writer, ConfigType type)
        {
            writer.WriteLine($"namespace {Config.Namespace}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                writer.WriteLine($"public sealed partial class {type.Name} : ConfigBase<{type.Name}, {type.IdField.Type.Name}>");
                writer.WriteLine("{");
                writer.Block(() =>
                {
                    //fields
                    foreach (var field in type.Fields)
                    {
                        WriteField(writer, field);
                        writer.WriteLine();
                    }

                    //inner class
                    foreach (var innerType in type.InnerTypes)
                    {
                        Write(writer, innerType);
                    }
                });
                writer.WriteLine("}");
            });
            writer.WriteLine("}");
        }

        private static void WriteField(IOutputWriter writer, ConfigType.Field field)
        {
            if (!string.IsNullOrWhiteSpace(field.Description))
            {
                writer.WriteLine("/// <summary>");
                writer.WriteLine($"/// {field.Description}");
                writer.WriteLine("/// </summary>");
            }
            writer.WriteLine($"public {field.Type.Name} {field.Name};");
        }

        private static void Write(IOutputWriter writer, CustomType type)
        {
            switch (type)
            {
                case CustomStructType t:
                    Write(writer, t);
                    break;
                case CustomEnumType t:
                    Write(writer, t);
                    break;
            }
        }

        private static void Write(IOutputWriter writer, CustomStructType type)
        {
            var keyword = type.IsStruct ? "struct" : "class";
            writer.WriteLine($"public {keyword} {type.Name}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                foreach (var field in type.Fields)
                {
                    if (!field.Description.IsNullOrEmpty())
                    {
                        writer.WriteLine("/// <summary>");
                        writer.WriteLine($"/// {field.Description}");
                        writer.WriteLine("/// </summary>");
                    }
                    writer.WriteLine($"public {field.Type.Name} {field.Name};");
                    writer.WriteLine();
                }

                foreach (var innerType in type.InnerTypes)
                {
                    Write(writer, innerType);
                    writer.WriteLine();
                }
            });
            writer.WriteLine("}");
            writer.WriteLine();
        }


        private static void Write(IOutputWriter writer, CustomEnumType type)
        {
            writer.WriteLine($"public enum {type.Name}");
            writer.WriteLine("{");
            writer.Block(() =>
            {
                foreach (var field in type.Fields)
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
