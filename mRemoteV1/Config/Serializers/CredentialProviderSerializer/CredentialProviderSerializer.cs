using System;
using System.Collections.Generic;
using System.Xml.Linq;
using mRemoteNG.Credential;


namespace mRemoteNG.Config.Serializers.CredentialProviderSerializer
{
    public class CredentialProviderSerializer
    {
        public string Serialize(IEnumerable<ICredentialRepository> credentialProviderCatalog)
        {
            var xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", ""));
            var rootElement = new XElement(XName.Get("CredentialProviders"));
            xmlDocument.Add(rootElement);
            foreach (var provider in credentialProviderCatalog)
            {
                rootElement.Add(SerializeCredentialProvider(provider));
            }
            var declaration = xmlDocument.Declaration.ToString();
            var documentBody = xmlDocument.ToString();
            return string.Concat(declaration, Environment.NewLine, documentBody);
        }

        private XElement SerializeCredentialProvider(ICredentialRepository provider)
        {
            var element = new XElement(XName.Get("Provider"));
            element.Add(new XAttribute(XName.Get("Id"), provider.Config.Id));
            element.Add(new XAttribute(XName.Get("Name"), provider.Config.Name));
            return element;
        }
    }
}