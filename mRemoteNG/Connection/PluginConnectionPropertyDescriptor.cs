using System;
using System.ComponentModel;
using System.Globalization;
using mRemoteNG.PluginContracts;
using mRemoteNG.Plugins;

namespace mRemoteNG.Connection;

internal sealed class PluginConnectionPropertyDescriptor(ConnectionPropertyDefinition definition)
    : PropertyDescriptor(definition.Key, BuildAttributes(definition))
{
    public ConnectionPropertyDefinition Definition { get; } = definition;

    public override string Category => Definition.Category;

    public override Type ComponentType => typeof(ConnectionInfo);

    public override string Description => Definition.Description;

    public override string DisplayName => Definition.DisplayName;

    public override bool IsReadOnly => false;

    public override Type PropertyType => Definition.PropertyType switch
    {
        PluginPropertyType.Boolean => typeof(bool),
        PluginPropertyType.Integer => typeof(int),
        _ => typeof(string),
    };

    public override bool CanResetValue(object component)
    {
        return component is ConnectionInfo connectionInfo &&
               !string.IsNullOrEmpty(connectionInfo.GetPluginProperty(Definition.Key));
    }

    public override object GetValue(object component)
    {
        ConnectionInfo connectionInfo = (ConnectionInfo)component;
        string value = connectionInfo.GetPluginProperty(Definition.Key, Definition.DefaultValue);

        return Definition.PropertyType switch
        {
            PluginPropertyType.Boolean => bool.TryParse(value, out bool boolValue) && boolValue,
            PluginPropertyType.Integer => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue) ? intValue : 0,
            _ => value,
        };
    }

    public override void ResetValue(object component)
    {
        ((ConnectionInfo)component).SetPluginProperty(Definition.Key, null);
    }

    public override void SetValue(object component, object value)
    {
        ConnectionInfo connectionInfo = (ConnectionInfo)component;
        string serializedValue = value switch
        {
            null => string.Empty,
            bool boolValue => boolValue.ToString(),
            int intValue => intValue.ToString(CultureInfo.InvariantCulture),
            _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty,
        };

        connectionInfo.SetPluginProperty(Definition.Key, serializedValue);
    }

    public override bool ShouldSerializeValue(object component)
    {
        return component is ConnectionInfo connectionInfo &&
               connectionInfo.TryGetPluginProperty(Definition.Key, out _);
    }

    internal bool IsValidForProtocol(ProtocolType protocolType)
    {
        return PluginProtocolMapper.SupportsProtocol(Definition, protocolType);
    }

    private static Attribute[] BuildAttributes(ConnectionPropertyDefinition definition)
    {
        List<Attribute> attributes = [new BrowsableAttribute(true)];
        if (definition.PropertyType == PluginPropertyType.Boolean)
        {
            attributes.Add(new TypeConverterAttribute(typeof(Tools.MiscTools.YesNoTypeConverter)));
        }

        return [.. attributes];
    }
}
