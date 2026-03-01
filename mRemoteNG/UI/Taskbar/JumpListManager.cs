using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.WindowsAPICodePack.Taskbar;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;

namespace mRemoteNG.UI.Taskbar
{
    /// <summary>
    /// Manages the Windows taskbar JumpList for mRemoteNG.
    /// Shows recent connections in the right-click context menu of the taskbar icon.
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal sealed class JumpListManager
    {
        private const int MaxRecentConnections = 10;
        private static readonly Lazy<JumpListManager> s_instance = new(() => new JumpListManager());

        public static JumpListManager Instance => s_instance.Value;

        private JumpListManager()
        {
            RecentConnectionsService.Instance.RecentConnectionsChanged += (s, e) =>
            {
                if (TaskbarManager.IsPlatformSupported)
                    Rebuild();
            };
        }

        /// <summary>
        /// Initializes the JumpList after connections are loaded.
        /// Call from FrmMain after connections are ready.
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (!TaskbarManager.IsPlatformSupported)
                    return;

                Rebuild();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("JumpList initialization failed", ex, MessageClass.WarningMsg);
            }
        }

        /// <summary>
        /// Records a connection as recently used and rebuilds the JumpList.
        /// </summary>
        public static void AddRecentConnection(ConnectionInfo connectionInfo)
        {
            RecentConnectionsService.Instance.Add(connectionInfo);
        }

        private static void Rebuild()
        {
            try
            {
                string exePath = GetExePath();
                if (string.IsNullOrEmpty(exePath))
                    return;

                JumpList jumpList = JumpList.CreateJumpList();

                // Add "Tasks" section — always-visible quick actions
                var newConnectionTask = new JumpListLink(exePath, "New Connection")
                {
                    IconReference = new Microsoft.WindowsAPICodePack.Shell.IconReference(exePath, 0)
                };

                jumpList.AddUserTasks(newConnectionTask);

                // Add recent connections as a custom category
                var recentConnections = RecentConnectionsService.Instance.GetRecentConnections().ToList();

                if (recentConnections.Count > 0)
                {
                    var recentCategory = new JumpListCustomCategory("Recent Connections");

                    foreach (var conn in recentConnections)
                    {
                        var link = new JumpListLink(exePath, conn.Name)
                        {
                            Arguments = $"--connect \"{conn.ConstantID}\"",
                            IconReference = new Microsoft.WindowsAPICodePack.Shell.IconReference(exePath, 0)
                        };
                        recentCategory.AddJumpListItems(link);
                    }

                    jumpList.AddCustomCategories(recentCategory);
                }

                // Also add connections from the tree (top-level, non-folder)
                if (recentConnections.Count == 0)
                {
                    AddTopConnectionsFromTree(jumpList, exePath);
                }

                jumpList.Refresh();
            }
            catch (UnauthorizedAccessException)
            {
                // JumpList may not be available (e.g. running as service)
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("JumpList rebuild failed", ex, MessageClass.WarningMsg);
            }
        }

        private static void AddTopConnectionsFromTree(JumpList jumpList, string exePath)
        {
            var treeModel = Runtime.ConnectionsService.ConnectionTreeModel;
            if (treeModel == null)
                return;

            var connections = new List<ConnectionInfo>();
            CollectConnections(treeModel.RootNodes, connections, MaxRecentConnections);

            if (connections.Count == 0)
                return;

            var connectionsCategory = new JumpListCustomCategory("Connections");

            foreach (var conn in connections)
            {
                if (string.IsNullOrEmpty(conn.Name))
                    continue;

                var link = new JumpListLink(exePath, conn.Name)
                {
                    Arguments = $"--connect \"{conn.Name}\"",
                    IconReference = new Microsoft.WindowsAPICodePack.Shell.IconReference(exePath, 0)
                };
                connectionsCategory.AddJumpListItems(link);
            }

            jumpList.AddCustomCategories(connectionsCategory);
        }

        private static void CollectConnections(IEnumerable<ConnectionInfo> nodes, List<ConnectionInfo> result, int max)
        {
            foreach (var node in nodes)
            {
                if (result.Count >= max)
                    return;

                if (node is ContainerInfo container)
                {
                    CollectConnections(container.Children, result, max);
                }
                else if (!string.IsNullOrEmpty(node.Name))
                {
                    result.Add(node);
                }
            }
        }

        private static string GetExePath()
        {
            // Get the running exe path
            string? exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
            if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
                return exePath;

            // Fallback: entry assembly location
            exePath = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
                return exePath;

            return string.Empty;
        }
    }
}
