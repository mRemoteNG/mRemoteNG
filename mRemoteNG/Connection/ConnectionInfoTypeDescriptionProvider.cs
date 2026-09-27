using System.ComponentModel;

namespace mRemoteNG.Connection;

public sealed class ConnectionInfoTypeDescriptionProvider : TypeDescriptionProvider
{
    private static readonly TypeDescriptionProvider DefaultProvider = TypeDescriptor.GetProvider(typeof(ConnectionInfo));

    public ConnectionInfoTypeDescriptionProvider()
        : base(DefaultProvider)
    {
    }

    public override ICustomTypeDescriptor GetTypeDescriptor(System.Type objectType, object? instance)
    {
        ICustomTypeDescriptor baseDescriptor = base.GetTypeDescriptor(objectType, instance);
        return instance is ConnectionInfo connectionInfo
            ? new PluginAwareConnectionTypeDescriptor(baseDescriptor, connectionInfo)
            : baseDescriptor;
    }
}
