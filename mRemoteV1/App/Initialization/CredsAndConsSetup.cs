using mRemoteNG.Connection;
using mRemoteNG.Tools;
using System.IO;

namespace mRemoteNG.App.Initialization
{
    public class CredsAndConsSetup
	{
	    private readonly IConnectionsService _connectionsService;

	    public CredsAndConsSetup(IConnectionsService connectionsService)
	    {
	        _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
	    }

	    public void LoadCredsAndCons()
        {
            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(_connectionsService.GetStartupConnectionFileName()))
                _connectionsService.NewConnectionsFile(_connectionsService.GetStartupConnectionFileName());

            _connectionsService.LoadConnections();
        }
    }
}