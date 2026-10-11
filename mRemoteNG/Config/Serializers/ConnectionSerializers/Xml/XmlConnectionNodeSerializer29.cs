using System;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("windows")]
    public class XmlConnectionNodeSerializer29(ICryptographyProvider cryptographyProvider,
                                         SecureString encryptionKey,
                                         SaveFilter saveFilter)
        : XmlConnectionNodeSerializer28(cryptographyProvider, encryptionKey, saveFilter)
    {
        public override Version Version { get; } = new Version(2, 9);

        protected override void SetElementAttributes(XContainer element, ConnectionInfo connectionInfo)
        {
            base.SetElementAttributes(element, connectionInfo);
            element.Add(new XAttribute("TerminalBackend", connectionInfo.TerminalBackend));
        }

        protected override void SetInheritanceAttributes(XContainer element, IInheritable connectionInfo)
        {
            base.SetInheritanceAttributes(element, connectionInfo);

            if (SaveInheritance && connectionInfo.Inheritance.TerminalBackend)
                element.Add(new XAttribute("InheritTerminalBackend", connectionInfo.Inheritance.TerminalBackend.ToString().ToLowerInvariant()));
        }
    }
}
