using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class DebugDumper
    {
        public static void CreateDebugBundle()
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Zip files (*.zip)|*.zip";
                sfd.FileName = $"mRemoteNG_Debug_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    CreateDebugBundle(sfd.FileName);
                    MessageBox.Show("Debug bundle created successfully!", "Debug Bundle", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to create debug bundle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void CreateDebugBundle(string destinationPath)
        {
            using (var archive = ZipFile.Open(destinationPath, ZipArchiveMode.Create))
            {
                AddSystemInfo(archive);
                AddLogFile(archive);
                AddConfigFile(archive);
            }
        }

        private static void AddSystemInfo(ZipArchive archive)
        {
            var sb = new StringBuilder();
            sb.AppendLine(CultureInfo.InvariantCulture, $"mRemoteNG Version: {GeneralAppInfo.ApplicationVersion}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"OS Version: {Environment.OSVersion}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"64-bit OS: {Environment.Is64BitOperatingSystem}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"64-bit Process: {Environment.Is64BitProcess}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"CLR Version: {Environment.Version}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"Current Culture: {System.Globalization.CultureInfo.CurrentCulture.Name}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"Portable Edition: {Runtime.IsPortableEdition}");
            
            var entry = archive.CreateEntry("SystemInfo.txt");
            using (var entryStream = entry.Open())
            using (var writer = new StreamWriter(entryStream))
            {
                writer.Write(sb.ToString());
            }
        }

        private static void AddLogFile(ZipArchive archive)
        {
             // Log path is typically %APPDATA%\mRemoteNG\mRemoteNG.log
             string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mRemoteNG", "mRemoteNG.log");
             
             if (Runtime.IsPortableEdition)
             {
                 string portableLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mRemoteNG.log");
                 if (File.Exists(portableLog)) logPath = portableLog;
             }

             if (File.Exists(logPath))
             {
                 try {
                     var entry = archive.CreateEntry("mRemoteNG.log");
                     using (var fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                     using (var entryStream = entry.Open())
                     {
                         fs.CopyTo(entryStream);
                     }
                 } catch (Exception ex) {
                     var entry = archive.CreateEntry("mRemoteNG.log.error.txt");
                     using (var entryStream = entry.Open())
                     using (var writer = new StreamWriter(entryStream))
                     {
                         writer.Write($"Could not read log file: {ex.Message}");
                     }
                 }
             }
             else
             {
                 var entry = archive.CreateEntry("mRemoteNG.log.missing.txt");
                 using (var entryStream = entry.Open())
                 using (var writer = new StreamWriter(entryStream))
                 {
                     writer.Write($"Log file not found at: {logPath}");
                 }
             }
        }

        private static void AddConfigFile(ZipArchive archive)
        {
             string configPath = "";
             
             // Try to find the loaded connection file path from properties
             try {
                if (!string.IsNullOrWhiteSpace(Properties.OptionsConnectionsPage.Default.ConnectionFilePath))
                {
                    configPath = Properties.OptionsConnectionsPage.Default.ConnectionFilePath;
                }
             } catch {}

             if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
             {
                 // Fallback to default
                 configPath = Path.Combine(SettingsFileInfo.SettingsPath, ConnectionsFileInfo.DefaultConnectionsFile);
             }

             if (File.Exists(configPath))
             {
                 try {
                     string content = File.ReadAllText(configPath);
                     // Sanitize - remove credentials from XML before archiving
                     var attrName = nameof(AbstractConnectionRecord.Password); // NOSONAR — S2068 false positive: not a credential
                     var attrPattern = $"{Regex.Escape(attrName)}=\"[^\"]*\"";
                     var sanitized = $"{attrName}=\"***REMOVED***\""; // NOSONAR — S2068 false positive: sanitization replacement, not a credential
                     content = Regex.Replace(content, attrPattern, sanitized, RegexOptions.CultureInvariant);
                     
                     var entry = archive.CreateEntry("confCons.xml");
                     using (var entryStream = entry.Open())
                     using (var writer = new StreamWriter(entryStream))
                     {
                         writer.Write(content);
                     }
                 } catch (Exception ex) {
                     var entry = archive.CreateEntry("confCons.xml.error.txt");
                     using (var entryStream = entry.Open())
                     using (var writer = new StreamWriter(entryStream))
                     {
                         writer.Write($"Could not read/sanitize config file: {ex.Message}");
                     }
                 }
             }
             else
             {
                 var entry = archive.CreateEntry("confCons.xml.missing.txt");
                 using (var entryStream = entry.Open())
                 using (var writer = new StreamWriter(entryStream))
                 {
                     writer.Write($"Config file not found at: {configPath}");
                 }
             }
        }
    }
}
