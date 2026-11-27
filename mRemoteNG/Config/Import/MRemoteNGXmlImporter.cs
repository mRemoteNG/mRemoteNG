using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using System;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    // ReSharper disable once InconsistentNaming
    public class MRemoteNGXmlImporter : IConnectionImporter<string>
    {
        public void Import(string fileName, ContainerInfo destinationContainer)
        {
            if (fileName == null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Unable to import file. File path is null.");
                return;
            }

            if (!File.Exists(fileName))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    $"Unable to import file. File does not exist. Path: {fileName}");
                return;
            }

            string xmlString = ImportFileReader.ReadText(fileName);
            XmlConnectionsDeserializer xmlConnectionsDeserializer = new();
            Tree.ConnectionTreeModel connectionTreeModel = xmlConnectionsDeserializer.Deserialize(xmlString, true);
            ImportBudget.ValidateTree(connectionTreeModel);

            if (connectionTreeModel?.RootNodes.FirstOrDefault() == null)
                throw new InvalidDataException("Import does not contain a valid connection tree.");

            ContainerInfo rootImportContainer = new() { Name = Path.GetFileNameWithoutExtension(fileName)};
            rootImportContainer.AddChildrenAtomically(connectionTreeModel.RootNodes[0].Children.ToArray());
            destinationContainer.AddChild(rootImportContainer);
        }
    }
}
