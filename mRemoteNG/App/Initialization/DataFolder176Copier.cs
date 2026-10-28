using System;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;

namespace mRemoteNG.App.Initialization
{
    /// <summary>
    /// mRemoteNG 1.76 kept its connections, external tools, panel layout and themes in
    /// %APPDATA%\mRemoteNG. Later versions use %APPDATA%\&lt;product name&gt;, which is now
    /// "mRemoteNG Connection Manager", and nothing looked in the old folder: after an upgrade
    /// the connection tree came up empty. On a start that finds no connections file in the
    /// new folder, the old folder's files are copied across. The old folder is left as it is,
    /// so 1.76 keeps working with its own copy.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class DataFolder176Copier
    {
        public static void CopyIfNeeded(MessageCollector messageCollector)
        {
            if (Runtime.IsPortableEdition) return;

            string oldFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mRemoteNG");
            try
            {
                int copied = Copy(oldFolder, SettingsFileInfo.SettingsPath);
                if (copied > 0)
                    messageCollector.AddMessage(MessageClass.InformationMsg,
                        $"Copied {copied} files of mRemoteNG 1.76 from {oldFolder} to {SettingsFileInfo.SettingsPath}", true);
            }
            catch (Exception ex)
            {
                messageCollector.AddExceptionMessage($"Copying the files of mRemoteNG 1.76 from {oldFolder} failed", ex);
            }
        }

        /// <summary>
        /// Copies every file of <paramref name="oldFolder"/>, log files aside, that
        /// <paramref name="newFolder"/> does not have yet; only when the old folder has a
        /// connections file and the new one does not.
        /// </summary>
        /// <returns>The number of files copied.</returns>
        internal static int Copy(string oldFolder, string newFolder)
        {
            if (!File.Exists(Path.Combine(oldFolder, ConnectionsFileInfo.DefaultConnectionsFile))) return 0;
            // Also true when both are the same folder.
            if (File.Exists(Path.Combine(newFolder, ConnectionsFileInfo.DefaultConnectionsFile))) return 0;

            int copied = 0;
            foreach (string file in Directory.EnumerateFiles(oldFolder, "*", SearchOption.AllDirectories))
            {
                if (string.Equals(Path.GetExtension(file), ".log", StringComparison.OrdinalIgnoreCase)) continue;

                string target = Path.Combine(newFolder, Path.GetRelativePath(oldFolder, file));
                if (File.Exists(target)) continue;

                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.Copy(file, target);
                copied++;
            }

            return copied;
        }
    }
}
