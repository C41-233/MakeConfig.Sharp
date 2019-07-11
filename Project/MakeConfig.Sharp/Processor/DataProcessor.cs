using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MakeConfig.Excel;
using MakeConfig.Utils;
using OfficeOpenXml;

namespace MakeConfig.Processor
{

    public static class DataProcessor
    {

        public static void Run(string inputFolder)
        {
            var mergedFiles = new Dictionary<string, List<FileInfo>>();
            foreach (var file in WalkAllExcelFiles(inputFolder))
            {
                //merge file
                if (file.Name.Contains("_"))
                {
                    var groupName = file.Name.Split(new[]{'_'}, 2)[0];
                    mergedFiles.GetValueOrCreate(groupName).Add(file);
                }
                else
                {
                    mergedFiles.GetValueOrCreate(file.Name.RemoveLast(".xlsx")).Add(file);
                }
            }

            foreach (var kv in mergedFiles)
            {
                Console.WriteLine(kv.Key);
                foreach (var value in kv.Value)
                {
                    Console.WriteLine($"\t{value}");
                }
            }
        }

        private static IEnumerable<FileInfo> WalkAllExcelFiles(string inputFolder)
        {
            return WalkAllFiles(inputFolder).Where(file => file.Name.EndsWith(".xlsx") && !file.Name.StartsWith("~$"));
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
