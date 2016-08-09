using System;


namespace mRemoteNG.Connection
{
    public class DefaultConnectionInheritance : ConnectionInfoInheritance
    {
        private const string SettingNamePrefix = "InhDefault";

        public static DefaultConnectionInheritance Instance { get; } = new DefaultConnectionInheritance();

        private DefaultConnectionInheritance() : base(null)
        {
        }

        static DefaultConnectionInheritance()
        { }


        public void LoadFrom<TSource>(TSource sourceInstance, Func<string,string> propertyNameMutator = null)
        {
            if (propertyNameMutator == null) propertyNameMutator = (a) => a;
            var inheritanceProperties = GetProperties();
            foreach (var property in inheritanceProperties)
            {
                var propertyFromSettings = typeof(TSource).GetProperty(propertyNameMutator(property.Name));
                var valueFromSettings = propertyFromSettings.GetValue(sourceInstance, null);
                property.SetValue(Instance, valueFromSettings, null);
            }
        }

        public void SaveToSettings()
        {
            var inheritanceProperties = GetProperties();
            foreach (var property in inheritanceProperties)
            {
                var propertyFromSettings = typeof(Settings).GetProperty($"{SettingNamePrefix}{property.Name}");
                var localValue = property.GetValue(Instance, null);
                propertyFromSettings.SetValue(Settings.Default, localValue, null);
            }
        }
    }
}