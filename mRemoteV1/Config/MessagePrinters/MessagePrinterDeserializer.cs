using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using mRemoteNG.Messages.MessagePrinters;

namespace mRemoteNG.Config.MessagePrinters
{
    public class MessagePrinterDeserializer
    {
        public void ApplySettings(string xml, IEnumerable<IMessagePrinter> messagePrinters)
        {
            var xdoc = XDocument.Parse(xml);
            var xmlMessagePrinters = xdoc.Descendants("MessagePrinter").ToArray();
            foreach (var printer in messagePrinters)
                ApplyPrinterSettings(printer, xmlMessagePrinters);
        }

        private void ApplyPrinterSettings(IMessagePrinter printer, IEnumerable<XElement> xmlMessagePrinters)
        {
            foreach (var element in xmlMessagePrinters)
            {
                var objectType = printer.GetType();
                if (!XmlTypeMatchesPrinterObjectType(element, objectType)) continue;
                foreach (var property in objectType.GetProperties())
                    ApplyPropertyToObject(printer, property, element);
            }
        }

        private bool XmlTypeMatchesPrinterObjectType(XElement element, Type objectType)
        {
            var typeString = element.Attribute("Type")?.Value ?? "";
            var typeFromXml = Type.GetType(typeString, false, false);
            return typeFromXml == objectType;
        }

        private void ApplyPropertyToObject(object instance, PropertyInfo property, XElement element)
        {
            var propertyFromXml = element.Attribute(property.Name);
            if (propertyFromXml == null) return;
            var valueFromXml = propertyFromXml.Value;
            var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
            if (typeConverter.CanConvertFrom(valueFromXml.GetType()))
                property.SetValue(instance, typeConverter.ConvertFrom(valueFromXml), null);
        }
    }
}