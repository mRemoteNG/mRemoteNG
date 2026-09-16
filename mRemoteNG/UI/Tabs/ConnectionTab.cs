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
        private DockAreas? _dockAreasBeforeMinimize;

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
            DockStateChanged += ConnectionTab_DockStateChanged;
        }

        internal bool CanMinimizeToBottomAutoHide()
        {
            return DockPanel != null && !IsDisposed && !Disposing;
        }

        internal void MinimizeToBottomAutoHide()
        {
            DockPanel dockPanel = DockPanel;
            if (!CanMinimizeToBottomAutoHide() || dockPanel == null)
                return;

            if ((DockAreas & DockAreas.DockBottom) != DockAreas.DockBottom)
            {
                _dockAreasBeforeMinimize ??= DockAreas;
                DockAreas |= DockAreas.DockBottom;
            }

            Show(dockPanel, DockState.DockBottomAutoHide);
        }

        private void ConnectionTab_DockStateChanged(object? sender, EventArgs e)
        {
            if (_dockAreasBeforeMinimize == null ||
                (DockState != DockState.Document &&
                 DockState != DockState.Hidden &&
                 DockState != DockState.Unknown))
            {
                return;
            }

            DockAreas = _dockAreasBeforeMinimize.Value;
            _dockAreasBeforeMinimize = null;
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