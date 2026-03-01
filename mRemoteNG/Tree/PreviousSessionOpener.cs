using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.UI.Controls.ConnectionTree;
using System.Runtime.Versioning;

namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public class PreviousSessionOpener : IConnectionTreeDelegate
    {
        private readonly IConnectionInitiator _connectionInitiator;
        private readonly Func<IEnumerable<ConnectionInfo>> _previousQuickConnectSessionLoader;

        public PreviousSessionOpener(
            IConnectionInitiator connectionInitiator,
            Func<IEnumerable<ConnectionInfo>>? previousQuickConnectSessionLoader = null)
        {
            ArgumentNullException.ThrowIfNull(connectionInitiator);
            _connectionInitiator = connectionInitiator;
            _previousQuickConnectSessionLoader = previousQuickConnectSessionLoader ?? QuickConnectHistoryLoader.LoadPreviouslyConnectedQuickConnectSessions;
        }

        public void Execute(IConnectionTree connectionTree)
        {
            IEnumerable<ConnectionInfo> connectionInfoList = connectionTree.GetRootConnectionNode().GetRecursiveChildList()
                                                   .Where(node => !(node is ContainerInfo));
            IEnumerable<ConnectionInfo> previouslyOpenedConnections = connectionInfoList
                .Where(item =>
                           item.PleaseConnect &&
                           //ignore items that have already connected
                           !_connectionInitiator.ActiveConnections.Contains(item.ConstantID, StringComparer.Ordinal));

            foreach (ConnectionInfo connectionInfo in previouslyOpenedConnections)
            {
                _connectionInitiator.OpenConnection(connectionInfo);
            }

            OpenPreviouslyConnectedQuickConnectSessions();
        }

        private void OpenPreviouslyConnectedQuickConnectSessions()
        {
            IEnumerable<ConnectionInfo> previouslyOpenedQuickConnections = _previousQuickConnectSessionLoader()
                .Where(item =>
                    item.PleaseConnect &&
                    !_connectionInitiator.ActiveConnections.Contains(item.ConstantID, StringComparer.Ordinal));

            foreach (ConnectionInfo connectionInfo in previouslyOpenedQuickConnections)
            {
                _connectionInitiator.OpenConnection(connectionInfo);
            }
        }
    }
}
