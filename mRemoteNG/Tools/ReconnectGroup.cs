using System;
using System.Drawing;
using Google.Protobuf;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Tools
{
    public partial class ReconnectGroup
    {
        public ReconnectGroup()
        {
            InitializeComponent();
            chkReconnectWhenReady.Checked = Properties.OptionsAdvancedPage.Default.NoReconnect;
        }

        private bool _ServerReady;

        public bool ServerReady
        {
            get => _ServerReady;
            set
            {
                SetStatusImage(value ? Properties.Resources.HostStatus_On : Properties.Resources.HostStatus_Off);

                _ServerReady = value;
            }
        }

        private delegate void SetStatusImageCB(Image Img);

        private void SetStatusImage(Image Img)
        {
            if (pbServerStatus.InvokeRequired)
            {
                var d = new SetStatusImageCB(SetStatusImage);
                ParentForm?.Invoke(d, new object[] {Img});
            }
            else
            {
                pbServerStatus.Image = Img;
            }
        }

        private void chkReconnectWhenReady_CheckedChanged(object sender, EventArgs e)
        {
            _ReconnectWhenReady = chkReconnectWhenReady.Checked;
        }

        private bool _ReconnectWhenReady;

        public bool ReconnectWhenReady
        {
            get => _ReconnectWhenReady;
            set
            {
                _ReconnectWhenReady = value;
                SetCheckbox(value);
            }
        }

        private delegate void SetCheckboxCB(bool Val);

        private void SetCheckbox(bool Val)
        {
            if (chkReconnectWhenReady.InvokeRequired)
            {
                var d = new SetCheckboxCB(SetCheckbox);
                ParentForm?.Invoke(d, new object[] {Val});
            }
            else
            {
                chkReconnectWhenReady.Checked = Val;
            }
        }

        public delegate void CloseClickedEventHandler();

        private CloseClickedEventHandler CloseClickedEvent;

        public event CloseClickedEventHandler CloseClicked
        {
            add => CloseClickedEvent = (CloseClickedEventHandler)Delegate.Combine(CloseClickedEvent, value);
            remove => CloseClickedEvent = (CloseClickedEventHandler)Delegate.Remove(CloseClickedEvent, value);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseClickedEvent?.Invoke();
        }

        private void tmrAnimation_Tick(object sender, EventArgs e)
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

        private delegate void DisposeReconnectGroupCB();

        public void DisposeReconnectGroup()
        {
            if (InvokeRequired)
            {
                var d = new DisposeReconnectGroupCB(DisposeReconnectGroup);
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
            grpAutomaticReconnect.Text = Language.GroupboxAutomaticReconnect;
            btnClose.Text = Language._Close;
            lblServerStatus.Text = Language.ServerStatus;
            chkReconnectWhenReady.Text = Language.CheckboxReconnectWhenReady;
        }
    }
}