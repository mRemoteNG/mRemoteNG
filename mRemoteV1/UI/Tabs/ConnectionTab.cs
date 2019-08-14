using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Tabs
{
    public partial class ConnectionTab : DockContent
    {
        private InterfaceControl InterfaceControl => Tag as InterfaceControl;

        /// <summary>
        ///Silent close ignores the popup asking for confirmation
        /// </summary>
        public bool SilentClose { get; set; }

        /// <summary>
        /// Protocol close ignores the interface controller cleanup and the user confirmation dialog
        /// </summary>
        public bool ProtocolClose { get; set; }

        public ConnectionTab()
        {
            InitializeComponent();
            GotFocus += ConnectionTab_GotFocus;
        }

        private void ConnectionTab_GotFocus(object sender, EventArgs e)
        {
            TabHelper.Instance.CurrentTab = this;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!ProtocolClose)
            {
                if (!SilentClose)
                {
                    if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                    {
                        ShowCloseConnectionTabPrompt(e);
                    }
                    else
                    {
                        // close without the confirmation prompt...
                        InterfaceControl?.Protocol.Close();
                    }
                }
                else
                {
                    InterfaceControl?.Protocol.Close();
                }
            }

            base.OnFormClosing(e);
        }

        public void RefreshInterfaceController()
        {
            try
            {
                if (InterfaceControl?.Info.Protocol == ProtocolType.VNC)
                    ((ProtocolVNC)InterfaceControl.Protocol).RefreshScreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RefreshIC (UI.Window.Connection) failed", ex);
            }
        }

        private void ShowCloseConnectionTabPrompt(FormClosingEventArgs e)
        {
            var result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                string.Format(Language.strConfirmCloseConnectionPanelMainInstruction, TabText),
                "", "", "",
                Language.strCheckboxDoNotShowThisMessageAgain,
                ETaskDialogButtons.YesNo, ESysIcons.Question,
                ESysIcons.Question);

            if (CTaskDialog.VerificationChecked)
            {
                Settings.Default.ConfirmCloseConnection--;
            }

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                InterfaceControl?.Protocol.Close();
            }
        }
    }
}