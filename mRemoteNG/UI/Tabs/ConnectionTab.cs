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
            return DockPanel != null &&
                   !IsDisposed &&
                   !Disposing &&
                   DockState == DockState.Document;
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
            if (_dockAreasBeforeMinimize == null || DockState != DockState.Document)
            {
                return;
            }

            RestoreDockAreasAfterMinimize();
        }

        private void RestoreDockAreasAfterMinimize()
        {
            if (_dockAreasBeforeMinimize == null)
            {
                return;
            }

            // Assigning DockAreas throws if the new value does not allow the current
            // DockState (e.g. restoring areas without DockBottom while still auto-hidden
            // at the bottom). Defer the restore until the state is compatible again.
            if (!DockAreasAllowState(_dockAreasBeforeMinimize.Value, DockState))
            {
                return;
            }

            try
            {
                DockAreas = _dockAreasBeforeMinimize.Value;
            }
            catch (InvalidOperationException ex)
            {
                // The docking library re-validates the value against the live DockState and
                // can still reject it if the state changed between the check above and the
                // assignment. Swallowing keeps tab close/restore from surfacing an unhandled
                // exception; the original DockAreas simply remain in effect.
                Runtime.MessageCollector?.AddExceptionMessage(
                    "RestoreDockAreasAfterMinimize (UI.Tabs.ConnectionTab) failed to restore DockAreas",
                    ex);
            }
            finally
            {
                _dockAreasBeforeMinimize = null;
            }
        }

        private static bool DockAreasAllowState(DockAreas dockAreas, DockState dockState)
        {
            return dockState switch
            {
                DockState.Float => (dockAreas & DockAreas.Float) == DockAreas.Float,
                DockState.DockLeft or DockState.DockLeftAutoHide => (dockAreas & DockAreas.DockLeft) == DockAreas.DockLeft,
                DockState.DockRight or DockState.DockRightAutoHide => (dockAreas & DockAreas.DockRight) == DockAreas.DockRight,
                DockState.DockTop or DockState.DockTopAutoHide => (dockAreas & DockAreas.DockTop) == DockAreas.DockTop,
                DockState.DockBottom or DockState.DockBottomAutoHide => (dockAreas & DockAreas.DockBottom) == DockAreas.DockBottom,
                DockState.Document => (dockAreas & DockAreas.Document) == DockAreas.Document,
                _ => true,
            };
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

            if (!e.Cancel && DockState == DockState.DockBottomAutoHide)
            {
                RestoreDockAreasAfterMinimize();
            }

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