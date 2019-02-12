namespace ResourcesComparer.Helper
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using ResourcesComparer.Calculation;

    public class XmlConverter
    {
        public static void SaveTo(XmlNode enXml,
                                   XmlNode jaXml,
                                   XmlDocument windowsResource,
                                   StreamWriter writer,
                                   Action<StreamWriter, XmlNodeWithValue, XmlNodeWithValue, XmlNodeWithValue> saveToAction)
        {
            foreach (XmlNode rootNode in windowsResource.ChildNodes)
            {
                if (rootNode.Name != "root")
                {
                    continue;
                }

                foreach (XmlNode node in rootNode.ChildNodes)
                {
                    if (node == null)
                    {
                        continue;
                    }

                    if (node.Name != "data")
                    {
                        continue;
                    }

                    if (node.Attributes["type"] != null)
                    {
                        continue;
                    }

                    //var winName = node.Attributes["name"];
                    //var winValue = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "value").InnerText;
                    //{ Node = node, Value = winValue, Name = winName.Value }
                    var winNode = new XmlNodeWithValue(node, false);
                    if (winNode.Node.NodeType == XmlNodeType.Comment)
                    {
                        continue;
                    }

                    var englishNodeWithTanimoto = enXml.ChildNodes
                                                        .OfType<XmlNode>()
                                                        .Where(x => x.NodeType != XmlNodeType.Comment)
                                                        .Select(x => new XmlNodeWithValue(x, true)
                                                        {
                                                            Tanimoto = TanimotoStringComparer.Tanimoto(x.InnerText, winNode.Value, 1.2)
                                                        }).Where(x => x.Tanimoto > 0.7);

                    if (englishNodeWithTanimoto.Count() == 0)
                    {
                        continue;
                    }

                    var maxTanimoto = englishNodeWithTanimoto.Max(x => x.Tanimoto);
                    var englishNode = englishNodeWithTanimoto.FirstOrDefault(x => x.Tanimoto == maxTanimoto);

                    var japanNode = jaXml.ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.NodeType != XmlNodeType.Comment
                                                                                           && x.Attributes["name"].Value == englishNode.Node.Attributes["name"].Value);

                    var ourJapanXmlNode = new XmlNodeWithValue(japanNode, true);
                    saveToAction(writer, winNode, englishNode, ourJapanXmlNode);
                }
            }
        }

        internal static void SaveToCsvSingle(StreamWriter writer,
                                      XmlNodeWithValue winNode,
                                      XmlNodeWithValue englishNode,
                                      XmlNodeWithValue japanNode)
        {
            writer.WriteLine($"{winNode.Name};"
                             + $"'{winNode.Value.Replace(";", "|").Replace(Environment.NewLine, string.Empty)}';"
                             + $"{englishNode.Name};"
                             + $"'{englishNode.Value.Replace(";", "|").Replace(Environment.NewLine, string.Empty)}';"
                             + $"{japanNode?.Name ?? englishNode.Name};"
                             + $"{(japanNode?.Value ?? englishNode.Value).Replace(";", " | ").Replace(Environment.NewLine, string.Empty)}");
        }
    }
}
