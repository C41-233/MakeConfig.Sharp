using System;
using System.IO;
using MakeConfig.Processor;
using OfficeOpenXml;

namespace MakeConfig
{
    internal static class Program
    {

        private static void Main(string[] args)
        {
            var inputFolder = @"D:\Workspace\MakeConfig.Sharp\Test\Input";
            var inputConfig = @"D:\Workspace\MakeConfig.Sharp\Test\config.txt";
            DataProcessor.Run(inputFolder);
        }

    }
}
