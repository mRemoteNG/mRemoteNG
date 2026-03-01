using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using mRemoteNG.App;
using mRemoteNG.Config.Import;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using System.Runtime.Versioning;

namespace mRemoteNG.Container
{
    [SupportedOSPlatform("windows")]
    public class DynamicFolderManager
    {
        private readonly Dictionary<string, Timer> _timers = new(StringComparer.Ordinal);

        public DynamicFolderManager()
        {
            Runtime.ConnectionsService.ConnectionsLoaded += OnConnectionsLoaded;
        }

        private void OnConnectionsLoaded(object sender, Config.Connections.ConnectionsLoadedEventArgs e)
        {
            StopAllTimers();
            if (e.NewConnectionTreeModel != null)
            {
                ScanAndSchedule(e.NewConnectionTreeModel.RootNodes);
            }
        }

        private void StopAllTimers()
        {
            foreach (var timer in _timers.Values)
            {
                timer.Stop();
                timer.Dispose();
            }
            _timers.Clear();
        }

        private void ScanAndSchedule(IEnumerable<ConnectionInfo> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is ContainerInfo container)
                {
                    if (container.DynamicSource != DynamicSourceType.None)
                    {
                        ScheduleRefresh(container);
                    }
                    ScanAndSchedule(container.Children);
                }
            }
        }

        public void ScheduleRefresh(ContainerInfo container)
        {
            if (_timers.TryGetValue(container.ConstantID, out var existingTimer))
            {
                existingTimer.Stop();
                existingTimer.Dispose();
                _timers.Remove(container.ConstantID);
            }

            if (container.DynamicRefreshInterval > 0)
            {
                Timer timer = new Timer(container.DynamicRefreshInterval * 60 * 1000); // Minutes to ms
                timer.Elapsed += (s, e) => RefreshFolder(container);
                timer.AutoReset = true;
                timer.Start();
                _timers[container.ConstantID] = timer;
            }
        }
        
        public void UnscheduleRefresh(ContainerInfo container)
        {
            if (_timers.TryGetValue(container.ConstantID, out var timerToRemove))
            {
                timerToRemove.Stop();
                timerToRemove.Dispose();
                _timers.Remove(container.ConstantID);
            }
        }

        public static void RefreshFolder(ContainerInfo container)
        {
            try
            {
                if (container.DynamicSource != DynamicSourceType.None)
                {
                    if (mRemoteNG.UI.Forms.FrmMain.Default?.InvokeRequired == true)
                    {
                        mRemoteNG.UI.Forms.FrmMain.Default.Invoke(new Action(() => RefreshFolderInternal(container)));
                    }
                    else
                    {
                        RefreshFolderInternal(container);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Error refreshing dynamic folder {container.Name}", ex);
            }
        }

        private static void RefreshFolderInternal(ContainerInfo container)
        {
            try 
            {
                var childrenToRemove = container.Children.ToList();
                container.RemoveChildRange(childrenToRemove);
                
                switch (container.DynamicSource)
                {
                    case DynamicSourceType.ActiveDirectory:
                        // Assuming true for recursive import
                        ActiveDirectoryImporter.Import(container.DynamicSourceValue, container, true);
                        break;
                    case DynamicSourceType.File:
                        ImportFromFile(container);
                        break;
                    case DynamicSourceType.Script:
                        ImportFromScript(container);
                        break;
                }
                
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Dynamic folder '{container.Name}' refreshed.");
            }
            catch (Exception ex)
            {
                 Runtime.MessageCollector.AddExceptionMessage($"Error executing refresh for {container.Name}", ex);
            }
        }

        private static void ImportFromFile(ContainerInfo container)
        {
            string filePath = container.DynamicSourceValue;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Dynamic folder '{container.Name}': File path is empty.");
                return;
            }

            if (!File.Exists(filePath))
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, $"Dynamic folder '{container.Name}': File '{filePath}' not found.");
                return;
            }

            try
            {
                string xmlContent = File.ReadAllText(filePath);
                ImportXml(xmlContent, container, filePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read file '{filePath}'", ex);
            }
        }

        private static void ImportFromScript(ContainerInfo container)
        {
            string scriptPath = container.DynamicSourceValue;
            if (string.IsNullOrWhiteSpace(scriptPath))
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Dynamic folder '{container.Name}': Script path is empty.");
                return;
            }

            try
            {
                string output = ExecuteScript(scriptPath);
                if (!string.IsNullOrWhiteSpace(output))
                {
                    ImportXml(output, container, "ScriptOutput");
                }
                else
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Dynamic folder '{container.Name}': Script returned no output.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to execute script '{scriptPath}'", ex);
            }
        }

        private static string ExecuteScript(string scriptPath)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (scriptPath.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase))
            {
                startInfo.FileName = GetSystemExecutablePath(Path.Combine("WindowsPowerShell", "v1.0", "powershell.exe"));
                startInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"";
            }
            else if (scriptPath.EndsWith(".bat", StringComparison.OrdinalIgnoreCase) || scriptPath.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase))
            {
                startInfo.FileName = GetSystemExecutablePath("cmd.exe");
                startInfo.Arguments = $"/c \"{scriptPath}\"";
            }
            else
            {
                startInfo.FileName = scriptPath;
            }

            using var process = new Process { StartInfo = startInfo };
            process.Start();

            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            Task.WaitAll(outputTask, errorTask);
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                 string error = errorTask.Result;
                 if (!string.IsNullOrWhiteSpace(error))
                     throw new InvalidOperationException($"Script exited with code {process.ExitCode}: {error}");

                 throw new InvalidOperationException($"Script exited with code {process.ExitCode}");
            }

            return outputTask.Result;
        }

        private static string GetSystemExecutablePath(string relativeExecutablePath)
        {
            string systemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
            if (string.IsNullOrWhiteSpace(systemDirectory))
                throw new InvalidOperationException("System directory could not be resolved.");

            string fullPath = Path.Combine(systemDirectory, relativeExecutablePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Executable not found in system directory: {fullPath}", fullPath);

            return fullPath;
        }

        private static void ImportXml(string xmlContent, ContainerInfo container, string sourceName)
        {
            var deserializer = new XmlConnectionsDeserializer(sourceName);
            var tree = deserializer.Deserialize(xmlContent, true);

            if (tree != null)
            {
                foreach (var rootNode in tree.RootNodes)
                {
                    container.AddChildRange(rootNode.Children.ToList());
                }
            }
        }
    }
}
