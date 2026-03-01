using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Network.Proxy;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Security;
using System.Runtime.ExceptionServices;
using System.Diagnostics;
using System.IO;

// ReSharper disable ArrangeAccessorOwnerBody


namespace mRemoteNG.Connection.Protocol.VNC
{
    /// <summary>
    /// Intercepts lock-key messages (Caps Lock, Num Lock, Scroll Lock) destined for the
    /// VncSharpCore RemoteDesktop control and sends the correct X11 keysyms instead.
    /// Without this filter, VncSharpCore's ToAscii fallback mistranslates these keys
    /// (e.g., Caps Lock → lowercase 't'). See GitHub issue #227.
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal sealed class VncLockKeyFilter : IMessageFilter
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_CHAR = 0x0102;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        private const int VK_CAPITAL = 0x14;   // Caps Lock
        private const int VK_NUMLOCK = 0x90;   // Num Lock
        private const int VK_SCROLL = 0x91;    // Scroll Lock
        private const int VK_LWIN = 0x5B;      // Left Windows key
        private const int VK_RWIN = 0x5C;      // Right Windows key

        // X11 keysym values for lock keys
        private const uint XK_Caps_Lock = 0xFFE5;
        private const uint XK_Num_Lock = 0xFF7F;
        private const uint XK_Scroll_Lock = 0xFF14;
        private const uint XK_Super_L = 0xFFEB;
        private const uint XK_Super_R = 0xFFEC;

        // X11 keysyms for modifier keys (sent on focus loss to release any stuck modifiers)
        private static readonly uint[] ModifierKeysyms =
        {
            0xFFE1, // XK_Shift_L
            0xFFE2, // XK_Shift_R
            0xFFE3, // XK_Control_L
            0xFFE4, // XK_Control_R
            0xFFE9, // XK_Alt_L
            0xFFEA, // XK_Alt_R
        };

        private readonly VncSharpCore.RemoteDesktop _remoteDesktop;
        private readonly FieldInfo? _vncField;
        private MethodInfo? _writeKeyboardEvent;

