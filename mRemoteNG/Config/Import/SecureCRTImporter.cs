using mRemoteNG.App;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class SecureCRTImporter : IConnectionImporter<string>
    {
        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            if (fileName == null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Unable to import file. File path is null.");
                return;
            }

            if (!File.Exists(fileName))
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    $"Unable to import file. File does not exist. Path: {fileName}");


            string content = ImportFileReader.ReadText(fileName);
            SecureCRTFileDeserializer deserializer = new();
            ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(content);
            ImportBudget.ValidateTree(connectionTreeModel);

            ContainerInfo rootImportContainer = new() { Name = Path.GetFileNameWithoutExtension(fileName) };
            rootImportContainer.AddChildrenAtomically(connectionTreeModel.RootNodes[0].Children.ToArray());
            destinationContainer.AddChild(rootImportContainer);
        }
    }


}
