using System.Collections.Generic;
using MakeConfig.Utils;
using OfficeOpenXml;

namespace MakeConfig.Excel
{

    public struct ColumnMeta
    {
        public string Description;
        public string DefaultValue;
        public string Name;
        public string Constraint;
        public string Type;
        public string Tag;

        public override string ToString()
        {
            return $"{Description}\t{DefaultValue}\t{Name}\t{Constraint}\t{Type}\t{Tag}";
        }
    }

    public class VirtualDataTable
    {

        public string ConfigName { get; }

        private readonly ExcelWorksheet sheet;

        public int ColumnCount => sheet.Dimension.Columns;

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

        public VirtualDataTable(string file, ExcelWorksheet sheet)
        {
            this.sheet = sheet;

            var configName = file.RemoveLast(Constant.Suffix);
            if (file.Contains("_"))
            {
                configName = file.Split(new []{'_'}, 2)[0];
            }
            if (sheet.Name != configName)
            {
                configName += sheet.Name;
            }

            ConfigName = configName + "Config";
        }

    }

}
