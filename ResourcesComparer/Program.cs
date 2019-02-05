namespace ResourcesComparer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    using ResourcesComparer.Calculation;

    partial class Program
    {
        /// <summary>
        /// At first - third param will be single file, after - load all resx from directory
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                return;
            }

            var assPath = new DirectoryInfo(Assembly.GetEntryAssembly().Location).Parent.FullName;

            args[1] = Path.Combine(args[0], args[1]);
            args[2] = Path.Combine(args[0], args[2]);
            args[3] = Path.Combine(args[0], args[3]);
            var output = Path.Combine(args[0], args[3], "_japan");

            if (!File.Exists(args[1]) || !File.Exists(args[2]) || !File.Exists(args[3]))
            {
                return;
            }

            var xmlArray = new List<XmlDocument>();
            for (int i = 1; i < args.Length; i++)
            {
                var resource = new XmlDocument();
                resource.Load(args[i]);
                xmlArray.Add(resource);
            }

            var enXml = xmlArray[0].ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "resources");

            var jaXml = xmlArray[1].ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "resources");

            using (StreamWriter writer = new StreamWriter(output))
            {

                foreach (XmlNode rootNode in xmlArray[2].ChildNodes)
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

                        var englishNodeWithTanimoto = enXml.ChildNodes.OfType<XmlNode>().Select(x => new XmlNodeWithValue()
                        {
                            Node = x,
                            Tanimoto = TanimotoStringComparer.Tanimoto(x.InnerText, winNode.Value, 1.2)
                        }).Where(x => x.Tanimoto > 0.7);
                        var maxTanimoto = englishNodeWithTanimoto.Max(x => x.Tanimoto);
                        var englishNode = englishNodeWithTanimoto.FirstOrDefault(x => x.Tanimoto == maxTanimoto);

                        var japanNode = jaXml.ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.NodeType != XmlNodeType.Comment
                                                                                               && x.Attributes["name"].Value == englishNode.Node.Attributes["name"].Value);

                        var ourXmlNode = new XmlNodeWithValue(japanNode, true);
                        SaveToCsv(writer, winNode, englishNode, japanNode);
                    }
                }

                writer.Close();
            }
        }

        private static void SaveToCsv(StreamWriter writer, 
                                      XmlNodeWithValue winNode, 
                                      XmlNodeWithValue englishNode, 
                                      XmlNodeWithValue japanNode)
        {
            writer.WriteLine($"'{winNode.Name}';'{winNode.Value}';'{englishNode.Name}';'{englishNode.Name}'"
                                + (japanNode));
        }
    }
}
