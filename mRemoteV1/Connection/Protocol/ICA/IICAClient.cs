using mRemoteNG.UI.Window;
using System;
using System.Windows.Forms;

namespace mRemoteNG.Connection.Protocol.ICA
{
    public interface IICAClient : IDisposable
    {
        event EventHandler<EventArgs> OnConnect;

        event EventHandler<EventArgs> OnConnectFailed;

        event EventHandler<EventArgs> OnConnecting;

        event EventHandler<EventArgs> OnDisconnect;

        string Address { get; set; }
        Control Control { get; }
        bool Created { get; }
        string Domain { get; set; }
        bool Encrypt { get; set; }
        string EncryptionLevelSession { get; set; }
        object Hotkey10Char { get; set; }
        object Hotkey10Shift { get; set; }
        object Hotkey11Char { get; set; }
        object Hotkey11Shift { get; set; }
        object Hotkey1Char { get; set; }
        object Hotkey1Shift { get; set; }
        object Hotkey2Char { get; set; }
        object Hotkey2Shift { get; set; }
        object Hotkey3Char { get; set; }
        object Hotkey3Shift { get; set; }
        object Hotkey4Char { get; set; }
        object Hotkey4Shift { get; set; }
        object Hotkey5Char { get; set; }
        object Hotkey5Shift { get; set; }
        object Hotkey6Char { get; set; }
        object Hotkey6Shift { get; set; }
        object Hotkey7Char { get; set; }
        object Hotkey7Shift { get; set; }
        object Hotkey8Char { get; set; }
        object Hotkey8Shift { get; set; }
        object Hotkey9Char { get; set; }
        object Hotkey9Shift { get; set; }
        ComponentsCheckWindow Parent { get; set; }
        bool PersistentCacheEnabled { get; set; }
        string Title { get; set; }
        string Username { get; set; }
        object Version { get; }

        void Connect();

        Control CreateControl();

        void FullScreenWindow();

        void SetProp(string name, string value);

        void SetWindowSize(int width, int height);
    }
}