        public VncLockKeyFilter(VncSharpCore.RemoteDesktop remoteDesktop)
        {
            _remoteDesktop = remoteDesktop;
            _vncField = typeof(VncSharpCore.RemoteDesktop)
                .GetField("vnc", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Sends key-up events for all common modifier keys (Shift, Ctrl, Alt).
        /// Call this when the VNC control loses focus to prevent stuck modifier keys
        /// caused by focus-change key events not reaching the VNC server (issue #1327).
        /// Sending a key-up for a key that was never pressed is a safe no-op on X11.
        /// </summary>
        public void ReleaseAllModifiers()
        {
            var vncClient = _vncField?.GetValue(_remoteDesktop);
            if (vncClient == null) return;

            _writeKeyboardEvent ??= vncClient.GetType()
                .GetMethod("WriteKeyboardEvent", BindingFlags.Public | BindingFlags.Instance);
            if (_writeKeyboardEvent == null) return;

            foreach (var keysym in ModifierKeysyms)
            {
                try { _writeKeyboardEvent.Invoke(vncClient, new object[] { keysym, false }); }
                catch { /* best-effort; ignore if connection is already gone */ }
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            // Only intercept key messages including WM_CHAR
            if (m.Msg != WM_KEYDOWN && m.Msg != WM_KEYUP &&
                m.Msg != WM_SYSKEYDOWN && m.Msg != WM_SYSKEYUP &&
                m.Msg != WM_CHAR)
                return false;

            // Handle lock keys (existing logic)
            if (m.Msg != WM_CHAR)
            {
                int vk = m.WParam.ToInt32();

                // Only intercept lock keys
                uint keysym;
                switch (vk)
                {
                    case VK_CAPITAL:    keysym = XK_Caps_Lock;   break;
                    case VK_NUMLOCK:    keysym = XK_Num_Lock;    break;
                    case VK_SCROLL:     keysym = XK_Scroll_Lock; break;
                    case VK_LWIN:       keysym = XK_Super_L;     break;
                    case VK_RWIN:       keysym = XK_Super_R;     break;
                    default: return false;
                }

                // Only intercept when targeted at the VNC RemoteDesktop control
                if (m.HWnd != _remoteDesktop.Handle)
                    return false;

                // Resolve the VncClient on each call (it's created during Connect)
                var vncClient = _vncField?.GetValue(_remoteDesktop);
                if (vncClient == null)
                    return false;

                _writeKeyboardEvent ??= vncClient.GetType()
                    .GetMethod("WriteKeyboardEvent", BindingFlags.Public | BindingFlags.Instance);
                if (_writeKeyboardEvent == null)
                    return false;

                // Send the correct keysym via the VNC protocol
                bool pressed = m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN;
                try
                {
                    _writeKeyboardEvent.Invoke(vncClient, new object[] { keysym, pressed });
                }
                catch
                {
                    // If reflection call fails, let the message through (degraded behavior)
                    return false;
                }

                // Suppress the original message so VncSharpCore doesn't mistranslate it
                return true;
            }
            else // WM_CHAR
            {
                // Handle composed characters from dead keys (issue #1742) and Cyrillic.
                char c = (char)m.WParam.ToInt32();

                // ASCII characters (< 0x80) are handled correctly by VncSharpCore — do not intercept.
                if (c < 0x80) return false;

                // Try Cyrillic-specific X11 keysyms first (e.g. issue #227 compatibility).
                uint keysym = GetCyrillicKeysym(c);

                // For non-Cyrillic non-ASCII characters (e.g. dead-key composed results like á, é, ñ, ü, ô)
                // use the X11 Unicode keysym encoding: 0x01000000 | Unicode code point.
                // vSphere's VNC server (and most modern X11 VNC servers) fully supports this encoding.
                if (keysym == 0)
                    keysym = 0x01000000u | (uint)c;

                // Only intercept when targeted at the VNC RemoteDesktop control
                if (m.HWnd != _remoteDesktop.Handle)
                    return false;

                // Resolve the VncClient on each call (it's created during Connect)
                var vncClient = _vncField?.GetValue(_remoteDesktop);
                if (vncClient == null)
                    return false;

                _writeKeyboardEvent ??= vncClient.GetType()
                    .GetMethod("WriteKeyboardEvent", BindingFlags.Public | BindingFlags.Instance);
                if (_writeKeyboardEvent == null)
                    return false;

                try
                {
                    // For WM_CHAR, we simulate a full key press (down then up)
                    _writeKeyboardEvent.Invoke(vncClient, new object[] { keysym, true });
                    _writeKeyboardEvent.Invoke(vncClient, new object[] { keysym, false });
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        private static uint GetCyrillicKeysym(char c)
        {
            // X11 keysyms for Cyrillic (from keysymdef.h).
            // IMPORTANT: X11 keysyms follow JCUKEN keyboard order, NOT Unicode order,
            // so a linear offset formula does NOT work. Use explicit lookup.
            return c switch
            {
                // Lowercase (U+0430–U+044F)
                'а' => 0x06C1, // XK_Cyrillic_a
                'б' => 0x06C2, // XK_Cyrillic_be
                'в' => 0x06D7, // XK_Cyrillic_ve
                'г' => 0x06C7, // XK_Cyrillic_ghe
                'д' => 0x06C4, // XK_Cyrillic_de
                'е' => 0x06C5, // XK_Cyrillic_ie
                'ж' => 0x06D6, // XK_Cyrillic_zhe
                'з' => 0x06DA, // XK_Cyrillic_ze
                'и' => 0x06C9, // XK_Cyrillic_i
                'й' => 0x06CA, // XK_Cyrillic_shorti
                'к' => 0x06CB, // XK_Cyrillic_ka
                'л' => 0x06CC, // XK_Cyrillic_el
                'м' => 0x06CD, // XK_Cyrillic_em
                'н' => 0x06CE, // XK_Cyrillic_en
                'о' => 0x06CF, // XK_Cyrillic_o
                'п' => 0x06D0, // XK_Cyrillic_pe
                'р' => 0x06D2, // XK_Cyrillic_er
                'с' => 0x06D3, // XK_Cyrillic_es
                'т' => 0x06D4, // XK_Cyrillic_te
                'у' => 0x06D5, // XK_Cyrillic_u
                'ф' => 0x06C6, // XK_Cyrillic_ef
                'х' => 0x06C8, // XK_Cyrillic_ha
                'ц' => 0x06C3, // XK_Cyrillic_tse
                'ч' => 0x06DE, // XK_Cyrillic_che
                'ш' => 0x06DB, // XK_Cyrillic_sha
                'щ' => 0x06DD, // XK_Cyrillic_shcha
                'ъ' => 0x06DF, // XK_Cyrillic_hardsign
                'ы' => 0x06D9, // XK_Cyrillic_yeru
                'ь' => 0x06D8, // XK_Cyrillic_softsign
                'э' => 0x06DC, // XK_Cyrillic_e
                'ю' => 0x06C0, // XK_Cyrillic_yu
                'я' => 0x06D1, // XK_Cyrillic_ya
                // Uppercase (U+0410–U+042F)
                'А' => 0x06E1, // XK_Cyrillic_A
                'Б' => 0x06E2, // XK_Cyrillic_BE
                'В' => 0x06F7, // XK_Cyrillic_VE
                'Г' => 0x06E7, // XK_Cyrillic_GHE
                'Д' => 0x06E4, // XK_Cyrillic_DE
                'Е' => 0x06E5, // XK_Cyrillic_IE
                'Ж' => 0x06F6, // XK_Cyrillic_ZHE
                'З' => 0x06FA, // XK_Cyrillic_ZE
                'И' => 0x06E9, // XK_Cyrillic_I
                'Й' => 0x06EA, // XK_Cyrillic_SHORTI
                'К' => 0x06EB, // XK_Cyrillic_KA
                'Л' => 0x06EC, // XK_Cyrillic_EL
                'М' => 0x06ED, // XK_Cyrillic_EM
                'Н' => 0x06EE, // XK_Cyrillic_EN
                'О' => 0x06EF, // XK_Cyrillic_O
                'П' => 0x06F0, // XK_Cyrillic_PE
                'Р' => 0x06F2, // XK_Cyrillic_ER
                'С' => 0x06F3, // XK_Cyrillic_ES
                'Т' => 0x06F4, // XK_Cyrillic_TE
                'У' => 0x06F5, // XK_Cyrillic_U
                'Ф' => 0x06E6, // XK_Cyrillic_EF
                'Х' => 0x06E8, // XK_Cyrillic_HA
                'Ц' => 0x06E3, // XK_Cyrillic_TSE
                'Ч' => 0x06FE, // XK_Cyrillic_CHE
                'Ш' => 0x06FB, // XK_Cyrillic_SHA
                'Щ' => 0x06FD, // XK_Cyrillic_SHCHA
                'Ъ' => 0x06FF, // XK_Cyrillic_HARDSIGN
                'Ы' => 0x06F9, // XK_Cyrillic_YERU
                'Ь' => 0x06F8, // XK_Cyrillic_SOFTSIGN
                'Э' => 0x06FC, // XK_Cyrillic_E
                'Ю' => 0x06E0, // XK_Cyrillic_YU
                'Я' => 0x06F1, // XK_Cyrillic_YA
                // Special characters
                'ё' => 0x06A3, // XK_Cyrillic_io
                'Ё' => 0x06B3, // XK_Cyrillic_IO
                _ => 0
            };
        }
    }

    [SupportedOSPlatform("windows")]
    public class ProtocolVNC : ProtocolBase
    {
        #region Private Declarations

        private const int VncConnectTimeoutMs = 10_000;
        // Keep-alive interval (ms). Sends a periodic FramebufferUpdateRequest to prevent
        // silent TCP drops caused by firewall/NAT state-table expiry or server idle timeouts
        // (issue #678). 30 seconds is well below any real-world firewall timeout.
        private const int VncKeepAliveIntervalMs = 30_000;
        private VncSharpCore.RemoteDesktop? _vnc;
        private ConnectionInfo? _info;
        private VncLockKeyFilter? _lockKeyFilter;
        private static volatile bool _isConnectionSuccessful;
        private static ExceptionDispatchInfo? _socketexception;
        private static readonly ManualResetEvent TimeoutObject = new(false);
        private static readonly Lock _testConnectLock = new();

        private TraceListener? _traceListener;
        private StringWriter? _traceWriter;
        private ProxyTunnel? _proxyTunnel;
        private bool _reconnectAttemptInProgress;
        private readonly System.Windows.Forms.Timer _keepAliveTimer = new() { Interval = VncKeepAliveIntervalMs };

        #endregion

        #region Public Methods

        public ProtocolVNC()
        {
            Control = new VncSharpCore.RemoteDesktop();
            tmrReconnect.Tick += tmrReconnect_Tick;
            _keepAliveTimer.Tick += KeepAlive_Tick;
        }

        public override bool Initialize()
        {
            base.Initialize();

            try
            {
                _vnc = Control as VncSharpCore.RemoteDesktop;
                _info = InterfaceControl.Info;
                if (_vnc == null || _info == null) return false;
                _vnc.VncPort = _info.Port;

                // Apply VNC color depth (issue #640).
                // VncSharpCore uses BitsPerPixel (8/16/32) and Depth (3/6/8/16).
                // When ColNormal (0), leave defaults so the server's native format is used.
                var bpp = (int)_info.VNCColors;
                if (bpp > 0)
                {
                    _vnc.BitsPerPixel = bpp;
                    _vnc.Depth = bpp switch
                    {
                        8 => 8,
                        16 => 16,
                        32 => 24, // 32 bpp with 24-bit true color depth
                        _ => 0
                    };
                }

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
            SetupTraceListener();
            try
            {
                if (_vnc == null || _info == null) return false;

                (string connectHost, int connectPort) = ResolveConnectionEndpoint();
                _vnc.VncPort = connectPort;

                bool requiresProxyHandshake = UsesExplicitProxy(_info.VNCProxyType);
                bool viewOnly = _info.VNCViewOnly || Force.HasFlag(ConnectionInfo.Force.ViewOnly);
                if (requiresProxyHandshake || TestConnect(connectHost, connectPort, 5000))
                    ConnectWithTimeout(_vnc, connectHost, _info, viewOnly, VncConnectTimeoutMs);

                // Install the lock-key filter after Connect() creates the VncClient.
                // Fixes Caps Lock sending 't' instead of toggle (issue #227).
                _lockKeyFilter = new VncLockKeyFilter(_vnc);
                Application.AddMessageFilter(_lockKeyFilter);
            }
            catch (Exception ex)
            {
                DisposeProxyTunnel();
                string traceLogs = CleanupTraceListener();
                if (!string.IsNullOrWhiteSpace(traceLogs))
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                        "VNC Trace Logs for " + (_info?.Hostname ?? "Unknown") + ":" + Environment.NewLine + traceLogs,
                        false);
                }
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.ConnectionOpenFailed + Environment.NewLine +
                                                    ex.Message);
                return false;
            }

            return true;
        }

        public override void Disconnect()
        {
            try
            {
                _keepAliveTimer.Enabled = false;

                if (_lockKeyFilter != null)
                {
                    Application.RemoveMessageFilter(_lockKeyFilter);
                    _lockKeyFilter = null;
                }

                if (_vnc != null)
                {
                    _vnc.Leave -= VNCEvent_LostFocus;
                    _vnc.Disconnect();
                }

                DisposeProxyTunnel();
                CleanupTraceListener();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.VncConnectionDisconnectFailed + Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _keepAliveTimer.Enabled = false;
                _keepAliveTimer.Dispose();
                DisposeProxyTunnel();
                CleanupTraceListener();
            }
            base.Dispose(disposing);
        }

        public void SendSpecialKeys(SpecialKeys Keys)
        {
            try
            {
                if (_vnc == null) return;
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

        public static void StartChat()
        {
            Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg,
                                                "VNC chat is not supported by the current VNC library.");
        }

        public static void StartFileTransfer()
        {
            Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg,
                                                "VNC file transfer is not supported by the current VNC library.");
        }

        public void RefreshScreen()
        {
            try
            {
                _vnc?.FullScreenUpdate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    Language.VncRefreshFailed + Environment.NewLine + ex.Message,
                                                    true);
            }
        }

        #endregion

        #region Private Methods

        private void SetupTraceListener()
        {
            if (_traceListener != null) return;
            try
            {
                _traceWriter = new StringWriter();
                _traceListener = new TextWriterTraceListener(_traceWriter);
                Trace.Listeners.Add(_traceListener);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to setup VNC trace listener", ex, Messages.MessageClass.WarningMsg, true);
            }
        }

        private string CleanupTraceListener()
        {
            try
            {
                string logs = "";
                if (_traceListener != null)
                {
                    Trace.Listeners.Remove(_traceListener);
                    _traceListener.Dispose();
                    _traceListener = null;
                }
                if (_traceWriter != null)
                {
                    logs = _traceWriter.ToString();
                    _traceWriter.Dispose();
                    _traceWriter = null;
                }
                return logs;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to cleanup VNC trace listener", ex, Messages.MessageClass.WarningMsg, true);
                return string.Empty;
            }
        }

        private void SetEventHandlers()
        {
            try
            {
                if (_vnc == null) return;
                _vnc.ConnectComplete += VNCEvent_Connected;
                _vnc.ConnectionLost += VNCEvent_Disconnected;
                // Release stuck modifier keys when the VNC control loses focus (issue #1327).
                // Windows does not always deliver key-up events when focus switches away,
                // leaving Shift/Ctrl/Alt pressed on the remote until the next keystroke.
                _vnc.Leave += VNCEvent_LostFocus;
                if (_info?.VNCClipboardRedirect != false)
                    FrmMain.ClipboardChanged += VNCEvent_ClipboardChanged;
                if (!Force.HasFlag(ConnectionInfo.Force.NoCredentials))
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

        private void VNCEvent_LostFocus(object? sender, EventArgs e)
        {
            _lockKeyFilter?.ReleaseAllModifiers();
        }

        private (string Hostname, int Port) ResolveConnectionEndpoint()
        {
            DisposeProxyTunnel();

            if (_info == null)
                throw new InvalidOperationException("Connection info is not initialized.");

            if (!UsesExplicitProxy(_info.VNCProxyType))
                return (_info.Hostname, _info.Port);

            if (string.IsNullOrWhiteSpace(_info.VNCProxyIP))
                throw new InvalidOperationException("VNC proxy address is required for the selected proxy type.");

            if (_info.VNCProxyPort <= 0 || _info.VNCProxyPort > ushort.MaxValue)
                throw new InvalidOperationException("VNC proxy port is invalid.");

            IProxyClient? proxyClient = ProxyClientFactory.Create(
                _info.VNCProxyType,
                _info.VNCProxyIP,
                _info.VNCProxyPort,
                _info.VNCProxyUsername,
                _info.VNCProxyPassword);

            if (proxyClient == null)
                return (_info.Hostname, _info.Port);

            TcpClient proxiedClient = proxyClient.Connect(_info.Hostname, _info.Port, 5000);
            _proxyTunnel = new ProxyTunnel(proxiedClient);

            return (IPAddress.Loopback.ToString(), _proxyTunnel.LocalPort);
        }

        private static bool UsesExplicitProxy(ProxyType proxyType)
        {
            return proxyType == ProxyType.ProxyHTTP ||
                   proxyType == ProxyType.ProxySocks4 ||
                   proxyType == ProxyType.ProxySocks5;
        }

        private static (string Hostname, int Port) GetReconnectProbeEndpoint(ConnectionInfo info)
        {
            if (UsesExplicitProxy(info.VNCProxyType) &&
                !string.IsNullOrWhiteSpace(info.VNCProxyIP) &&
                info.VNCProxyPort > 0)
            {
                return (info.VNCProxyIP, info.VNCProxyPort);
            }

            return (info.Hostname, info.Port);
        }

        private void DisposeProxyTunnel()
        {
            _proxyTunnel?.Dispose();
            _proxyTunnel = null;
        }

        private sealed class ProxyTunnel : IDisposable
        {
            private readonly TcpListener _listener;
            private readonly TcpClient _proxiedClient;
            private readonly CancellationTokenSource _cancellationTokenSource = new();
            private TcpClient? _localClient;
            private bool _disposed;

            public ProxyTunnel(TcpClient proxiedClient)
            {
                _proxiedClient = proxiedClient;
                _listener = new TcpListener(IPAddress.Loopback, 0);
                _listener.Start(1);
                _ = Task.Run(AcceptAndBridgeAsync);
            }

            public int LocalPort => ((IPEndPoint)_listener.LocalEndpoint).Port;

            private async Task AcceptAndBridgeAsync()
            {
                try
                {
                    _localClient = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);

                    using NetworkStream localStream = _localClient.GetStream();
                    using NetworkStream remoteStream = _proxiedClient.GetStream();

                    Task upstream = PumpAsync(localStream, remoteStream, _cancellationTokenSource.Token);
                    Task downstream = PumpAsync(remoteStream, localStream, _cancellationTokenSource.Token);

                    await Task.WhenAny(upstream, downstream).ConfigureAwait(false);
                }
                catch (ObjectDisposedException) when (_cancellationTokenSource.IsCancellationRequested)
                {
                    // Expected when disposed during accept/bridge.
                }
                catch (SocketException) when (_cancellationTokenSource.IsCancellationRequested)
                {
                    // Expected when disposed during accept/bridge.
                }
                catch (OperationCanceledException)
                {
                    // Expected on cancellation.
                }
                finally
                {
                    Dispose();
                }
            }

            private static async Task PumpAsync(Stream source, Stream destination, CancellationToken cancellationToken)
            {
                byte[] buffer = new byte[81920];
                while (!cancellationToken.IsCancellationRequested)
                {
                    int read = await source.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken)
                                           .ConfigureAwait(false);
                    if (read <= 0) break;

                    await destination.WriteAsync(buffer.AsMemory(0, read), cancellationToken)
                                     .ConfigureAwait(false);
                    await destination.FlushAsync(cancellationToken).ConfigureAwait(false);
                }
            }

            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;

                _cancellationTokenSource.Cancel();

                try { _listener.Stop(); } catch { /* best effort */ }
                try { _localClient?.Close(); } catch { /* best effort */ }
                try { _proxiedClient.Close(); } catch { /* best effort */ }

                _cancellationTokenSource.Dispose();
            }
        }

        /// <summary>
        /// Runs the blocking VncSharpCore Connect() on a background thread with a timeout
        /// so that unreachable VNC servers fail fast instead of freezing the UI (issue #636).
        /// </summary>
        private static void ConnectWithTimeout(VncSharpCore.RemoteDesktop vnc, string hostName, ConnectionInfo info, bool viewOnly, int timeoutMs)
        {
            Exception? connectException = null;
            var connectTask = Task.Run(() =>
            {
                try
                {
                    ConnectRemoteDesktop(vnc, hostName, info, viewOnly);
                }
                catch (Exception ex)
                {
                    connectException = ex;
                }
            });

            if (!connectTask.Wait(timeoutMs))
            {
                // Timed out — force-disconnect to unblock the background thread
                try { vnc.Disconnect(); } catch { /* best-effort cleanup */ }
                throw new TimeoutException(
                    $"VNC connection to {info.Hostname}:{info.Port} timed out after {timeoutMs / 1000} seconds.");
            }

            if (connectException != null)
                ExceptionDispatchInfo.Capture(connectException).Throw();
        }

        /// <summary>
        /// Connects using the best available VncSharpCore overload while forcing shared sessions on
        /// servers that support it (issue #1943). Falls back to the legacy overload behavior if
        /// no explicit shared-session overload is available.
        /// </summary>
        private static void ConnectRemoteDesktop(VncSharpCore.RemoteDesktop vnc, string hostName, ConnectionInfo info, bool viewOnly)
        {
            bool smartSizeEnabled = info.VNCSmartSizeMode != SmartSizeMode.SmartSNo;

            // Preferred: Connect(host, display, viewOnly, scaled, shared)
            MethodInfo? connectWithSharedAndDisplay = typeof(VncSharpCore.RemoteDesktop).GetMethod(
                "Connect",
                BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                types: new[] { typeof(string), typeof(int), typeof(bool), typeof(bool), typeof(bool) },
                modifiers: null);

            if (connectWithSharedAndDisplay != null)
            {
                InvokeConnectMethod(connectWithSharedAndDisplay, vnc, new object[] { hostName, 0, viewOnly, smartSizeEnabled, true });
                return;
            }

            // Preferred: Connect(host, viewOnly, scaled, shared)
            MethodInfo? connectWithShared = typeof(VncSharpCore.RemoteDesktop).GetMethod(
                "Connect",
                BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                types: new[] { typeof(string), typeof(bool), typeof(bool), typeof(bool) },
                modifiers: null);

            if (connectWithShared != null)
            {
                InvokeConnectMethod(connectWithShared, vnc, new object[] { hostName, viewOnly, smartSizeEnabled, true });
                return;
            }

            // Older builds may expose Connect(host, display, viewOnly, bool).
            // If that bool is named "shared", force shared mode; otherwise preserve SmartSize behavior.
            MethodInfo? connectWithDisplay = typeof(VncSharpCore.RemoteDesktop).GetMethod(
                "Connect",
                BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                types: new[] { typeof(string), typeof(int), typeof(bool), typeof(bool) },
                modifiers: null);

            if (connectWithDisplay != null)
            {
                bool fourthArg = smartSizeEnabled;
                ParameterInfo[] parameters = connectWithDisplay.GetParameters();
                if (parameters.Length == 4 &&
                    string.Equals(parameters[3].Name, "shared", StringComparison.OrdinalIgnoreCase))
                {
                    fourthArg = true;
                }

                InvokeConnectMethod(connectWithDisplay, vnc, new object[] { hostName, 0, viewOnly, fourthArg });
                return;
            }

            // Older builds expose only Connect(host, viewOnly, bool).
            // If that bool is named "shared", force shared mode; otherwise preserve SmartSize behavior.
            MethodInfo? connectThreeArgs = typeof(VncSharpCore.RemoteDesktop).GetMethod(
                "Connect",
                BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                types: new[] { typeof(string), typeof(bool), typeof(bool) },
                modifiers: null);

            if (connectThreeArgs != null)
            {
                bool thirdArg = smartSizeEnabled;
                ParameterInfo[] parameters = connectThreeArgs.GetParameters();
                if (parameters.Length == 3 &&
                    string.Equals(parameters[2].Name, "shared", StringComparison.OrdinalIgnoreCase))
                {
                    thirdArg = true;
                }

                InvokeConnectMethod(connectThreeArgs, vnc, new object[] { hostName, viewOnly, thirdArg });
                return;
            }

            // Last-resort fallback (existing behavior).
            vnc.Connect(hostName, viewOnly, smartSizeEnabled);
        }

        private static void InvokeConnectMethod(MethodInfo connectMethod, VncSharpCore.RemoteDesktop vnc, object[] args)
        {
            try
            {
                connectMethod.Invoke(vnc, args);
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
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
            TcpClient? tcpclient = null;
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
            tmrReconnect.Enabled = false;
            _reconnectAttemptInProgress = false;
            if (ReconnectGroup != null)
            {
                if (!ReconnectGroup.IsDisposed)
                {
                    ReconnectGroup.DisposeReconnectGroup();
                }
                ReconnectGroup = null;
            }

            // Start keep-alive to prevent silent TCP drops through firewalls/NAT and
            // to prevent VNC server idle timeouts (issue #678).
            _keepAliveTimer.Enabled = true;

            Event_Connected(this);
            if (_vnc != null && _info != null)
                _vnc.AutoScroll = _info.VNCSmartSizeMode == SmartSizeMode.SmartSNo;
        }

        private void VNCEvent_Disconnected(object sender, EventArgs e)
        {
            _keepAliveTimer.Enabled = false;
            _reconnectAttemptInProgress = false;
            DisposeProxyTunnel();

            if (_lockKeyFilter != null)
            {
                Application.RemoveMessageFilter(_lockKeyFilter);
                _lockKeyFilter = null;
            }

            FrmMain.ClipboardChanged -= VNCEvent_ClipboardChanged;

            string logs = CleanupTraceListener();
            string msg = "VncSharpCore Disconnected.";
            if (!string.IsNullOrWhiteSpace(logs))
            {
                msg += Environment.NewLine + "Check Messages panel for details.";
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg, "VNC Trace Logs for " + (_info?.Hostname ?? "Unknown") + ":" + Environment.NewLine + logs, false);
            }

            Event_Disconnected(sender is ProtocolBase ? sender : this, msg, null);

            if (Properties.OptionsAdvancedPage.Default.ReconnectOnDisconnect)
            {
                if (ReconnectGroup == null || ReconnectGroup.IsDisposed)
                {
                    ReconnectGroup = new ReconnectGroup();
                    ReconnectGroup.CloseClicked += Event_ReconnectGroupCloseClicked;
                    ReconnectGroup.Left = (int)((double)Control!.Width / 2 - (double)ReconnectGroup.Width / 2);
                    ReconnectGroup.Top = (int)((double)Control.Height / 2 - (double)ReconnectGroup.Height / 2);
                    ReconnectGroup.Parent = Control;
                    ReconnectGroup.Show();
                }

                tmrReconnect.Enabled = true;
            }
            else
            {
                Close();
            }
        }

        private void tmrReconnect_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (ReconnectGroup == null || _vnc == null || _info == null || _reconnectAttemptInProgress) return;

                // Mark in-progress immediately on the UI thread to prevent re-entry.
                _reconnectAttemptInProgress = true;

                // Run the port probe (and reconnect if needed) on a background thread so that
                // PortScanner.IsPortOpen — which uses a blocking TcpClient with no timeout —
                // cannot freeze the UI when the VNC server (e.g. a VirtualBox VM) is temporarily
                // unreachable (OS TCP connect timeout on Windows is up to 20 seconds).
                var capturedInfo = _info;
                Task.Run(() => ProbeAndReconnectBG(capturedInfo));
            }
            catch (Exception ex)
            {
                _reconnectAttemptInProgress = false;
                DisposeProxyTunnel();
                Runtime.MessageCollector.AddExceptionMessage(
                    string.Format(CultureInfo.InvariantCulture, Language.AutomaticReconnectError, _info?.Hostname),
                    ex, Messages.MessageClass.WarningMsg, false);
            }
        }

