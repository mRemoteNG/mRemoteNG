using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;


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

            FileDataProvider dataProvider = new(fileName);
            string xmlString = dataProvider.Load();
            XmlConnectionsDeserializer xmlConnectionsDeserializer = new()
            {
                AuthenticationRequestor = RequestPassword
            };
            Tree.ConnectionTreeModel? connectionTreeModel = xmlConnectionsDeserializer.Deserialize(xmlString, true);

            if (connectionTreeModel == null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    $"Unable to import file. Deserialization returned null. Path: {fileName}");
                return;
            }

            ContainerInfo rootImportContainer = new() { Name = Path.GetFileNameWithoutExtension(fileName)};
            rootImportContainer.AddChildRange(connectionTreeModel.RootNodes.First().Children.ToArray());
            destinationContainer.AddChild(rootImportContainer);
        }

        private Optional<SecureString> RequestPassword()
        {
            using (FrmInputBox input = new("Password Required", "Please enter the password for the encrypted file:", "", true))
            {
                if (input.ShowDialog() == DialogResult.OK)
                {
                    return (input.returnValue ?? string.Empty).ConvertToSecureString();
                }
            }
            return Optional<SecureString>.Empty;
        }
    }
}
