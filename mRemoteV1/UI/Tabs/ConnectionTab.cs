using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.UI.TaskDialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Forms
{
    public partial class ConnectionTab : DockContent
    {
         
        public ConnectionTab()
        {
            InitializeComponent(); 
            this.FormClosing += formClosingEventHandler;
        }




        #region TabEvents      
        private void formClosingEventHandler(object sender, FormClosingEventArgs e)
        {
             
            try
            {  
                if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                {
                    var result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName, string.Format(Language.strConfirmCloseConnectionMainInstruction, this.TabText), "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
                    if (CTaskDialog.VerificationChecked)
                    {
                        Settings.Default.ConfirmCloseConnection--;
                    }
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                } 
                var interfaceControl = (InterfaceControl)this.Tag;
                interfaceControl.Protocol.Close();  
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.CloseConnectionTab() failed", ex);
            } 

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



        #endregion

        #region HelperFunctions  
        public void RefreshInterfaceController()
        {
            try
            {
                var interfaceControl = this.Tag as InterfaceControl;
                if (interfaceControl?.Info.Protocol == ProtocolType.VNC)
                    ((ProtocolVNC)interfaceControl.Protocol).RefreshScreen();
            }
            catch (Exception ex)
            {
                App.Runtime.MessageCollector.AddExceptionMessage("RefreshIC (UI.Window.Connection) failed", ex);
            }
        }
        #endregion
    }
}
