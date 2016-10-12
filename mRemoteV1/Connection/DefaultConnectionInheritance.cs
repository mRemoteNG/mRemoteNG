using System;

namespace mRemoteNG.Connection
{
    public class DefaultConnectionInheritance : ConnectionInfoInheritance
    {
        static DefaultConnectionInheritance()
        {
        }

        private DefaultConnectionInheritance() : base(null, true)
        {
        }

        public static DefaultConnectionInheritance Instance { get; } = new DefaultConnectionInheritance();


        public void LoadFrom<TSource>(TSource sourceInstance, Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = a => a;
            var inheritanceProperties = GetProperties();
            foreach (var property in inheritanceProperties)
            {
                var propertyFromSettings = typeof(TSource).GetProperty(propertyNameMutator(property.Name));
                var valueFromSettings = propertyFromSettings.GetValue(sourceInstance, null);
                property.SetValue(Instance, valueFromSettings, null);
            }
        }

        public void SaveTo<TDestination>(TDestination destinationInstance,
            Func<string, string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = a => a;
            var inheritanceProperties = GetProperties();
            foreach (var property in inheritanceProperties)
            {
                var propertyFromSettings = typeof(TDestination).GetProperty(propertyNameMutator(property.Name));
                var localValue = property.GetValue(Instance, null);
                propertyFromSettings.SetValue(destinationInstance, localValue, null);
            }
        }
    }
}