using System;
using System.IO;
using OfficeOpenXml;

namespace MakeConfig.Sharp
{
    internal static class Program
    {

        private static void Main(string[] args)
        {
            using (var package = LoadExcelPackage(@"D:\Workspace\MakeConfig.Sharp\Test\Input\Item.xlsx"))
            {
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    Console.WriteLine(sheet.Name);
                }
            }
        }

        private static ExcelPackage LoadExcelPackage(string path)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            FileStream fileStream = null;
            try
            {
                fileStream = File.OpenRead(path);
            }
            catch (IOException)
            {
                string tempFileName = Path.GetTempFileName();
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
