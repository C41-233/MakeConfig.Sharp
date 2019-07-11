using System;
using System.Collections.Generic;
using System.Text;
using MakeConfig.Excel;
using MakeConfig.Output;

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
                if (meta.Type == "string")
                {
                    configType.AddField(new RuntimeType(typeof(string)), meta.Name);
                }
            }

            var writer = new StringWriter();
            configType.Write(writer);
            Console.WriteLine(writer);
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
