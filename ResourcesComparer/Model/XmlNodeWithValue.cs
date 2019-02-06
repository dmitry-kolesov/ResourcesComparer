namespace ResourcesComparer
{
    using System.Linq;
    using System.Xml;

    public class XmlNodeWithValue
    {
        public double Tanimoto { get; set; }

        public XmlNode Node { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }

        public XmlNodeWithValue(XmlNode node, bool isAndroid)
        {
            Node = node;
            if (node == null)
            {
                return;
            }

            if (isAndroid)
            {
                Value = this.GetAndroidValue();
            }
            else
            {
                Value = this.GetWinValue();
            }

            Name = this.GetNodeName();
        }

        public string GetWinValue()
        {
            if (this.Node == null)
            {
                return string.Empty;
            }

            var text = this.Node.ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "value").InnerText;
            return text;
        }

        public void SetWinValue(string value)
        {
            if (this.Node == null)
            {
                return;
            }

            this.Node.ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "value").InnerText = value;
        }

        internal string GetNodeName()
        {
            return Node.Attributes?["name"].Value;
        }

        internal string GetAndroidValue()
        {
            return Node.InnerText;
        }
    }
}