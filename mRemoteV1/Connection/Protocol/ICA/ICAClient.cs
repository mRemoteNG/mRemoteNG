#if (DEBUG && !DISABLE_ICA_PROTOCOL) || !DEBUG

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

            OnConnect += _icaClient.OnConnect;
            OnConnectFailed += _icaClient.OnConnectFailed;
            OnConnecting += _icaClient.OnConnecting;
            OnDisconnect += _icaClient.OnDisconnect;
        }

        public Control Control => _icaClient;

        public bool Created => _icaClient.Created;

        public string Address { get => _icaClient.Address; set => _icaClient.Address = value; }
        public string Username { get => _icaClient.Username; set => _icaClient.Username = value; }
        public string Domain { get => _icaClient.Domain; set => _icaClient.Domain = value; }
        public bool Encrypt { get => _icaClient.Encrypt; set => _icaClient.Encrypt = value; }
        public string EncryptionLevelSession { get => _icaClient.EncryptionLevelSession; set => _icaClient.EncryptionLevelSession = value; }
        public object Hotkey1Shift { get => _icaClient.Hotkey1Shift; set => _icaClient.Hotkey1Shift = value; }
        public object Hotkey1Char { get => _icaClient.Hotkey1Char; set => _icaClient.Hotkey1Char = value; }
        public object Hotkey2Shift { get => _icaClient.Hotkey2Shift; set => _icaClient.Hotkey2Shift = value; }
        public object Hotkey2Char { get => _icaClient.Hotkey2Char; set => _icaClient.Hotkey2Char = value; }
        public object Hotkey3Shift { get => _icaClient.Hotkey3Shift; set => _icaClient.Hotkey3Shift = value; }
        public object Hotkey3Char { get => _icaClient.Hotkey3Char; set => _icaClient.Hotkey3Char = value; }
        public object Hotkey4Shift { get => _icaClient.Hotkey4Shift; set => _icaClient.Hotkey4Shift = value; }
        public object Hotkey4Char { get => _icaClient.Hotkey4Char; set => _icaClient.Hotkey4Char = value; }
        public object Hotkey5Shift { get => _icaClient.Hotkey5Shift; set => _icaClient.Hotkey5Shift = value; }
        public object Hotkey5Char { get => _icaClient.Hotkey5Char; set => _icaClient.Hotkey5Char = value; }
        public object Hotkey6Shift { get => _icaClient.Hotkey6Shift; set => _icaClient.Hotkey6Shift = value; }
        public object Hotkey6Char { get => _icaClient.Hotkey6Char; set => _icaClient.Hotkey6Char = value; }
        public object Hotkey7Shift { get => _icaClient.Hotkey7Shift; set => _icaClient.Hotkey7Shift = value; }
        public object Hotkey7Char { get => _icaClient.Hotkey7Char; set => _icaClient.Hotkey7Char = value; }
        public object Hotkey8Shift { get => _icaClient.Hotkey8Shift; set => _icaClient.Hotkey8Shift = value; }
        public object Hotkey8Char { get => _icaClient.Hotkey8Char; set => _icaClient.Hotkey8Char = value; }
        public object Hotkey9Shift { get => _icaClient.Hotkey9Shift; set => _icaClient.Hotkey9Shift = value; }
        public object Hotkey9Char { get => _icaClient.Hotkey9Char; set => _icaClient.Hotkey9Char = value; }
        public object Hotkey10Shift { get => _icaClient.Hotkey10Shift; set => _icaClient.Hotkey10Shift = value; }
        public object Hotkey10Char { get => _icaClient.Hotkey10Char; set => _icaClient.Hotkey10Char = value; }
        public object Hotkey11Shift { get => _icaClient.Hotkey11Shift; set => _icaClient.Hotkey11Shift = value; }
        public object Hotkey11Char { get => _icaClient.Hotkey11Char; set => _icaClient.Hotkey11Char = value; }
        public bool PersistentCacheEnabled { get => _icaClient.PersistentCacheEnabled; set => _icaClient.PersistentCacheEnabled = value; }
        public string Title { get => _icaClient.Title; set => _icaClient.Title = value; }
        public ComponentsCheckWindow Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object Version => throw new NotImplementedException();

        public event EventHandler<EventArgs> OnConnecting;

        public event EventHandler<EventArgs> OnConnectFailed;

        public event EventHandler<EventArgs> OnDisconnect;

        public event EventHandler<EventArgs> OnConnect;

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
            _icaClient.Dispose();
        }

        public void FullScreenWindow()
        {
            _icaClient.FullScreenWindow();
        }

        public void SetProp(string name, string value)
        {
            _icaClient.SetProp(name, value);
        }

        public void SetWindowSize(int width, int height)
        {
            _icaClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, width, height, 0);
        }
    }
}

#endif