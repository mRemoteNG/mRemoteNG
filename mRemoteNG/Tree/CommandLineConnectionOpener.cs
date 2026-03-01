using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;

namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public class CommandLineConnectionOpener : IConnectionTreeDelegate
    {
        private readonly IConnectionInitiator _connectionInitiator;
        private readonly string? _connectTo;
        private readonly string _argumentName;

        public CommandLineConnectionOpener(IConnectionInitiator connectionInitiator)
            : this(connectionInitiator, StartupArgumentsInterpreter.ConnectTo, "--connect")
        {
        }

        public CommandLineConnectionOpener(IConnectionInitiator connectionInitiator, string? connectTo, string argumentName)
        {
            _connectionInitiator = connectionInitiator ?? throw new ArgumentNullException(nameof(connectionInitiator));
            _connectTo = connectTo;
            _argumentName = argumentName;
        }

        public void Execute(IConnectionTree connectionTree)
        {
            ArgumentNullException.ThrowIfNull(connectionTree);

            Execute(connectionTree.GetRootConnectionNode());
        }

        public void Execute(RootNodeInfo rootConnectionNode)
        {
            ArgumentNullException.ThrowIfNull(rootConnectionNode);

            string? connectTo = _connectTo;
            if (string.IsNullOrEmpty(connectTo))
                return;

            var allConnections = rootConnectionNode
                .GetRecursiveChildList()
                .Where(node => node is not ContainerInfo)
                .ToList();

            // Support multiple connections separated by semicolons
            string[] targets = connectTo.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (string target in targets)
            {
                ConnectionInfo? match = FindConnection(allConnections, target);

                if (match != null)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"Auto-connecting to \"{match.Name}\" ({_argumentName} argument)");
                    _connectionInitiator.OpenConnection(match);
                }
                else
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                        $"{_argumentName}: connection \"{target}\" not found");
                }
            }
        }

        private static ConnectionInfo? FindConnection(List<ConnectionInfo> allConnections, string target)
        {
            // If target contains '/', treat it as a folder path (e.g. "Servers/Production/WebServer01")
            if (target.Contains('/'))
                return FindByFolderPath(allConnections, target);

            // Try exact name match (case-insensitive)
            ConnectionInfo? match = allConnections
                .FirstOrDefault(c => string.Equals(c.Name, target, StringComparison.OrdinalIgnoreCase));

            // Fall back to ConstantID match
            match ??= allConnections
                .FirstOrDefault(c => string.Equals(c.ConstantID, target, StringComparison.OrdinalIgnoreCase));

            // Fall back to hostname match
            match ??= allConnections
                .FirstOrDefault(c => string.Equals(c.Hostname, target, StringComparison.OrdinalIgnoreCase));

            return match;
        }

        private static ConnectionInfo? FindByFolderPath(List<ConnectionInfo> allConnections, string path)
        {
            // Split "Folder/SubFolder/ConnectionName" into path segments
            string[] segments = path.Split('/');
            string connectionName = segments[^1];
            string[] folderPath = segments[..^1];

            return allConnections.FirstOrDefault(c =>
            {
                if (!string.Equals(c.Name, connectionName, StringComparison.OrdinalIgnoreCase))
                    return false;

                // Walk up the parent chain and verify each folder segment matches (in reverse)
                ContainerInfo? parent = c.Parent;
                for (int i = folderPath.Length - 1; i >= 0; i--)
                {
                    if (parent == null)
                        return false;
                    if (!string.Equals(parent.Name, folderPath[i], StringComparison.OrdinalIgnoreCase))
                        return false;
                    parent = parent.Parent;
                }

                return true;
            });
        }
    }
}
