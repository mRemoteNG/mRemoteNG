using System;

namespace mRemoteNG.App.Info
{
    public static class ConnectionsFileInfo
    {
        public static readonly string DefaultConnectionsPath = SettingsFileInfo.SettingsPath;
        public static readonly string DefaultConnectionsFile = "confCons.xml";
        public static readonly string DefaultConnectionsFileNew = "confConsNew.xml";
        public static readonly Version ConnectionFileVersion = new Version(2, 9);
    }
}