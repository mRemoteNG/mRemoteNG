using System.IO;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Tools;

namespace mRemoteNG.App.Initialization
{
	public class CredsAndConsSetup
	{
	    private readonly ConnectionsService _connectionsService;

	    public CredsAndConsSetup(ConnectionsService connectionsService)
	    {
	        _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
	    }

	    public void LoadCredsAndCons()
        {
            new SaveConnectionsOnEdit(_connectionsService);

            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(_connectionsService.GetStartupConnectionFileName()))
                _connectionsService.NewConnectionsFile(_connectionsService.GetStartupConnectionFileName());

            _connectionsService.LoadConnections();
        }
    }
}