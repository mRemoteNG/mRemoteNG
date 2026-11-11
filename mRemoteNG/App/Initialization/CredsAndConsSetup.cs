using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.Config.Connections;
using mRemoteNG.Properties;

namespace mRemoteNG.App.Initialization
{
    [SupportedOSPlatform("windows")]
    public class CredsAndConsSetup
    {
        public void LoadCredsAndCons()
        {
            new SaveConnectionsOnEdit(Runtime.ConnectionsService);

            if (Properties.App.Default.FirstStart && !Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.ConnectionsService.GetStartupConnectionFileName()))
                Runtime.ConnectionsService.NewConnectionsFile(Runtime.ConnectionsService.GetStartupConnectionFileName());

            // User profiles are an opt-in, still experimental storage backend. Off by default so that
            // existing installations keep loading their connection file without a startup password prompt.
            if (Properties.OptionsAdvancedPage.Default.UseUserProfiles)
                Runtime.TryInitializeUserProfile();

            Runtime.LoadConnections();
        }
    }
}