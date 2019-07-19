using System.Collections.Generic;
using System.Linq;
using System.Text;
using MakeConfig.Configs;
using MakeConfig.Excel;
using MakeConfig.Output;
using MakeConfig.Processor.Types;
using MakeConfig.Utils;

namespace MakeConfig.Processor
{

    internal static class TypeGenerator
    {

        private class SplitField
        {
            public VirtualType Type;
            public readonly List<DeclaredMember> DeclaredMembers = new List<DeclaredMember>();
            public string Description;
            public readonly Dictionary<string, SplitField> ChildFields = new Dictionary<string, SplitField>();
        }

        private struct DeclaredMember
        {
            public string Name;
            public VirtualType Type;
            public string Description;
        }

        public static ConfigType GenerateType(List<VirtualDataTable> tables)
        {
            AssertSameMetas(tables);
            return GenerateType(tables[0]);
        }

        private static ConfigType GenerateType(VirtualDataTable table)
        {
            CheckAndGetIdMeta(table, out var idMeta);

            var configType = new ConfigType(table.TableName + Config.GenerateClassSuffix);

            configType.SetIdField(GetType(idMeta.FieldFullName, idMeta.TypeSpec), idMeta.Description);

            var rootSplitFields = new Dictionary<string, SplitField>();

            var tableConfig = TableConfigs.Get(table.TableName);

            //先填充xml定义的列
            if (tableConfig != null)
            {
                foreach (var typeDefine in tableConfig.DefineTypes)
                {
                    var splitField = new SplitField
                    {
                        Description = typeDefine.Description,
                    };
                    if (typeDefine.ImportType != null)
                    {
                        splitField.Type = ImportTypePool.Get(typeDefine.ImportType);
                    }
                    rootSplitFields.Add(typeDefine.FieldName, splitField);
                }
            }

            foreach (var meta in table.ColumnMetas.Skip(1))
            {
                var fieldFullName = meta.FieldFullName.Trim();
                try
                {
                    var fieldType = GetType(fieldFullName.Replace(".", ""), meta.TypeSpec);
                    //split field
                    if (fieldFullName.Contains("."))
                    {
                        ParseSplitField(rootSplitFields, fieldFullName, fieldType, meta.Description);
                    }
                    //normal field
                    else
                    {
                        configType.AddField(fieldType, fieldFullName, meta.Description);
                    }
                }
                catch (MakeConfigException e)
                {
                    throw new MakeConfigException($"在文件{table.File.GetAbsolutePath()}中解析字段{fieldFullName}时遇到错误：{e.Message}");
                }
            }

            foreach (var kv in rootSplitFields)
            {
                var fieldName = kv.Key;
                var ctx = kv.Value;

                var virtualType = ctx.Type ?? CreateSplitType(fieldName, ctx);

                configType.AddField(virtualType, fieldName, ctx.Description);
            }

            return configType;
        }

        private static void ParseSplitField(Dictionary<string, SplitField> parent, string fieldPartName, VirtualType type, string description)
        {
            fieldPartName.Split2By('.', out var name, out var body);
            if (!parent.TryGetValue(name, out var splitField))
            {
                splitField = new SplitField();
                parent.Add(name, splitField);
            }

            //check consist
            splitField.Type?.CheckImportField(body, type);

            if (body.Contains("."))
            {
                ParseSplitField(splitField.ChildFields, body, type, description);
            }
            else
            {
                splitField.DeclaredMembers.Add(new DeclaredMember
                {
                    Name = body,
                    Type = type,
                    Description = description,
                });
            }
        }

        private static VirtualType GetType(string fullName, string typeSpec)
        {
            {
                if (TryCreateEnumType(fullName, typeSpec, out var vt))
                {
                    return vt;
                }
            }
            {
                if (TryCreateStructType(fullName, typeSpec, out var vt))
                {
                    return vt;
                }
            }
            {
                var vt = ImportTypePool.Get(typeSpec);

                if (vt == null)
                {
                    throw MakeConfigException.TypeNotFound(typeSpec);
                }

                return vt;
            }
        }

        private static bool TryCreateEnumType(string fullName, string typeSpec, out CustomEnumType vt)
        {
            if (!typeSpec.StartsWith("enum"))
            {
                vt = null;
                return false;
            }

            typeSpec = typeSpec.RemoveFirst("enum").Trim();
            if (!typeSpec.StartsWith("{") || !typeSpec.EndsWith("}"))
            {
                throw MakeConfigException.TypeFormatError(typeSpec);
            }

            typeSpec = typeSpec.RemoveBoth("{", "}");

            vt = new CustomEnumType(fullName + "Enum");
            foreach (var token in typeSpec.Split(','))
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

        private static bool TryCreateStructType(string fieldFullName, string typeSpec, out CustomStructType vt)
        {

            if (!typeSpec.StartsWith("struct"))
            {
                vt = null;
                return false;
            }

            typeSpec = typeSpec.RemoveFirst("struct").Trim();
            if (!typeSpec.StartsWith("{") || !typeSpec.EndsWith("}"))
            {
                throw MakeConfigException.TypeFormatError(typeSpec);
            }

            typeSpec = typeSpec.RemoveBoth("{", "}");

            //pass for recursive type
            var tokens = new List<string>();
            var sb = new StringBuilder();
            var depth = 0;
            foreach (var ch in typeSpec)
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

            vt = new CustomStructType(fieldFullName + "Type", true);
            foreach (var token in tokens)
            {
                var whitespace = token.LastIndexOf(' ');
                var splits = token.SplitAt(whitespace);
                var childTypeSpec = splits[0].Trim();
                var childFieldName = splits[1].Trim();
                vt.AddField(childFieldName, GetType(fieldFullName + childFieldName, childTypeSpec), null);
            }

            return true;
        }

        private static VirtualType CreateSplitType(string bodyName, SplitField splitField)
        {
            var type = new CustomStructType(bodyName + "Type");
            foreach (var member in splitField.DeclaredMembers)
            {
                type.AddField(member.Name, member.Type, member.Description);
            }

            foreach (var kv in splitField.ChildFields)
            {
                var name = kv.Key;
                var child = kv.Value;
                type.AddField(name, CreateSplitType(bodyName + name, child), child.Description);
            }

            return type;
        }

        private static void CheckAndGetIdMeta(VirtualDataTable table, out ColumnMeta meta)
        {
            if (!table.TryGetColumnMeta(0, out meta))
            {
                throw MakeConfigException.NeedId(table);
            }

            if (meta.FieldFullName != Config.IdName || meta.Constraint != "#id")
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
