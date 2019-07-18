using System;
using System.Xml;

namespace MakeConfig.Utils
{
    internal static class XmlUtils
    {
        public static void ForeachNodes(this XmlNode root, string path, Action<XmlNode> action)
        {
            var nodes = root.SelectNodes(path);
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    action(node);
                }
            }
        }

        public static void TryParseSingleNode(this XmlNode root, string path, ref string value)
        {
            var node = root.SelectSingleNode(path);
            if (node != null)
            {
                value = node.InnerXml;
            }
        }

    }

}
