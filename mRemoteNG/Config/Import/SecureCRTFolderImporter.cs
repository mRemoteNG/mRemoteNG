using mRemoteNG.App;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using System;
using System.IO;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public static class SecureCRTFolderImporter
    {

        /// <summary>
        /// Returns the default SecureCRT sessions folder path, or null if not found.
        /// </summary>
        public static string? GetDefaultSessionsPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sessionsPath = Path.Combine(appData, "VanDyke", "Config", "Sessions");
            return Directory.Exists(sessionsPath) ? sessionsPath : null;
        }

        /// <summary>
        /// Imports all SecureCRT sessions from a folder (recursively) into the destination container.
        /// </summary>
        public static void Import(string folderPath, ContainerInfo destinationContainer)
        {
            if (!Directory.Exists(folderPath))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    $"SecureCRT sessions folder not found: {folderPath}");
                return;
            }

            ContainerInfo rootContainer = new() { Name = "Imported from SecureCRT" };
            ImportFolder(folderPath, rootContainer);

            if (rootContainer.Children.Count > 0)
                destinationContainer.AddChild(rootContainer);
        }

        private static void ImportFolder(string folderPath, ContainerInfo parentContainer)
        {
            // Import .ini session files in this directory
            foreach (string filePath in Directory.GetFiles(folderPath, "*.ini"))
            {
                try
                {
                    string sessionName = Path.GetFileNameWithoutExtension(filePath);

                    // Skip SecureCRT default/template sessions
                    if (sessionName.Equals("Default", StringComparison.OrdinalIgnoreCase) ||
                        sessionName.Equals("Default_LocalShell", StringComparison.OrdinalIgnoreCase) ||
                        sessionName.StartsWith("__", StringComparison.Ordinal))
                        continue;

                    string content = File.ReadAllText(filePath);
                    ConnectionInfo? connectionInfo = SecureCRTIniDeserializer.Deserialize(content, sessionName);
                    if (connectionInfo != null)
                        parentContainer.AddChild(connectionInfo);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(
                        $"Error importing SecureCRT session file: {filePath}", ex);
                }
            }

            // Recurse into subdirectories (SecureCRT uses folders for session groups)
            foreach (string subDir in Directory.GetDirectories(folderPath))
            {
                string dirName = Path.GetFileName(subDir);

                // Skip hidden/internal directories
                if (dirName.StartsWith('.') ||
                    dirName.StartsWith("__", StringComparison.Ordinal))
                    continue;

                ContainerInfo subContainer = new() { Name = dirName };
                ImportFolder(subDir, subContainer);

                // Only add non-empty containers
                if (subContainer.Children.Count > 0)
                    parentContainer.AddChild(subContainer);
            }
        }
    }
}
