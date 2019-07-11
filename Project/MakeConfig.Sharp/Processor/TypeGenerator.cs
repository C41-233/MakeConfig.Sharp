using System;
using System.Collections.Generic;
using MakeConfig.Excel;

namespace MakeConfig.Processor
{

    internal static class TypeGenerator
    {

        public static void GenerateType(string type, List<VirtualDataTable> tables)
        {
            foreach (var table in tables)
            {
                foreach (var meta in table.ColumnMetas)
                {
                    Console.WriteLine(meta);
                }
            }
        }

    }

}
