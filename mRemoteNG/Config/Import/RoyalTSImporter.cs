using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class RoyalTSImporter : IConnectionImporter<string>
    {
        public void Import(string filePath, ContainerInfo destinationContainer)
        {
            FileDataProvider dataProvider = new(filePath);
            string fileContent = dataProvider.Load();

            RoyalTSDeserializer deserializer = new();
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(fileContent);

            ContainerInfo importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            Connection.ConnectionInfo[] childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildRange(childrenToAdd);
        }
    }
}
