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
            }
            else
            {
                ((InterfaceControl)Tag).Protocol.Close();
            }
            base.OnFormClosing(e);
        }

        /*     [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
             protected override void WndProc(ref Message m)
             {
                 if (m.Msg == (int)mRemoteNG.UI.Tabs.Msgs.WM_LBUTTONDBLCLK)
                 {
                     base.WndProc(ref m);

                     int index = HitTest();
                     if (DockPane.DockPanel.AllowEndUserDocking && index != -1)
                     {
                         IDockContent content = Tabs[index].Content;
                         if (content.DockHandler.CheckDockState(!content.DockHandler.IsFloat) != DockState.Unknown)
                             content.DockHandler.IsFloat = !content.DockHandler.IsFloat;
                     }

                     return;
                 }

                 base.WndProc(ref m);
                 return;
             }
             */




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
