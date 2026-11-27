using System;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Net.Sockets;
using mRemoteNG.App;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Security;
using System.Runtime.ExceptionServices;

// ReSharper disable ArrangeAccessorOwnerBody


namespace mRemoteNG.Connection.Protocol.VNC
{
    [SupportedOSPlatform("windows")]
    public class ProtocolVNC : ProtocolBase
    {
        #region Private Declarations

        private VncSharpCore.RemoteDesktop _vnc;
        private ConnectionInfo _info;
        private static volatile bool _isConnectionSuccessful;
        private static ExceptionDispatchInfo _socketexception;
        private static readonly ManualResetEvent TimeoutObject = new(false);
        private static readonly object _testConnectLock = new();

        // Stall recovery: see VncStallWatchdog for why a frozen session needs a reconnect.
        private static readonly TimeSpan StallCheckInterval = TimeSpan.FromSeconds(5);
        private readonly Stopwatch _clock = Stopwatch.StartNew();
        private readonly VncStallWatchdog _watchdog;
        private readonly System.Windows.Forms.Timer _stallTimer;
        private bool _reconnecting;

        #endregion

        #region Public Methods

        public ProtocolVNC()
        {
            Control = new VncSharpCore.RemoteDesktop();
            _watchdog = new VncStallWatchdog(() => _clock.Elapsed, VncStallWatchdog.DefaultStallTimeout,
                                             VncStallWatchdog.DefaultMaxReconnects);
            _stallTimer = new System.Windows.Forms.Timer { Interval = (int)StallCheckInterval.TotalMilliseconds };
            _stallTimer.Tick += StallTimer_Tick;
        }

        public override bool Initialize()
        {
            base.Initialize();

            try
            {
                _vnc = Control as VncSharpCore.RemoteDesktop;
                _info = InterfaceControl.Info;
                _vnc.VncPort = _info.Port;
                // RemoteDesktop invalidates the region it has just drawn for every update
                // rectangle, so Invalidated is the per-frame heartbeat. It exposes no
                // dedicated update event.
                _vnc.Invalidated += VNCEvent_FrameDrawn;

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.VncSetPropsFailed + Environment.NewLine + ex.Message,
                                                    true);
                return false;
            }
        }
 
        public override bool Connect()
        {
            SetEventHandlers();
            try
            {
                if (TestConnect(_info.Hostname, _info.Port, 500))
                    ConnectVnc();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.ConnectionOpenFailed + Environment.NewLine +
                                                    ex.Message);
                return false;
            }

