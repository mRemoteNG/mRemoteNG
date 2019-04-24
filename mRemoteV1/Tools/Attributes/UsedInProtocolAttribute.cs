using System;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Tools.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UsedInProtocolAttribute : Attribute
    {
        public ProtocolType[] SupportedProtocolTypes { get; }

        public UsedInProtocolAttribute(params ProtocolType[] supportedProtocolTypes)
        {
            SupportedProtocolTypes = supportedProtocolTypes;
        }
    }
}
