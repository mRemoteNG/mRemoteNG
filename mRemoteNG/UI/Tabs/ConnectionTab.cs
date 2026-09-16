using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Tabs
{
    [SupportedOSPlatform("windows")]
    public partial class ConnectionTab : DockContent
    {
        /// <summary>
        ///Silent close ignores the popup asking for confirmation
        /// </summary>
        public bool silentClose { get; set; }

        /// <summary>
        /// Protocol close ignores the interface controller cleanup and the user confirmation dialog
        /// </summary>
        public bool protocolClose { get; set; }

        public ConnectionTab()
        {
            InitializeComponent();
            GotFocus += ConnectionTab_GotFocus;
        }

        internal void MinimizeToBottomAutoHide()
        {
            DockPanel dockPanel = DockPanel;
            if (dockPanel == null)
                return;

            DockAreas originalDockAreas = DockAreas;
            try
            {
                DockAreas |= DockAreas.DockBottom;
                Show(dockPanel, DockState.DockBottomAutoHide);
            }
            finally
            {
                DockAreas = originalDockAreas;
            }
        }

        private void ConnectionTab_GotFocus(object sender, EventArgs e)
        {
            TabHelper.Instance.CurrentTab = this;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!protocolClose)
            {
                if (!silentClose)
                {
                    if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                    {
                        DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                            string
                                                                .Format(Language.ConfirmCloseConnectionPanelMainInstruction,
                                                                        TabText), "", "", "",
                                                            Language.CheckboxDoNotShowThisMessageAgain,
                                                            ETaskDialogButtons.YesNo, ESysIcons.Question,
                                                            ESysIcons.Question);
                        if (CTaskDialog.VerificationChecked)
                        {
                            Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
                            Settings.Default.Save();
                        }

                        if (result == DialogResult.No)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            ((InterfaceControl)Tag)?.Protocol.Close();
                        }
                    }
                    else
                    {
                        // close without the confirmation prompt...
                        ((InterfaceControl)Tag)?.Protocol.Close();
                    }
                }
                else
                {
                    ((InterfaceControl)Tag)?.Protocol.Close();
                }
            }

            base.OnFormClosing(e);

            if (e.Cancel || FrmMain.Default == null || FrmMain.Default.IsClosing)
                return;

            ConnectionWindow parentWindow = FindForm() as ConnectionWindow;
            if (parentWindow == null || parentWindow.IsGeneralPanel)
                return;

            IDockContent[] remainingDocuments = DockPanel?.DocumentsToArray() ?? [];
            if (remainingDocuments.Length > 1)
                return;

            FrmMain.Default?.ShowHidePanelTabs(this);
            parentWindow.Close();
        }


        #region HelperFunctions  

        public void RefreshInterfaceController()
        {
            try
            {
                InterfaceControl interfaceControl = Tag as InterfaceControl;
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