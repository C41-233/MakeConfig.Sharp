using System.Collections.Generic;
using System.IO;
using MakeConfig.Configs;
using MakeConfig.Utils;
using OfficeOpenXml;

namespace MakeConfig.Excel
{
    public class VirtualDataTable
    {

        public string SimpleName { get; }
        public string ConfigName { get; }

        private readonly ExcelWorksheet sheet;

        public int ColumnCount => sheet.Dimension.Columns;

        public FileInfo File { get; }

        public ColumnMeta GetColumnMeta(int column)
        {
            return new ColumnMeta
            {
                Description = sheet.Cells[1, column + 1].Text,
                DefaultValue = sheet.Cells[2, column + 1].Text,
                Name = sheet.Cells[3, column + 1].Text,
                Constraint = sheet.Cells[4, column + 1].Text,
                Type = sheet.Cells[5, column + 1].Text.Trim(),
                Tag = sheet.Cells[6, column + 1].Text,
            };
        }

        public bool TryGetColumnMeta(int column, out ColumnMeta meta)
        {
            if (column >= ColumnCount)
            {
                meta = default;
                return false;
            }
            meta = GetColumnMeta(column);
            return true;
        }

        public IEnumerable<ColumnMeta> ColumnMetas
        {
            get
            {
                var count = ColumnCount;
                for (var i=0; i<count; i++)
                {
                    yield return GetColumnMeta(i);
                }
            }
        }

        public VirtualDataTable(FileInfo file, ExcelWorksheet sheet)
        {
            this.sheet = sheet;
            File = file;

            var configName = file.Name.RemoveLast(Config.Suffix);
            if (file.Name.Contains("_"))
            {
                configName = file.Name.Split(new []{'_'}, 2)[0];
            }
            if (sheet.Name != configName)
            {
                configName += sheet.Name;
            }

            SimpleName = configName;
            ConfigName = configName + "Config";
        }

    }

}
