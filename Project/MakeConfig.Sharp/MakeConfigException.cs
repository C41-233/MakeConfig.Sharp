﻿using System;
using MakeConfig.Excel;

namespace MakeConfig
{
    public class MakeConfigException : Exception
    {

        public MakeConfigException(string msg) : base(msg)
        {
        }

        public static MakeConfigException SheetNotMatch(VirtualDataTable left, VirtualDataTable right, int i)
        {
            if (!left.TryGetColumnMeta(i, out _))
            {
                throw new MakeConfigException($"分表{left.File}和{right.File}数据第{i+1}列不一致，左侧缺失");
            }
            if (!right.TryGetColumnMeta(i, out _))
            {
                throw new MakeConfigException($"分表{left.File}和{right.File}数据第{i+1}列不一致，右侧缺失");
            }
            return null;
        }

        internal static MakeConfigException SheetNotMatch(VirtualDataTable left, VirtualDataTable right, int i, string column)
        {
            throw new MakeConfigException($"分表{left.File}和{right.File}数据第{i+1}列{column}不一致");
        }

        public static MakeConfigException NeedId(VirtualDataTable table)
        {
            throw new MakeConfigException($"表结构缺少Id列：{table.File}，Id列的名称必须设为{Config.IdName}，约束指定#id");
        }

        public static MakeConfigException FormatError(string field, string type)
        {
            throw new MakeConfigException($"字段{field}：{type}");
        }

        public static MakeConfigException TypeNotFound(string type)
        {
            throw new MakeConfigException($"无法解析的数据类型：{type}");
        }

    }
}
