using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Controls.SCP;

namespace mRemoteNG.Connection.Protocol.SCP
{
    [SupportedOSPlatform("windows")]
    public class ProtocolSCP_DotNet : ProtocolBase
    {
        #region Private Fields

        private readonly ConnectionInfo _connectionInfo;
        private ScpFileTransferControl _scpControl;

        #endregion

        #region Constructor

        public ProtocolSCP_DotNet(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        #endregion

        #region Public Methods

        public override bool Connect()
        {
            try
            {
                var connectMsg = $"Attempting to start SCP/SFTP session to {_connectionInfo.Hostname}:{_connectionInfo.Port}";
                Logger.Instance.Log?.Info($"[ProtocolSCP_DotNet.Connect] {connectMsg}");
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, connectMsg, true);

                // Create the SCP file transfer control
                _scpControl = new ScpFileTransferControl
                {
                    Dock = DockStyle.Fill
                };

                // Add the control to the interface
                InterfaceControl.Controls.Add(_scpControl);
                Control = _scpControl;

                Logger.Instance.Log?.Debug($"[ProtocolSCP_DotNet.Connect] Calling ScpFileTransferControl.Connect()");

                // Connect to the remote server
                bool connected = _scpControl.Connect(_connectionInfo);

                if (connected)
                {
                    base.Connect();
                    var successMsg = $"Successfully connected to {_connectionInfo.Hostname}";
                    Logger.Instance.Log?.Info($"[ProtocolSCP_DotNet.Connect] {successMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, successMsg, true);
                    return true;
                }
                else
                {
                    var failMsg = $"Failed to connect to {_connectionInfo.Hostname}";
                    Logger.Instance.Log?.Error($"[ProtocolSCP_DotNet.Connect] {failMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg, failMsg, true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ProtocolSCP_DotNet.Connect] Connection failed", ex);
                Runtime.MessageCollector?.AddExceptionMessage(Language.ConnectionFailed, ex);
                return false;
            }
        }

        public override void Disconnect()
        {
            try
            {
                Logger.Instance.Log?.Debug("[ProtocolSCP_DotNet.Disconnect] Disconnecting SCP session");
                _scpControl?.Disconnect();
                // Note: Don't call base.Disconnect() here as it just calls Close(),
                // which our Close() override already handles by calling Disconnect().
                // Calling base.Disconnect() would create an infinite recursion loop.
                Logger.Instance.Log?.Info("[ProtocolSCP_DotNet.Disconnect] Disconnect complete");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ProtocolSCP_DotNet.Disconnect] Error disconnecting", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error disconnecting SCP session", ex);
            }
        }

        public override void Close()
        {
            try
            {
                Logger.Instance.Log?.Debug("[ProtocolSCP_DotNet.Close] Closing SCP session");
                Disconnect();

                // Dispose resources
                _scpControl?.Dispose();
                _scpControl = null;

                Logger.Instance.Log?.Info("[ProtocolSCP_DotNet.Close] SCP protocol closed successfully");
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                    "SCP protocol closed and resources disposed", true);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ProtocolSCP_DotNet.Close] Error closing session", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error closing SCP session", ex);
            }

            base.Close();
        }

        public override void Focus()
        {
            try
            {
                Logger.Instance.Log?.Debug("[ProtocolSCP_DotNet.Focus] Focusing SCP control");
                _scpControl?.Focus();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ProtocolSCP_DotNet.Focus] Error focusing control", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error focusing SCP control", ex);
            }
        }

        #endregion

        #region Enumerations

        public enum Defaults
        {
            Port = 22
        }

        #endregion

    }
}
