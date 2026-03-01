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
        private readonly MessageCollector _messageCollector;
        private readonly Func<string, Optional<SecureString>> _passwordRequestor;

        public XmlConnectionsLoader(string connectionFilePath, MessageCollector? messageCollector = null)
            : this(connectionFilePath, messageCollector,
                   (fileNameToPrompt) => MiscTools.PasswordDialog(Path.GetFileName(connectionFilePath), false))
        {
        }

        public XmlConnectionsLoader(string connectionFilePath, MessageCollector? messageCollector,
                                    Func<string, Optional<SecureString>> passwordRequestor)
        {
            if (string.IsNullOrEmpty(connectionFilePath))
                throw new ArgumentException($"{nameof(connectionFilePath)} cannot be null or empty", nameof(connectionFilePath));

            if (!File.Exists(connectionFilePath))
                throw new FileNotFoundException($"{connectionFilePath} does not exist");

            _connectionFilePath = connectionFilePath;
            _messageCollector = messageCollector ?? Runtime.MessageCollector;
            _passwordRequestor = passwordRequestor;
        }

        public ConnectionTreeModel Load()
        {
            FileDataProvider dataProvider = new(_connectionFilePath);
            string xmlString = dataProvider.Load();
            XmlConnectionsDeserializer deserializer = new(_connectionFilePath, () => PromptForPassword());

            // An empty file may indicate crash-time corruption (#1395).
            // Deserialize() returns null silently for empty input, so we must intercept here
            // before backup recovery is bypassed.
            if (string.IsNullOrWhiteSpace(xmlString))
            {
                _messageCollector.AddMessage(MessageClass.WarningMsg,
                    $"Connection file '{_connectionFilePath}' is empty. Attempting backup recovery.");

                if (TryRecoverFromBackup(deserializer, out ConnectionTreeModel? recovered))
                    return recovered!;

                throw new XmlException($"Connection file '{_connectionFilePath}' is empty and no valid backup was found.");
            }

            try
            {
                return deserializer.Deserialize(xmlString);
            }
            catch (Exception ex) when (ex is XmlException or FormatException or InvalidOperationException or NotSupportedException)
            {
                _messageCollector.AddExceptionMessage(
                    $"Failed to parse XML connection file '{_connectionFilePath}'. Attempting backup recovery.",
                    ex,
                    MessageClass.WarningMsg);

                if (TryRecoverFromBackup(deserializer, out ConnectionTreeModel? recoveredTreeModel))
                    return recoveredTreeModel!;

                throw;
            }
        }

        private bool TryRecoverFromBackup(XmlConnectionsDeserializer deserializer, out ConnectionTreeModel? recoveredTreeModel)
        {
            recoveredTreeModel = null;
            string? directoryPath = Path.GetDirectoryName(_connectionFilePath);

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
                    _messageCollector.AddMessage(MessageClass.WarningMsg, $"Recovered connection file from backup '{backupFile}'.");

                    recoveredTreeModel = backupTreeModel;
                    return true;
                }
                catch (Exception ex)
                {
                    _messageCollector.AddExceptionMessage(
                        $"Failed to recover connections from backup '{backupFile}'.",
                        ex,
                        MessageClass.WarningMsg);
                }
            }

            return false;
        }

        private Optional<SecureString> PromptForPassword()
        {
            return _passwordRequestor(Path.GetFileName(_connectionFilePath));
        }
    }
}
