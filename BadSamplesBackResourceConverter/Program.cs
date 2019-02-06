namespace BadSamplesBackResourceConverter
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    using ResourcesComparer;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                return;
            }

            var directory = new DirectoryInfo(args[0]);

            args[1] = Path.Combine(args[0], args[1]);
            args[2] = Path.Combine(args[0], args[2]);

            if (!File.Exists(args[1]) || !File.Exists(args[2]))
            {
                return;
            }


            Dictionary<string, MappedItem> readContents = new Dictionary<string, MappedItem>();
            using (StreamReader streamReader = new StreamReader(args[2], Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var splitted = line?.Split(';');
                    if (splitted == null || splitted.Length < 2)
                    {
                        continue;
                    }

                    readContents.Add(
                        splitted[1],
                        new MappedItem
                        {
                            IsBadSamlpe = splitted[0] == "1",
                            WinResourceName = splitted[1],
                            WinResourceValue = splitted[2],
                            AndroidEnResourceName = splitted[3],
                            AndroidEnResourceValue = splitted[4],
                            AndroidJaResourceName = splitted[5],
                            AndroidJaResourceValue = splitted[6]
                        });
                }
            }


            var windowsResource = new XmlDocument();

            windowsResource.Load(args[1]);
            var output = Path.Combine(args[1]) + "_japan.resx";

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

                    var winNode = new XmlNodeWithValue(node, false);
                    if (readContents.ContainsKey(winNode.Name) && !readContents[winNode.Name].IsBadSamlpe)
                    {
                        winNode.SetWinValue(readContents[winNode.Name].AndroidJaResourceValue);
                    }
                }
            }

            windowsResource.Save(output);
        }

        public class MappedItem
        {
            public bool IsBadSamlpe { get; set; }

            public string WinResourceName { get; set; }
            public string WinResourceValue { get; set; }
            public string AndroidEnResourceName { get; set; }
            public string AndroidEnResourceValue { get; set; }
            public string AndroidJaResourceName { get; set; }
            public string AndroidJaResourceValue { get; set; }
        }
    }
}
