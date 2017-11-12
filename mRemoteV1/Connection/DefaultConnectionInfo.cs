using System;
using System.ComponentModel;
using mRemoteNG.App;


namespace mRemoteNG.Connection
{
    public class DefaultConnectionInfo : ConnectionInfo
    {
        public static DefaultConnectionInfo Instance { get; } = new DefaultConnectionInfo();
        private readonly string[] _excludedProperties = { "Parent", "Name", "Hostname", "Port", "Inheritance",
            "OpenConnections", "IsContainer", "IsDefault", "PositionID", "ConstantID", "TreeNode", "IsQuickConnect", "PleaseConnect" };

        private DefaultConnectionInfo()
        {
            IsDefault = true;
        }

        public void LoadFrom<TSource>(TSource sourceInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = a => a;
            var connectionProperties = GetProperties(_excludedProperties);
            foreach (var property in connectionProperties)
            {
                try
                {
                    var propertyFromSource = typeof(TSource).GetProperty(propertyNameMutator(property.Name));
                    if (propertyFromSource == null) continue;
                    var valueFromSource = propertyFromSource.GetValue(sourceInstance, null);
                    var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                    if (typeConverter.CanConvertFrom(valueFromSource.GetType()))
                        property.SetValue(Instance, typeConverter.ConvertFrom(valueFromSource), null);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector?.AddExceptionStackTrace($"Error loading default connectioninfo property {property.Name}", ex);
                }
            }
        }

        public void SaveTo<TDestination>(TDestination destinationInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = (a) => a;
            var inheritanceProperties = GetProperties(_excludedProperties);
            foreach (var property in inheritanceProperties)
            {
                try
                {
                    var propertyFromDestination = typeof(TDestination).GetProperty(propertyNameMutator(property.Name));
                    var localValue = property.GetValue(Instance, null);
                    var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                    if (propertyFromDestination != null && !typeConverter.CanConvertTo(propertyFromDestination.PropertyType)) continue;
                    if (propertyFromDestination == null) continue;
                    var convertedValue = typeConverter.ConvertTo(localValue, propertyFromDestination.PropertyType);
                    propertyFromDestination.SetValue(destinationInstance, convertedValue, null);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector?.AddExceptionStackTrace($"Error saving default connectioninfo property {property.Name}", ex);
                }
            }
        }
    }
}