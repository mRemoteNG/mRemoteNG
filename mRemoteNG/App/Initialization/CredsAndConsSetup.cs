using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Properties;

namespace mRemoteNG.App.Initialization
{
    [SupportedOSPlatform("windows")]
    public static class CredsAndConsSetup
    {
        public static void LoadCredsAndCons()
        {
            _ = new SaveConnectionsOnEdit(Runtime.ConnectionsService);

            if (Properties.App.Default.FirstStart && !Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation && !File.Exists(ConnectionsService.GetStartupConnectionFileName()))
                Runtime.ConnectionsService.NewConnectionsFile(ConnectionsService.GetStartupConnectionFileName());

            Runtime.LoadConnections();

            // Restore additional connection files from previous session (#2331)
            Runtime.ConnectionsService.LoadAdditionalConnectionFiles();
        }
    }
}