using System;
using System.Runtime.Versioning;

namespace mRemoteNG.App.Info
{
    [SupportedOSPlatform("windows")]
    public static class ConnectionsFileInfo
    {
        public static readonly string DefaultConnectionsPath = SettingsFileInfo.SettingsPath;
        public static readonly string DefaultConnectionsFile = "confCons.xml";
        public static readonly string DefaultConnectionsFileNew = "confConsNew.xml";
        public static readonly Version ConnectionFileVersion = new Version(2, 9);
    }
}