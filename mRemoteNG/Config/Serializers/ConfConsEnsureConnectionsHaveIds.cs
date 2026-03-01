using System;
using System.Xml.Linq;


namespace mRemoteNG.Config.Serializers
{
    public static class ConfConsEnsureConnectionsHaveIds
    {
        public static void EnsureElementsHaveIds(XDocument xdoc)
        {
            foreach (XElement element in xdoc.Descendants("Node"))
            {
                if (element.Attribute("Id") != null) continue;
                Guid id = Guid.NewGuid();
                element.Add(new XAttribute("Id", id));
            }
        }
    }
}