using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MakeConfig.Excel;
using MakeConfig.Output;
using MakeConfig.Processor.Types;
using MakeConfig.Template;
using MakeConfig.Utils;
using OfficeOpenXml;

namespace MakeConfig.Processor
{

    public static class DataProcessor
    {

        public static void Run()
        {
            var configNameToTables = new Dictionary<string, List<VirtualDataTable>>();
            foreach (var file in WalkAllExcelFiles(Config.InputFolder))
            {
                var package = LoadExcelPackage(file.ToString());
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    var table = new VirtualDataTable(file, sheet);
                    configNameToTables.GetValueOrCreate(table.ConfigName).Add(table);
                }
            }

            using (var writer = new FileWriter($"{Config.OutputFolder}/ConfigBase.cs"))
            {
                TemplateFile.Copy("ConfigBase.txt", writer);
            }

            if (Config.BaseTypeDll != null)
            {
                var assembly = Assembly.ReflectionOnlyLoadFrom(Config.BaseTypeDll);
                VirtualTypePool.Load(assembly);
            }

            foreach (var kv in configNameToTables)
            {
                TypeGenerator.GenerateType(kv.Key, kv.Value);
            }
        }

        private static IEnumerable<FileInfo> WalkAllExcelFiles(string inputFolder)
        {
            return WalkAllFiles(inputFolder).Where(file => file.Name.EndsWith(Constant.Suffix) && !file.Name.StartsWith("~"));
        }

        private static IEnumerable<FileInfo> WalkAllFiles(string inputFolder)
        {
            foreach (var folder in Directory.EnumerateDirectories(inputFolder))
            {
                foreach (var file in WalkAllExcelFiles(folder))
                {
                    yield return file;
                }
            }

            foreach (var file in Directory.EnumerateFiles(inputFolder))
            {
                yield return new FileInfo(file);
            }
        }

        private static ExcelPackage LoadExcelPackage(string path)
        {
            var excelPackage = new ExcelPackage();
            FileStream fileStream;
            try
            {
                fileStream = File.OpenRead(path);
            }
            catch (IOException)
            {
                var tempFileName = Path.GetTempFileName();
                File.Copy(path, tempFileName, true);
                fileStream = File.OpenRead(tempFileName);
            }
            using (fileStream)
            {
                excelPackage.Load(fileStream);
            }
            return excelPackage;
        }


    }

}
