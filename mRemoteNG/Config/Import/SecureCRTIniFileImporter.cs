using mRemoteNG.App;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using System.IO;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class SecureCRTIniFileImporter : IConnectionImporter<string>
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

            string content = File.ReadAllText(fileName);
            string sessionName = Path.GetFileNameWithoutExtension(fileName);

            ConnectionInfo? connectionInfo = SecureCRTIniDeserializer.Deserialize(content, sessionName);
            if (connectionInfo != null)
                destinationContainer.AddChild(connectionInfo);
        }
    }
}
