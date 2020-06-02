using System;
using System.Linq;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Tools.Attributes
{
    public class UsedInAllProtocolsExceptAttribute : UsedInProtocolAttribute
    {
        public UsedInAllProtocolsExceptAttribute(params ProtocolType[] exceptions)
            : base(Enum
                .GetValues(typeof(ProtocolType))
                .Cast<ProtocolType>()
                .Except(exceptions)
                .ToArray())
        {
        }
    }
}
