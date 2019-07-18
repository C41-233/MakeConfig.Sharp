using System.Xml;
using MakeConfig.Utils;

namespace MakeConfig.Configs
{
    internal static class ConfigReader
    {

        public static void Read()
        {
            if (Config.MetaConfig == null)
            {
                Config.MetaConfig = Config.InputFolder + "/config.xml";
            }

            var document = new XmlDocument();
            document.Load(Config.MetaConfig);

            document.TryParseSingleNode("/tables/id", ref Config.IdName);
            document.TryParseSingleNode("/tables/suffix", ref Config.Suffix);

            document.ForeachNodes("/tables/ignore", node =>
            {
                var text = node.InnerXml;
                if (!text.EndsWith(Config.Suffix))
                {
                    text += Config.Suffix;
                }
                Config.IgnoreFiles.Add(text);
            });

            document.ForeachNodes("/tables/table", ProcessTable);
        }

        private static void ProcessTable(XmlNode table)
        {
            var name = table.Attributes?["name"]?.Value;
            if (name == null)
            {
                throw MakeConfigException.ConfigTableNameMiss();
            }

            var tableConfig = new TableConfig();
            table.ForeachNodes("define", define =>
            {
                var type = new TableConfig.DefineType
                {
                    FieldName = define.Attributes?["name"]?.Value
                };
                define.TryParseSingleNode("comment", ref type.Description);
                define.TryParseSingleNode("import", ref type.ImportType);
                tableConfig.AddDefineType(type);
            });
            TableConfigs.Add(name, tableConfig);
        }

    }
}