            return true;
        }

        public override void Disconnect()
        {
            StopStallWatch();
            try
            {
                _vnc.Disconnect();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.VncConnectionDisconnectFailed + Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        public void SendSpecialKeys(SpecialKeys Keys)
        {
            try
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (Keys)
                {
                    case SpecialKeys.CtrlAltDel:
                        _vnc.SendSpecialKeys(VncSharpCore.SpecialKeys.CtrlAltDel);
                        break;
                    case SpecialKeys.CtrlEsc:
                        _vnc.SendSpecialKeys(VncSharpCore.SpecialKeys.CtrlEsc);
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.VncSendSpecialKeysFailed + Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        public void StartChat()
        {
            throw new NotImplementedException();
        }

        public void StartFileTransfer()
        {
            throw new NotImplementedException();
        }

        public void RefreshScreen()
        {
            try
            {
                _vnc.FullScreenUpdate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.VncRefreshFailed + Environment.NewLine + ex.Message,
                                                    true);
            }
        }

        public override void Close()
        {
            StopStallWatch();
            base.Close();
        }

        #endregion

        #region Private Methods

        // The single place the control is connected, so the first connection and a stall
        // reconnect take the same path.
        private void ConnectVnc()
        {
            _vnc.Connect(_info.Hostname, _info.VNCViewOnly, _info.VNCSmartSizeMode != SmartSizeMode.SmartSNo);
        }

        private void StopStallWatch()
        {
            _stallTimer.Stop();
            _watchdog.Stop();
        }

        private void StallTimer_Tick(object sender, EventArgs e)
        {
            switch (_watchdog.Check())
            {
                case VncStallAction.Reconnect:
                    ReconnectAfterStall();
                    break;
                case VncStallAction.GiveUp:
                    _stallTimer.Stop();
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                        $"VNC session to {_info.Hostname} stopped updating and did not recover after {_watchdog.MaxReconnects} reconnects.");
                    break;
            }
        }

        // Runs on the UI thread (timer tick). Disconnect() joins VncSharp's worker for up to
        // three seconds, so the tab can pause briefly before the new connection draws.
        private void ReconnectAfterStall()
        {
            _stallTimer.Stop();
            // Not log-only: whether a warning reaches the Notifications panel or a popup is the
            // user's notification setting, as for any other warning.
            Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg,
                                                $"VNC session to {_info.Hostname} stopped updating for {_watchdog.StallTimeout.TotalSeconds:0} seconds; reconnecting.");

            // RemoteDesktop.Disconnect raises ConnectionLost, whose handler closes the tab.
            // This is a reconnect, so keep the tab.
            _reconnecting = true;
            try
            {
                if (_vnc.IsConnected)
                    _vnc.Disconnect();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg,
                                                    Language.VncConnectionDisconnectFailed + Environment.NewLine +
                                                    ex.Message, true);
            }
            finally
            {
                _reconnecting = false;
            }

            try
            {
                // Not through Connect(): that also re-registers the event handlers, which
                // are still attached.
                ConnectVnc();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.ConnectionOpenFailed + Environment.NewLine +
                                                    ex.Message);
                VNCEvent_Disconnected(this, EventArgs.Empty);
            }
        }

        private void SetEventHandlers()
        {
            try
            {
                _vnc.ConnectComplete += VNCEvent_Connected;
                _vnc.ConnectionLost += VNCEvent_Disconnected;
                FrmMain.ClipboardChanged += VNCEvent_ClipboardChanged;
                if (!Force.HasFlag(ConnectionInfo.Force.NoCredentials) && _info?.Password?.Length > 0)
                {
                    _vnc.GetPassword = VNCEvent_Authenticate;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.VncSetEventHandlersFailed + Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        private static bool TestConnect(string hostName, int port, int timeoutMSec)
        {
            lock (_testConnectLock)
            {
                _socketexception = null;
                TcpClient tcpclient = new();

                TimeoutObject.Reset();
                tcpclient.BeginConnect(hostName, port, CallBackMethod, tcpclient);

                if (TimeoutObject.WaitOne(timeoutMSec, false))
                {
                    if (_isConnectionSuccessful) return true;
                    // Connection completed but failed - tcpclient will be closed in CallBackMethod's finally block
                    if (_socketexception != null)
                    {
                        _socketexception.Throw();
                    }
                }
                else
                {
                    tcpclient.Close();
                    throw new TimeoutException($"Connection timed out to host " + hostName + " on port " + port);
                }

                return false;
            }
        }

        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            TcpClient tcpclient = null;
            try
            {
                _isConnectionSuccessful = false;
                tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient?.Client == null) return;

                tcpclient.EndConnect(asyncresult);
                _isConnectionSuccessful = true;
            }
            catch (Exception ex)
            {
                _isConnectionSuccessful = false;
                _socketexception = ExceptionDispatchInfo.Capture(ex);
            }
            finally
            {
                tcpclient?.Close();
                TimeoutObject.Set();
            }
        }

        #endregion

        #region Private Events & Handlers

        private void VNCEvent_Connected(object sender, EventArgs e)
        {
            Event_Connected(this);
            _vnc.AutoScroll = _info.VNCSmartSizeMode == SmartSizeMode.SmartSNo;
            _watchdog.Start();
            _stallTimer.Start();
        }

        private void VNCEvent_FrameDrawn(object sender, System.Windows.Forms.InvalidateEventArgs e)
        {
            _watchdog.FrameReceived();
        }

        private void VNCEvent_Disconnected(object sender, EventArgs e)
        {
            if (_reconnecting) return;
            StopStallWatch();
            FrmMain.ClipboardChanged -= VNCEvent_ClipboardChanged;
            Event_Disconnected(this, @"VncSharp Disconnected.", null);
            Close();
        }

        private void VNCEvent_ClipboardChanged()
        {
            _vnc.FillServerClipboard();
        }

        private string VNCEvent_Authenticate()
        {
            //return _info.Password.ConvertToUnsecureString();
            return _info.Password;
        }

        #endregion

        #region Enums

        public enum Defaults
        {
            Port = 5900
        }

        public enum SpecialKeys
        {
            CtrlAltDel,
            CtrlEsc
        }

        public enum Compression
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.NoCompression))]
            CompNone = 99,
            [Description("0")] Comp0 = 0,
            [Description("1")] Comp1 = 1,
            [Description("2")] Comp2 = 2,
            [Description("3")] Comp3 = 3,
            [Description("4")] Comp4 = 4,
            [Description("5")] Comp5 = 5,
            [Description("6")] Comp6 = 6,
            [Description("7")] Comp7 = 7,
            [Description("8")] Comp8 = 8,
            [Description("9")] Comp9 = 9
        }

        public enum Encoding
        {
            [Description("Raw")] EncRaw,
            [Description("RRE")] EncRRE,
            [Description("CoRRE")] EncCorre,
            [Description("Hextile")] EncHextile,
            [Description("Zlib")] EncZlib,
            [Description("Tight")] EncTight,
            [Description("ZlibHex")] EncZLibHex,
            [Description("ZRLE")] EncZRLE
        }

        public enum AuthMode
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.Vnc))]
            AuthVNC,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Windows))]
            AuthWin
        }

        public enum ProxyType
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.None))]
            ProxyNone,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Http))]
            ProxyHTTP,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Socks5))]
            ProxySocks5,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.UltraVncRepeater))]
            ProxyUltra
        }

        public enum Colors
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.Normal))]
            ColNormal,
            [Description("8-bit")] Col8Bit
        }

        public enum SmartSizeMode
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.NoSmartSize))]
            SmartSNo,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Free))]
            SmartSFree,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Aspect))]
            SmartSAspect
        }

        #endregion
    }
}
