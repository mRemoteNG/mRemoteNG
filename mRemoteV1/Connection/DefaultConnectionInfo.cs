using System;
using System.ComponentModel;


namespace mRemoteNG.Connection
{
    public class DefaultConnectionInfo : ConnectionInfo
    {
        public static DefaultConnectionInfo Instance { get; } = new DefaultConnectionInfo();
        private readonly string[] _excludedProperties = { "Parent", "Name", "Panel", "Hostname", "Port", "Inheritance",
            "OpenConnections", "IsContainer", "IsDefault", "PositionID", "ConstantID", "TreeNode", "IsQuickConnect", "PleaseConnect" };

        private DefaultConnectionInfo()
        {
            IsDefault = true;
        }

        public void LoadFrom<TSource>(TSource sourceInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = (a) => a;
            var connectionProperties = GetProperties(_excludedProperties);
            foreach (var property in connectionProperties)
            {
                var propertyFromSource = typeof(TSource).GetProperty(propertyNameMutator(property.Name));
                var valueFromSource = propertyFromSource.GetValue(sourceInstance, null);
                
                var descriptor = TypeDescriptor.GetProperties(Instance)[property.Name];
                var converter = descriptor.Converter;
                if (converter != null && converter.CanConvertFrom(valueFromSource.GetType()))
                    property.SetValue(Instance, converter.ConvertFrom(valueFromSource), null);
                else
                    property.SetValue(Instance, valueFromSource, null);
            }
        }

        public void SaveTo<TDestination>(TDestination destinationInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = (a) => a;
            var inheritanceProperties = GetProperties(_excludedProperties);
            foreach (var property in inheritanceProperties)
            {
                var propertyFromDestination = typeof(TDestination).GetProperty(propertyNameMutator(property.Name));
                var localValue = property.GetValue(Instance, null);

                var descriptor = TypeDescriptor.GetProperties(Instance)[property.Name];
                var converter = descriptor.Converter;
                if (converter != null && converter.CanConvertFrom(localValue.GetType()))
                    propertyFromDestination.SetValue(destinationInstance, converter.ConvertFrom(localValue), null);
                else
                    propertyFromDestination.SetValue(destinationInstance, localValue, null);
            }
        }
    }
}