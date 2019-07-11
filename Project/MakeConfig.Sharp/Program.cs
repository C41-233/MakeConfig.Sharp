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
            var inputFolder = @"..\..\..\..\Test\Input";
            DataProcessor.Run(inputFolder);
        }

    }
}
