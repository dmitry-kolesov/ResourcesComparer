namespace ResourcesComparer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    using ResourcesComparer.Calculation;
    using ResourcesComparer.Helper;

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

            var directory = new DirectoryInfo(args[0]);

            var windowsResources = directory.GetFiles("*.resx");
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
            
            var enXml = xmlArray[0].ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "resources");

            var jaXml = xmlArray[1].ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "resources");

            foreach (var windowsResource in windowsResources)
            {
                var resource = new XmlDocument();
                resource.Load(windowsResource.FullName);
                var output = Path.Combine(windowsResource.FullName) + "_japan";

                using (StreamWriter writer = new StreamWriter(output))
                {
                    writer.WriteLine($"Windows app English res name;Windows value;Android Res English name;Android value;Japan Res name;Japan value");

                    XmlConverter.SaveTo(enXml, jaXml, resource, writer, XmlConverter.SaveToCsvSingle);

                    writer.Close();
                }
            }
        }

    }
}
