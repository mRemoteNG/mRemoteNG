using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Messages.MessagePrinters;


namespace mRemoteNG.Config.MessagePrinters
{
    public class MessagePrinterSerializer
    {
        public string Serialize(IEnumerable<IMessagePrinter> messagePrinters)
        {
            var xdoc = new XDocument {Declaration = new XDeclaration("1.0", "utf-8", null)};
            xdoc.Add(
                new XElement("MessagePrinters",
                    from p in messagePrinters
                    select new XElement("MessagePrinter",
                        new XAttribute("Type", p.GetType()),
                        from a in p.GetType().GetProperties()
                        select new XAttribute(a.Name, a.GetValue(p, null))
                    )
                )
            );
            var fullDocument = xdoc.Declaration + Environment.NewLine + xdoc;
            return fullDocument;
        }
    }
}