using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace mRemoteNG.Connection
{
    public interface ConnectionProtocolOptions : IEnumerable
    {
        bool DomainFieldSupported { get; }
        bool UsernameFieldSupported { get; }
        bool PasswordFieldSupported { get; }
        PropertyInfo[] GetSupportedOptions();
    }
}