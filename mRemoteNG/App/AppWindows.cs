#region Usings
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using mRemoteNG.Connection;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using Renci.SshNet;
#endregion

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class AppWindows
    {
        private static ActiveDirectoryImportWindow _adimportForm;
        private static ExternalToolsWindow _externalappsForm;
        private static PortScanWindow _portscanForm;
        private static UltraVNCWindow _ultravncscForm;
        private static ConnectionTreeWindow _treeForm;
        private static SftpFileManagerWindow _sftpFileManagerForm;

        internal static ConnectionTreeWindow TreeForm
        {
            get => _treeForm ?? (_treeForm = new ConnectionTreeWindow());
            set => _treeForm = value;
        }

        internal static ConfigWindow ConfigForm { get; set; } = new ConfigWindow();
        internal static ErrorAndInfoWindow ErrorsForm { get; set; } = new ErrorAndInfoWindow();
        internal static UpdateWindow UpdateForm { get; set; } = new UpdateWindow();
        internal static SSHTransferWindow SshtransferForm { get; private set; } = new SSHTransferWindow();
        internal static SftpFileManagerWindow SftpFileManagerForm { get; private set; } = new SftpFileManagerWindow();
        internal static OptionsWindow OptionsFormWindow { get; private set; }


        public static void Show(WindowType windowType)
        {
            try
            {
                Show(windowType, null);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("App.Runtime.Windows.Show() failed.", ex);
            }
        }

        public static void Show(WindowType windowType, Connection.ConnectionInfo connectionInfo)
        {
            try
            {
                WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel = FrmMain.Default.pnlDock;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (windowType)
                {
                    case WindowType.ActiveDirectoryImport:
                        if (_adimportForm == null || _adimportForm.IsDisposed)
                            _adimportForm = new ActiveDirectoryImportWindow();
                        _adimportForm.Show(dockPanel);
                        break;
                    case WindowType.Options:
                        if (OptionsFormWindow == null || OptionsFormWindow.IsDisposed)
                            OptionsFormWindow = new OptionsWindow();
                        OptionsFormWindow.SetActivatedPage(Language.StartupExit);
                        // Reload controls from stored settings before every show so that any
                        // edits left over from a previous hide (Tab-X without Apply/OK) are
                        // discarded.  Safe on first call — no-op until FrmOptions is embedded.
                        OptionsFormWindow.RefreshSettings();
                        OptionsFormWindow.Show(dockPanel);
                        break;
                    case WindowType.SSHTransfer:
                        if (SshtransferForm == null || SshtransferForm.IsDisposed)
                            SshtransferForm = new SSHTransferWindow();
                        SshtransferForm.Show(dockPanel);
                        break;
                    case WindowType.Update:
                        if (UpdateForm == null || UpdateForm.IsDisposed)
                            UpdateForm = new UpdateWindow();
                        UpdateForm.Show(dockPanel);
                        break;
                    case WindowType.ExternalApps:
                        if (_externalappsForm == null || _externalappsForm.IsDisposed)
                            _externalappsForm = new ExternalToolsWindow();
                        _externalappsForm.Show(dockPanel);
                        break;
                    case WindowType.PortScan:
                        _portscanForm = new PortScanWindow();
                        _portscanForm.Show(dockPanel);
                        break;
                    case WindowType.UltraVNCSC:
                        if (_ultravncscForm == null || _ultravncscForm.IsDisposed)
                            _ultravncscForm = new UltraVNCWindow();
                        _ultravncscForm.Show(dockPanel);
                        break;
                    case WindowType.SftpFileManager:
                        if (SftpFileManagerForm == null || SftpFileManagerForm.IsDisposed)
                            SftpFileManagerForm = new SftpFileManagerWindow();
                        SftpFileManagerForm.Show(dockPanel);
                        break;
                    case WindowType.SftpFileManagerWithConnection:
                        // Special case: show SFTP manager with existing connection info
                        if (SftpFileManagerForm == null || SftpFileManagerForm.IsDisposed)
                            SftpFileManagerForm = new SftpFileManagerWindow();
                        
                        if (connectionInfo != null)
                        {
                            // If connection info provided, try to find existing connection with same hostname/port that is already connected
                            var (existingConnInfo, sshClient) = FindExistingSshConnection(connectionInfo);
                            if (existingConnInfo != null)
                            {
                                SftpFileManagerForm.SetConnectionWithExistingSession(existingConnInfo, sshClient);
                            }
                            else
                            {
                                // No existing connection found, create SSH.NET ConnectionInfo from mRemoteNG ConnectionInfo
                                var sshNetConnInfo = CreateSshNetConnectionInfo(connectionInfo);
                                SftpFileManagerForm.SetConnection(sshNetConnInfo);
                            }
                        }
                        else
                        {
                            // No connection info provided, try to find any open SSH connection
                            var (anyConnInfo, _) = FindAnyOpenSshConnection();
                            if (anyConnInfo != null)
                            {
                                SftpFileManagerForm.SetConnectionWithExistingSession(anyConnInfo, null);
                            }
                            // else: SFTP manager will show empty, user can enter details manually
                        }
                        SftpFileManagerForm.Show(dockPanel);
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("App.Runtime.Windows.Show() failed.", ex);
            }
        }

        /// <summary>
        /// Creates an SSH.NET ConnectionInfo from a mRemoteNG ConnectionInfo.
        /// </summary>
        private static Renci.SshNet.ConnectionInfo CreateSshNetConnectionInfo(Connection.ConnectionInfo connectionInfo)
        {
            int port = connectionInfo.Port > 0 ? connectionInfo.Port : 22;
            string password = connectionInfo.Password ?? "";
            
            var authMethods = new List<Renci.SshNet.AuthenticationMethod>();
            if (!string.IsNullOrEmpty(password))
            {
                authMethods.Add(new Renci.SshNet.PasswordAuthenticationMethod(
                    connectionInfo.Username ?? "", password));
            }
            
            if (authMethods.Count == 0)
            {
                authMethods.Add(new Renci.SshNet.PasswordAuthenticationMethod(
                    connectionInfo.Username ?? "", ""));
            }
            
            return new Renci.SshNet.ConnectionInfo(
                connectionInfo.Hostname,
                port,
                connectionInfo.Username ?? "",
                authMethods.ToArray());
        }

        /// <summary>
        /// Finds an existing SSH connection with the same hostname and port.
        /// Returns the connection info that can be used for SFTP.
        /// </summary>
        /// <param name="connectionInfo">The connection info to match</param>
        /// <returns>A tuple of the existing ConnectionInfo and SshClient (may be null)</returns>
        private static (Renci.SshNet.ConnectionInfo sshNetConnectionInfo, SshClient sshClient) FindExistingSshConnection(Connection.ConnectionInfo connectionInfo)
        {
            try
            {
                // Check if Runtime and ConnectionsService are available
                if (Runtime.ConnectionsService?.ConnectionTreeModel == null)
                    return (null, null);

                // Get all connections in the tree
                var allConnections = Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveChildList();

                // Find connections with the same hostname and port that are already connected
                foreach (var conn in allConnections)
                {
                    // Check if this connection matches the hostname and port
                    if (conn.Hostname?.Equals(connectionInfo.Hostname, StringComparison.OrdinalIgnoreCase) == true &&
                        conn.Port == connectionInfo.Port &&
                        conn.OpenConnections.Count > 0)
                    {
                        // Found a connection with open connections to the same host
                        // Check if it's an SSH connection (SSH1 or SSH2)
                        var protocol = conn.Protocol;
                        if (protocol != mRemoteNG.Connection.Protocol.ProtocolType.SSH1 &&
                            protocol != mRemoteNG.Connection.Protocol.ProtocolType.SSH2)
                            continue;

                        // Build SSH.NET ConnectionInfo using the existing connection's credentials
                        int port = conn.Port > 0 ? conn.Port : 22;
                        
                        // Get password from the connection
                        string password = conn.Password ?? "";
                        
                        // Create authentication method
                        var authMethods = new List<Renci.SshNet.AuthenticationMethod>();
                        if (!string.IsNullOrEmpty(password))
                        {
                            authMethods.Add(new Renci.SshNet.PasswordAuthenticationMethod(conn.Username ?? "", password));
                        }

                        if (authMethods.Count == 0)
                        {
                            // No authentication method available, try with empty password
                            authMethods.Add(new Renci.SshNet.PasswordAuthenticationMethod(conn.Username ?? "", ""));
                        }

                        var sshNetConnInfo = new Renci.SshNet.ConnectionInfo(
                            conn.Hostname,
                            port,
                            conn.Username ?? "",
                            authMethods.ToArray());

                        // Return the connection info with null for sshClient
                        // The SFTP client will use this connection info directly
                        Runtime.MessageCollector.AddMessage(
                            mRemoteNG.Messages.MessageClass.InformationMsg,
                            $"Found existing SSH connection to {conn.Hostname}:{port}, reusing credentials for SFTP");
                        
                        return (sshNetConnInfo, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("FindExistingSshConnection failed", ex);
            }

            return (null, null);
        }

        /// <summary>
        /// Finds any open SSH connection in the connection tree.
        /// Returns the connection info that can be used for SFTP.
        /// </summary>
        /// <returns>A tuple of the ConnectionInfo and SshClient (may be null)</returns>
        private static (Renci.SshNet.ConnectionInfo sshNetConnectionInfo, SshClient sshClient) FindAnyOpenSshConnection()
        {
            try
            {
                // Check if Runtime and ConnectionsService are available
                if (Runtime.ConnectionsService?.ConnectionTreeModel == null)
                    return (null, null);

                // Get all connections in the tree
                var allConnections = Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveChildList();

                // Find any connection with open connections that is an SSH connection
                foreach (var conn in allConnections)
                {
                    // Check if this connection has open connections
                    if (conn.OpenConnections.Count > 0)
                    {
                        // Check if it's an SSH connection (SSH1 or SSH2)
                        var protocol = conn.Protocol;
                        if (protocol != mRemoteNG.Connection.Protocol.ProtocolType.SSH1 &&
                            protocol != mRemoteNG.Connection.Protocol.ProtocolType.SSH2)
                            continue;

                        // Build SSH.NET ConnectionInfo using the existing connection's credentials
                        int port = conn.Port > 0 ? conn.Port : 22;
                        
                        // Get password from the connection
                        string password = conn.Password ?? "";
                        
                        // Create authentication method
                        var authMethods = new List<Renci.SshNet.AuthenticationMethod>();
                        if (!string.IsNullOrEmpty(password))
                        {
                            authMethods.Add(new Renci.SshNet.PasswordAuthenticationMethod(conn.Username ?? "", password));
                        }

                        if (authMethods.Count == 0)
                        {
                            // No authentication method available, try with empty password
                            authMethods.Add(new Renci.SshNet.PasswordAuthenticationMethod(conn.Username ?? "", ""));
                        }

                        var sshNetConnInfo = new Renci.SshNet.ConnectionInfo(
                            conn.Hostname,
                            port,
                            conn.Username ?? "",
                            authMethods.ToArray());

                        // Return the connection info with null for sshClient
                        Runtime.MessageCollector.AddMessage(
                            mRemoteNG.Messages.MessageClass.InformationMsg,
                            $"Found open SSH connection to {conn.Hostname}:{port}, reusing credentials for SFTP");
                        
                        return (sshNetConnInfo, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("FindAnyOpenSshConnection failed", ex);
            }

            return (null, null);
        }
    }
}