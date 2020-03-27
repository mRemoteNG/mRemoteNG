#if DEBUG

using mRemoteNG.UI.Window;
using System;
using System.Windows.Forms;

namespace mRemoteNG.Connection.Protocol.ICA
{
    public class DummyICAClient : IICAClient
    {
        public DummyICAClient()
        {
            throw new NotImplementedException("This version has been build with ICA Protocol disabled via compile constant 'DISABLE_ICA_PROTOCOL' this should not have happened in a release version!");
        }

        public event EventHandler<EventArgs> OnConnect;

        public event EventHandler<EventArgs> OnConnectFailed;

        public event EventHandler<EventArgs> OnConnecting;

        public event EventHandler<EventArgs> OnDisconnect;

        public string Address { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Control Control => throw new NotImplementedException();

        public bool Created => throw new NotImplementedException();
        public string Domain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Encrypt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EncryptionLevelSession { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ComponentsCheckWindow Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool PersistentCacheEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Username { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object Version => throw new NotImplementedException();

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public Control CreateControl()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void FullScreenWindow()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void SetProp(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public void SetWindowSize(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}

#endif