        /// <summary>
        /// Runs on a background thread: probes the VNC server port and, if reachable and
        /// auto-reconnect is enabled, attempts to re-establish the VNC connection.
        /// Keeping these blocking network calls off the UI thread prevents the application
        /// from appearing frozen when the server is temporarily unreachable.
        /// </summary>
        private void ProbeAndReconnectBG(ConnectionInfo capturedInfo)
        {
            try
            {
                (string probeHost, int probePort) = GetReconnectProbeEndpoint(capturedInfo);
                bool srvReady = PortScanner.IsPortOpen(probeHost, Convert.ToString(probePort, CultureInfo.InvariantCulture));

                // ServerReady setter already handles InvokeRequired internally.
                if (ReconnectGroup != null)
                    ReconnectGroup.ServerReady = srvReady;

                if (!srvReady || ReconnectGroup == null || !ReconnectGroup.ReconnectWhenReady)
                {
                    _reconnectAttemptInProgress = false;
                    return;
                }

                // Server is reachable and reconnect is requested.
                // ConnectWithTimeout blocks this background thread — not the UI — for up to
                // VncConnectTimeoutMs while the VNC handshake completes.
                SetupTraceListener();
                (string connectHost, int connectPort) = ResolveConnectionEndpoint();
                if (_vnc == null || _info == null) { _reconnectAttemptInProgress = false; return; }
                _vnc.VncPort = connectPort;
                bool viewOnly = _info.VNCViewOnly || Force.HasFlag(ConnectionInfo.Force.ViewOnly);
                ConnectWithTimeout(_vnc, connectHost, capturedInfo, viewOnly, VncConnectTimeoutMs);

                // Reconnect succeeded — perform UI-thread-only cleanup via BeginInvoke.
                var control = Control;
                if (control != null && !control.IsDisposed)
                {
                    control.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            tmrReconnect.Enabled = false;
                            if (ReconnectGroup != null && !ReconnectGroup.IsDisposed)
                                ReconnectGroup.DisposeReconnectGroup();
                            ReconnectGroup = null;

                            if (_vnc != null)
                            {
                                _lockKeyFilter = new VncLockKeyFilter(_vnc);
                                Application.AddMessageFilter(_lockKeyFilter);
                            }
                        }
                        finally
                        {
                            _reconnectAttemptInProgress = false;
                        }
                    }));
                }
                else
                {
                    _reconnectAttemptInProgress = false;
                }
            }
            catch (Exception ex)
            {
                _reconnectAttemptInProgress = false;
                DisposeProxyTunnel();
                Runtime.MessageCollector.AddExceptionMessage(
                    string.Format(CultureInfo.InvariantCulture, Language.AutomaticReconnectError, capturedInfo.Hostname),
                    ex, Messages.MessageClass.WarningMsg, false);
            }
        }

        /// <summary>
        /// Fires every <see cref="VncKeepAliveIntervalMs"/> ms while connected.
        /// Sends a non-incremental FramebufferUpdateRequest so the TCP connection is not
        /// silently dropped by stateful firewalls or VNC server idle-timeout logic (issue #678).
        /// </summary>
        private void KeepAlive_Tick(object? sender, EventArgs e)
        {
            try
            {
                _vnc?.FullScreenUpdate();
            }
            catch
            {
                // Best-effort — if the connection is already gone, ConnectionLost will fire.
            }
        }

        private void VNCEvent_ClipboardChanged()
        {
            _vnc?.FillServerClipboard();
        }

        private string VNCEvent_Authenticate()
        {
            //return _info.Password.ConvertToUnsecureString();
            return _info?.Password ?? string.Empty;
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

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Socks4))]
            ProxySocks4,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Socks4a))]
            ProxySocks4a,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.Socks5))]
            ProxySocks5,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.UltraVncRepeater))]
            ProxyUltra
        }

        public enum Colors
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.Normal))]
            ColNormal = 0,
            [Description("8-bit")] Col8Bit = 8,
            [Description("16-bit")] Col16Bit = 16,
            [Description("32-bit")] Col32Bit = 32
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
