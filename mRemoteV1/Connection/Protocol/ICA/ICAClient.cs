#if (DEBUG && !DISABLE_ICA_PROTOCOL) || !DEBUG

using AxWFICALib;
using mRemoteNG.UI.Window;
using System;
using System.Windows.Forms;

namespace mRemoteNG.Connection.Protocol.ICA
{
    public class ICAClient : IICAClient
    {
        private AxICAClient _icaClient;

        public ICAClient()
        {
            _icaClient = new AxICAClient();

            _icaClient.OnConnect += OnConnectHandler;
            _icaClient.OnConnectFailed += OnConnectFailedHandler;
            _icaClient.OnConnecting += OnConnectingHandler;
            _icaClient.OnDisconnect += OnDisconnectHandler;
        }

        public event EventHandler<EventArgs> OnConnect;

        public event EventHandler<EventArgs> OnConnectFailed;

        public event EventHandler<EventArgs> OnConnecting;

        public event EventHandler<EventArgs> OnDisconnect;

        public string Address { get => _icaClient.Address; set => _icaClient.Address = value; }

        public Control Control => _icaClient;

        public bool Created => _icaClient.Created;

        public string Domain { get => _icaClient.Domain; set => _icaClient.Domain = value; }

        public bool Encrypt { get => _icaClient.Encrypt; set => _icaClient.Encrypt = value; }

        public string EncryptionLevelSession { get => _icaClient.EncryptionLevelSession; set => _icaClient.EncryptionLevelSession = value; }

        public string Hotkey10Char { get => _icaClient.Hotkey10Char; set => _icaClient.Hotkey10Char = value; }

        public string Hotkey10Shift { get => _icaClient.Hotkey10Shift; set => _icaClient.Hotkey10Shift = value; }

        public string Hotkey11Char { get => _icaClient.Hotkey11Char; set => _icaClient.Hotkey11Char = value; }

        public string Hotkey11Shift { get => _icaClient.Hotkey11Shift; set => _icaClient.Hotkey11Shift = value; }

        public string Hotkey1Char { get => _icaClient.Hotkey1Char; set => _icaClient.Hotkey1Char = value; }

        public string Hotkey1Shift { get => _icaClient.Hotkey1Shift; set => _icaClient.Hotkey1Shift = value; }

        public string Hotkey2Char { get => _icaClient.Hotkey2Char; set => _icaClient.Hotkey2Char = value; }

        public string Hotkey2Shift { get => _icaClient.Hotkey2Shift; set => _icaClient.Hotkey2Shift = value; }

        public string Hotkey3Char { get => _icaClient.Hotkey3Char; set => _icaClient.Hotkey3Char = value; }

        public string Hotkey3Shift { get => _icaClient.Hotkey3Shift; set => _icaClient.Hotkey3Shift = value; }

        public string Hotkey4Char { get => _icaClient.Hotkey4Char; set => _icaClient.Hotkey4Char = value; }

        public string Hotkey4Shift { get => _icaClient.Hotkey4Shift; set => _icaClient.Hotkey4Shift = value; }

        public string Hotkey5Char { get => _icaClient.Hotkey5Char; set => _icaClient.Hotkey5Char = value; }

        public string Hotkey5Shift { get => _icaClient.Hotkey5Shift; set => _icaClient.Hotkey5Shift = value; }

        public string Hotkey6Char { get => _icaClient.Hotkey6Char; set => _icaClient.Hotkey6Char = value; }

        public string Hotkey6Shift { get => _icaClient.Hotkey6Shift; set => _icaClient.Hotkey6Shift = value; }

        public string Hotkey7Char { get => _icaClient.Hotkey7Char; set => _icaClient.Hotkey7Char = value; }

        public string Hotkey7Shift { get => _icaClient.Hotkey7Shift; set => _icaClient.Hotkey7Shift = value; }

        public string Hotkey8Char { get => _icaClient.Hotkey8Char; set => _icaClient.Hotkey8Char = value; }

        public string Hotkey8Shift { get => _icaClient.Hotkey8Shift; set => _icaClient.Hotkey8Shift = value; }

        public string Hotkey9Char { get => _icaClient.Hotkey9Char; set => _icaClient.Hotkey9Char = value; }

        public string Hotkey9Shift { get => _icaClient.Hotkey9Shift; set => _icaClient.Hotkey9Shift = value; }

        public ComponentsCheckWindow Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool PersistentCacheEnabled { get => _icaClient.PersistentCacheEnabled; set => _icaClient.PersistentCacheEnabled = value; }

        public string Title { get => _icaClient.Title; set => _icaClient.Title = value; }

        public string Username { get => _icaClient.Username; set => _icaClient.Username = value; }

        public object Version => throw new NotImplementedException();

        public void Connect()
        {
            _icaClient.Connect();
        }

        public Control CreateControl()
        {
            _icaClient.CreateControl();
            return _icaClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void FullScreenWindow()
        {
            _icaClient.FullScreenWindow();
        }

        public void Initialize()
        {
            //Disable hotkeys for international users
            _icaClient.Hotkey1Shift = null;
            _icaClient.Hotkey1Char = null;
            _icaClient.Hotkey2Shift = null;
            _icaClient.Hotkey2Char = null;
            _icaClient.Hotkey3Shift = null;
            _icaClient.Hotkey3Char = null;
            _icaClient.Hotkey4Shift = null;
            _icaClient.Hotkey4Char = null;
            _icaClient.Hotkey5Shift = null;
            _icaClient.Hotkey5Char = null;
            _icaClient.Hotkey6Shift = null;
            _icaClient.Hotkey6Char = null;
            _icaClient.Hotkey7Shift = null;
            _icaClient.Hotkey7Char = null;
            _icaClient.Hotkey8Shift = null;
            _icaClient.Hotkey8Char = null;
            _icaClient.Hotkey9Shift = null;
            _icaClient.Hotkey9Char = null;
            _icaClient.Hotkey10Shift = null;
            _icaClient.Hotkey10Char = null;
            _icaClient.Hotkey11Shift = null;
            _icaClient.Hotkey11Char = null;
        }

        public void SetProp(string name, string value)
        {
            _icaClient.SetProp(name, value);
        }

        public void SetWindowSize(int width, int height)
        {
            _icaClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, width, height, 0);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _icaClient.OnConnect += OnConnectHandler;
                _icaClient.OnConnectFailed += OnConnectFailedHandler;
                _icaClient.OnConnecting += OnConnectingHandler;
                _icaClient.OnDisconnect += OnDisconnectHandler;

                _icaClient.Dispose();
                _icaClient = null;
            }
        }

        private void OnConnectFailedHandler(object sender, EventArgs e)
        {
            OnConnectFailed?.Invoke(sender, e);
        }

        private void OnConnectHandler(object sender, EventArgs e)
        {
            OnConnect?.Invoke(sender, e);
        }

        private void OnConnectingHandler(object sender, EventArgs e)
        {
            OnConnecting?.Invoke(sender, e);
        }

        private void OnDisconnectHandler(object sender, EventArgs e)
        {
            OnDisconnect?.Invoke(sender, e);
        }
    }
}

#endif