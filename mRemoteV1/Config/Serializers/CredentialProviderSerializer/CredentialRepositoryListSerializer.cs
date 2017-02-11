using System;
using System.Collections.Generic;
using System.Xml.Linq;
using mRemoteNG.Credential;


namespace mRemoteNG.Config.Serializers.CredentialProviderSerializer
{
    public class CredentialRepositoryListSerializer
    {
        public string Serialize(IEnumerable<ICredentialRepository> credentialProviderCatalog)
        {
            var xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var rootElement = new XElement(XName.Get("CredentialRepositories"));
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
            return new XElement("CredentialRepository",
                new XAttribute(XName.Get("Id"), provider.Config.Id),
                new XAttribute(XName.Get("TypeName"), provider.Config.TypeName),
                new XAttribute(XName.Get("Source"), provider.Config.Source)
            );
        }
    }
}