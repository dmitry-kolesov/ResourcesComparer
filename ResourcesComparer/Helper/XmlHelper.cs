using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesComparer.Helper
{
    using System.Xml;

    class XmlHelper
    {
        internal string GetNodeName(XmlNode node)
        {
            return node.Attributes["name"].Value;
        }

        internal string GetNodeValue()
        {

        }
    }
}
