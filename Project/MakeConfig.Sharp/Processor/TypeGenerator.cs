using System;
using System.Collections.Generic;
using System.IO;
using MakeConfig.Excel;
using MakeConfig.Output;
using MakeConfig.Template;
using MakeConfig.Utils;

namespace MakeConfig.Processor
{

    internal static class TypeGenerator
    {

        public static void GenerateType(string type, List<VirtualDataTable> tables)
        {
            AssertSameMetas(tables);

            var table = tables[0];
            var configType = new ConfigType(table.ConfigName);
            foreach (var meta in table.ColumnMetas)
            {
                if (BuiltInType.TryGetBuiltIn(meta.Type, out var map))
                {
                    configType.AddField(new CLRType(map.Type), meta.Name);
                }
            }

            using (var writer = new FileWriter($"{Config.OutputFolder}/{type}.cs"))
            {
                configType.Write(writer);
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
                    if (!table.TryGetColumnMeta(column, out ColumnMeta meta))
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
