using System;


namespace mRemoteNG.Connection
{
    public class DefaultConnectionInfo : ConnectionInfo
    {
        public static DefaultConnectionInfo Instance { get; } = new DefaultConnectionInfo();
        private readonly string[] _excludedProperties = { "Parent" };

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
                var propertyFromSettings = typeof(TSource).GetProperty(propertyNameMutator(property.Name));
                var valueFromSettings = propertyFromSettings.GetValue(sourceInstance, null);
                property.SetValue(Instance, valueFromSettings, null);
            }
        }

        public void SaveTo<TDestination>(TDestination destinationInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = (a) => a;
            var inheritanceProperties = GetProperties(_excludedProperties);
            foreach (var property in inheritanceProperties)
            {
                var propertyFromSettings = typeof(TDestination).GetProperty(propertyNameMutator(property.Name));
                var localValue = property.GetValue(Instance, null);
                propertyFromSettings.SetValue(destinationInstance, localValue, null);
            }
        }
    }
}