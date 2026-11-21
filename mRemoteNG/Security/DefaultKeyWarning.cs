using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Security
{
    /// <summary>
    /// A connection list whose root node has no password of its own encrypts its passwords under
    /// the default key, which is public, so the encryption only hides them. This says so in the
    /// Notifications panel, once a session for the connections file and once for the database,
    /// when the list has passwords stored in it.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class DefaultKeyWarning
    {
        private static int _connectionsFileWarned;
        private static int _databaseWarned;

        /// <param name="connectionTree">The connections just loaded from the file.</param>
        /// <param name="dpapiProtected">True for a file protected with DPAPI, which uses no key of its own.</param>
        /// <param name="messageCollector">Where the warning goes.</param>
        public static void ForConnectionsFile(ConnectionTreeModel connectionTree, bool dpapiProtected, MessageCollector messageCollector)
        {
            if (dpapiProtected || !Applies(connectionTree)) return;
            if (Interlocked.Exchange(ref _connectionsFileWarned, 1) == 1) return;

            messageCollector.AddMessage(MessageClass.WarningMsg, Language.WarningDefaultKeyConnectionsFile);
        }

        /// <param name="connectionTree">The connections just loaded from the database.</param>
        /// <param name="messageCollector">Where the warning goes.</param>
        public static void ForDatabase(ConnectionTreeModel connectionTree, MessageCollector messageCollector)
        {
            if (!Applies(connectionTree)) return;
            if (Interlocked.Exchange(ref _databaseWarned, 1) == 1) return;

            messageCollector.AddMessage(MessageClass.WarningMsg, Language.WarningDefaultKeyDatabase);
        }

        /// <summary>
        /// True when the root node has no password of its own and a connection or folder has a
        /// password or an RD Gateway access token stored, both encrypted under the root's key.
        /// </summary>
        internal static bool Applies(ConnectionTreeModel connectionTree)
        {
            RootNodeInfo root = connectionTree?.RootNodes.OfType<RootNodeInfo>().FirstOrDefault(node => node.Type == RootNodeType.Connection);
            if (root == null || root.PasswordString != root.DefaultPassword) return false;

            return root.GetRecursiveChildList().Any(HasStoredPassword);
        }

        private static bool HasStoredPassword(ConnectionInfo connection) =>
            !string.IsNullOrEmpty(connection.Password)
            || !string.IsNullOrEmpty(connection.RDGatewayPassword)
            || !string.IsNullOrEmpty(connection.VNCProxyPassword)
            || !string.IsNullOrEmpty(connection.RDGatewayAccessToken);

        internal static void Reset()
        {
            Interlocked.Exchange(ref _connectionsFileWarned, 0);
            Interlocked.Exchange(ref _databaseWarned, 0);
        }
    }
}
