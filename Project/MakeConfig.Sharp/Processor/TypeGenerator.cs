using System.Collections.Generic;
using System.Linq;
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
            AssertIdMeta(table, out var idMeta);

            var configType = new ConfigType(table.ConfigName);

            configType.SetIdField(VirtualTypePool.Get(idMeta.Type), idMeta.Description);

            foreach (var meta in table.ColumnMetas.Skip(1))
            {
                configType.AddField(VirtualTypePool.Get(meta.Type), meta.Name, meta.Description);
            }

            using (var writer = new FileWriter($"{Config.OutputFolder}/{type}.cs"))
            {
                configType.Write(writer);
            }
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
