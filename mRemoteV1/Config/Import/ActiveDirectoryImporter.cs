using System;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
	public class ActiveDirectoryImporter : IConnectionImporter
	{
        public void Import(object ldapPath, ContainerInfo destinationContainer)
        {
            var ldapPathAsString = ldapPath as string;
            if (ldapPathAsString == null) return;
            Import(ldapPathAsString, destinationContainer);
        }

        public void Import(string ldapPath, ContainerInfo destinationContainer)
		{
			try
			{
				var deserializer = new ActiveDirectoryDeserializer(ldapPath);
			    var connectionTreeModel = deserializer.Deserialize();
                var importedRootNode = connectionTreeModel.RootNodes.First();
                if (importedRootNode == null) return;
                var childrenToAdd = importedRootNode.Children.ToArray();
                destinationContainer.AddChildRange(childrenToAdd);
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.Import() failed.", ex, logOnly: true);
			}
		}
	}
}