using System.Collections.Generic;
using System.Linq;
using System.Text;
using MakeConfig.Configs;
using MakeConfig.Excel;
using MakeConfig.Output;
using MakeConfig.Processor.Types;
using MakeConfig.Utils;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace MakeConfig.Processor
{

    internal static class TypeGenerator
    {

        private class SplitField
        {
            public readonly List<DeclaredMember> DeclaredMembers = new List<DeclaredMember>();
            public string Description;
            public VirtualType Type;
            public readonly List<SplitField> ChildFields = new List<SplitField>();
        }

        private struct DeclaredMember
        {
            public string RawText;
            public string Name;
            public VirtualType Type;
            public string Description;
        }

        public static void GenerateType(List<VirtualDataTable> tables)
        {
            AssertSameMetas(tables);
            GenerateType(tables[0]);
        }

        private static void GenerateType(VirtualDataTable table)
        {
            AssertIdMeta(table, out var idMeta);

            var configType = new ConfigType(table.TableName + Config.GenerateClassSuffix);

            configType.SetIdField(GetType(idMeta.Name, idMeta.Type), idMeta.Description);

            var splitFields = new Dictionary<string, SplitField>();

            var tableConfig = TableConfigs.Get(table.TableName);

            if (tableConfig != null)
            {
                foreach (var typeDefine in tableConfig.Types)
                {
                    var splitField = new SplitField
                    {
                        Description = typeDefine.Comment
                    };
                    if (typeDefine.ImportType != null)
                    {
                        splitField.Type = ImportTypePool.Get(typeDefine.ImportType);
                    }
                    splitFields.Add(typeDefine.FieldName, splitField);
                }
            }

            foreach (var meta in table.ColumnMetas.Skip(1))
            {
                var field = meta.Name.Trim();
                try
                {
                    var fieldType = GetType(field, meta.Type);

                    //split field
                    if (field.Contains("."))
                    {
                        ParseSplitField(field, fieldType, meta.Description);
                    }
                    //normal field
                    else
                    {
                        configType.AddField(fieldType, field, meta.Description);
                    }
                }
                catch (MakeConfigException e)
                {
                    throw new MakeConfigException($"在文件{table.File.GetAbsolutePath()}中解析字段{field}时遇到错误：{e.Message}");
                }
            }

            foreach (var kv in splitFields)
            {
                var fieldName = kv.Key;
                var ctx = kv.Value;

                VirtualType virtualType = null;

                //import type
                if (ctx.Type != null)
                {
                    virtualType = ctx.Type;

                    if (virtualType != null)
                    {
                        //check consist
                        foreach (var member in ctx.DeclaredMembers)
                        {
                            try
                            {
                                virtualType.CheckImportField(member.Name, member.Type);
                            }
                            catch (MakeConfigException e)
                            {
                                throw new MakeConfigException($"在文件{table.File.GetAbsolutePath()}中解析字段{member.RawText}失败：{e.Message}");
                            }
                        }
                    }
                }

                if (virtualType == null)
                {
                    virtualType = CreateSplitType(fieldName, ctx.DeclaredMembers);
                }

                configType.AddField(virtualType, fieldName, ctx.Description);
            }

            using (var writer = new FileWriter($"{Config.OutputFolder}/{table.TableName + Config.GenerateClassSuffix}.cs"))
            {
                configType.Write(writer);
            }

            void ParseSplitField(string field, VirtualType fieldType, string description)
            {
                var tokens = field.Split(new[] { '.' }, 2);
                if (!splitFields.TryGetValue(tokens[0], out var splitField))
                {
                    splitField = new SplitField();
                    splitFields.Add(tokens[0], splitField);
                }

                if (tokens[1].Contains("."))
                {

                }
                else
                {
                    splitField.DeclaredMembers.Add(new DeclaredMember
                    {
                        RawText = field,
                        Name = tokens[1],
                        Type = fieldType,
                        Description = description,
                    });
                }

            }

        }

        private static SplitField ParseChildSplitField()
        {

        }

        private static VirtualType GetType(string field, string type)
        {
            {
                if (TryCreateEnumType(field, type, out var vt))
                {
                    return vt;
                }
            }
            {
                if (TryCreateStructType(field, type, out var vt))
                {
                    return vt;
                }
            }
            {
                var vt = ImportTypePool.Get(type);

                if (vt == null)
                {
                    throw MakeConfigException.TypeNotFound(type);
                }

                return vt;
            }
        }

        private static bool TryCreateEnumType(string field, string type, out CustomEnumType vt)
        {
            if (!type.StartsWith("enum"))
            {
                vt = null;
                return false;
            }

            type = type.RemoveFirst("enum").Trim();
            if (!type.StartsWith("{") || !type.EndsWith("}"))
            {
                throw MakeConfigException.FormatError(field, type);
            }

            type = type.RemoveBoth("{", "}");

            vt = new CustomEnumType(field + "Type");
            foreach (var token in type.Split(','))
            {
                if (token.Contains("="))
                {
                    var split = token.Split(new[] { '=' }, 2);
                    vt.Add(split[0].Trim(), split[1].Trim());
                }
                else
                {
                    vt.Add(token.Trim());
                }
            }
            return true;
        }

        private static bool TryCreateStructType(string field, string type, out CustomStructType vt)
        {

            if (!type.StartsWith("struct"))
            {
                vt = null;
                return false;
            }

            type = type.RemoveFirst("struct").Trim();
            if (!type.StartsWith("{") || !type.EndsWith("}"))
            {
                throw MakeConfigException.FormatError(field, type);
            }

            type = type.RemoveBoth("{", "}");

            //pass for recursive type
            var tokens = new List<string>();
            var sb = new StringBuilder();
            var depth = 0;
            foreach (var ch in type)
            {
                switch (ch)
                {
                    case '{':
                        depth++;
                        break;
                    case '}':
                        depth--;
                        break;
                }

                if (ch == ';' && depth == 0)
                {
                    tokens.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                }
            }

            if (sb.Length > 0)
            {
                tokens.Add(sb.ToString());
            }

            vt = new CustomStructType(field + "Type", true);
            foreach (var token in tokens)
            {
                var whitespace = token.LastIndexOf(' ');
                var splits = token.SplitAt(whitespace);
                var fieldType = splits[0].Trim();
                var fieldName = splits[1].Trim();
                vt.AddField(fieldName, GetType(fieldName, fieldType), null);
            }

            return true;
        }

        private static VirtualType CreateSplitType(string fieldName, List<DeclaredMember> declaredMembers)
        {
            var type = new CustomStructType(fieldName + "Type");
            foreach (var member in declaredMembers)
            {
                type.AddField(member.Name, member.Type, member.Description);
            }
            return type;
        }

        private static void AssertIdMeta(VirtualDataTable table, out ColumnMeta meta)
        {
            if (!table.TryGetColumnMeta(0, out meta))
            {
                throw MakeConfigException.NeedId(table);
            }

            if (meta.Name != Config.IdName || meta.Constraint != "#id")
            {
                throw MakeConfigException.NeedId(table);
            }
        }

        private static void AssertSameMetas(List<VirtualDataTable> tables)
        {
            var target = tables[0];
            var len = target.ColumnCount;

            for (var column = 0; column < len; column++)
            {
                var targetMeta = target.GetColumnMeta(column);
                for (var i=1; i<tables.Count; i++)
                {
                    var table = tables[i];
                    if (!table.TryGetColumnMeta(column, out var meta))
                    {
                        throw MakeConfigException.SheetNotMatch(target, table, column);
                    }
                    if (!ColumnMeta.Equals(meta, targetMeta, out var error))
                    {
                        throw MakeConfigException.SheetNotMatch(target, table, column, error);
                    }
                }
            }
        }

    }

}
