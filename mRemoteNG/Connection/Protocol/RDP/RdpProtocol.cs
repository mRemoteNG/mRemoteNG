using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using AxMSTSCLib;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using MSTSCLib;
using mRemoteNG.Resources.Language;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol6 : ProtocolBase, ISupportsViewOnly
    {
        /* RDP v8 requires Windows 7 with:
         * https://support.microsoft.com/en-us/kb/2592687
         * OR
         * https://support.microsoft.com/en-us/kb/2923545
         *
         * Windows 8+ support RDP v8 out of the box.
         */
        private MsRdpClient6NotSafeForScripting _rdpClient;
        protected ConnectionInfo connectionInfo;
        protected bool loginComplete;
        private Version _rdpVersion;
        private bool _redirectKeys;
        private bool _alertOnIdleDisconnect;
        private readonly DisplayProperties _displayProperties;
        private readonly FrmMain _frmMain = FrmMain.Default;
        protected virtual RdpVersion RdpProtocolVersion => RdpVersion.Rdc6;
        private AxHost AxHost => (AxHost)Control;

        #region Properties

        public virtual bool SmartSize
        {
            get => _rdpClient.AdvancedSettings2.SmartSizing;
            protected set => _rdpClient.AdvancedSettings2.SmartSizing = value;
        }

        public virtual bool Fullscreen
        {
            get => _rdpClient.FullScreen;
            protected set => _rdpClient.FullScreen = value;
        }

        private bool RedirectKeys
        {
/*
			get
			{
				return _redirectKeys;
			}
*/
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
                    var msRdpClientSecuredSettings = _rdpClient.SecuredSettings2;
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
            get => !AxHost.Enabled;
            set => AxHost.Enabled = !value;
        }

        #endregion

        #region Constructors

        public RdpProtocol6()
        {
            _displayProperties = new DisplayProperties();
            tmrReconnect.Elapsed += tmrReconnect_Elapsed;
        }

        #endregion

        #region Public Methods
        protected virtual AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient6NotSafeForScripting();
        }


        public override bool Initialize()
        {
            connectionInfo = InterfaceControl.Info;
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                $"Requesting RDP version: {connectionInfo.RdpVersion}. Using: {RdpProtocolVersion}");
            Control = CreateActiveXRdpClientControl();
            base.Initialize();

            try
            {
                if (!InitializeActiveXControl())
                    return false;

                _rdpVersion = new Version(_rdpClient.Version);
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
                Control.GotFocus += RdpClient_GotFocus;
                Control.CreateControl();
                while (!Control.Created)
                {
                    Thread.Sleep(0);
                    Application.DoEvents();
                }
                Control.Anchor = AnchorStyles.None;

                _rdpClient = (MsRdpClient6NotSafeForScripting)((AxHost)Control).GetOcx();
                return true;
            }
            catch (COMException ex)
            {
                if (ex.Message.Contains("CLASS_E_CLASSNOTAVAILABLE"))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                        string.Format(Language.RdpProtocolVersionNotSupported, connectionInfo.RdpVersion));
                }
                else
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.RdpControlCreationFailed, ex);
                }
                Control.Dispose();
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
                Fullscreen = !Fullscreen;
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
                if (Control.ContainsFocus == false)
                {
                    Control.Focus();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpFocusFailed, ex);
            }
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
                using (var control = CreateActiveXRdpClientControl())
                {
                    control.CreateControl();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods

        private void SetRdpClientProperties()
        {
            _rdpClient.Server = connectionInfo.Hostname;

            SetCredentials();
            SetResolution();
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
            _rdpClient.AdvancedSettings3.MaxReconnectAttempts = Settings.Default.RdpReconnectionCount;
            _rdpClient.AdvancedSettings2.keepAliveInterval = 60000; //in milliseconds (10,000 = 10 seconds)
            _rdpClient.AdvancedSettings5.AuthenticationLevel = 0;
            _rdpClient.AdvancedSettings2.EncryptionEnabled = 1;

            _rdpClient.AdvancedSettings2.overallConnectionTimeout = Settings.Default.ConRDPOverallConnectionTimeout;

            _rdpClient.AdvancedSettings2.BitmapPeristence = Convert.ToInt32(connectionInfo.CacheBitmaps);
            if (_rdpVersion >= Versions.RDC61)
            {
                _rdpClient.AdvancedSettings7.EnableCredSspSupport = connectionInfo.UseCredSsp;
            }
            if(_rdpVersion >= Versions.RDC81)
            {
                if (connectionInfo.UseRestrictedAdmin)
                    SetExtendedProperty("RestrictedLogon", true);
                else if (connectionInfo.UseRCG)
                {
                    SetExtendedProperty("DisableCredentialsDelegation", true);
                    SetExtendedProperty("RedirectedAuthentication", true);
                }
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

            _rdpClient.ConnectingText = Language.Connecting;
        }

        protected object GetExtendedProperty(string property)
        {
            try
            {
                // ReSharper disable once UseIndexedProperty
                return ((IMsRdpExtendedSettings)_rdpClient).get_Property(property);
            }
            catch (Exception e)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Error getting extended RDP property '{property}'",
                                                             e, MessageClass.WarningMsg, false);
                return null;
            }
        }

        protected void SetExtendedProperty(string property, object value)
        {
            try
            {
                // ReSharper disable once UseIndexedProperty
                ((IMsRdpExtendedSettings)_rdpClient).set_Property(property, ref value);
            }
            catch (Exception e)
            {
                Runtime.MessageCollector.AddExceptionMessage($"Error setting extended RDP property '{property}'",
                                                             e, MessageClass.WarningMsg, false);
            }
        }

        private void SetRdGateway()
        {
            try
            {
                if (_rdpClient.TransportSettings.GatewayIsSupported == 0)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.RdpGatewayNotSupported,
                                                        true);
                    return;
                }

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.RdpGatewayIsSupported,
                                                    true);

                if (connectionInfo.RDGatewayUsageMethod != RDGatewayUsageMethod.Never)
                {
                    _rdpClient.TransportSettings.GatewayUsageMethod = (uint)connectionInfo.RDGatewayUsageMethod;
                    _rdpClient.TransportSettings.GatewayHostname = connectionInfo.RDGatewayHostname;
                    _rdpClient.TransportSettings.GatewayProfileUsageMethod = 1; // TSC_PROXY_PROFILE_MODE_EXPLICIT
                    if (connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
                    {
                        _rdpClient.TransportSettings.GatewayCredsSource = 1; // TSC_PROXY_CREDS_MODE_SMARTCARD
                    }

                    if (_rdpVersion >= Versions.RDC61 && !Force.HasFlag(ConnectionInfo.Force.NoCredentials))
                    {
                        if (connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes)
                        {
                            _rdpClient.TransportSettings2.GatewayUsername = connectionInfo.Username;
                            _rdpClient.TransportSettings2.GatewayPassword = connectionInfo.Password;
                            _rdpClient.TransportSettings2.GatewayDomain = connectionInfo?.Domain;
                        }
                        else if (connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
                        {
                            _rdpClient.TransportSettings2.GatewayCredSharing = 0;
                        }
                        else
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
                            _rdpClient.TransportSettings2.GatewayUsername = gwu;
                            _rdpClient.TransportSettings2.GatewayPassword = gwp;
                            _rdpClient.TransportSettings2.GatewayDomain = gwd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetGatewayFailed, ex);
            }
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

                if (_rdpVersion >= Versions.RDC61)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                        string.Format(Language.RdpSetConsoleSwitch, _rdpVersion),
                                                        true);
                    _rdpClient.AdvancedSettings7.ConnectToAdministerServer = value;
                }
                else
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                        string.Format(Language.RdpSetConsoleSwitch, _rdpVersion) +
                                                        Environment.NewLine +
                                                        "No longer supported in this RDP version. Reference: https://msdn.microsoft.com/en-us/library/aa380863(v=vs.85).aspx",
                                                        true);
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

                var userName = connectionInfo?.Username ?? "";
                var password = connectionInfo?.Password ?? "";
                var domain = connectionInfo?.Domain ?? "";
                var UserViaAPI = connectionInfo?.UserViaAPI ?? "";
                string pkey = "";

                // access secret server api if necessary
                if (InterfaceControl.Info.ExternalCredentialProvider == ExternalCredentialProvider.DelineaSecretServer)
                {
                    try
                    {
                        ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer($"{UserViaAPI}", out userName, out password, out domain, out pkey);
                    }
                    catch (Exception ex)
                    {
                        Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                    }
                    
                }

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
                            try
                            {
                                ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer(Properties.OptionsCredentialsPage.Default.UserViaAPIDefault, out userName, out password, out domain, out pkey);
                                _rdpClient.UserName = userName;
                            }
                            catch (Exception ex)
                            {
                                Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
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
                    if (Properties.OptionsCredentialsPage.Default.EmptyCredentials == "custom")
                    {
                        if (Properties.OptionsCredentialsPage.Default.DefaultPassword != "")
                        {
                            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
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

        private void SetResolution()
        {
            try
            {
                var scaleFactor = (uint)(_displayProperties.ResolutionScalingFactor.Width * 100);
                SetExtendedProperty("DesktopScaleFactor", scaleFactor);
                SetExtendedProperty("DeviceScaleFactor", (uint)100);

                if (Force.HasFlag(ConnectionInfo.Force.Fullscreen))
                {
                    _rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(_frmMain).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(_frmMain).Bounds.Height;

                    return;
                }

                if (InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow ||
                    InterfaceControl.Info.Resolution == RDPResolutions.SmartSize)
                {
                    _rdpClient.DesktopWidth = InterfaceControl.Size.Width;
                    _rdpClient.DesktopHeight = InterfaceControl.Size.Height;

                    if (InterfaceControl.Info.Resolution == RDPResolutions.SmartSize)
                    {
                        _rdpClient.AdvancedSettings2.SmartSizing = true;
                    }
                }
                else if (InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen)
                {
                    _rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(_frmMain).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(_frmMain).Bounds.Height;
                }
                else
                {
                    var resolution = connectionInfo.Resolution.GetResolutionRectangle();
                    _rdpClient.DesktopWidth = resolution.Width;
                    _rdpClient.DesktopHeight = resolution.Height;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetResolutionFailed, ex);
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
                _rdpClient.AdvancedSettings2.RedirectDrives = connectionInfo.RedirectDiskDrives;
                _rdpClient.AdvancedSettings2.RedirectPorts = connectionInfo.RedirectPorts;
                _rdpClient.AdvancedSettings2.RedirectPrinters = connectionInfo.RedirectPrinters;
                _rdpClient.AdvancedSettings2.RedirectSmartCards = connectionInfo.RedirectSmartCards;
                _rdpClient.SecuredSettings2.AudioRedirectionMode = (int)connectionInfo.RedirectSound;
                _rdpClient.AdvancedSettings6.RedirectClipboard = connectionInfo.RedirectClipboard;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetRedirectionFailed, ex);
            }
        }

        private void SetPerformanceFlags()
        {
            try
            {
                var pFlags = 0;
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

        private void SetLoadBalanceInfo()
        {
            if (string.IsNullOrEmpty(connectionInfo.LoadBalanceInfo))
            {
                return;
            }

            try
            {
                _rdpClient.AdvancedSettings2.LoadBalanceInfo = LoadBalanceInfoUseUtf8
                    ? new AzureLoadBalanceInfoEncoder().Encode(connectionInfo.LoadBalanceInfo)
                    : connectionInfo.LoadBalanceInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Unable to set load balance info.", ex);
            }
        }

        private void SetEventHandlers()
        {
            try
            {
                _rdpClient.OnConnecting += RDPEvent_OnConnecting;
                _rdpClient.OnConnected += RDPEvent_OnConnected;
                _rdpClient.OnLoginComplete += RDPEvent_OnLoginComplete;
                _rdpClient.OnFatalError += RDPEvent_OnFatalError;
                _rdpClient.OnDisconnected += RDPEvent_OnDisconnected;
                _rdpClient.OnLeaveFullScreenMode += RDPEvent_OnLeaveFullscreenMode;
                _rdpClient.OnIdleTimeoutNotification += RDPEvent_OnIdleTimeoutNotification;
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
            Close(); //Simply close the RDP Session if the idle timeout has been triggered.

            if (!_alertOnIdleDisconnect) return;
            MessageBox.Show($"The {connectionInfo.Name} session was disconnected due to inactivity",
                            "Session Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void RDPEvent_OnFatalError(int errorCode)
        {
            var errorMsg = RdpErrorCodes.GetError(errorCode);
            Event_ErrorOccured(this, errorMsg, errorCode);
        }

        private void RDPEvent_OnDisconnected(int discReason)
        {
            const int UI_ERR_NORMAL_DISCONNECT = 0xB08;
            if (discReason != UI_ERR_NORMAL_DISCONNECT)
            {
                var reason =
                    _rdpClient.GetErrorDescription((uint)discReason, (uint)_rdpClient.ExtendedDisconnectReason);
                Event_Disconnected(this, reason, discReason);
            }

            if (Properties.OptionsAdvancedPage.Default.ReconnectOnDisconnect)
            {
                ReconnectGroup = new ReconnectGroup();
                ReconnectGroup.CloseClicked += Event_ReconnectGroupCloseClicked;
                ReconnectGroup.Left = (int)((double)Control.Width / 2 - (double)ReconnectGroup.Width / 2);
                ReconnectGroup.Top = (int)((double)Control.Height / 2 - (double)ReconnectGroup.Height / 2);
                ReconnectGroup.Parent = Control;
                ReconnectGroup.Show();
                tmrReconnect.Enabled = true;
            }
            else
            {
                Close();
            }
        }

        private void RDPEvent_OnConnecting()
        {
            Event_Connecting(this);
        }

        private void RDPEvent_OnConnected()
        {
            Event_Connected(this);
        }

        private void RDPEvent_OnLoginComplete()
        {
            loginComplete = true;
        }

        private void RDPEvent_OnLeaveFullscreenMode()
        {
            Fullscreen = false;
            _leaveFullscreenEvent?.Invoke(this, new EventArgs());
        }

        private void RdpClient_GotFocus(object sender, EventArgs e)
        {
            ((ConnectionTab)Control.Parent.Parent).Focus();
        }
        #endregion

        #region Public Events & Handlers

        public delegate void LeaveFullscreenEventHandler(object sender, EventArgs e);

        private LeaveFullscreenEventHandler _leaveFullscreenEvent;

        public event LeaveFullscreenEventHandler LeaveFullscreen
        {
            add => _leaveFullscreenEvent = (LeaveFullscreenEventHandler)Delegate.Combine(_leaveFullscreenEvent, value);
            remove =>
                _leaveFullscreenEvent = (LeaveFullscreenEventHandler)Delegate.Remove(_leaveFullscreenEvent, value);
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

        public static class Versions
        {
            public static readonly Version RDC60 = new Version(6, 0, 6000);
            public static readonly Version RDC61 = new Version(6, 0, 6001);
            public static readonly Version RDC70 = new Version(6, 1, 7600);
            public static readonly Version RDC80 = new Version(6, 2, 9200);
            public static readonly Version RDC81 = new Version(6, 3, 9600);
            public static readonly Version RDC100 = new Version(10, 0, 0);
        }

        #region Reconnect Stuff

        public void tmrReconnect_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var srvReady = PortScanner.IsPortOpen(connectionInfo.Hostname, Convert.ToString(connectionInfo.Port));

                ReconnectGroup.ServerReady = srvReady;

                if (!ReconnectGroup.ReconnectWhenReady || !srvReady) return;
                tmrReconnect.Enabled = false;
                ReconnectGroup.DisposeReconnectGroup();
                //SetProps()
                _rdpClient.Connect();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    string.Format(Language.AutomaticReconnectError, connectionInfo.Hostname),
                    ex, MessageClass.WarningMsg, false);
            }
        }

        #endregion
    }
}