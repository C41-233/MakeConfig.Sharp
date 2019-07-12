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
            try
            {
                DataProcessor.Run();
            }
            catch (MakeConfigException e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
                Environment.Exit(-1);
            }
        }

    }
}
