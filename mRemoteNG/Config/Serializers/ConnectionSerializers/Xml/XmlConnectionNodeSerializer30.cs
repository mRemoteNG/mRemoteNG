using System;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    /// <summary>
    /// The connection nodes are written as in 2.9. What 3.0 changes is the root element: with a
    /// password-derived key, the key is derived once for the whole file, from the salt in the
    /// KdfSalt attribute, so versions that read only up to 2.9 must not try to open it.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("windows")]
    public class XmlConnectionNodeSerializer30(ICryptographyProvider cryptographyProvider,
                                         SecureString encryptionKey,
                                         SaveFilter saveFilter)
        : XmlConnectionNodeSerializer29(cryptographyProvider, encryptionKey, saveFilter)
    {
        public override Version Version { get; } = new Version(3, 0);
    }
}
