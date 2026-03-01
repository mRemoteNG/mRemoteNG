using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Putty
{
    [SupportedOSPlatform("windows")]
    public class PuttySessionsFileProvider : AbstractPuttySessionsProvider
    {
        private FileSystemWatcher? _fileWatcher;

        private static string? GetSessionsDirectory()
        {
            // We re-evaluate this every time because PuttyPath might change
            string? puttyPath = PuttyBase.PuttyPath;
            if (string.IsNullOrEmpty(puttyPath)) return null;

            string? puttyDir = Path.GetDirectoryName(puttyPath);
            if (string.IsNullOrEmpty(puttyDir)) return null;

            // KiTTY portable sessions are in "Sessions" folder next to executable
            string sessionsDir = Path.Combine(puttyDir, "Sessions");
            return Directory.Exists(sessionsDir) ? sessionsDir : null;
        }

        public override string[] GetSessionNames(bool raw = false)
        {
            string? sessionsDir = GetSessionsDirectory();
            if (sessionsDir == null) return Array.Empty<string>();

            try
            {
                // KiTTY stores sessions as files. Filename is the session name (URL encoded).
                string[] files = Directory.GetFiles(sessionsDir);
                List<string> sessionNames = new();

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    // Filter out non-session files if any? KiTTY usually has clean Sessions folder.
                    // Decode if raw=false
                    sessionNames.Add(raw ? fileName : PuttySessionNameDecoder.Decode(fileName));
                }

                // Note: KiTTY might have "Default%20Settings" file for defaults.
                // PuttySessionsRegistryProvider handles this specially for sorting, but here we just list them.
                
                return sessionNames.ToArray();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to list KiTTY sessions.", ex, MessageClass.WarningMsg);
                return Array.Empty<string>();
            }
        }

        public override PuttySessionInfo? GetSession(string sessionName)
        {
            if (string.IsNullOrEmpty(sessionName)) return null;

            string? sessionsDir = GetSessionsDirectory();
            if (sessionsDir == null) return null;

            // sessionName here is RAW filename (URL encoded) as returned by GetSessionNames(true)
            string filePath = Path.Combine(sessionsDir, sessionName);
            if (!File.Exists(filePath)) return null;

            Dictionary<string, string> sessionData = new(StringComparer.Ordinal);
            try
            {
                foreach (string line in File.ReadAllLines(filePath))
                {
                    // Format: Key=Value
                    // Some values might contain =, so we only split on first =
                    int idx = line.IndexOf('=');
                    if (idx > 0)
                    {
                        string key = line.Substring(0, idx);
                        string value = line.Substring(idx + 1);
                        // KiTTY format: key=value
                        // We store keys as is (case sensitive? usually keys are distinct)
                        sessionData[key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Failed to read KiTTY session file {filePath}.", ex, MessageClass.WarningMsg);
                return null;
            }

            string decodedName = PuttySessionNameDecoder.Decode(sessionName);

            PuttySessionInfo sessionInfo = new()
            {
                PuttySession = decodedName,
                Name = decodedName,
                Hostname = sessionData.GetValueOrDefault("HostName", ""),
                Username = sessionData.GetValueOrDefault("UserName", "")
            };

            // Protocol mapping
            string protocol = sessionData.GetValueOrDefault("Protocol", "ssh");
            
            switch (protocol.ToLowerInvariant())
            {
                case "raw":
                    sessionInfo.Protocol = ProtocolType.RAW;
                    break;
                case "rlogin":
                    sessionInfo.Protocol = ProtocolType.Rlogin;
                    break;
                case "serial":
                    // Serial not fully supported in mRemoteNG connection info? 
                    // PuttySessionsRegistryProvider returns null for serial.
                    return null;
                case "ssh":
                    int sshVersion = 2;
                    if (sessionData.TryGetValue("SshProt", out string? sshProtStr) && int.TryParse(sshProtStr, out int ver))
                    {
                         sshVersion = ver;
                    }
                    /* Per PUTTY.H in PuTTYNG & PuTTYNG Upstream
                     * expect 0 for SSH1, 3 for SSH2 ONLY
                     * 1 for SSH1 with a 2 fallback
                     * 2 for SSH2 with a 1 fallback
                     */
                    sessionInfo.Protocol = sshVersion == 1 || sshVersion == 0 ? ProtocolType.SSH1 : ProtocolType.SSH2;
                    break;
                case "telnet":
                    sessionInfo.Protocol = ProtocolType.Telnet;
                    break;
                default:
                    // Unknown protocol
                    return null;
            }

            // Port
            if (sessionData.TryGetValue("PortNumber", out string? portStr) && int.TryParse(portStr, out int portNumber))
            {
                sessionInfo.Port = portNumber;
            }
            else
            {
                sessionInfo.SetDefaultPort();
            }

            return sessionInfo;
        }

        public override void StartWatcher()
        {
            if (_fileWatcher != null) return;

            string? sessionsDir = GetSessionsDirectory();
            if (sessionsDir == null) return;

            FileSystemWatcher watcher;
            try
            {
                watcher = new FileSystemWatcher(sessionsDir);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("PuttySessionsFileProvider.StartWatcher() failed.", ex, MessageClass.WarningMsg);
                return;
            }

            try
            {
                watcher.NotifyFilter = NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                watcher.Created += OnFileChanged;
                watcher.Deleted += OnFileChanged;
                watcher.Renamed += OnFileRenamed;
                watcher.Changed += OnFileChanged;

                watcher.EnableRaisingEvents = true;
                _fileWatcher = watcher;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("PuttySessionsFileProvider.StartWatcher() failed.", ex, MessageClass.WarningMsg);
                watcher.Dispose();
            }
        }

        public override void StopWatcher()
        {
            if (_fileWatcher == null) return;
            try
            {
                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher.Dispose();
            }
            catch { /* ignore */ }
            finally
            {
                _fileWatcher = null;
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            RaiseSessionChangedEvent(new PuttySessionChangedEventArgs());
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            RaiseSessionChangedEvent(new PuttySessionChangedEventArgs());
        }
    }
}
