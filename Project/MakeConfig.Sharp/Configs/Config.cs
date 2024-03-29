﻿
using System.Collections.Generic;

namespace MakeConfig.Configs
{

    public enum Mode
    {
        Config,
        Xml,
    }

    public enum OutputType
    {
        CSharp,
    }

    public static class Config
    {

        public static string IdName = "Id";

        public static string Namespace = "Config";

        public static string GenerateClassSuffix = "Config";

        public static string InputFolder = @"..\..\..\..\Test\Input";

        public static string OutputFolder = @"..\..\..\..\Test\Output\AutoGen";

        public static string BaseTypeDll = @"..\..\..\..\Test\BaseType\bin\BaseType.dll";

        public static string MetaConfig = null;

        public static List<string> IgnoreFiles = new List<string>();

        public static string Suffix = ".xlsx";

        public static Mode Mode = Mode.Config;

        public static OutputType OutputType = OutputType.CSharp;

    }
}
