using System;
using System.Linq;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Tools.Attributes
{
    public class AttributeUsedInAllProtocolsExcept : AttributeUsedInProtocol
    {
        public AttributeUsedInAllProtocolsExcept(params ProtocolType[] exceptions)
            : base(Enum
                .GetValues(typeof(ProtocolType))
                .Cast<ProtocolType>()
                .Except(exceptions)
                .ToArray())
        {
        }
    }
}
