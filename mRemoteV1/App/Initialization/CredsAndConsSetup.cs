using System.IO;

namespace mRemoteNG.App.Initialization
{
	public class CredsAndConsSetup
    {
        public void LoadCredsAndCons()
        {
            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.ConnectionsService.GetStartupConnectionFileName()))
                Runtime.ConnectionsService.NewConnections(Runtime.ConnectionsService.GetStartupConnectionFileName());

            Runtime.LoadConnections();
        }
    }
}