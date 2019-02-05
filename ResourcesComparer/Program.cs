namespace ResourcesComparer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    class Program
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

            var enXml = xmlArray[0].ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "root");

            var jaXml = xmlArray[0].ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "root");

            foreach (XmlNode rootNode in xmlArray[2].ChildNodes)
            {
                if (rootNode.Name != "root")
                {
                    return;
                }

                foreach (XmlNode node in xmlArray[2].ChildNodes)
                {
                    if (node.Name != "data")
                    {
                        continue;
                    }

                    var winName = node.Attributes["name"];
                    var winValue = node.Attributes["value"];
                }
            }
        }


    }
}
