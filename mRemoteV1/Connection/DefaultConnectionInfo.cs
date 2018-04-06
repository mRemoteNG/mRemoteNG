using System;
using System.Configuration;
using mRemoteNG.App;


namespace mRemoteNG.Connection
{
	public class DefaultConnectionInfo : ConnectionInfo
    {
        public static DefaultConnectionInfo Instance { get; } = new DefaultConnectionInfo();

        private DefaultConnectionInfo()
        {
            IsDefault = true;
	        Inheritance = DefaultConnectionInheritance.Instance;
        }

        public void LoadFrom<TSource>(TSource sourceInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null)
	            propertyNameMutator = a => a;

            var connectionProperties = GetSerializableProperties();
            foreach (var property in connectionProperties)
            {
                try
                {
	                var expectedPropertyName = propertyNameMutator(property.Name);
					var propertyFromSource = typeof(TSource).GetProperty(expectedPropertyName);
                    if (propertyFromSource == null)
						throw new SettingsPropertyNotFoundException($"No property with name '{expectedPropertyName}' found.");

					var valueFromSource = propertyFromSource.GetValue(sourceInstance, null);
	                var value = Convert.ChangeType(valueFromSource, property.PropertyType);

                    property.SetValue(Instance, value, null);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector?.AddExceptionStackTrace($"Error loading default connectioninfo property {property.Name}", ex);
                }
            }
        }

        public void SaveTo<TDestination>(TDestination destinationInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null)
	            propertyNameMutator = (a) => a;

            var connectionProperties = GetSerializableProperties();

            foreach (var property in connectionProperties)
            {
                try
                {
	                var expectedPropertyName = propertyNameMutator(property.Name);
					var propertyFromDestination = typeof(TDestination).GetProperty(expectedPropertyName);

	                if (propertyFromDestination == null)
		                throw new SettingsPropertyNotFoundException($"No property with name '{expectedPropertyName}' found.");

					// ensure value is of correct type
	                var value = Convert.ChangeType(property.GetValue(Instance, null), propertyFromDestination.PropertyType);

                    propertyFromDestination.SetValue(destinationInstance, value, null);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector?.AddExceptionStackTrace($"Error saving default connectioninfo property {property.Name}", ex);
                }
            }
        }
    }
}