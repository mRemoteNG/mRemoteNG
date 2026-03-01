using AxMSTSCLib;
using System.Drawing;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using MSTSCLib;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol : ProtocolBase, ISupportsViewOnly, IMessageFilter
    {
        [DllImport("user32.dll")]
        private static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

        /* RDP v8 requires Windows 7 with:
         * https://support.microsoft.com/en-us/kb/2592687
         * OR
         * https://support.microsoft.com/en-us/kb/2923545
         *
         * Windows 8+ support RDP v8 out of the box.
         */

        private MsRdpClient6NotSafeForScripting _rdpClient = null!; // lowest version supported, initialized in Initialize()
        protected virtual RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc6;
        protected ConnectionInfo connectionInfo = null!; // initialized in Initialize()
        protected Version RdpVersion = null!; // initialized in Initialize()
        protected readonly FrmMain _frmMain = FrmMain.Default;
        protected bool loginComplete;
        private int _extendedReconnectAttemptsRemaining;
        private readonly System.Windows.Forms.Timer _extendedReconnectTimer;
        private bool _redirectKeys;
        private bool _alertOnIdleDisconnect;
        private bool _viewOnly;
        private bool _smartSizeBeforeFullscreen;
        protected uint DesktopScaleFactor
        {
            get
            {
                if (connectionInfo == null)
                {
                    return (uint)(ResolutionScalingFactor.Width * 100);
                }

                return connectionInfo.DesktopScaleFactor switch
                {
                    RDPDesktopScaleFactor.Scale100 => 100,
                    RDPDesktopScaleFactor.Scale125 => 125,
                    RDPDesktopScaleFactor.Scale150 => 150,
                    RDPDesktopScaleFactor.Scale200 => 200,
                    _ => (uint)(ResolutionScalingFactor.Width * 100)
                };
            }
        }
        protected uint DeviceScaleFactor
        {
            get
            {
                uint raw = (uint)(ResolutionScalingFactor.Width * 100);
                // DeviceScaleFactor only accepts 100, 140, or 180 per the RDP specification
                // (devicescalefactor:i in .rdp files / IMsRdpExtendedSettings).
                // Values outside this set (e.g. 200 at 200% DPI) are silently rejected by the
                // COM control, breaking cursor scaling on high-DPI displays (#2222).
                // Round to the nearest valid value, matching MSTSC.exe behavior.
                if (raw <= 120) return 100;
                if (raw <= 160) return 140;
                return 180;
            }
        }
        protected readonly uint Orientation;
        private AxHost AxHost => (AxHost)Control!;

        private SizeF ResolutionScalingFactor
        {
            get
            {
                // Use the DPI of the control hosting the RDP session (per-monitor aware)
                // rather than _frmMain which may be on a different monitor with different DPI.
                // Fix for #1438: incorrect smartsize on multi-monitor mixed-DPI setups.
                Control? dpiSource = InterfaceControl ?? (Control?)_frmMain;

                if (dpiSource == null || dpiSource.IsDisposed)
                {
                    return new SizeF(1f, 1f);
                }

                // 96 DPI is the baseline (100% scale)
                float scale = dpiSource.DeviceDpi / 96f;
                return new SizeF(scale, scale);
            }
        }


        #region Properties

        public virtual bool SmartSize
        {
            get
            {
                if (_rdpClient == null)
                {
                    return false;
                }

                try
                {
                    return _rdpClient.AdvancedSettings2.SmartSizing;
                }
                catch (InvalidComObjectException ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(
                        "Unable to read RDP SmartSize state because the COM client is no longer valid.",
                        ex,
                        MessageClass.WarningMsg,
                        false);
                    return false;
                }
                catch (COMException ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(
                        "Unable to read RDP SmartSize state due to a COM access error.",
                        ex,
                        MessageClass.WarningMsg,
                        false);
                    return false;
                }
            }
            protected set
            {
                if (_rdpClient == null)
                {
                    return;
                }

                try
                {
                    _rdpClient.AdvancedSettings2.SmartSizing = value;
                }
                catch (InvalidComObjectException ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(
                        "Unable to update RDP SmartSize because the COM client is no longer valid.",
                        ex,
                        MessageClass.WarningMsg,
                        false);
                }
                catch (COMException ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(
                        "Unable to update RDP SmartSize due to a COM access error.",
                        ex,
                        MessageClass.WarningMsg,
                        false);
                }
            }
        }

        public virtual bool Fullscreen
        {
            get => _rdpClient.FullScreen;
            protected set => _rdpClient.FullScreen = value;
        }

        public bool RedirectKeysEnabled => _redirectKeys;

        private bool RedirectKeys
        {
            set
            {
                _redirectKeys = value;
                try
                {
                    if (!_redirectKeys)
                    {
                        return;
                    }

                    Debug.Assert(Convert.ToBoolean(_rdpClient.SecuredSettingsEnabled));
                    IMsRdpClientSecuredSettings msRdpClientSecuredSettings = _rdpClient.SecuredSettings2;
                    msRdpClientSecuredSettings.KeyboardHookMode = 1; // Apply key combinations at the remote server.
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetRedirectKeysFailed, ex);
                }
            }
        }

        public bool LoadBalanceInfoUseUtf8 { get; set; }

        public bool ViewOnly
        {
            get => _viewOnly;
            set
            {
                if (_viewOnly == value) return;
                _viewOnly = value;

                if (_viewOnly)
                {
                    Application.AddMessageFilter(this);
                }
                else
                {
                    Application.RemoveMessageFilter(this);
                }
            }
        }

        #endregion

        #region Constructors

        public RdpProtocol()
        {
            tmrReconnect.Tick += tmrReconnect_Tick;
            _extendedReconnectTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            _extendedReconnectTimer.Tick += ExtendedReconnectTimer_Tick;
        }

        #endregion

        #region Public Methods

        protected virtual AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient6NotSafeForScripting();
        }

        public override bool Initialize()
        {
            // Keep synchronous Initialize for backward compatibility,
            // but use the same logic (minus the await).
            connectionInfo = InterfaceControl.Info;
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Requesting RDP version: {connectionInfo.RdpVersion}. Using: {RdpProtocolVersion}");
            Control = CreateActiveXRdpClientControl();
            Control.Disposed += OnControlDisposed;
            base.Initialize();

            try
            {
                if (!InitializeActiveXControl()) return false;

                RdpVersion = new Version(_rdpClient.Version);

                if (RdpVersion < Versions.RDC61) return false;

                SetRdpClientProperties();

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetPropsFailed, ex);
                return false;
            }
        }

        public override async System.Threading.Tasks.Task<bool> InitializeAsync()
        {
            connectionInfo = InterfaceControl.Info;
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Requesting RDP version: {connectionInfo.RdpVersion}. Using: {RdpProtocolVersion}");
            Control = CreateActiveXRdpClientControl();
            Control.Disposed += OnControlDisposed;
            base.Initialize();

            try
            {
                if (!await InitializeActiveXControlAsync()) return false;

                RdpVersion = new Version(_rdpClient.Version);

                if (RdpVersion < Versions.RDC61) return false;

                SetRdpClientProperties();

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetPropsFailed, ex);
                return false;
            }
        }

        private bool InitializeActiveXControl()
        {
            try
            {
                if (!Properties.OptionsStartupExitPage.Default.DisableRefocus)
                {
                    Control!.GotFocus += RdpClient_GotFocus;
                }

                // Release stuck modifier keys when the RDP control loses focus (#354).
                Control!.Leave += RdpControl_Leave;

                Control!.CreateControl();

                // ActiveX controls require the message pump to complete creation.
                // DoEvents() is unavoidable here but introduces re-entrancy risk:
                // the user can interact with the UI while the control is half-initialized.
                // The timeout guard prevents an infinite loop if creation fails silently.
                var deadline = Environment.TickCount64 + 10_000; // 10 second timeout
                while (!Control!.Created)
                {
                    if (Environment.TickCount64 > deadline)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                            "RDP ActiveX control creation timed out after 10 seconds.");
                        Control.Dispose();
                        return false;
                    }
                    Thread.Sleep(10);
                    Application.DoEvents();
                }

                _rdpClient = (MsRdpClient6NotSafeForScripting)((AxHost)Control).GetOcx()!;

                return true;
            }
            catch (COMException ex)
            {
                if (ex.Message.Contains("CLASS_E_CLASSNOTAVAILABLE", StringComparison.Ordinal))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(CultureInfo.InvariantCulture, Language.RdpProtocolVersionNotSupported, connectionInfo.RdpVersion));
                }
                else
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.RdpControlCreationFailed, ex);
                }
                Control?.Dispose();
                return false;
            }
        }

        private async System.Threading.Tasks.Task<bool> InitializeActiveXControlAsync()
        {
            try
            {
                if (!Properties.OptionsStartupExitPage.Default.DisableRefocus)
                {
                    Control!.GotFocus += RdpClient_GotFocus;
                }

                // Release stuck modifier keys when the RDP control loses focus (#354).
                Control!.Leave += RdpControl_Leave;

                Control!.CreateControl();

                var deadline = Environment.TickCount64 + 10_000;
                while (!Control!.Created)
                {
                    if (Environment.TickCount64 > deadline)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                            "RDP ActiveX control creation timed out after 10 seconds.");
                        Control.Dispose();
                        return false;
                    }

                    await System.Threading.Tasks.Task.Delay(10);
                }

                _rdpClient = (MsRdpClient6NotSafeForScripting)((AxHost)Control).GetOcx()!;

                return true;
            }
            catch (COMException ex)
            {
                if (ex.Message.Contains("CLASS_E_CLASSNOTAVAILABLE", StringComparison.Ordinal))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(CultureInfo.InvariantCulture, Language.RdpProtocolVersionNotSupported, connectionInfo.RdpVersion));
                }
                else
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.RdpControlCreationFailed, ex);
                }
                Control?.Dispose();
                return false;
            }
        }

        public override bool Connect()
        {
            loginComplete = false;
            SetEventHandlers();

            try
            {
                _rdpClient.Connect();
                base.Connect();

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionOpenFailed, ex);
            }

            return false;
        }

        public override void Disconnect()
        {
            try
            {
                _rdpClient.Disconnect();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpDisconnectFailed, ex);
                Close();
            }
        }

        public void ToggleFullscreen()
        {
            try
            {
                bool goingFullscreen = !Fullscreen;
                if (goingFullscreen)
                {
                    // Save current SmartSize state and enable SmartSize so the remote desktop
                    // content fills the entire fullscreen window, preventing black borders (#1402).
                    _smartSizeBeforeFullscreen = SmartSize;
                    SmartSize = true;
                }
                Fullscreen = goingFullscreen;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpToggleFullscreenFailed, ex);
            }
        }

        public void ToggleSmartSize()
        {
            try
            {
                SmartSize = !SmartSize;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpToggleSmartSizeFailed, ex);
            }
        }

        /// <summary>
        /// Moves a fullscreen RDP session to the specified monitor by finding the
        /// RDP container window and repositioning it with SetWindowPos.
        /// </summary>
        public void MoveFullscreenToMonitor(Screen targetScreen)
        {
            if (!Fullscreen) return;

            try
            {
                System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            await System.Threading.Tasks.Task.Delay(50);

                            if (_frmMain == null || _frmMain.IsDisposed) return;

                            bool moved = false;
                            _frmMain.Invoke((MethodInvoker)delegate
                            {
                                IntPtr hwnd = NativeMethods.GetForegroundWindow();
                                if (hwnd == IntPtr.Zero) return;

                                StringBuilder className = new StringBuilder(256);
                                _ = NativeMethods.GetClassName(hwnd, className, className.Capacity);
                                string cls = className.ToString();

                                if (cls.Contains("TscShellContainerClass", StringComparison.Ordinal) || cls.Contains("UIContainerClass", StringComparison.Ordinal))
                                {
                                    Rectangle bounds = targetScreen.Bounds;
                                    NativeMethods.SetWindowPos(hwnd, IntPtr.Zero,
                                        bounds.X, bounds.Y, bounds.Width, bounds.Height,
                                        NativeMethods.SWP_NOZORDER | NativeMethods.SWP_FRAMECHANGED | NativeMethods.SWP_NOACTIVATE);
                                    moved = true;
                                }
                            });

                            if (moved) break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Runtime.MessageCollector.AddExceptionStackTrace("Error moving RDP fullscreen to monitor", ex, MessageClass.WarningMsg, false);
                    }
                });
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error initiating RDP fullscreen monitor move", ex, MessageClass.WarningMsg, false);
            }
        }

        /// <summary>
        /// Sends Ctrl+Alt+End to the active RDP session via the IMsRdpClientNonScriptable COM API.
        /// Ctrl+Alt+End maps to Ctrl+Alt+Del on the remote host, bypassing keyboard interception
        /// by intermediate RDP sessions (works for nested/jump-box scenarios).
        /// </summary>
        public void SendCtrlAltEnd()
        {
            try
            {
                if (Control == null) return;
                var nonScriptable = (IMsRdpClientNonScriptable)((AxHost)Control).GetOcx()!;

                const int VK_CONTROL = 0x11;
                const int VK_MENU = 0x12;
                const int VK_END = 0x23;

                bool keyDown = false;
                bool keyUp = true;
                int vkCtrl = VK_CONTROL;
                int vkAlt = VK_MENU;
                int vkEnd = VK_END;

                nonScriptable.SendKeys(1, ref keyDown, ref vkCtrl);
                nonScriptable.SendKeys(1, ref keyDown, ref vkAlt);
                nonScriptable.SendKeys(1, ref keyDown, ref vkEnd);
                nonScriptable.SendKeys(1, ref keyUp, ref vkEnd);
                nonScriptable.SendKeys(1, ref keyUp, ref vkAlt);
                nonScriptable.SendKeys(1, ref keyUp, ref vkCtrl);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to send Ctrl+Alt+End to RDP session", ex);
            }
        }

        /// <summary>
        /// Toggles whether the RDP ActiveX control will capture and send input events to the remote host.
        /// The local host will continue to receive data from the remote host regardless of this setting.
        /// </summary>
        public void ToggleViewOnly()
        {
            try
            {
                ViewOnly = !ViewOnly;
            }
            catch
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Could not toggle view only mode for host {connectionInfo.Hostname}");
            }
        }

        public override void Focus()
        {
            try
            {
                if (Control is { ContainsFocus: false })
                {
                    Control.Focus();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpFocusFailed, ex);
            }
        }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (!_viewOnly || Control == null || Control.IsDisposed || !Control.Created) return false;

            // Only filter messages for the RDP control and its children
            if (m.HWnd == Control.Handle || IsChild(Control.Handle, m.HWnd))
            {
                const int WM_KEYFIRST = 0x0100;
                const int WM_KEYLAST = 0x0109;
                const int WM_MOUSEFIRST = 0x0200;
                const int WM_MOUSELAST = 0x020D;

                if ((m.Msg >= WM_KEYFIRST && m.Msg <= WM_KEYLAST) ||
                    (m.Msg >= WM_MOUSEFIRST && m.Msg <= WM_MOUSELAST))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if this version of the RDP client
        /// is supported on this machine.
        /// </summary>
        /// <returns></returns>
        public bool RdpVersionSupported()
        {
            try
            {
                using AxHost control = CreateActiveXRdpClientControl();
                control.CreateControl();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods

        protected static class Versions
        {
            // https://en.wikipedia.org/wiki/Remote_Desktop_Protocol
            public static readonly Version RDC60 = new(6, 0, 6000);
            public static readonly Version RDC61 = new(6, 0, 6001);
            public static readonly Version RDC70 = new(6, 1, 7600);
            public static readonly Version RDC80 = new(6, 2, 9200);
            public static readonly Version RDC81 = new(6, 3, 9600);
            public static readonly Version RDC100 = new(10, 0, 0);
        }

        private void SetRdpClientProperties()
        {
            // Fix for #1005: Gateway Authentication box hidden behind other windows
            try
            {
                // Use dynamic to set UIParentWindowHandle to avoid manual construction of _RemotableHandle
                // which is required by the strongly-typed interface method in the interop assembly.
                if (_rdpClient is IMsRdpClientNonScriptable3)
                {
                    ((dynamic)_rdpClient).UIParentWindowHandle = _frmMain.Handle;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to set UIParentWindowHandle for RDP client.", ex, MessageClass.WarningMsg, false);
            }

            // https://learn.microsoft.com/en-us/windows-server/remote/remote-desktop-services/clients/rdp-files

            _rdpClient.Server = connectionInfo.Hostname;

            SetCredentials();
            SetResolution();
            SetMultimon();
            _rdpClient.FullScreenTitle = connectionInfo.Name;

            _alertOnIdleDisconnect = connectionInfo.RDPAlertIdleTimeout;
            _rdpClient.AdvancedSettings2.MinutesToIdleTimeout = connectionInfo.RDPMinutesToIdleTimeout;

            #region Remote Desktop Services
            _rdpClient.SecuredSettings2.StartProgram = connectionInfo.RDPStartProgram;
            _rdpClient.SecuredSettings2.WorkDir = connectionInfo.RDPStartProgramWorkDir;
            #endregion

            //not user changeable
            _rdpClient.AdvancedSettings2.GrabFocusOnConnect = true;
            _rdpClient.AdvancedSettings3.EnableAutoReconnect = true;
            try
            {
                int reconnectCount = Settings.Default.RdpReconnectionCount;
                if (reconnectCount > 20)
                {
                    _rdpClient.AdvancedSettings3.MaxReconnectAttempts = 20;
                    _extendedReconnectAttemptsRemaining = reconnectCount - 20;
                }
                else
                {
                    _rdpClient.AdvancedSettings3.MaxReconnectAttempts = reconnectCount;
                    _extendedReconnectAttemptsRemaining = 0;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Failed to set RDP MaxReconnectAttempts to {Settings.Default.RdpReconnectionCount}. Reverting to default maximum (20).", ex, MessageClass.WarningMsg, false);
                _rdpClient.AdvancedSettings3.MaxReconnectAttempts = 20;
                _extendedReconnectAttemptsRemaining = 0;
            }
            _rdpClient.AdvancedSettings2.keepAliveInterval = 60000; //in milliseconds (10,000 = 10 seconds)
            _rdpClient.AdvancedSettings5.AuthenticationLevel = 0;
            _rdpClient.AdvancedSettings2.EncryptionEnabled = 1;

            _rdpClient.AdvancedSettings2.overallConnectionTimeout = Settings.Default.ConRDPOverallConnectionTimeout;

            _rdpClient.AdvancedSettings2.BitmapPeristence = Convert.ToInt32(connectionInfo.CacheBitmaps);

            if (RdpVersion >= Versions.RDC61)
            {
                _rdpClient.AdvancedSettings7.EnableCredSspSupport = connectionInfo.UseCredSsp;
            }
            
            SetUseConsoleSession();
            SetPort();
            RedirectKeys = connectionInfo.RedirectKeys;
            SetRedirection();
            SetAuthenticationLevel();
            SetLoadBalanceInfo();
            SetRdGateway();
            ViewOnly = Force.HasFlag(ConnectionInfo.Force.ViewOnly);

            _rdpClient.ColorDepth = (int)connectionInfo.Colors;

            SetPerformanceFlags();
            SetRdpSignature();

            _rdpClient.ConnectingText = Language.Connecting;
        }

        protected object? GetExtendedProperty(string property)
        {
            try
            {
                // ReSharper disable once UseIndexedProperty
                return ((IMsRdpExtendedSettings)_rdpClient).get_Property(property);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Error getting extended RDP property '{property}'", ex, MessageClass.WarningMsg, false);
                return null;
            }
        }

        protected void SetExtendedProperty(string property, object value, bool silent = false)
        {
            try
            {
                // ReSharper disable once UseIndexedProperty
                ((IMsRdpExtendedSettings)_rdpClient).set_Property(property, ref value);
            }
            catch (COMException ex) when (silent && ex.HResult == unchecked((int)0x8000FFFF))
            {
                // Silently ignore E_UNEXPECTED for non-essential properties (#1693).
                // Older RDP clients don't support certain extended properties like scale factors.
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Error setting extended RDP property '{property}'", ex, MessageClass.WarningMsg, false);
            }
        }

        private void SetRdGateway()
        {
            try
            {
                if (_rdpClient.TransportSettings.GatewayIsSupported == 0)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.RdpGatewayNotSupported, true);
                    return;
                }

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.RdpGatewayIsSupported, true);

                string gatewayHostname = connectionInfo.RDGatewayHostname ?? string.Empty;
                if (!ShouldApplyExplicitGatewaySettings(connectionInfo.RDGatewayUsageMethod, gatewayHostname)) return;

                // USE GATEWAY
                _rdpClient.TransportSettings.GatewayUsageMethod = (uint)connectionInfo.RDGatewayUsageMethod;
                _rdpClient.TransportSettings.GatewayHostname = gatewayHostname;
                _rdpClient.TransportSettings.GatewayProfileUsageMethod = 1; // TSC_PROXY_PROFILE_MODE_EXPLICIT
                if (connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
                {
                    _rdpClient.TransportSettings.GatewayCredsSource = 1; // TSC_PROXY_CREDS_MODE_SMARTCARD
                }

                if (RdpVersion < Versions.RDC61 || Force.HasFlag(ConnectionInfo.Force.NoCredentials)) return;

                switch (connectionInfo.RDGatewayUseConnectionCredentials)
                {
                    case RDGatewayUseConnectionCredentials.Yes:
                        _rdpClient.TransportSettings2.GatewayCredSharing = 0;
                        _rdpClient.TransportSettings2.GatewayUsername = connectionInfo.Username;
                        //_rdpClient.TransportSettings2.GatewayPassword = connectionInfo.Password.ConvertToUnsecureString();
                        _rdpClient.TransportSettings2.GatewayPassword = connectionInfo.Password;
                        _rdpClient.TransportSettings2.GatewayDomain = connectionInfo?.Domain;
                        break;
                    case RDGatewayUseConnectionCredentials.SmartCard:
                        _rdpClient.TransportSettings2.GatewayCredSharing = 0;
                        break;
                    default:
                    {
                        _rdpClient.TransportSettings2.GatewayCredSharing = 0;

                            string gwu = connectionInfo.RDGatewayUsername;
                            string gwp = connectionInfo.RDGatewayPassword;
                            string gwd = connectionInfo.RDGatewayDomain;
                            string pkey = "";

                        // access secret server api if necessary
                        if (InterfaceControl.Info.RDGatewayExternalCredentialProvider == ExternalCredentialProvider.DelineaSecretServer)
                        {
                            try
                            {
                                string RDGUserViaAPI = InterfaceControl.Info.RDGatewayUserViaAPI;
                                ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer($"{RDGUserViaAPI}", out gwu, out gwp, out gwd, out pkey);
                            }
                            catch (Exception ex)
                            {
                                Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                            }
                        }
                        else if (InterfaceControl.Info.RDGatewayExternalCredentialProvider == ExternalCredentialProvider.ClickstudiosPasswordState)
                        {
                            try
                            {
                                string RDGUserViaAPI = InterfaceControl.Info.RDGatewayUserViaAPI;
                                ExternalConnectors.CPS.PasswordstateInterface.FetchSecretFromServer($"{RDGUserViaAPI}", out gwu, out gwp, out gwd, out pkey);
                            }
                            catch (Exception ex)
                            {
                                Event_ErrorOccured(this, "Passwordstate Interface Error: " + ex.Message, 0);
                            }
                        }
                        else if (InterfaceControl.Info.RDGatewayExternalCredentialProvider == ExternalCredentialProvider.OnePassword)
                        {
                            try
                            {
                                string RDGUserViaAPI = InterfaceControl.Info.RDGatewayUserViaAPI;
                                ExternalConnectors.OP.OnePasswordCli.ReadPassword($"{RDGUserViaAPI}", out gwu, out gwp, out gwd, out pkey);
                            }
                            catch (ExternalConnectors.OP.OnePasswordCliException ex)
                            {
                                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPOnePasswordCommandLine + ": " + ex.Arguments);
                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPOnePasswordReadFailed + Environment.NewLine + ex.Message);
                            }
                        }
                        else if (InterfaceControl.Info.RDGatewayExternalCredentialProvider == ExternalCredentialProvider.PasswordSafe)
                        {
                            try
                            {
                                string RDGUserViaAPI = InterfaceControl.Info.RDGatewayUserViaAPI;
                                ExternalConnectors.PasswordSafe.PasswordSafeCli.ReadPassword($"{RDGUserViaAPI}", out gwu, out gwp, out gwd, out pkey);
                            }
                            catch (ExternalConnectors.PasswordSafe.PasswordSafeCliException ex)
                            {
                                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPPasswordSafeCommandLine + ": " + ex.Arguments);
                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPPasswordSafeReadFailed + Environment.NewLine + ex.Message);
                            }
                        }
                        else if (InterfaceControl.Info.RDGatewayExternalCredentialProvider == ExternalCredentialProvider.VaultOpenbao)
                        {
                            try {
                                if (connectionInfo.VaultOpenbaoSecretEngine == VaultOpenbaoSecretEngine.Kv)
                                    gwu = connectionInfo.RDGatewayUsername;
                                ExternalConnectors.VO.VaultOpenbao.ReadPasswordRDP((int)connectionInfo.VaultOpenbaoSecretEngine, connectionInfo.VaultOpenbaoMount, connectionInfo.VaultOpenbaoRole, ref gwu, out gwp);
                            } catch (ExternalConnectors.VO.VaultOpenbaoException ex) {
                                Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                            }
                        }


                            if (connectionInfo.RDGatewayUseConnectionCredentials != RDGatewayUseConnectionCredentials.AccessToken)
                        {
                            _rdpClient.TransportSettings2.GatewayUsername = gwu;
                            _rdpClient.TransportSettings2.GatewayPassword = gwp;
                            _rdpClient.TransportSettings2.GatewayDomain = gwd;
                        }
                        else
                        {
                            //TODO: should we check client version and throw if it is less than 7
                        }
                        
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetGatewayFailed, ex);
            }
        }

        private static bool ShouldApplyExplicitGatewaySettings(RDGatewayUsageMethod usageMethod, string gatewayHostname)
        {
            if (usageMethod == RDGatewayUsageMethod.Never)
            {
                return false;
            }

            if (usageMethod != RDGatewayUsageMethod.Detect)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(gatewayHostname))
            {
                return false;
            }

            return Uri.CheckHostName(gatewayHostname.Trim()) != UriHostNameType.Unknown;
        }

        /// <summary>
        /// When a username contains a backslash-separated domain prefix (e.g. <c>AzureAD\user@domain.com</c>
        /// or <c>CORP\john</c>), splits it into the domain part and the bare username so that the RDP
        /// control receives them as separate fields.  If the username contains no backslash the values are
        /// returned unchanged.
        /// </summary>
        internal static (string username, string domain) ParseDomainFromUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return (username, string.Empty);

            int slashIdx = username.IndexOf('\\');
            if (slashIdx < 0)
                return (username, string.Empty);

            return (username.Substring(slashIdx + 1), username.Substring(0, slashIdx));
        }

        private void SetUseConsoleSession()
        {
            try
            {
                bool value;

                if (Force.HasFlag(ConnectionInfo.Force.UseConsoleSession))
                {
                    value = true;
                }
                else if (Force.HasFlag(ConnectionInfo.Force.DontUseConsoleSession))
                {
                    value = false;
                }
                else
                {
                    value = connectionInfo.UseConsoleSession;
                }

                if (RdpVersion >= Versions.RDC61)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(CultureInfo.InvariantCulture, Language.RdpSetConsoleSwitch, RdpVersion), true);
                    _rdpClient.AdvancedSettings7.ConnectToAdministerServer = value;
                }
                else
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"{string.Format(CultureInfo.InvariantCulture, Language.RdpSetConsoleSwitch, RdpVersion)}{Environment.NewLine}No longer supported in this RDP version. Reference: https://msdn.microsoft.com/en-us/library/aa380863(v=vs.85).aspx", true);
                    // ConnectToServerConsole is deprecated
                    //https://msdn.microsoft.com/en-us/library/aa380863(v=vs.85).aspx
                    //_rdpClient.AdvancedSettings2.ConnectToServerConsole = value;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetConsoleSessionFailed, ex);
            }
        }

        private void SetCredentials()
        {
            try
            {
                if (Force.HasFlag(ConnectionInfo.Force.NoCredentials))
                {
                    return;
                }

                string userName = connectionInfo.Username ?? "";
                string domain = connectionInfo.Domain ?? "";
                string userViaApi = connectionInfo.UserViaAPI ?? "";
                string pkey = "";
                //string password = (connectionInfo?.Password?.ConvertToUnsecureString() ?? "");
                string password = connectionInfo.Password ?? "";

                // access secret server api if necessary
                if (InterfaceControl.Info.ExternalCredentialProvider == ExternalCredentialProvider.DelineaSecretServer)
                {
                    try
                    {
                        ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer($"{userViaApi}", out userName, out password, out domain, out pkey);
                    }
                    catch (Exception ex)
                    {
                        Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                    }
                }
                else if (InterfaceControl.Info.ExternalCredentialProvider == ExternalCredentialProvider.ClickstudiosPasswordState)
                {
                    try
                    {
                        ExternalConnectors.CPS.PasswordstateInterface.FetchSecretFromServer($"{userViaApi}", out userName, out password, out domain, out pkey);
                    }
                    catch (Exception ex)
                    {
                        Event_ErrorOccured(this, "Passwordstate Interface Error: " + ex.Message, 0);
                    }
                }
                else if (InterfaceControl.Info.ExternalCredentialProvider == ExternalCredentialProvider.OnePassword)
                {
                    try
                    {
                        ExternalConnectors.OP.OnePasswordCli.ReadPassword($"{userViaApi}", out userName, out password, out domain, out pkey);
                    }
                    catch (ExternalConnectors.OP.OnePasswordCliException ex)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPOnePasswordCommandLine + ": " + ex.Arguments);
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPOnePasswordReadFailed + Environment.NewLine + ex.Message);
                    }
                }
                else if (InterfaceControl.Info.ExternalCredentialProvider == ExternalCredentialProvider.PasswordSafe)
                {
                    try
                    {
                        ExternalConnectors.PasswordSafe.PasswordSafeCli.ReadPassword($"{userViaApi}", out userName, out password, out domain, out pkey);
                    }
                    catch (ExternalConnectors.PasswordSafe.PasswordSafeCliException ex)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPPasswordSafeCommandLine + ": " + ex.Arguments);
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPPasswordSafeReadFailed + Environment.NewLine + ex.Message);
                    }
                }
                else if (InterfaceControl.Info.ExternalCredentialProvider == ExternalCredentialProvider.VaultOpenbao) {
                    try {
                        if(connectionInfo.VaultOpenbaoSecretEngine == VaultOpenbaoSecretEngine.Kv)
                            userName = connectionInfo.Username ?? "";
                        ExternalConnectors.VO.VaultOpenbao.ReadPasswordRDP((int)connectionInfo.VaultOpenbaoSecretEngine, connectionInfo.VaultOpenbaoMount ?? "", connectionInfo.VaultOpenbaoRole ?? "", ref userName, out password);
                    } catch (ExternalConnectors.VO.VaultOpenbaoException ex) {
                        Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                    }
                }
                else if (InterfaceControl.Info.ExternalCredentialProvider == ExternalCredentialProvider.LAPS)
                {
                    try
                    {
                        ExternalConnectors.LAPS.LAPSHelper.QueryLAPSPassword(connectionInfo.Hostname, out userName, out password, out domain);
                    }
                    catch (ExternalConnectors.LAPS.LAPSException ex)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPLAPSQueryFailed + Environment.NewLine + ex.Message);
                    }
                }

                // If the username contains a backslash-separated domain prefix (e.g. AzureAD\user@domain.com)
                // and no explicit domain is configured, extract the prefix as the domain so the RDP
                // control receives them as separate fields and the prefix is not silently discarded.
                if (string.IsNullOrEmpty(domain))
                    (userName, domain) = ParseDomainFromUsername(userName);

                if (string.IsNullOrEmpty(userName))
                {
                    switch (Properties.OptionsCredentialsPage.Default.EmptyCredentials)
                    {
                        case "windows":
                            _rdpClient.UserName = Environment.UserName;
                            break;
                        case "custom" when !string.IsNullOrEmpty(Properties.OptionsCredentialsPage.Default.DefaultUsername):
                            _rdpClient.UserName = Properties.OptionsCredentialsPage.Default.DefaultUsername;
                            break;
                        case "custom":
                            switch (Properties.OptionsCredentialsPage.Default.ExternalCredentialProviderDefault)
                            {
                                case ExternalCredentialProvider.DelineaSecretServer:
                                    try
                                    {
                                        ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer(
                                            Properties.OptionsCredentialsPage.Default.UserViaAPIDefault, out userName, out password, out domain, out pkey);
                                    }
                                    catch (Exception ex)
                                    {
                                        Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                                    }

                                    break;
                                case ExternalCredentialProvider.ClickstudiosPasswordState:
                                    try
                                    {
                                        ExternalConnectors.CPS.PasswordstateInterface.FetchSecretFromServer(
                                            Properties.OptionsCredentialsPage.Default.UserViaAPIDefault, out userName, out password, out domain, out pkey);
                                    }
                                    catch (Exception ex)
                                    {
                                        Event_ErrorOccured(this, "Passwordstate Interface Error: " + ex.Message, 0);
                                    }

                                    break;
                                case ExternalCredentialProvider.OnePassword:
                                    try
                                    {
                                        ExternalConnectors.OP.OnePasswordCli.ReadPassword(
                                            Properties.OptionsCredentialsPage.Default.UserViaAPIDefault, out userName, out password, out domain, out pkey);
                                    }
                                    catch (ExternalConnectors.OP.OnePasswordCliException ex)
                                    {
                                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPOnePasswordCommandLine + ": " + ex.Arguments);
                                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPOnePasswordReadFailed + Environment.NewLine + ex.Message);
                                    }

                                    break;
                                case ExternalCredentialProvider.PasswordSafe:
                                    try
                                    {
                                        ExternalConnectors.PasswordSafe.PasswordSafeCli.ReadPassword(
                                            Properties.OptionsCredentialsPage.Default.UserViaAPIDefault, out userName, out password, out domain, out pkey);
                                    }
                                    catch (ExternalConnectors.PasswordSafe.PasswordSafeCliException ex)
                                    {
                                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPPasswordSafeCommandLine + ": " + ex.Arguments);
                                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPPasswordSafeReadFailed + Environment.NewLine + ex.Message);
                                    }

                                    break;
                            }

                            if (!string.IsNullOrEmpty(userName))
                            {
                                _rdpClient.UserName = userName;
                            }

                            break;
                    }
                }
                else
                {
                    _rdpClient.UserName = userName;
                }

                if (string.IsNullOrEmpty(password))
                {
                    if (string.Equals(Properties.OptionsCredentialsPage.Default.EmptyCredentials, "custom", StringComparison.Ordinal))
                    {
                        if (!string.IsNullOrEmpty(Properties.OptionsCredentialsPage.Default.DefaultPassword))
                        {
                            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
                            _rdpClient.AdvancedSettings2.ClearTextPassword = cryptographyProvider.Decrypt(Properties.OptionsCredentialsPage.Default.DefaultPassword, Runtime.EncryptionKey);
                        }
                    }
                }
                else
                {
                    _rdpClient.AdvancedSettings2.ClearTextPassword = password;
                }

                if (string.IsNullOrEmpty(domain))
                {
                    _rdpClient.Domain = Properties.OptionsCredentialsPage.Default.EmptyCredentials switch
                    {
                        "windows" => Environment.UserDomainName,
                        "custom" => Properties.OptionsCredentialsPage.Default.DefaultDomain,
                        _ => _rdpClient.Domain
                    };
                }
                else
                {
                    _rdpClient.Domain = domain;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetCredentialsFailed, ex);
            }
        }

        protected override void Resize(object sender, EventArgs e)
        {
            base.Resize(sender, e);
            if (InterfaceControl?.Info.Resolution == RDPResolutions.SmartSizeAspect)
            {
                ApplySmartSizeAspect();
            }
        }

        private void ApplySmartSizeAspect()
        {
            if (Control == null || Control.IsDisposed || InterfaceControl == null || InterfaceControl.IsDisposed || _rdpClient == null)
            {
                return;
            }

            try
            {
                // Get source resolution (set during connection)
                int sourceWidth = _rdpClient.DesktopWidth;
                int sourceHeight = _rdpClient.DesktopHeight;

                if (sourceWidth <= 0 || sourceHeight <= 0) return;

                double sourceRatio = (double)sourceWidth / sourceHeight;

                // Get available area
                int targetWidth = InterfaceControl.ClientSize.Width;
                int targetHeight = InterfaceControl.ClientSize.Height;

                if (targetWidth <= 0 || targetHeight <= 0) return;

                double targetRatio = (double)targetWidth / targetHeight;

                int newWidth, newHeight;

                if (targetRatio > sourceRatio)
                {
                    // Available area is wider than source -> height limited
                    newHeight = targetHeight;
                    newWidth = (int)(newHeight * sourceRatio);
                }
                else
                {
                    // Available area is taller than source -> width limited
                    newWidth = targetWidth;
                    newHeight = (int)(newWidth / sourceRatio);
                }

                // Only modify if needed to prevent infinite loop or jitter
                if (Control.Width != newWidth || Control.Height != newHeight || Control.Dock != DockStyle.None)
                {
                    if (Control.Dock != DockStyle.None)
                    {
                        Control.Dock = DockStyle.None;
                    }

                    int x = (targetWidth - newWidth) / 2;
                    int y = (targetHeight - newHeight) / 2;

                    Control.SetBounds(x, y, newWidth, newHeight);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Error applying SmartSize aspect ratio.", ex, MessageClass.WarningMsg, true);
            }
        }

        private void SetResolution()
        {
            try
            {
                if (InterfaceControl == null || _rdpClient == null)
                {
                    return;
                }

                uint scaleFactor;
                switch (connectionInfo.DesktopScaleFactor)
                {
                    case RDPDesktopScaleFactor.Scale100:
                        scaleFactor = 100;
                        break;
                    case RDPDesktopScaleFactor.Scale125:
                        scaleFactor = 125;
                        break;
                    case RDPDesktopScaleFactor.Scale150:
                        scaleFactor = 150;
                        break;
                    case RDPDesktopScaleFactor.Scale200:
                        scaleFactor = 200;
                        break;
                    case RDPDesktopScaleFactor.Auto:
                    default:
                        scaleFactor = DesktopScaleFactor;
                        break;
                }

                SetExtendedProperty("DesktopScaleFactor", scaleFactor, silent: true);
                SetExtendedProperty("DeviceScaleFactor", DeviceScaleFactor, silent: true);

                if (Force.HasFlag(ConnectionInfo.Force.Fullscreen))
                {
                    _rdpClient.FullScreen = true;
                    return;
                }

                // Determine sizing mode: use the new RDPSizingMode property,
                // but legacy SmartSize/SmartSizeAspect Resolution values still override.
                var sizingMode = connectionInfo.RDPSizingMode;
                bool isLegacySmartSize = connectionInfo.Resolution == RDPResolutions.SmartSize;
                bool isLegacySmartSizeAspect = connectionInfo.Resolution == RDPResolutions.SmartSizeAspect;

                if (isLegacySmartSize)
                    sizingMode = RDPSizingMode.SmartSize;
                else if (isLegacySmartSizeAspect)
                    sizingMode = RDPSizingMode.SmartSizeAspect;

                bool enableSmartSizing = sizingMode == RDPSizingMode.SmartSize ||
                                         sizingMode == RDPSizingMode.SmartSizeAspect;

                switch (InterfaceControl.Info.Resolution)
                {
                    case RDPResolutions.FitToWindow:
                    case RDPResolutions.SmartSize:
                    case RDPResolutions.SmartSizeAspect:
                        if (TrySetRdpClientDesktopSize(InterfaceControl.Size.Width, InterfaceControl.Size.Height,
                                                       $"panel size for '{InterfaceControl.Info.Resolution}'"))
                        {
                            if (enableSmartSizing)
                            {
                                _rdpClient.AdvancedSettings2.SmartSizing = true;
                            }

                            if (sizingMode == RDPSizingMode.SmartSizeAspect)
                            {
                                ApplySmartSizeAspect();
                            }
                        }

                        break;
                    case RDPResolutions.Fullscreen:
                        _rdpClient.FullScreen = true;
                        break;
                    case RDPResolutions.Custom:
                        {
                            int w = connectionInfo.ResolutionWidth;
                            int h = connectionInfo.ResolutionHeight;
                            if (!TrySetRdpClientDesktopSize(w, h, "custom resolution"))
                            {
                                TrySetRdpClientDesktopSize(InterfaceControl.Size.Width, InterfaceControl.Size.Height, "custom fallback panel size");
                            }

                            if (enableSmartSizing)
                            {
                                _rdpClient.AdvancedSettings2.SmartSizing = true;
                            }

                            if (sizingMode == RDPSizingMode.SmartSizeAspect)
                            {
                                ApplySmartSizeAspect();
                            }

                            break;
                        }
                    default:
                        {
                            System.Drawing.Rectangle resolution = connectionInfo.Resolution.GetResolutionRectangle();
                            if (TrySetRdpClientDesktopSize(resolution.Width, resolution.Height,
                                                           $"resolution preset '{InterfaceControl.Info.Resolution}'"))
                            {
                                if (enableSmartSizing)
                                {
                                    _rdpClient.AdvancedSettings2.SmartSizing = true;
                                }

                                if (sizingMode == RDPSizingMode.SmartSizeAspect)
                                {
                                    ApplySmartSizeAspect();
                                }
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetResolutionFailed, ex);
            }
        }

        private bool TrySetRdpClientDesktopSize(int width, int height, string context)
        {
            if (width <= 0 || height <= 0)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                    $"RDP resolution skipped: invalid {context}: {width}x{height}.");
                return false;
            }

            _rdpClient.DesktopWidth = width;
            _rdpClient.DesktopHeight = height;
            return true;
        }

        private void SetMultimon()
        {
            if (!connectionInfo.RDPUseMultimon) return;
            try
            {
                if (Control is AxHost axHost &&
                    axHost.GetOcx() is IMsRdpClientNonScriptable5 ns5)
                {
                    ns5.UseMultimon = true;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to enable RDP multi-monitor (UseMultimon).", ex, MessageClass.WarningMsg, false);
            }
        }

        private void SetPort()
        {
            try
            {
                if (connectionInfo.Port != (int)Defaults.Port)
                {
                    _rdpClient.AdvancedSettings2.RDPPort = connectionInfo.Port;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetPortFailed, ex);
            }
        }

        private void SetRedirection()
        {
            try
            {
                // Clipboard must be set independently of drive redirection so that a drive
                // redirection failure (#1824) cannot silently prevent clipboard from working.
                _rdpClient.AdvancedSettings6.RedirectClipboard = connectionInfo.RedirectClipboard;
                _rdpClient.AdvancedSettings2.RedirectPorts = connectionInfo.RedirectPorts;
                _rdpClient.AdvancedSettings2.RedirectPrinters = connectionInfo.RedirectPrinters;
                _rdpClient.AdvancedSettings2.RedirectSmartCards = connectionInfo.RedirectSmartCards;
                _rdpClient.SecuredSettings2.AudioRedirectionMode = (int)connectionInfo.RedirectSound;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetRedirectionFailed, ex);
            }

            try
            {
                SetDriveRedirection();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetRedirectionFailed, ex);
            }
        }

        private void SetDriveRedirection()
        {
            bool redirectAnyDrives = connectionInfo.RedirectDiskDrives != RDPDiskDrives.None;
            _rdpClient.AdvancedSettings2.RedirectDrives = redirectAnyDrives;

            if (RDPDiskDrives.None == connectionInfo.RedirectDiskDrives)
            {
                return;
            }

            if (RDPDiskDrives.All == connectionInfo.RedirectDiskDrives)
            {
                return;
            }

            if (RDPDiskDrives.Custom == connectionInfo.RedirectDiskDrives)
            {
                IMsRdpClientNonScriptable5 rdpNS5 = (IMsRdpClientNonScriptable5)((AxHost)Control!).GetOcx()!;
                for (uint i = 0; i < rdpNS5.DriveCollection.DriveCount; i++)
                {
                    IMsRdpDrive drive = rdpNS5.DriveCollection.DriveByIndex[i];
                    drive.RedirectionState = connectionInfo.RedirectDiskDrivesCustom.Contains(drive.Name.Substring(0, 1));
                }
            }
            else
            {
                // Local Drives
                IMsRdpClientNonScriptable5 rdpNS5 = (IMsRdpClientNonScriptable5)((AxHost)Control!).GetOcx()!;
                for (uint i = 0; i < rdpNS5.DriveCollection.DriveCount; i++)
                {
                    IMsRdpDrive drive = rdpNS5.DriveCollection.DriveByIndex[i];
                    drive.RedirectionState = IsLocal(drive);
                }
            }
        }

        private static bool IsLocal(IMsRdpDrive drive)
        {
            DriveInfo[] myDrives = DriveInfo.GetDrives();
            foreach (DriveInfo myDrive in myDrives)
            {
                if (myDrive.Name.Substring(0, 1).Equals(drive.Name.Substring(0, 1), StringComparison.Ordinal))
                {
                    return myDrive.DriveType == DriveType.Fixed;
                }
            }
            return false;
        }

        private void SetPerformanceFlags()
        {
            try
            {
                int pFlags = 0;
                if (connectionInfo.DisplayThemes == false)
                    pFlags += (int)RDPPerformanceFlags.DisableThemes;

                if (connectionInfo.DisplayWallpaper == false)
                    pFlags += (int)RDPPerformanceFlags.DisableWallpaper;

                if (connectionInfo.EnableFontSmoothing)
                    pFlags += (int)RDPPerformanceFlags.EnableFontSmoothing;

                if (connectionInfo.EnableDesktopComposition)
                    pFlags += (int)RDPPerformanceFlags.EnableDesktopComposition;

                if (connectionInfo.DisableFullWindowDrag)
                    pFlags += (int)RDPPerformanceFlags.DisableFullWindowDrag;

                if (connectionInfo.DisableMenuAnimations)
                    pFlags += (int)RDPPerformanceFlags.DisableMenuAnimations;

                if (connectionInfo.DisableCursorShadow)
                    pFlags += (int)RDPPerformanceFlags.DisableCursorShadow;

                if (connectionInfo.DisableCursorBlinking)
                    pFlags += (int)RDPPerformanceFlags.DisableCursorBlinking;

                _rdpClient.AdvancedSettings2.PerformanceFlags = pFlags;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetPerformanceFlagsFailed, ex);
            }
        }

        private void SetAuthenticationLevel()
        {
            try
            {
                _rdpClient.AdvancedSettings5.AuthenticationLevel = (uint)connectionInfo.RDPAuthenticationLevel;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetAuthenticationLevelFailed, ex);
            }
        }

        private void SetRdpSignature()
        {
            if (string.IsNullOrEmpty(connectionInfo.RDPSignScope) && string.IsNullOrEmpty(connectionInfo.RDPSignature))
                return;

            try
            {
                if (!string.IsNullOrEmpty(connectionInfo.RDPSignScope))
                    SetExtendedProperty("SignScope", connectionInfo.RDPSignScope);

                if (!string.IsNullOrEmpty(connectionInfo.RDPSignature))
                    SetExtendedProperty("Signature", connectionInfo.RDPSignature);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to set RDP signature properties.", ex, MessageClass.WarningMsg, false);
            }
        }

        private void SetLoadBalanceInfo()
        {
            if (string.IsNullOrEmpty(connectionInfo.LoadBalanceInfo))
            {
                return;
            }

            try
            {
                _rdpClient.AdvancedSettings2.LoadBalanceInfo = LoadBalanceInfoUseUtf8
                    ? AzureLoadBalanceInfoEncoder.Encode(connectionInfo.LoadBalanceInfo)
                    : connectionInfo.LoadBalanceInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Unable to set load balance info.", ex);
            }
        }

        public override void SendText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                if (Control == null || Control.IsDisposed) return;
                var nonScriptable = (IMsRdpClientNonScriptable)((AxHost)Control).GetOcx()!;

                bool keyDown = false;
                bool keyUp = true;

                // VK constants
                const int VK_SHIFT = 0x10;
                const int VK_CONTROL = 0x11;
                const int VK_MENU = 0x12; // Alt

                // Map VK to Scan Code
                int shiftScanCode = NativeMethods.MapVirtualKey(VK_SHIFT, 0);
                int ctrlScanCode = NativeMethods.MapVirtualKey(VK_CONTROL, 0);
                int altScanCode = NativeMethods.MapVirtualKey(VK_MENU, 0);

                foreach (char c in text)
                {
                    short vkScan = NativeMethods.VkKeyScan(c);
                    if (vkScan == -1)
                    {
                        // Fallback to SendKeys for unmappable chars
                        base.SendText(c.ToString());
                        continue;
                    }

                    int vk = vkScan & 0xFF;
                    int shiftState = (vkScan >> 8) & 0xFF;

                    int scanCode = NativeMethods.MapVirtualKey(vk, 0);
                    if (scanCode == 0)
                    {
                        base.SendText(c.ToString());
                        continue;
                    }

                    // Press modifiers
                    if ((shiftState & 1) != 0) nonScriptable.SendKeys(1, ref keyDown, ref shiftScanCode);
                    if ((shiftState & 2) != 0) nonScriptable.SendKeys(1, ref keyDown, ref ctrlScanCode);
                    if ((shiftState & 4) != 0) nonScriptable.SendKeys(1, ref keyDown, ref altScanCode);

                    // Press key
                    nonScriptable.SendKeys(1, ref keyDown, ref scanCode);
                    nonScriptable.SendKeys(1, ref keyUp, ref scanCode);

                    // Release modifiers (in reverse order)
                    if ((shiftState & 4) != 0) nonScriptable.SendKeys(1, ref keyUp, ref altScanCode);
                    if ((shiftState & 2) != 0) nonScriptable.SendKeys(1, ref keyUp, ref ctrlScanCode);
                    if ((shiftState & 1) != 0) nonScriptable.SendKeys(1, ref keyUp, ref shiftScanCode);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to send text to RDP session via direct scan codes. Falling back to SendKeys.", ex);
                base.SendText(text);
            }
        }

        protected virtual void SetEventHandlers()
        {
            try
            {
                _rdpClient.OnConnecting += RDPEvent_OnConnecting;
                _rdpClient.OnConnected += RDPEvent_OnConnected;
                _rdpClient.OnLoginComplete += RDPEvent_OnLoginComplete;
                _rdpClient.OnFatalError += RDPEvent_OnFatalError;
                _rdpClient.OnDisconnected += RDPEvent_OnDisconnected;
                _rdpClient.OnIdleTimeoutNotification += RDPEvent_OnIdleTimeoutNotification;
                _rdpClient.OnEnterFullScreenMode += RDPEvent_OnEnterFullScreenMode;
                _rdpClient.OnLeaveFullScreenMode += RDPEvent_OnLeaveFullscreenMode;
                _rdpClient.OnLogonError += RDPEvent_OnLogonError;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetEventHandlersFailed, ex);
            }
        }
        
        #endregion

        #region Private Events & Handlers

        private void RDPEvent_OnIdleTimeoutNotification()
        {
            RestoreLocalKeyboardLayout();
            Close(); //Simply close the RDP Session if the idle timeout has been triggered.

            if (!_alertOnIdleDisconnect) return;
            MessageBox.Show($@"The {connectionInfo.Name} session was disconnected due to inactivity", @"Session Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RDPEvent_OnFatalError(int errorCode)
        {
            string errorMsg = RdpErrorCodes.GetError(errorCode, connectionInfo.Hostname);
            Event_ErrorOccured(this, errorMsg, errorCode);
        }

        private void RDPEvent_OnDisconnected(int discReason)
        {
            const int UI_ERR_NORMAL_DISCONNECT = 0xB08;
            const int UI_ERR_NLA_NOT_ENABLED = 0xB09;
            const int UI_ERR_CONNECT_FAILED_DOWN = 0x1807;

            if (discReason != UI_ERR_NORMAL_DISCONNECT && _extendedReconnectAttemptsRemaining > 0)
            {
                uint extendedDisconnectReason = (uint)_rdpClient.ExtendedDisconnectReason;
                // 4 = exDiscReasonLogoff, 12 = exDiscReasonLogoffByUser
                if (extendedDisconnectReason != 4 && extendedDisconnectReason != 12)
                {
                    int attemptsToSet = Math.Min(_extendedReconnectAttemptsRemaining, 20);
                    _extendedReconnectAttemptsRemaining -= attemptsToSet;

                    try
                    {
                        _rdpClient.AdvancedSettings3.MaxReconnectAttempts = attemptsToSet;
                    }
                    catch { }

                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"Auto-reconnect exhausted. Retrying... ({_extendedReconnectAttemptsRemaining} extended attempts remaining)");

                    _extendedReconnectTimer.Start();
                    return;
                }
            }

            if (discReason != UI_ERR_NORMAL_DISCONNECT)
            {
                uint extendedDisconnectReason = (uint)_rdpClient.ExtendedDisconnectReason;
                string reason = _rdpClient.GetErrorDescription((uint)discReason, extendedDisconnectReason);
                if (discReason == UI_ERR_NLA_NOT_ENABLED || extendedDisconnectReason == (uint)UI_ERR_NLA_NOT_ENABLED)
                {
                    reason = "The remote computer requires Network Level Authentication (NLA). Enable \"Use CredSSP\" for this RDP connection and try again.";
                }
                else if (discReason == UI_ERR_CONNECT_FAILED_DOWN || extendedDisconnectReason == (uint)UI_ERR_CONNECT_FAILED_DOWN)
                {
                    reason = "The connection failed. This may be due to a domain trust issue or NLA settings. Please check your domain trust relationship and NLA settings.";
                }

                Event_Disconnected(this, reason, discReason);
            }

            if (Properties.OptionsAdvancedPage.Default.ReconnectOnDisconnect)
            {
                ReconnectGroup = new ReconnectGroup();
                ReconnectGroup.CloseClicked += Event_ReconnectGroupCloseClicked;
                ReconnectGroup.Left = (int)((double)Control!.Width / 2 - (double)ReconnectGroup.Width / 2);
                ReconnectGroup.Top = (int)((double)Control.Height / 2 - (double)ReconnectGroup.Height / 2);
                ReconnectGroup.Parent = Control;
                ReconnectGroup.Show();
                tmrReconnect.Enabled = true;
            }
            else
            {
                RestoreLocalKeyboardLayout();
                Close();
            }
        }

        private void RDPEvent_OnConnecting()
        {
            Event_Connecting(this);
        }

        private void RDPEvent_OnConnected()
        {
            try
            {
                int reconnectCount = Settings.Default.RdpReconnectionCount;
                if (reconnectCount > 20)
                {
                    _rdpClient.AdvancedSettings3.MaxReconnectAttempts = 20;
                    _extendedReconnectAttemptsRemaining = reconnectCount - 20;
                }
                else
                {
                    _extendedReconnectAttemptsRemaining = 0;
                }
            }
            catch { }

            // Release any modifier keys (Ctrl/Alt/Shift) that may have been left stuck
            // by the previous session. AltGr = Left-Ctrl + Right-Alt, so a stuck Ctrl
            // from the disconnected session prevents AltGr from working after reconnect (#1461).
            ReleaseLocalModifierKeys();

            Event_Connected(this);
        }

        private void RDPEvent_OnLoginComplete()
        {
            loginComplete = true;
        }

        private void RDPEvent_OnEnterFullScreenMode()
        {
            try
            {
                // Fix for #1294: RDP session on wrong monitor's taskbar
                // When RDP goes fullscreen, we want it to have its own taskbar button on the correct monitor.
                // We find the foreground window (which should be the RDP window) and add WS_EX_APPWINDOW style.
                
                System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        bool fixApplied = false;
                        for (int i = 0; i < 10; i++) // Try for ~1 second
                        {
                            await System.Threading.Tasks.Task.Delay(100);

                            if (_frmMain == null || _frmMain.IsDisposed) return;

                            _frmMain.Invoke((MethodInvoker)delegate
                            {
                                IntPtr hwnd = NativeMethods.GetForegroundWindow();
                                if (hwnd == IntPtr.Zero) return;

                                StringBuilder className = new StringBuilder(256);
                                _ = NativeMethods.GetClassName(hwnd, className, className.Capacity);
                                string cls = className.ToString();

                                // Check for standard RDP container classes
                                if (cls.Contains("TscShellContainerClass", StringComparison.Ordinal) || cls.Contains("UIContainerClass", StringComparison.Ordinal))
                                {
                                    int exStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
                                    bool needsAppWindow = (exStyle & NativeMethods.WS_EX_APPWINDOW) == 0;
                                    bool hasToolWindow = (exStyle & NativeMethods.WS_EX_TOOLWINDOW) != 0;
                                    // Fix for #1413: Ensure fullscreen RDP container appears in Alt+Tab
                                    // WS_EX_APPWINDOW forces the window into Alt+Tab and taskbar;
                                    // WS_EX_TOOLWINDOW must be removed as it hides the window from Alt+Tab.
                                    if (needsAppWindow || hasToolWindow)
                                    {
                                        int newStyle = (exStyle | NativeMethods.WS_EX_APPWINDOW) & ~NativeMethods.WS_EX_TOOLWINDOW;
                                        _ = NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE, newStyle);
                                    }

                                    // Fix #1402: position the fullscreen window to cover the entire monitor
                                    // so there are no black borders on any side.
                                    // Fix #192: when multi-monitor is enabled, use the virtual screen
                                    // (all monitors combined) so the RDP session spans all screens.
                                    Rectangle screenBounds;
                                    if (connectionInfo?.RDPUseMultimon == true)
                                    {
                                        screenBounds = SystemInformation.VirtualScreen;
                                    }
                                    else
                                    {
                                        Screen screen = Screen.FromControl(_frmMain);
                                        screenBounds = screen.Bounds;
                                    }
                                    NativeMethods.SetWindowPos(hwnd, IntPtr.Zero,
                                        screenBounds.X, screenBounds.Y, screenBounds.Width, screenBounds.Height,
                                        NativeMethods.SWP_NOZORDER | NativeMethods.SWP_FRAMECHANGED | NativeMethods.SWP_NOACTIVATE);

                                    // Fix #1873: stop retrying once the window is found and positioned.
                                    // Without this break the loop kept invoking SetWindowPos 10 times,
                                    // flooding the UI thread and causing a ~1 s freeze on every
                                    // fullscreen transition.
                                    fixApplied = true;
                                }
                            });

                            if (fixApplied) break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Runtime.MessageCollector.AddExceptionStackTrace("Error in RDP fullscreen taskbar fix task", ex, MessageClass.WarningMsg, false);
                    }
                });
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error initiating RDP fullscreen taskbar fix", ex, MessageClass.WarningMsg, false);
            }
        }

        private void RDPEvent_OnLeaveFullscreenMode()
        {
            Fullscreen = false;
            // Restore SmartSize to the state it was in before entering fullscreen (#1402).
            SmartSize = _smartSizeBeforeFullscreen;
            _leaveFullscreenEvent?.Invoke(this, EventArgs.Empty);

            try
            {
                if (_frmMain.WindowState == FormWindowState.Minimized && !Properties.OptionsTabsPanelsPage.Default.DoNotRestoreOnRdpMinimize)
                {
                    _frmMain.WindowState = FormWindowState.Normal;
                }

                // Clamp frmMain to working area after leaving RDP fullscreen so the title bar
                // is always reachable when switching back to the tabbed view (#1804)
                if (_frmMain.WindowState == FormWindowState.Normal)
                {
                    var bounds = _frmMain.Bounds;
                    var workingArea = Screen.GetWorkingArea(bounds);
                    if (bounds.Top < workingArea.Top)
                    {
                        bounds.Y = workingArea.Top;
                        _frmMain.Bounds = bounds;
                    }
                }

                _frmMain.Activate();
                InterfaceControl?.Parent?.Focus();
                Focus();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("RDP leave-fullscreen refocus failed", ex, MessageClass.WarningMsg, false);
            }
        }

        private void RDPEvent_OnLogonError(int lError)
        {
            try
            {
                // Transient NLA/credential UI state transitions — not actual errors.
                // These fire during normal authentication when the user interacts with
                // the Windows credential dialog. Closing for these causes #1653 (crash
                // when user clicks "More choices").
                // -1 = NTLM_CHALLENGE_SENT_TO_CLIENT
                // -2 = MORE_CHOICES_REQUESTED  ← "More choices" click fires this
                // -3 = CREDUI_SUCCESS_NOMAPPED
                // -5 = NTLMv2_CHALLENGE_SENT_TO_CLIENT
                if (lError == -1 || lError == -2 || lError == -3 || lError == -5)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"RDP credential UI state transition: {lError}", true);
                    return;
                }

                string errorMsg = $"RDP Logon Error: {lError}";
                bool isAuthFailure = false;
                // 0x2000c = Authentication failure (131084)
                if (lError == 0x2000c || lError == -2147023570) // E_ACCESSDENIED / 0x8007000E
                {
                    errorMsg = "Authentication failed";
                    isAuthFailure = true;
                }

                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, errorMsg);
                Event_ErrorOccured(this, errorMsg, lError);

                if (isAuthFailure)
                {
                    PromptToUpdatePassword();
                }

                Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionOpenFailed, ex);
            }
        }

        private void ExtendedReconnectTimer_Tick(object? sender, EventArgs e)
        {
            _extendedReconnectTimer.Stop();

            try
            {
                SetResolution();
                _rdpClient.Connect();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionOpenFailed, ex);
                Close();
            }
        }

        private void RdpClient_GotFocus(object sender, EventArgs e)
        {
            // Update the current tab tracker without redirecting Win32 keyboard focus away from
            // the RDP ActiveX control. Calling ConnectionTab.Focus() would steal focus, causing
            // keystrokes from external tools (e.g. KeePass AutoType) to be delivered to the
            // WinForms container instead of the RDP session (#1450).
            if (Control?.Parent?.Parent is ConnectionTab tab)
                TabHelper.Instance.CurrentTab = tab;
        }

        private static void RdpControl_Leave(object sender, EventArgs e)
        {
            // When focus leaves the RDP control, release any modifier keys that the
            // RDP ActiveX may have left stuck in the "pressed" state on the local machine.
            // This restores AltGr-based character input (e.g. Polish: śćźżąćłó) which
            // relies on Left-Ctrl + Right-Alt and is broken when Ctrl or Alt remain stuck
            // after switching focus away from an RDP session (#354).
            ReleaseLocalModifierKeys();
        }

        /// <summary>
        /// Sends key-up events for common modifier keys on the local machine.
        /// The RDP ActiveX control can consume modifier key-up messages (sending them to
        /// the remote server instead of the local OS), leaving Ctrl/Alt/Shift stuck in the
        /// pressed state.  Sending KEYEVENTF_KEYUP for a key that is already up is a no-op.
        /// Uses SendInput (modern API) instead of legacy keybd_event.
        /// Only releases keys that are not physically held down, to avoid breaking
        /// Shift/Ctrl/Alt input during transient focus changes (#2232).
        /// </summary>
        private static void ReleaseLocalModifierKeys()
        {
            const ushort VK_SHIFT   = 0x10;
            const ushort VK_CONTROL = 0x11;
            const ushort VK_MENU    = 0x12; // Alt / AltGr
            try
            {
                // GetAsyncKeyState returns the physical key state regardless of focus.
                // High bit set (0x8000) means the key is physically pressed right now.
                // Only release keys that are NOT physically held — this prevents
                // premature release of Shift/Ctrl/Alt during brief focus transitions
                // while preserving the fix for genuinely stuck modifier keys (#354).
                bool shiftHeld  = (NativeMethods.GetAsyncKeyState(VK_SHIFT)   & 0x8000) != 0;
                bool ctrlHeld   = (NativeMethods.GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0;
                bool altHeld    = (NativeMethods.GetAsyncKeyState(VK_MENU)    & 0x8000) != 0;

                int count = (shiftHeld ? 0 : 1) + (ctrlHeld ? 0 : 1) + (altHeld ? 0 : 1);
                if (count == 0) return;

                NativeMethods.INPUT[] inputs = new NativeMethods.INPUT[count];
                int i = 0;
                if (!shiftHeld) inputs[i++] = MakeKeyUp(VK_SHIFT);
                if (!ctrlHeld)  inputs[i++] = MakeKeyUp(VK_CONTROL);
                if (!altHeld)   inputs[i++] = MakeKeyUp(VK_MENU);

                _ = NativeMethods.SendInput((uint)inputs.Length, inputs,
                    System.Runtime.InteropServices.Marshal.SizeOf<NativeMethods.INPUT>());
            }
            catch { }
        }

        private static NativeMethods.INPUT MakeKeyUp(ushort vk) => new()
        {
            type = NativeMethods.INPUT.INPUT_KEYBOARD,
            u = new NativeMethods.INPUTUNION
            {
                ki = new NativeMethods.KEYBDINPUT
                {
                    wVk = vk,
                    dwFlags = NativeMethods.KEYBDINPUT.KEYEVENTF_KEYUP,
                    dwExtraInfo = UIntPtr.Zero,
                }
            }
        };

        private static void RestoreLocalKeyboardLayout()
        {
            // Release any stuck modifier keys before restoring the layout so that
            // AltGr-based characters (e.g. Polish: śćźżąćłó) work immediately (#354).
            ReleaseLocalModifierKeys();

            // Refresh the Windows language bar indicator after RDP session ends (#2269).
            // The RDP ActiveX control intercepts keyboard input and suppresses the language
            // bar during a session. On disconnect, re-activating the current local layout
            // forces Windows to update the indicator in the taskbar.
            try
            {
                IntPtr hkl = NativeMethods.GetKeyboardLayout(0);
                if (hkl != IntPtr.Zero)
                    NativeMethods.ActivateKeyboardLayout(hkl, 0);
            }
            catch { }
        }

        private void CleanupResources()
        {
            try
            {
                if (Control != null)
                {
                    Control.Disposed -= OnControlDisposed;
                    Control.Leave -= RdpControl_Leave;
                }

                Application.RemoveMessageFilter(this);

                _extendedReconnectTimer?.Stop();
                _extendedReconnectTimer?.Dispose();

                if (_rdpClient != null)
                {
                    // If connected, try to disconnect first
                    try
                    {
                        if (_rdpClient.Connected == 1)
                        {
                            _rdpClient.Disconnect();
                        }
                    }
                    catch { }

                    // Force release the COM object
                    try
                    {
                        int refCount = Marshal.FinalReleaseComObject(_rdpClient);
                        while (refCount > 0)
                        {
                            refCount = Marshal.FinalReleaseComObject(_rdpClient);
                        }
                    }
                    catch { }
                    finally
                    {
                        _rdpClient = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error during RDP control disposal", ex);
            }
        }

        private void OnControlDisposed(object? sender, EventArgs e)
        {
            Dispose();
        }
        #endregion

        #region Public Events & Handlers

        public delegate void LeaveFullscreenEventHandler(object sender, EventArgs e);

        private LeaveFullscreenEventHandler? _leaveFullscreenEvent;

        public event LeaveFullscreenEventHandler LeaveFullscreen
        {
            add => _leaveFullscreenEvent = (LeaveFullscreenEventHandler?)Delegate.Combine(_leaveFullscreenEvent, value);
            remove => _leaveFullscreenEvent = (LeaveFullscreenEventHandler?)Delegate.Remove(_leaveFullscreenEvent, value);
        }

        #endregion

        #region Enums

        public enum Defaults
        {
            Colors = RDPColors.Colors16Bit,
            Sounds = RDPSounds.DoNotPlay,
            Resolution = RDPResolutions.FitToWindow,
            Port = 3389
        }

        #endregion
        
        #region Reconnect Stuff

        private void tmrReconnect_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (ReconnectGroup == null) return;

                bool srvReady = PortScanner.IsPortOpen(connectionInfo.Hostname, Convert.ToString(connectionInfo.Port, CultureInfo.InvariantCulture));

                ReconnectGroup.ServerReady = srvReady;

                if (!ReconnectGroup.ReconnectWhenReady || !srvReady) return;
                tmrReconnect.Enabled = false;
                ReconnectGroup.DisposeReconnectGroup();
                SetResolution();
                _rdpClient.Connect();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    string.Format(CultureInfo.InvariantCulture, Language.AutomaticReconnectError, connectionInfo.Hostname),
                    ex, MessageClass.WarningMsg, false);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CleanupResources();
            }
            base.Dispose(disposing);
        }

    }
}
