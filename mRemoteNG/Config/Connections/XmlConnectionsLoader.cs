using mRemoteNG.Config.DataProviders;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionsLoader : IConnectionsLoader
    {
        private readonly string _connectionFilePath;

        public XmlConnectionsLoader(string connectionFilePath)
        {
            if (string.IsNullOrEmpty(connectionFilePath))
                throw new ArgumentException($"{nameof(connectionFilePath)} cannot be null or empty");

            if (!File.Exists(connectionFilePath))
                throw new FileNotFoundException($"{connectionFilePath} does not exist");

            _connectionFilePath = connectionFilePath;
        }

        public ConnectionTreeModel Load()
        {
            FileDataProvider dataProvider = new(_connectionFilePath);
            string xmlString = dataProvider.Load();
            XmlConnectionsDeserializer deserializer = new(PromptForPassword);

            try
            {
                return deserializer.Deserialize(xmlString);
            }
            catch (XmlException ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    $"Failed to parse XML connection file '{_connectionFilePath}'. Attempting backup recovery.",
                    ex,
                    MessageClass.WarningMsg);

                if (TryRecoverFromBackup(deserializer, out ConnectionTreeModel recoveredTreeModel))
                    return recoveredTreeModel;

                throw;
            }
        }

        private bool TryRecoverFromBackup(XmlConnectionsDeserializer deserializer, out ConnectionTreeModel recoveredTreeModel)
        {
            recoveredTreeModel = null;
            string directoryPath = Path.GetDirectoryName(_connectionFilePath);

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
                return false;

            string fileName = Path.GetFileName(_connectionFilePath);
            string backupPattern = $"{fileName}.*.backup";
            string[] backupFiles = Directory.GetFiles(directoryPath, backupPattern, SearchOption.TopDirectoryOnly)
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .ToArray();

            foreach (string backupFile in backupFiles)
            {
                try
                {
                    FileDataProvider backupDataProvider = new(backupFile);
                    string backupXml = backupDataProvider.Load();
                    ConnectionTreeModel backupTreeModel = deserializer.Deserialize(backupXml);
                    if (backupTreeModel == null)
                        continue;

                    File.Copy(backupFile, _connectionFilePath, overwrite: true);
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Recovered connection file from backup '{backupFile}'.");

                    recoveredTreeModel = backupTreeModel;
                    return true;
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(
                        $"Failed to recover connections from backup '{backupFile}'.",
                        ex,
                        MessageClass.WarningMsg);
                }
            }

            return false;
        }

        private Optional<SecureString> PromptForPassword()
        {
            Optional<SecureString> password = MiscTools.PasswordDialog(Path.GetFileName(_connectionFilePath), false);
            return password;
        }
    }
}
