using System;
using MakeConfig.Processor;

namespace MakeConfig
{
    internal static class Program
    {

        private static void Main(string[] args)
        {
            try
            {
                Main0(args);
            }
            catch (MakeConfigException e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
                Environment.Exit(-1);
            }
        }

        private static void Main0(string[] args)
        {
            //读取命令行参数初始化Config
            DataProcessor.Run();
        }

    }
}
