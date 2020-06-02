namespace mRemoteNG.Tools.WindowsRegistry
{
    public interface IRegistry
    {
        Optional<string> GetKeyValue(RegistryHive hive, string keyPath, string propertyName);
        string[] GetSubKeyNames(RegistryHive hive, string keyPath);
    }
}