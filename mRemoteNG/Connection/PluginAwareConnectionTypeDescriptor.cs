using System.Collections.Generic;
using System.ComponentModel;
using mRemoteNG.Plugins;

namespace mRemoteNG.Connection;

internal sealed class PluginAwareConnectionTypeDescriptor(ICustomTypeDescriptor parent, ConnectionInfo connectionInfo)
    : CustomTypeDescriptor(parent)
{
    private readonly ConnectionInfo _connectionInfo = connectionInfo;

    public override PropertyDescriptorCollection GetProperties()
    {
        return GetProperties([]);
    }

    public override PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
    {
        List<PropertyDescriptor> descriptors = [];
        foreach (PropertyDescriptor descriptor in base.GetProperties(attributes))
        {
            descriptors.Add(descriptor);
        }

        foreach (var definition in App.Runtime.PluginService.GetConnectionPropertyDefinitions(_connectionInfo))
        {
            descriptors.Add(new PluginConnectionPropertyDescriptor(definition));
        }

        return new PropertyDescriptorCollection([.. descriptors], true);
    }
}
