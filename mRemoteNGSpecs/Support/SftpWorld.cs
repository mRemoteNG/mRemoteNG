using System;
using System.Collections.Generic;
using System.IO;
using mRemoteNG.Tools;

namespace mRemoteNGSpecs.Support
{
    /// <summary>
    /// Per-scenario state shared between SFTP step definitions. Reqnroll creates
    /// one instance per scenario through constructor injection and disposes it
    /// afterwards, which disconnects the service and removes temp files.
    /// </summary>
    public sealed class SftpWorld : IDisposable
    {
        public SftpFileService Service { get; } = new();

        /// <summary>Local scratch directory, unique per scenario.</summary>
        public string LocalTempDir { get; } =
            Directory.CreateDirectory(
                Path.Combine(Path.GetTempPath(), "mRemoteNG_sftp_e2e", Guid.NewGuid().ToString("N")))
                .FullName;

        public List<SftpFileItem> LastListing { get; set; } = new();
        public string LastDownloadedFile { get; set; }

        public void Dispose()
        {
            try { Service.Dispose(); } catch { /* best effort */ }

            try
            {
                if (Directory.Exists(LocalTempDir))
                    Directory.Delete(LocalTempDir, recursive: true);
            }
            catch { /* best effort */ }
        }
    }
}
