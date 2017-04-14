using System;
using System.Xml.Linq;


namespace mRemoteNG.Config.Serializers
{
    public class ConfConsEnsureConnectionsHaveIds
    {
        public void EnsureElementsHaveIds(XDocument xdoc)
        {
            foreach (var element in xdoc.Descendants("Node"))
            {
                if (element.Attribute("Id") != null) continue;
                var id = Guid.NewGuid();
                element.Add(new XAttribute("Id", id));
            }
        }
    }
}