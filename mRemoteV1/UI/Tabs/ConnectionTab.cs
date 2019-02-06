using System;
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
        public bool silentClose { get; set; }

        public ConnectionTab()
        {
            InitializeComponent();
            GotFocus += ConnectionTab_GotFocus;
        }

        private void ConnectionTab_GotFocus(object sender, EventArgs e)
        {
            Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,"Tab got focused: " + TabText); 
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if(!silentClose)
            {
                if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                {
                    var result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName, string.Format(Language.strConfirmCloseConnectionPanelMainInstruction, TabText), "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
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
                        ((InterfaceControl)Tag).Protocol.Close();
                    }
                }
                else
                {
                    // close without the confirmation prompt...
                    ((InterfaceControl)Tag).Protocol.Close();
                }
            }
            else
            {
                ((InterfaceControl)Tag).Protocol.Close();
            }
            base.OnFormClosing(e);
        }


        #region HelperFunctions  
        public void RefreshInterfaceController()
        {
            try
            {
                var interfaceControl = Tag as InterfaceControl;
                if (interfaceControl?.Info.Protocol == ProtocolType.VNC)
                    ((ProtocolVNC)interfaceControl.Protocol).RefreshScreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RefreshIC (UI.Window.Connection) failed", ex);
            }
        }
        #endregion
    }
}
