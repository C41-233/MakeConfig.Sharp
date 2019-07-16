using System.Collections.Generic;
using System.Linq;
using System.Text;
using MakeConfig.Excel;
using MakeConfig.Output;
using MakeConfig.Processor.DataType;
using MakeConfig.Utils;

namespace MakeConfig.Processor
{

    internal static class TypeGenerator
    {

        public static void GenerateType(string type, List<VirtualDataTable> tables)
        {
            AssertSameMetas(tables);

            var table = tables[0];
            AssertIdMeta(table, out var idMeta);

            var configType = new ConfigType(table.ConfigName);

            configType.AddField(GetType(idMeta.Name, idMeta.Type), Config.IdName, idMeta.Description);

            foreach (var meta in table.ColumnMetas.Skip(1))
            {
                var field = meta.Name;
                try
                {
                    configType.AddField(GetType(field, meta.Type), field, meta.Description);
                }
                catch (MakeConfigException e)
                {
                    throw new MakeConfigException($"在文件{table.File.GetAbsolutePath()}中解析字段{field}时遇到错误：{e.Message}");
                }
            }

            using (var writer = new FileWriter($"{Config.OutputFolder}/{type}.cs"))
            {
                configType.Write(writer);
            }
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
                var vt = VirtualTypePool.Get(type);

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

            vt = new CustomStructType(field + "Type");
            foreach (var token in tokens)
            {
                var whitespace = token.LastIndexOf(' ');
                var splits = token.SplitAt(whitespace);
                var fieldType = splits[0].Trim();
                var fieldName = splits[1].Trim();
                vt.AddField(fieldName, GetType(fieldName, fieldType));
            }

            return true;
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
