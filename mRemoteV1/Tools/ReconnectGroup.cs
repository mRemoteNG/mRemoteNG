using System;
using System.Drawing;

namespace mRemoteNG.Tools
{
    public partial class ReconnectGroup
    {
        public delegate void CloseClickedEventHandler();

        private bool _ReconnectWhenReady;
        private bool _ServerReady;
        private CloseClickedEventHandler CloseClickedEvent;

        public ReconnectGroup()
        {
            InitializeComponent();
        }

        public bool ServerReady
        {
            get { return _ServerReady; }
            set
            {
                SetStatusImage(value ? Resources.HostStatus_On : Resources.HostStatus_Off);

                _ServerReady = value;
            }
        }

        public bool ReconnectWhenReady
        {
            get { return _ReconnectWhenReady; }
            set
            {
                _ReconnectWhenReady = value;
                SetCheckbox(value);
            }
        }

        private void SetStatusImage(Image Img)
        {
            if (pbServerStatus.InvokeRequired)
            {
                SetStatusImageCB d = SetStatusImage;
                ParentForm?.Invoke(d, Img);
            }
            else
            {
                pbServerStatus.Image = Img;
            }
        }

        public void chkReconnectWhenReady_CheckedChanged(object sender, EventArgs e)
        {
            _ReconnectWhenReady = chkReconnectWhenReady.Checked;
        }

        private void SetCheckbox(bool Val)
        {
            if (chkReconnectWhenReady.InvokeRequired)
            {
                SetCheckboxCB d = SetCheckbox;
                ParentForm?.Invoke(d, Val);
            }
            else
            {
                chkReconnectWhenReady.Checked = Val;
            }
        }

        public event CloseClickedEventHandler CloseClicked
        {
            add { CloseClickedEvent = (CloseClickedEventHandler) Delegate.Combine(CloseClickedEvent, value); }
            remove { CloseClickedEvent = (CloseClickedEventHandler) Delegate.Remove(CloseClickedEvent, value); }
        }


        public void btnClose_Click(object sender, EventArgs e)
        {
            if (CloseClickedEvent != null)
                CloseClickedEvent();
        }

        public void tmrAnimation_Tick(object sender, EventArgs e)
        {
            switch (lblAnimation.Text)
            {
                case "":
                    lblAnimation.Text = "»";
                    break;
                case "»":
                    lblAnimation.Text = "»»";
                    break;
                case "»»":
                    lblAnimation.Text = "»»»";
                    break;
                case "»»»":
                    lblAnimation.Text = "";
                    break;
            }
        }

        public void DisposeReconnectGroup()
        {
            if (InvokeRequired)
            {
                DisposeReconnectGroupCB d = DisposeReconnectGroup;
                ParentForm?.Invoke(d);
            }
            else
            {
                Dispose();
            }
        }

        public void ReconnectGroup_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            grpAutomaticReconnect.Text = Language.strGroupboxAutomaticReconnect;
            btnClose.Text = Language.strButtonClose;
            lblServerStatus.Text = Language.strLabelServerStatus;
            chkReconnectWhenReady.Text = Language.strCheckboxReconnectWhenReady;
        }

        private delegate void SetStatusImageCB(Image Img);

        private delegate void SetCheckboxCB(bool Val);

        private delegate void DisposeReconnectGroupCB();
    }
}