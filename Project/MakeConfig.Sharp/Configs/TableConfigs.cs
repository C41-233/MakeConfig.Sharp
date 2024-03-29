﻿using System.Collections.Generic;
using MakeConfig.Utils;

namespace MakeConfig.Configs
{

    internal static class TableConfigs
    {

        private static readonly Dictionary<string, TableConfig> configs = new Dictionary<string, TableConfig>();

        public static void Add(string table, TableConfig config)
        {
            configs.Add(table, config);
        }

        public static TableConfig Get(string table)
        {
            return configs.GetValueOrDefault(table);
        }

    }

    internal class TableConfig
    {

        internal class DefineType
        {

            public string FieldName;
            public string ImportType;
            public string Description;

        }

        private readonly List<DefineType> defineTypes = new List<DefineType>();

        public IEnumerable<DefineType> DefineTypes => defineTypes;

        public void AddDefineType(DefineType type)
        {
            defineTypes.Add(type);
        }

    }

}
