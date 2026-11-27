using System;
using System.IO;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;

namespace mRemoteNG.Config.DataProviders
{
    /// <summary>Replaces a file only after its complete new contents have reached the filesystem.</summary>
    public static class AtomicFile
    {
        internal static FileStream CreatePrivateStagingFile(string path)
        {
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            FileSecurity security = new();
            security.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
            security.AddAccessRule(new FileSystemAccessRule(identity.User,
                FileSystemRights.FullControl, AccessControlType.Allow));
            security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null),
                FileSystemRights.FullControl, AccessControlType.Allow));
            return new FileInfo(path).Create(FileMode.CreateNew, FileSystemRights.Write, FileShare.None,
                4096, FileOptions.WriteThrough, security);
        }

        public static void WriteAllText(string path, string content)
        {
            string destination = Path.GetFullPath(path);
            string directory = Path.GetDirectoryName(destination);
            Directory.CreateDirectory(directory);
            string temporary = Path.Combine(directory, Guid.NewGuid().ToString("N") + ".tmp");
            try
            {
                using (var stream = CreatePrivateStagingFile(temporary))
                {
                    using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 4096, leaveOpen: true))
                    {
                        writer.Write(content);
                        writer.Flush();
                    }
                    stream.Flush(flushToDisk: true);
                }
                if (File.Exists(destination)) File.Replace(temporary, destination, null);
                else File.Move(temporary, destination);
            }
            finally
            {
                if (File.Exists(temporary)) File.Delete(temporary);
            }
        }
    }
}
