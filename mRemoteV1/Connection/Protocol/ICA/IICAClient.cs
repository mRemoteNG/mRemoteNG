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

        ComponentsCheckWindow Parent { get; set; }

        bool PersistentCacheEnabled { get; set; }

        string Title { get; set; }

        string Username { get; set; }

        object Version { get; }

        void Connect();

        Control CreateControl();

        void FullScreenWindow();

        void Initialize();

        void SetProp(string name, string value);

        void SetWindowSize(int width, int height);
    }
}