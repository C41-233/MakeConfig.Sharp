using System.Collections.Generic;
using System.IO;
using MakeConfig.Utils;
using OfficeOpenXml;

namespace MakeConfig.Excel
{
    public class VirtualDataTable
    {

        public string ConfigName { get; }

        private readonly ExcelWorksheet sheet;

        public int ColumnCount => sheet.Dimension.Columns;

        public FileInfo File { get; }

        public ColumnMeta GetColumnMeta(int column)
        {
            return new ColumnMeta
            {
                Description = (string) sheet.Cells[1, column + 1].Value,
                DefaultValue = (string)sheet.Cells[2, column + 1].Value,
                Name = (string)sheet.Cells[3, column + 1].Value,
                Constraint = (string)sheet.Cells[4, column + 1].Value,
                Type = (string)sheet.Cells[5, column + 1].Value,
                Tag = (string)sheet.Cells[6, column + 1].Value,
            };
        }

        public bool TryGetColumnMeta(int column, out ColumnMeta meta)
        {
            if (column >= ColumnCount)
            {
                meta = default(ColumnMeta);
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

            var configName = file.Name.RemoveLast(Constant.Suffix);
            if (file.Name.Contains("_"))
            {
                configName = file.Name.Split(new []{'_'}, 2)[0];
            }
            if (sheet.Name != configName)
            {
                configName += sheet.Name;
            }

            ConfigName = configName + "Config";
        }

    }

}
