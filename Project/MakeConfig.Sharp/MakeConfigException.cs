using System;
using MakeConfig.Excel;

namespace MakeConfig
{
    public class MakeConfigException : Exception
    {

        private MakeConfigException(string msg) : base(msg)
        {
            
        }

        public static MakeConfigException SheetNotMatch(VirtualDataTable left, VirtualDataTable right, int i)
        {
            ColumnMeta meta;
            if (!left.TryGetColumnMeta(i, out meta))
            {
                throw new MakeConfigException($"分表{left.File}和{right.File}数据第{i}列不一致，左侧缺失");
            }
            if (!right.TryGetColumnMeta(i, out meta))
            {
                throw new MakeConfigException($"分表{left.File}和{right.File}数据第{i}列不一致，右侧缺失");
            }
            return null;
        }

        internal static Exception SheetNotMatch(VirtualDataTable left, VirtualDataTable right, int i, string column)
        {
            throw new MakeConfigException($"分表{left.File}和{right.File}数据第{i}列{column}不一致");
        }
    }
}
