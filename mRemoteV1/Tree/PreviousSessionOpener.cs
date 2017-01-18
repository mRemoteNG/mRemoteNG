using System;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls;


namespace mRemoteNG.Tree
{
    public class PreviousSessionOpener : IConnectionTreeDelegate
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public PreviousSessionOpener(IConnectionInitiator connectionInitiator)
        {
            if (connectionInitiator == null)
                throw new ArgumentNullException(nameof(connectionInitiator));
            _connectionInitiator = connectionInitiator;
        }

        public void Execute(IConnectionTree connectionTree)
        {
            var connectionInfoList = connectionTree.GetRootConnectionNode().GetRecursiveChildList().Where(node => !(node is ContainerInfo));
            var previouslyOpenedConnections = connectionInfoList.Where(item => item.PleaseConnect);
            foreach (var connectionInfo in previouslyOpenedConnections)
            {
                _connectionInitiator.OpenConnection(connectionInfo);
            }
        }
    }
}