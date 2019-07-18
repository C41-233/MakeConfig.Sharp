using System;
using MakeConfig.Configs;
using MakeConfig.Excel;
using MakeConfig.Processor.Constraints;
using MakeConfig.Utils;

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
                throw new MakeConfigException($"分表{left.File.GetAbsolutePath()}和{right.File.GetAbsolutePath()}数据第{i+1}列不一致，左侧缺失");
            }
            if (!right.TryGetColumnMeta(i, out _))
            {
                throw new MakeConfigException($"分表{left.File.GetAbsolutePath()}和{right.File.GetAbsolutePath()}数据第{i+1}列不一致，右侧缺失");
            }
            return null;
        }

        internal static MakeConfigException SheetNotMatch(VirtualDataTable left, VirtualDataTable right, int i, string column)
        {
            throw new MakeConfigException($"分表{left.File.GetAbsolutePath()}和{right.File.GetAbsolutePath()}数据第{i+1}列{column}不一致");
        }

        public static MakeConfigException NeedId(VirtualDataTable table)
        {
            throw new MakeConfigException($"表结构缺少Id列：{table.File.GetAbsolutePath()}，Id列的名称必须设为{Config.IdName}，约束指定#id");
        }

        public static MakeConfigException TypeFormatError(string typeSpec)
        {
            throw new MakeConfigException($"类型解析失败：{typeSpec}");
        }

        public static MakeConfigException TypeNotFound(string type)
        {
            throw new MakeConfigException($"无法解析的数据类型：{type}");
        }

        public static MakeConfigException IllegalConstraint(string constraint)
        {
            throw new MakeConfigException($"无法解析的约束：{constraint}");
        }

        public static MakeConfigException ImportTypeConstraintNotMatch()
        {
            throw new MakeConfigException($"拆分字段的导入类型约束不一致");
        }

        public static MakeConfigException FieldReferenceNotExist(Type type, string field)
        {
            throw new MakeConfigException($"在类型{type.FullName}中不存在的字段引用{field}");
        }

        public static MakeConfigException FieldReferenceTypeNotMatch(string expect, string real)
        {
            throw new MakeConfigException($"类型不一致，期望{expect}，实际{real}");
        }

        public static MakeConfigException RedundantSplitFieldDef(string field)
        {
            throw new MakeConfigException($"拆分字段{field}存在重复的定义列");
        }

        public static MakeConfigException ConfigTableNameMiss()
        {
            throw new MakeConfigException("配置文件table缺少name列");
        }
    }
}
