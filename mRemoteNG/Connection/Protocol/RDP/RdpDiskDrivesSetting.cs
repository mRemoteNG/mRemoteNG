using System;

namespace mRemoteNG.Connection.Protocol.RDP
{
    /// <summary>
    /// Reads the default disk drive redirection kept in the ConDefaultRedirectDiskDrives setting.
    /// The setting holds the name of an <see cref="RDPDiskDrives"/> value; it used to be a bool, and
    /// a value saved that way still loads: true redirected every drive, false none.
    /// </summary>
    public static class RdpDiskDrivesSetting
    {
        public static RDPDiskDrives Parse(string value)
        {
            if (Enum.TryParse(value, true, out RDPDiskDrives drives) && Enum.IsDefined(drives))
                return drives;

            return bool.TryParse(value, out bool redirect) && redirect ? RDPDiskDrives.All : RDPDiskDrives.None;
        }
    }
}
