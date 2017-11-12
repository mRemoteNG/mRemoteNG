using mRemoteNG.Config.Putty;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
	public class ConnectionsLoader
	{
        /// <summary>
        /// Load connections from a source. <see cref="connectionFileName"/> is ignored if
        /// <see cref="useDatabase"/> is true.
        /// </summary>
        /// <param name="useDatabase"></param>
        /// <param name="import"></param>
        /// <param name="connectionFileName"></param>
		public ConnectionTreeModel LoadConnections(bool useDatabase, bool import, string connectionFileName)
		{
		    ConnectionTreeModel connectionTreeModel;

            if (useDatabase)
			{
			    var sqlLoader = new SqlConnectionsLoader();
                connectionTreeModel = sqlLoader.Load();
            }
			else
			{
                var xmlLoader = new XmlConnectionsLoader(connectionFileName);
			    connectionTreeModel = xmlLoader.Load();
            }

            if (connectionTreeModel == null)
                connectionTreeModel = new ConnectionTreeModel();

		    if (!import)
		        AddPuttySessions(connectionTreeModel);

            return connectionTreeModel;
		}

	    private void AddPuttySessions(ConnectionTreeModel connectionTreeModel)
	    {
            PuttySessionsManager.Instance.AddSessions();
            connectionTreeModel.RootNodes.AddRange(PuttySessionsManager.Instance.RootPuttySessionsNodes);
        }
    }
}