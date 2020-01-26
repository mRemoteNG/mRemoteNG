using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;

namespace mRemoteNG.Config.Import
{
    /// <summary>
    /// Imports connections from Devolutions *.rdm files.
    /// </summary>
    public class RemoteDesktopManagerImporter : IConnectionImporter<string>
    {
        public void Import(string source, ContainerInfo destinationContainer)
        {
            var deserializer = new RemoteDesktopManagerDeserializer();
            var tree = deserializer.Deserialize(source);
            destinationContainer.AddChildRange(tree.RootNodes[0].Children);
        }
    }
}
