using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.CredentialSerializer
{
    public class XmlCredentialRecordSerializer : ISerializer<IEnumerable<ICredentialRecord>, string>
    {
        public string SchemaVersion { get; } = "1.0";

        public string Serialize(IEnumerable<ICredentialRecord> credentialRecords)
        {
            var xdoc = new XDocument(
                new XElement("Credentials",
                    new XAttribute("SchemaVersion", SchemaVersion),
                    from r in credentialRecords
                    select new XElement("Credential",
                        new XAttribute("Id", r.Id),
                        new XAttribute("Title", r.Title),
                        new XAttribute("Username", r.Username),
                        new XAttribute("Domain", r.Domain),
                        new XAttribute("Password", r.Password.ConvertToUnsecureString())
                    )
                )
            )
            {
                Declaration = new XDeclaration("1.0", "utf-8", null)
            };
            return xdoc.Declaration + Environment.NewLine + xdoc;
        }
    }
}