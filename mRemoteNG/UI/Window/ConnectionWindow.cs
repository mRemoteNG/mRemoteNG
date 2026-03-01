using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.Security;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class ConnectionWindow : BaseWindow
    {
        private VisualStudioToolStripExtender? _vsToolStripExtender;
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();
        private readonly ToolStripMenuItem _cmenTabMoveToPanel = new();
        private readonly ToolStripMenuItem _cmenTabIncludeInMultiSsh = new();
        private readonly ToolStripMenuItem _cmenTabExcludeFromMultiSsh = new();
        private readonly ToolStripSeparator _cmenTabMultiSshSeparator = new();
        private readonly ToolStripMenuItem _cmenTabScreenshotManager = new();
        private readonly ToolStripMenuItem _cmenTabTileConnections = new();
        private readonly ToolStripMenuItem _cmenTabTileHorizontally = new();
        private readonly ToolStripMenuItem _cmenTabTileVertically = new();
        private readonly ToolStripMenuItem _cmenTabCollapseToTabs = new();
        private readonly ToolStripMenuItem _cmenTabSendCtrlAltEnd = new();
        private bool _isAddingTab;
        private readonly List<IDockContent> _tabActivationHistory = new();
        // Tracks panel activation order across all ConnectionWindow instances (MRU, index 0 = oldest)
        private static readonly List<ConnectionWindow> _panelActivationHistory = new();

        #region Public Methods

        public ConnectionWindow(DockContent panel, string formText = "")
        {
            if (formText == "")
            {
                formText = "New Panel";
            }

            WindowType = WindowType.Connection;
            DockPnl = panel;
            InitializeComponent();
            SetEventHandlers();
            // ReSharper disable once VirtualMemberCallInConstructor
            Text = formText;
            TabText = formText;
            connDock.DocumentStyle = Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs
                ? DocumentStyle.DockingWindow
                : DocumentStyle.DockingSdi;
            connDock.ShowDocumentIcon = true;

            connDock.ActiveContentChanged += ConnDockOnActiveContentChanged;
            InitializeConnectionTabDragDropTargets();
        }

        internal void ShowHideConnectionTabs()
        {
            if (_isAddingTab || IsDisposed || Disposing) return;

            if (InvokeRequired)
            {
                try
                {
                    Invoke(new MethodInvoker(ShowHideConnectionTabs));
                }
                catch (ObjectDisposedException)

                {

                    _ = 0; // Intentionally empty — window may be disposed

                }
                catch (InvalidOperationException)

                {

                    _ = 0; // Intentionally empty — window may be disposed

                }
                return;
            }

            DocumentStyle newDocumentStyle;

            if (Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs)
            {
                newDocumentStyle = DocumentStyle.DockingWindow;
            }
            else
            {
                newDocumentStyle = connDock.Contents.Count > 1
                    ? DocumentStyle.DockingWindow
                    : DocumentStyle.DockingSdi;
            }

            if (connDock.DocumentStyle != newDocumentStyle)
            {
                connDock.DocumentStyle = newDocumentStyle;
            }
        }

        private InterfaceControl? GetInterfaceControl()
        {
            return InterfaceControl.FindInterfaceControl(connDock);
        }

        private ConnectionTab? GetSelectedTab()
        {
            // ActiveDocument is null when the tab is floating (DockState.Float); fall back to ActiveContent (#1875)
            return connDock.ActiveDocument as ConnectionTab
                ?? connDock.ActiveContent as ConnectionTab
                ?? GetInterfaceControl()?.Parent as ConnectionTab;
        }

        private static ConnectionInfo? GetConnectionInfoForTab(ConnectionTab? connectionTab)
        {
            if (connectionTab == null) return null;

            if (connectionTab.Tag is InterfaceControl interfaceControl)
                return interfaceControl.Info;

            if (connectionTab.Tag is ConnectionInfo connectionInfo)
                return connectionInfo;

            return connectionTab.TrackedConnectionInfo;
        }

        private static ConnectionInfo? GetMultiSshConnectionInfoForTab(ConnectionTab? connectionTab)
        {
            if (connectionTab?.Tag is InterfaceControl interfaceControl)
                return interfaceControl.OriginalInfo ?? interfaceControl.Info;

            return GetConnectionInfoForTab(connectionTab);
        }

        private ConnectionTab? FindReusableClosedTab(ConnectionInfo connectionInfo)
        {
            foreach (IDockContent dockContent in connDock.Contents)
            {
                if (dockContent is not ConnectionTab connectionTab) continue;
                if (InterfaceControl.FindInterfaceControl(connectionTab) != null) continue;

                if (GetConnectionInfoForTab(connectionTab) == connectionInfo)
                    return connectionTab;
            }

            return null;
        }

        private void SetEventHandlers()
        {
            SetFormEventHandlers();
            SetContextMenuEventHandlers();
            connDock.ContentAdded += ConnDock_ContentAdded;
            connDock.ContentRemoved += ConnDock_ContentRemoved;
        }

        private void ConnDock_ContentAdded(object? sender, DockContentEventArgs e)
        {
            ShowHideConnectionTabs();
            AttachConnectionTabDropTarget(e.Content.DockHandler.Form);
            if (e.Content is ConnectionTab tab)
                tab.FormClosing += OnConnectionTabFormClosing;
        }

        private void ConnDock_ContentRemoved(object? sender, DockContentEventArgs e)
        {
            ShowHideConnectionTabs();
            if (e.Content is ConnectionTab tab)
            {
                tab.FormClosing -= OnConnectionTabFormClosing;
                _tabActivationHistory.Remove(e.Content);
            }
            ClosePanelIfEmpty();
        }

        // Before a tab closes, activate the MRU (previously used) tab so DockPanelSuite
        // does not auto-select the positionally adjacent tab.
        private void OnConnectionTabFormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.Cancel || sender is not ConnectionTab closingTab) return;
            if (!ReferenceEquals(connDock.ActiveContent, closingTab)) return;

            for (int i = _tabActivationHistory.Count - 1; i >= 0; i--)
            {
                IDockContent candidate = _tabActivationHistory[i];
                if (ReferenceEquals(candidate, closingTab)) continue;
                if (candidate is DockContent dc && dc.IsDisposed) continue;
                candidate.DockHandler.Activate();
                return;
            }
        }

        private void SetFormEventHandlers()
        {
            Load += Connection_Load;
            DockStateChanged += Connection_DockStateChanged;
            FormClosing += Connection_FormClosing;
        }

        private void SetContextMenuEventHandlers()
        {
            InitializeMoveToPanelContextMenuItems();
            InitializeMultiSshContextMenuItems();
            InitializeScreenshotManagerMenuItem();
            InitializeTileContextMenuItems();
            InitializeRdpContextMenuItems();

            // event handler to adjust the items within the context menu
            cmenTab.Opening += ShowHideMenuButtons;

            // event handlers for all context menu items...
            cmenTabFullscreen.Click += (sender, args) => ToggleFullscreen();
            cmenTabSmartSize.Click += (sender, args) => ToggleSmartSize();
            cmenTabViewOnly.Click += (sender, args) => ToggleViewOnly();
            cmenTabStartChat.Click += (sender, args) => StartChat();
            cmenTabTransferFile.Click += (sender, args) => TransferFile();
            cmenTabRefreshScreen.Click += (sender, args) => RefreshScreen();
            cmenTabScreenshot.Click += (sender, args) => TakeScreenshotToFile();
            cmenTabSendSpecialKeysCtrlAltDel.Click += (sender, args) => SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlAltDel);
            cmenTabSendSpecialKeysCtrlEsc.Click += (sender, args) => SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlEsc);
            cmenTabRenameTab.Click += (sender, args) => RenameTab();
            cmenTabDuplicateTab.Click += (sender, args) => DuplicateTab();
            cmenTabReconnect.Click += (sender, args) => Reconnect();
            cmenTabDisconnect.Click += (sender, args) => CloseTabMenu();
            cmenTabDisconnectOthers.Click += (sender, args) => CloseOtherTabs();
            cmenTabDisconnectOthersRight.Click += (sender, args) => CloseOtherTabsToTheRight();
            cmenTabPuttySettings.Click += (sender, args) => ShowPuttySettingsDialog();
            _cmenTabIncludeInMultiSsh.Click += (sender, args) => ToggleMultiSshInclude();
            _cmenTabExcludeFromMultiSsh.Click += (sender, args) => ToggleMultiSshExclude();
            _cmenTabSendCtrlAltEnd.Click += (sender, args) => SendCtrlAltEnd();
            GotFocus += ConnectionWindow_GotFocus;
        }

        private void InitializeMoveToPanelContextMenuItems()
        {
            _cmenTabMoveToPanel.Name = "cmenTabMoveToPanel";
            _cmenTabMoveToPanel.Image = Properties.Resources.Panel_16x;
            _cmenTabMoveToPanel.DropDownOpening += MoveToPanelMenu_DropDownOpening;

            int insertIndex = cmenTab.Items.IndexOf(cmenTabSep1);
            if (insertIndex < 0)
                insertIndex = cmenTab.Items.Count;

            cmenTab.Items.Insert(insertIndex, _cmenTabMoveToPanel);
            _cmenTabMoveToPanel.Visible = false;
        }

        private void InitializeMultiSshContextMenuItems()
        {
            _cmenTabIncludeInMultiSsh.Name = "cmenTabIncludeInMultiSsh";
            _cmenTabExcludeFromMultiSsh.Name = "cmenTabExcludeFromMultiSsh";
            _cmenTabMultiSshSeparator.Name = "cmenTabMultiSshSeparator";

            int puttySettingsIndex = cmenTab.Items.IndexOf(cmenTabPuttySettings);
            if (puttySettingsIndex < 0)
                puttySettingsIndex = cmenTab.Items.Count;

            cmenTab.Items.Insert(puttySettingsIndex, _cmenTabMultiSshSeparator);
            cmenTab.Items.Insert(puttySettingsIndex + 1, _cmenTabIncludeInMultiSsh);
            cmenTab.Items.Insert(puttySettingsIndex + 2, _cmenTabExcludeFromMultiSsh);

            _cmenTabMultiSshSeparator.Visible = false;
            _cmenTabIncludeInMultiSsh.Visible = false;
            _cmenTabExcludeFromMultiSsh.Visible = false;
        }

        private void InitializeScreenshotManagerMenuItem()
        {
            _cmenTabScreenshotManager.Name = "cmenTabScreenshotManager";
            _cmenTabScreenshotManager.Image = Properties.Resources.Monitor_16x;
            _cmenTabScreenshotManager.Text = "Screenshot Manager...";
            _cmenTabScreenshotManager.Click += (sender, args) => OpenScreenshotManager();

            int insertIndex = cmenTab.Items.IndexOf(cmenTabScreenshot);
            if (insertIndex < 0)
                insertIndex = cmenTab.Items.Count;
            else
                insertIndex++; // insert right after "Take Screenshot"

            cmenTab.Items.Insert(insertIndex, _cmenTabScreenshotManager);
        }

        private void OpenScreenshotManager()
        {
            using FrmScreenshotManager manager = new();
            manager.ShowDialog(this);
        }

        private void InitializeTileContextMenuItems()
        {
            _cmenTabTileConnections.Name = "cmenTabTileConnections";
            _cmenTabTileConnections.Text = "Tile Connections";
            _cmenTabTileConnections.Image = Properties.Resources.Panel_16x;

            _cmenTabTileHorizontally.Name = "cmenTabTileHorizontally";
            _cmenTabTileHorizontally.Text = "Tile Horizontally";
            _cmenTabTileHorizontally.Click += (sender, args) => TileConnectionsHorizontally();

            _cmenTabTileVertically.Name = "cmenTabTileVertically";
            _cmenTabTileVertically.Text = "Tile Vertically";
            _cmenTabTileVertically.Click += (sender, args) => TileConnectionsVertically();

            _cmenTabCollapseToTabs.Name = "cmenTabCollapseToTabs";
            _cmenTabCollapseToTabs.Text = "Collapse to Tabs";
            _cmenTabCollapseToTabs.Click += (sender, args) => CollapseToTabs();

            _cmenTabTileConnections.DropDownItems.AddRange(new ToolStripItem[]
            {
                _cmenTabTileHorizontally,
                _cmenTabTileVertically,
                new ToolStripSeparator(),
                _cmenTabCollapseToTabs
            });

            // Insert after _cmenTabMoveToPanel (which itself sits before cmenTabSep1)
            int insertIndex = cmenTab.Items.IndexOf(_cmenTabMoveToPanel);
            if (insertIndex < 0)
                insertIndex = cmenTab.Items.IndexOf(cmenTabSep1);
            if (insertIndex < 0)
                insertIndex = cmenTab.Items.Count;
            else
                insertIndex++; // insert right after _cmenTabMoveToPanel

            cmenTab.Items.Insert(insertIndex, _cmenTabTileConnections);
            _cmenTabTileConnections.Visible = false;
        }

        private void InitializeRdpContextMenuItems()
        {
            _cmenTabSendCtrlAltEnd.Name = "cmenTabSendCtrlAltEnd";
            _cmenTabSendCtrlAltEnd.Image = Properties.Resources.ToggleOfficeKeyboardScheme_16x;

            int insertIndex = cmenTab.Items.IndexOf(cmenTabSendSpecialKeys);
            if (insertIndex < 0)
                insertIndex = cmenTab.Items.Count;
            else
                insertIndex++; // insert right after the VNC special keys menu

            cmenTab.Items.Insert(insertIndex, _cmenTabSendCtrlAltEnd);
            _cmenTabSendCtrlAltEnd.Visible = false;
        }

        private void InitializeConnectionTabDragDropTargets()
        {
            connDock.AllowDrop = true;
            connDock.DragEnter += ConnectionTabDragEnter;
            connDock.DragOver += ConnectionTabDragOver;
            connDock.DragDrop += ConnectionTabDragDrop;
            connDock.ControlAdded += ConnDock_ControlAdded;

            AttachConnectionTabDropTarget(connDock);
        }

        private void ConnDock_ControlAdded(object? sender, ControlEventArgs e)
        {
            if (e.Control is null) return;
            AttachConnectionTabDropTarget(e.Control);
        }

        private void AttachConnectionTabDropTarget(Control control)
        {
            if (control is DockPaneStripNG dockPaneStrip)
            {
                dockPaneStrip.AllowDrop = true;
                dockPaneStrip.DragEnter -= ConnectionTabDragEnter;
                dockPaneStrip.DragOver -= ConnectionTabDragOver;
                dockPaneStrip.DragDrop -= ConnectionTabDragDrop;
                dockPaneStrip.DragEnter += ConnectionTabDragEnter;
                dockPaneStrip.DragOver += ConnectionTabDragOver;
                dockPaneStrip.DragDrop += ConnectionTabDragDrop;
            }

            foreach (Control child in control.Controls)
            {
                AttachConnectionTabDropTarget(child);
            }
        }

        private void ConnectionTabDragEnter(object? sender, DragEventArgs e)
        {
            if (CanDropConnectionTab(e.Data, out _))
                e.Effect = DragDropEffects.Move;
            else if (CanDropConnectionInfo(e.Data, out _))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ConnectionTabDragOver(object? sender, DragEventArgs e)
        {
            if (CanDropConnectionTab(e.Data, out _))
                e.Effect = DragDropEffects.Move;
            else if (CanDropConnectionInfo(e.Data, out _))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ConnectionTabDragDrop(object? sender, DragEventArgs e)
        {
            if (CanDropConnectionTab(e.Data, out ConnectionTab? draggedTab) && draggedTab != null)
            {
                e.Effect = MoveConnectionTabToPanel(draggedTab, this)
                    ? DragDropEffects.Move
                    : DragDropEffects.None;
                return;
            }

            if (CanDropConnectionInfo(e.Data, out List<ConnectionInfo> connectionInfos))
            {
                e.Effect = DragDropEffects.Copy;
                foreach (var info in connectionInfos)
                {
                    Runtime.ConnectionInitiator.OpenConnection(info, ConnectionInfo.Force.None, this);
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private bool CanDropConnectionTab(IDataObject? dataObject, out ConnectionTab? draggedTab)
        {
            draggedTab = null;
            if (!TryGetDraggedConnectionTab(dataObject, out draggedTab) || draggedTab == null)
                return false;

            ConnectionWindow? sourcePanel = GetOwningConnectionWindow(draggedTab);
            return sourcePanel != null && !ReferenceEquals(sourcePanel, this);
        }

        private static bool CanDropConnectionInfo(IDataObject? dataObject, out List<ConnectionInfo> connectionInfos)
        {
            return TryGetDraggedConnectionInfos(dataObject, out connectionInfos);
        }

        private static bool TryGetDraggedConnectionInfos(IDataObject? dataObject, out List<ConnectionInfo> connectionInfos)
        {
            connectionInfos = new List<ConnectionInfo>();
            if (dataObject == null) return false;

            if (dataObject.GetDataPresent("System.Collections.ArrayList"))
            {
                if (dataObject.GetData("System.Collections.ArrayList") is System.Collections.ArrayList list)
                {
                    foreach (var item in list)
                    {
                        if (item is ConnectionInfo ci)
                        {
                            connectionInfos.Add(ci);
                        }
                    }
                }
            }

            if (connectionInfos.Count == 0 && dataObject.GetDataPresent(typeof(ConnectionInfo)))
            {
                if (dataObject.GetData(typeof(ConnectionInfo)) is ConnectionInfo ci)
                {
                    connectionInfos.Add(ci);
                }
            }

            return connectionInfos.Any();
        }

        private static bool TryGetDraggedConnectionTab(IDataObject? dataObject, out ConnectionTab? draggedTab)
        {
            draggedTab = null;
            if (dataObject == null || !dataObject.GetDataPresent(typeof(ConnectionTab)))
                return false;

            draggedTab = dataObject.GetData(typeof(ConnectionTab)) as ConnectionTab;
            return draggedTab is { IsDisposed: false };
        }

        private void MoveToPanelMenu_DropDownOpening(object? sender, EventArgs e)
        {
            for (int i = _cmenTabMoveToPanel.DropDownItems.Count - 1; i >= 0; i--)
                _cmenTabMoveToPanel.DropDownItems[i].Dispose();

            _cmenTabMoveToPanel.DropDownItems.Clear();

            ConnectionTab? selectedTab = GetSelectedTab();
            if (selectedTab == null)
            {
                _cmenTabMoveToPanel.Enabled = false;
                return;
            }

            ConnectionWindow[] targetPanels = GetOtherConnectionPanels().ToArray();
            if (targetPanels.Length == 0)
            {
                _cmenTabMoveToPanel.Enabled = false;
                return;
            }

            _cmenTabMoveToPanel.Enabled = true;
            foreach (ConnectionWindow panel in targetPanels)
            {
                if (panel.IsDisposed) continue;

                ToolStripMenuItem panelItem = new(GetPanelName(panel))
                {
                    Tag = panel
                };

                panelItem.Click += MoveToPanelMenuItem_Click;
                _cmenTabMoveToPanel.DropDownItems.Add(panelItem);
            }
        }

        private void MoveToPanelMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is not ToolStripMenuItem { Tag: ConnectionWindow targetPanel })
                return;

            MoveSelectedTabToPanel(targetPanel);
        }

        private void MoveSelectedTabToPanel(ConnectionWindow targetPanel)
        {
            ConnectionTab? selectedTab = GetSelectedTab();
            if (selectedTab == null)
                return;

            MoveConnectionTabToPanel(selectedTab, targetPanel);
        }

        private static bool MoveConnectionTabToPanel(ConnectionTab connectionTab, ConnectionWindow targetPanel)
        {
            if (targetPanel.IsDisposed)
                return false;

            ConnectionWindow? sourcePanel = GetOwningConnectionWindow(connectionTab);
            if (sourcePanel == null || ReferenceEquals(sourcePanel, targetPanel))
                return false;

            string targetPanelName = GetPanelName(targetPanel);
            UpdateConnectionPanelAssignment(connectionTab, targetPanelName);
            connectionTab.TabPageContextMenuStrip = targetPanel.cmenTab;

            try
            {
                if (targetPanel.DockState == DockState.Unknown || targetPanel.DockState == DockState.Hidden || !targetPanel.Visible)
                    targetPanel.Show(FrmMain.Default.pnlDock, DockState.Document);
                else
                    targetPanel.Show(FrmMain.Default.pnlDock);

                connectionTab.Show(targetPanel.connDock, DockState.Document);
                connectionTab.DockHandler.Activate();
                connectionTab.Focus();
                TabHelper.Instance.CurrentPanel = targetPanel;

                ConnectionInfo? movedConnectionInfo = GetConnectionInfoForTab(connectionTab);
                if (movedConnectionInfo != null)
                    FrmMain.Default.SelectedConnection = movedConnectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("MoveConnectionTabToPanel (UI.Window.ConnectionWindow) failed", ex);
                return false;
            }

            if (!sourcePanel.IsDisposed && !sourcePanel.Disposing)
                sourcePanel.ClosePanelIfEmpty();
            return true;
        }

        private IEnumerable<ConnectionWindow> GetOtherConnectionPanels()
        {
            if (Runtime.WindowList == null)
                return Enumerable.Empty<ConnectionWindow>();

            return Runtime.WindowList
                .OfType<ConnectionWindow>()
                .Where(window => !window.IsDisposed && !ReferenceEquals(window, this))
                .OrderBy(window => window.Text, StringComparer.CurrentCultureIgnoreCase);
        }

        private static string GetPanelName(ConnectionWindow panel)
        {
            return panel.Text.Replace("&&", "&", StringComparison.Ordinal);
        }

        private static ConnectionWindow? GetOwningConnectionWindow(ConnectionTab connectionTab)
        {
            if (connectionTab.DockPanel?.FindForm() is ConnectionWindow dockPanelOwner)
                return dockPanelOwner;

            Control? current = connectionTab.Parent;
            while (current != null && current is not ConnectionWindow)
            {
                current = current.Parent;
            }

            return current as ConnectionWindow;
        }

        private static void UpdateConnectionPanelAssignment(ConnectionTab connectionTab, string panelName)
        {
            if (connectionTab.Tag is InterfaceControl interfaceControl)
            {
                interfaceControl.Info.Panel = panelName;
                if (interfaceControl.OriginalInfo != null)
                    interfaceControl.OriginalInfo.Panel = panelName;
            }

            if (connectionTab.Tag is ConnectionInfo taggedConnectionInfo)
                taggedConnectionInfo.Panel = panelName;

            if (connectionTab.TrackedConnectionInfo != null)
                connectionTab.TrackedConnectionInfo.Panel = panelName;
        }

        private void ConnectionWindow_GotFocus(object sender, EventArgs e)
        {
            TabHelper.Instance.CurrentPanel = this;
            _panelActivationHistory.RemoveAll(w => w.IsDisposed || ReferenceEquals(w, this));
            _panelActivationHistory.Add(this);
        }

        // Activates the MRU sibling panel before this one closes, preventing DockPanelSuite
        // from briefly flashing the first panel (issue #1989).
        private void PreActivateSiblingPanel()
        {
            if (FrmMain.Default?.pnlDock?.ActiveDocument is not ConnectionWindow activePanel ||
                !ReferenceEquals(activePanel, this))
                return;

            for (int i = _panelActivationHistory.Count - 1; i >= 0; i--)
            {
                ConnectionWindow candidate = _panelActivationHistory[i];
                if (candidate.IsDisposed || candidate.Disposing || ReferenceEquals(candidate, this)) continue;
                candidate.DockHandler.Activate();
                return;
            }
        }

        private sealed class FocusSnapshot
        {
            public IDockContent? ActiveMainDocument { get; init; }
            public IDockContent? ActiveConnectionDocument { get; init; }
            public Control? FocusedControl { get; init; }
        }

        private FocusSnapshot CaptureFocusSnapshot()
        {
            return new FocusSnapshot
            {
                ActiveMainDocument = FrmMain.Default.pnlDock.ActiveDocument,
                ActiveConnectionDocument = connDock.ActiveContent,
                FocusedControl = GetFocusedControl(Form.ActiveForm as ContainerControl)
            };
        }

        private static Control? GetFocusedControl(ContainerControl? containerControl)
        {
            Control? activeControl = containerControl?.ActiveControl;
            while (activeControl is ContainerControl nestedContainer && nestedContainer.ActiveControl != null)
            {
                activeControl = nestedContainer.ActiveControl;
            }

            return activeControl;
        }

        private void RestoreFocusSnapshot(FocusSnapshot? snapshot, ConnectionTab openedTab)
        {
            if (snapshot == null) return;

            try
            {
                if (ReferenceEquals(snapshot.ActiveMainDocument, this))
                {
                    if (snapshot.ActiveConnectionDocument != null &&
                        !ReferenceEquals(snapshot.ActiveConnectionDocument, openedTab))
                    {
                        snapshot.ActiveConnectionDocument.DockHandler.Activate();
                    }
                }
                else if (snapshot.ActiveMainDocument != null)
                {
                    snapshot.ActiveMainDocument.DockHandler.Activate();
                }
            }
            catch (ObjectDisposedException)
            {
                _ = 0; // Intentionally empty — control may be disposed
            }
            catch (InvalidOperationException)
            {
                _ = 0; // Intentionally empty — control may be disposed
            }

            try
            {
                if (snapshot.FocusedControl is { IsDisposed: false } && snapshot.FocusedControl.CanFocus)
                {
                    snapshot.FocusedControl.Focus();
                }
            }
            catch (ObjectDisposedException)
            {
                _ = 0; // Intentionally empty — control may be disposed
            }
            catch (InvalidOperationException)
            {
                _ = 0; // Intentionally empty — control may be disposed
            }
        }

        public ConnectionTab? AddConnectionTab(ConnectionInfo connectionInfo, bool switchToConnection = true)
        {
            try
            {
                FocusSnapshot? focusSnapshot = switchToConnection ? null : CaptureFocusSnapshot();

                //Set the connection text based on name and preferences
                string titleText;
                if (Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs)
                    titleText = connectionInfo.Protocol + @": ";
                else
                    titleText = "";

                titleText += ConnectionNameFormatter.FormatName(connectionInfo);

                if (Properties.OptionsTabsPanelsPage.Default.ShowFolderPathOnTabs)
                {
                    var folderPath = GetFolderPath(connectionInfo);
                    if (!string.IsNullOrEmpty(folderPath))
                        titleText += $" \u2014 {folderPath}";
                }

                if (Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs)
                {
                    titleText += @" (";
                    if (connectionInfo.Domain != "")
                        titleText += connectionInfo.Domain;

                    if (connectionInfo.Username != "")
                    {
                        if (connectionInfo.Domain != "")
                            titleText += @"\";
                        titleText += connectionInfo.Username;
                    }

                    titleText += @")";
                }

                titleText = titleText.Replace("&", "&&", StringComparison.Ordinal);

                string tabToolTip = BuildTabToolTip(connectionInfo);

                ConnectionTab conTab = new()
                {
                    Tag = connectionInfo,
                    DockAreas = DockAreas.Document | DockAreas.Float,
                    Icon = ConnectionIcon.FromString(connectionInfo.Icon),
                    TabText = titleText,
                    TabPageContextMenuStrip = cmenTab
                };

                conTab.DockHandler.ToolTipText = tabToolTip;

                conTab.TrackConnection(connectionInfo);
                conTab.HideClosedState();

                // Connection tab visibility is controlled by connDock.DocumentStyle
                // set in the constructor based on AlwaysShowConnectionTabs setting.

                _isAddingTab = true;
                try
                {
                    // Check if we need to switch style BEFORE showing to prevent SDI from closing existing tabs
                    if (!Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs)
                    {
                        if (connDock.DocumentStyle == DocumentStyle.DockingSdi && connDock.Contents.Count >= 1)
                        {
                            connDock.DocumentStyle = DocumentStyle.DockingWindow;
                        }
                    }

                    // Ensure the ConnectionWindow is visible before adding the tab
                    // This prevents visibility issues when the window was created but not yet shown
                    // Check DockState instead of Visible to properly detect if window is shown in DockPanel
                    if (DockState == DockState.Unknown || DockState == DockState.Hidden || !Visible)
                    {
                        Show(FrmMain.Default.pnlDock, DockState.Document);
                    }

                    //Show the tab — insert after the currently active tab (issue #2159)
                    DockPane? activePane = connDock.ActivePane;
                    if (activePane != null)
                    {
                        IDockContent? activeContent = connDock.ActiveContent;
                        IDockContent? beforeContent = null;
                        if (activeContent != null)
                        {
                            int activeIndex = activePane.Contents.IndexOf(activeContent);
                            if (activeIndex >= 0 && activeIndex + 1 < activePane.Contents.Count)
                                beforeContent = activePane.Contents[activeIndex + 1];
                        }
                        conTab.Show(activePane, beforeContent);
                    }
                    else
                    {
                        conTab.Show(connDock, DockState.Document);
                    }
                }
                finally
                {
                    _isAddingTab = false;
                    ShowHideConnectionTabs();
                }

                if (switchToConnection)
                {
                    conTab.Focus();
                }
                else
                {
                    RestoreFocusSnapshot(focusSnapshot, conTab);
                }

                return conTab;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("AddConnectionTab (UI.Window.ConnectionWindow) failed", ex);
            }

            return null;
        }

        public ConnectionTab? GetOrAddConnectionTab(ConnectionInfo connectionInfo, bool switchToConnection = true)
        {
            ConnectionTab? reusableTab = FindReusableClosedTab(connectionInfo);
            if (reusableTab != null)
            {
                reusableTab.TrackConnection(connectionInfo);
                reusableTab.HideClosedState();

                if (switchToConnection)
                {
                    reusableTab.DockHandler.Activate();
                    reusableTab.Focus();
                }

                return reusableTab;
            }

            return AddConnectionTab(connectionInfo, switchToConnection);
        }

        private static string GetFolderPath(ConnectionInfo connectionInfo)
        {
            var parts = new List<string>();
            var current = connectionInfo.Parent;
            while (current?.Parent != null)
            {
                parts.Insert(0, current.Name);
                current = current.Parent;
            }

            return string.Join(" / ", parts);
        }

        /// <summary>
        /// Builds a tooltip string for connection tabs showing the full hierarchical path,
        /// protocol, hostname, port, logon credentials, and description.
        /// </summary>
        private static string BuildTabToolTip(ConnectionInfo connectionInfo)
        {
            var lines = new List<string>();

            // Full hierarchical path (e.g., "Folder / Subfolder / ConnectionName")
            var folderPath = GetFolderPath(connectionInfo);
            string fullName = !string.IsNullOrEmpty(folderPath)
                ? $"{folderPath} / {connectionInfo.Name}"
                : connectionInfo.Name;
            lines.Add(fullName);

            string host = connectionInfo.Hostname ?? string.Empty;
            if (!string.IsNullOrEmpty(host))
            {
                string portSuffix = connectionInfo.Port != 0
                    ? $":{connectionInfo.Port}"
                    : string.Empty;
                lines.Add($"{connectionInfo.Protocol}  {host}{portSuffix}");
            }

            string domain = connectionInfo.Domain ?? string.Empty;
            string username = connectionInfo.Username ?? string.Empty;
            if (!string.IsNullOrEmpty(domain) || !string.IsNullOrEmpty(username))
            {
                string user = !string.IsNullOrEmpty(domain)
                    ? $"{domain}\\{username}"
                    : username;
                lines.Add($"User: {user}");
            }

            if (!string.IsNullOrEmpty(connectionInfo.Description))
                lines.Add(connectionInfo.Description);

            return string.Join(Environment.NewLine, lines);
        }

        #endregion

        public void ReconnectAll(IConnectionInitiator initiator)
        {
            List<InterfaceControl> controlList = new();
            try
            {
                foreach (IDockContent dockContent in connDock.DocumentsToArray())
                {
                    if (dockContent is not ConnectionTab tab) continue;
                    if (tab.Tag is InterfaceControl ic)
                        controlList.Add(ic);
                }

                if (controlList.Count > 0 && Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                {
                    DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                        Language.ConfirmReconnectAllConnections, "", "", "",
                                                        Language.CheckboxDoNotShowThisMessageAgain,
                                                        ETaskDialogButtons.YesNo, ESysIcons.Question,
                                                        ESysIcons.Question);
                    if (CTaskDialog.VerificationChecked)
                    {
                        Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
                        Settings.Default.Save();
                    }

                    if (result == DialogResult.No)
                        return;
                }

                foreach (InterfaceControl iControl in controlList)
                {
                    iControl.Protocol.Close();
                    initiator.OpenConnection(iControl.Info, ConnectionInfo.Force.DoNotJump, this);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("reconnectAll (UI.Window.ConnectionWindow) failed", ex);
            }

            controlList.Clear();
        }

        #region Form

        private void Connection_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive)
            {
                connDock.Theme = ThemeManager.getInstance().DefaultTheme.Theme;
                return;
            }

            base.ApplyTheme();
            try
            {
                connDock.Theme = ThemeManager.getInstance().ActiveTheme.Theme;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ConnectionWindow.ApplyTheme() failed", ex);
            }

            _vsToolStripExtender = new VisualStudioToolStripExtender(components)
            {
                DefaultRenderer = _toolStripProfessionalRenderer
            };
            _vsToolStripExtender.SetStyle(cmenTab, ThemeManager.getInstance().ActiveTheme.Version, ThemeManager.getInstance().ActiveTheme.Theme);

            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            connDock.DockBackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette?.getColor("Tab_Item_Background") ?? connDock.DockBackColor;
        }

        private bool _documentHandlersAdded;
        private bool _floatHandlersAdded;
        private bool _emptyPanelCloseQueued;
        private bool _panelFormClosingInProgress;

        private void Connection_DockStateChanged(object sender, EventArgs e)
        {
            switch (DockState)
            {
                case DockState.Float:
                    {
                        if (_documentHandlersAdded)
                        {
                            FrmMain.Default.ResizeBegin -= Connection_ResizeBegin;
                            FrmMain.Default.ResizeEnd -= Connection_ResizeEnd;
                            _documentHandlersAdded = false;
                        }

                        var floatWindow = DockHandler.FloatPane.FloatWindow;
                        floatWindow.ResizeBegin += Connection_ResizeBegin;
                        floatWindow.ResizeEnd += Connection_ResizeEnd;
                        _floatHandlersAdded = true;

                        // Set a reasonable default size (75% of primary screen working area)
                        // instead of DockPanelSuite's small default
                        var workingArea = Screen.PrimaryScreen?.WorkingArea
                                          ?? new Rectangle(0, 0, 1920, 1080);
                        int width = (int)(workingArea.Width * 0.75);
                        int height = (int)(workingArea.Height * 0.75);
                        floatWindow.Size = new Size(width, height);

                        // Center on the working area
                        floatWindow.Location = new Point(
                            workingArea.X + (workingArea.Width - width) / 2,
                            workingArea.Y + (workingArea.Height - height) / 2);
                        break;
                    }
                case DockState.Document:
                    {
                        if (_floatHandlersAdded)
                        {
                            DockHandler.FloatPane.FloatWindow.ResizeBegin -= Connection_ResizeBegin;
                            DockHandler.FloatPane.FloatWindow.ResizeEnd -= Connection_ResizeEnd;
                            _floatHandlersAdded = false;
                        }

                        FrmMain.Default.ResizeBegin += Connection_ResizeBegin;
                        FrmMain.Default.ResizeEnd += Connection_ResizeEnd;
                        _documentHandlersAdded = true;
                        break;
                    }
            }
        }

        private void ApplyLanguage()
        {
            _cmenTabMoveToPanel.Text = Language.SendTo;
            cmenTabFullscreen.Text = Language.Fullscreen;
            cmenTabSmartSize.Text = Language.SmartSize;
            cmenTabViewOnly.Text = Language.ViewOnly;
            cmenTabStartChat.Text = Language.StartChat;
            cmenTabTransferFile.Text = Language.TransferFile;
            cmenTabRefreshScreen.Text = Language.RefreshScreen;
            cmenTabScreenshot.Text = Language.Screenshot;
            cmenTabSendSpecialKeys.Text = Language.SendSpecialKeys;
            cmenTabSendSpecialKeysCtrlAltDel.Text = Language.CtrlAltDel;
            cmenTabSendSpecialKeysCtrlEsc.Text = Language.CtrlEsc;
            _cmenTabSendCtrlAltEnd.Text = Language.CtrlAltEnd;
            cmenTabExternalApps.Text = Language._Tools;
            cmenTabRenameTab.Text = Language.RenameTab;
            cmenTabDuplicateTab.Text = Language.DuplicateTab;
            cmenTabReconnect.Text = Language.Reconnect;
            cmenTabDisconnect.Text = Language.Disconnect;
            cmenTabDisconnectOthers.Text = Language.DisconnectOthers;
            cmenTabDisconnectOthersRight.Text = Language.DisconnectOthersRight;
            cmenTabPuttySettings.Text = Language.PuttySettings;
            _cmenTabIncludeInMultiSsh.Text = "Include in Multi SSH";
            _cmenTabExcludeFromMultiSsh.Text = "Exclude from Multi SSH";
        }

        private void Connection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!FrmMain.Default.IsClosing &&
                (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All & connDock.Documents.Any() ||
                 Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple &
                 connDock.Documents.Count() > 1))
            {
                DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName, string.Format(CultureInfo.CurrentCulture, Language.ConfirmCloseConnectionPanelMainInstruction, Text), "", "", "", Language.CheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.DisconnectCancel, ESysIcons.Question, ESysIcons.Question);
                if (CTaskDialog.VerificationChecked)
                {
                    if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                    {
                        Settings.Default.ConfirmCloseConnection = connDock.Documents.Count() == 1
                            ? (int)ConfirmCloseEnum.Multiple
                            : (int)ConfirmCloseEnum.Exit;
                    }
                    else if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple)
                    {
                        Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Exit;
                    }
                    else
                    {
                        Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
                    }

                    Settings.Default.Save();
                }

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Pre-activate the MRU sibling panel before this one closes to prevent a
            // brief flash of the first panel (issue #1989). Only needed when not closing
            // because FrmMain itself is shutting down.
            if (!FrmMain.Default.IsClosing)
                PreActivateSiblingPanel();

            _panelFormClosingInProgress = true;
            try
            {
                foreach (IDockContent dockContent in connDock.Documents.ToArray())
                {
                    ConnectionTab tabP = (ConnectionTab)dockContent;
                    if (tabP.Tag == null) continue;
                    tabP.silentClose = true;
                    tabP.Close();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.Connection_FormClosing() failed", ex);
            }
            finally
            {
                _panelFormClosingInProgress = false;
            }
        }

        public new event EventHandler? ResizeBegin;

        private void Connection_ResizeBegin(object sender, EventArgs e)
        {
            ResizeBegin?.Invoke(this, e);
        }

        public new event EventHandler? ResizeEnd;

        private void Connection_ResizeEnd(object sender, EventArgs e)
        {
            ResizeEnd?.Invoke(this, e);
            if (connDock == null || connDock.IsDisposed) return;
            foreach (var doc in connDock.Documents)
            {
                if (doc is ConnectionTab tab)
                {
                    tab.FireResizeEnd();
                }
            }
        }

        internal void NavigateToNextTab()
        {
            try
            {
                var documents = connDock.DocumentsToArray();
                if (documents.Length <= 1) return;

                var currentIndex = Array.IndexOf(documents, connDock.ActiveContent);
                if (currentIndex == -1)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "NavigateToNextTab: ActiveContent not found in documents array");
                    return;
                }

                var nextIndex = (currentIndex + 1) % documents.Length;
                documents[nextIndex].DockHandler.Activate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("NavigateToNextTab (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        internal void NavigateToPreviousTab()
        {
            try
            {
                var documents = connDock.DocumentsToArray();
                if (documents.Length <= 1) return;

                var currentIndex = Array.IndexOf(documents, connDock.ActiveContent);
                if (currentIndex == -1)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "NavigateToPreviousTab: ActiveContent not found in documents array");
                    return;
                }

                var previousIndex = currentIndex - 1;
                if (previousIndex < 0)
                    previousIndex = documents.Length - 1;
                documents[previousIndex].DockHandler.Activate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("NavigateToPreviousTab (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        internal void NavigateToTab(int index)
        {
            try
            {
                var documents = connDock.DocumentsToArray();
                if (index < 0 || index >= documents.Length) return;

                documents[index].DockHandler.Activate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("NavigateToTab (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        internal IDockContent[] GetDocuments()
        {
            try
            {
                return connDock.DocumentsToArray();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("GetDocuments (UI.Window.ConnectionWindow) failed", ex);
                return Array.Empty<IDockContent>();
            }
        }

        #endregion

        #region Events

        private void ConnDockOnActiveContentChanged(object sender, EventArgs e)
        {
            // Track MRU activation history so closing a tab returns to the previously used tab
            if (connDock.ActiveContent is ConnectionTab)
            {
                _tabActivationHistory.Remove(connDock.ActiveContent);
                _tabActivationHistory.Add(connDock.ActiveContent);
            }

            ConnectionTab? selectedTab = GetSelectedTab();
            ConnectionInfo? selectedConnectionInfo = GetConnectionInfoForTab(selectedTab);
            if (selectedConnectionInfo == null) return;
            FrmMain.Default.SelectedConnection = selectedConnectionInfo;

            // Refocus the protocol window so the embedded process (e.g. PuTTY) regains input (#2237)
            if (selectedTab?.Tag is InterfaceControl activeIc)
                activeIc.Protocol?.Focus();
        }

        private bool HasConnectionTabs()
        {
            if (connDock == null || connDock.IsDisposed)
            {
                return false;
            }

            return connDock.DocumentsToArray()
                .OfType<ConnectionTab>()
                .Any(tab => !tab.IsDisposed && !tab.Disposing);
        }

        private void ClosePanelIfEmpty()
        {
            if (!Properties.OptionsTabsPanelsPage.Default.AutoClosePanelOnLastTabClose)
            {
                return;
            }

            if (_emptyPanelCloseQueued || IsDisposed || Disposing || !IsHandleCreated || _panelFormClosingInProgress)
            {
                return;
            }

            if (FrmMain.Default?.IsClosing == true)
            {
                return;
            }

            if (HasConnectionTabs())
            {
                return;
            }

            _emptyPanelCloseQueued = true;
            try
            {
                BeginInvoke((MethodInvoker)ClosePanelIfEmptyOnUiTick);
            }
            catch (ObjectDisposedException)
            {
                _emptyPanelCloseQueued = false;
            }
            catch (InvalidOperationException)
            {
                _emptyPanelCloseQueued = false;
            }
        }

        private void ClosePanelIfEmptyOnUiTick()
        {
            _emptyPanelCloseQueued = false;

            if (!Properties.OptionsTabsPanelsPage.Default.AutoClosePanelOnLastTabClose)
            {
                return;
            }

            if (IsDisposed || Disposing || !IsHandleCreated)
            {
                return;
            }

            if (FrmMain.Default?.IsClosing == true)
            {
                return;
            }

            if (HasConnectionTabs())
            {
                return;
            }

            try
            {
                Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ClosePanelIfEmptyOnUiTick (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        #endregion

        #region Tab Menu

        private void ShowHideMenuButtons(object sender, CancelEventArgs e)
        {
            try
            {
                ConnectionTab? selectedTab = GetSelectedTab();
                bool canMoveToAnotherPanel = selectedTab != null && GetOtherConnectionPanels().Any();
                _cmenTabMoveToPanel.Visible = canMoveToAnotherPanel;
                _cmenTabMoveToPanel.Enabled = canMoveToAnotherPanel;

                int activeTabCount = connDock.Contents.OfType<ConnectionTab>()
                    .Count(t => !t.IsDisposed && !t.Disposing);
                bool canTile = activeTabCount >= 2;
                _cmenTabTileConnections.Visible = canTile;
                _cmenTabTileConnections.Enabled = canTile;

                InterfaceControl? interfaceControl = GetInterfaceControl();
                _cmenTabScreenshotManager.Visible = true;
                if (interfaceControl == null)
                {
                    cmenTabViewOnly.Visible = false;
                    cmenTabFullscreen.Visible = false;
                    cmenTabSmartSize.Visible = false;
                    cmenTabSendSpecialKeys.Visible = false;
                    cmenTabStartChat.Visible = false;
                    cmenTabRefreshScreen.Visible = false;
                    cmenTabScreenshot.Visible = false;
                    cmenTabTransferFile.Visible = false;
                    cmenTabPuttySettings.Visible = false;
                    cmenTabExternalApps.Visible = false;
                    _cmenTabMultiSshSeparator.Visible = false;
                    _cmenTabIncludeInMultiSsh.Visible = false;
                    _cmenTabExcludeFromMultiSsh.Visible = false;
                    return;
                }

                cmenTabExternalApps.Visible = true;
                cmenTabScreenshot.Visible = true;

                if (interfaceControl.Protocol is ISupportsViewOnly viewOnly)
                {
                    cmenTabViewOnly.Visible = true;
                    cmenTabViewOnly.Checked = viewOnly.ViewOnly;
                }
                else
                {
                    cmenTabViewOnly.Visible = false;
                }

                if (interfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    RdpProtocol rdp = (RdpProtocol)interfaceControl.Protocol;
                    cmenTabFullscreen.Visible = true;
                    cmenTabFullscreen.Enabled = !rdp.RedirectKeysEnabled || !rdp.Fullscreen;
                    cmenTabFullscreen.Checked = rdp.Fullscreen;
                    cmenTabSmartSize.Visible = true;
                    cmenTabSmartSize.Checked = rdp.SmartSize;
                    _cmenTabSendCtrlAltEnd.Visible = true;
                }
                else
                {
                    cmenTabFullscreen.Visible = false;
                    cmenTabFullscreen.Enabled = true;
                    cmenTabSmartSize.Visible = false;
                    _cmenTabSendCtrlAltEnd.Visible = false;
                }

                if (interfaceControl.Info.Protocol == ProtocolType.VNC)
                {
                    cmenTabSendSpecialKeys.Visible = true;
                    cmenTabSmartSize.Visible = true;
                    cmenTabStartChat.Visible = false;
                    cmenTabRefreshScreen.Visible = true;
                    cmenTabTransferFile.Visible = false;
                }
                else
                {
                    cmenTabSendSpecialKeys.Visible = false;
                    cmenTabStartChat.Visible = false;
                    cmenTabRefreshScreen.Visible = false;
                    cmenTabTransferFile.Visible = false;
                }

                if (interfaceControl.Info.Protocol == ProtocolType.SSH1 |
                    interfaceControl.Info.Protocol == ProtocolType.SSH2)
                {
                    cmenTabTransferFile.Visible = true;
                }

                ConnectionInfo? selectedConnectionInfo = GetMultiSshConnectionInfoForTab(GetSelectedTab());
                bool showMultiSshFilters = interfaceControl.Protocol is PuttyBase && selectedConnectionInfo != null;

                _cmenTabMultiSshSeparator.Visible = showMultiSshFilters;
                _cmenTabIncludeInMultiSsh.Visible = showMultiSshFilters;
                _cmenTabExcludeFromMultiSsh.Visible = showMultiSshFilters;

                if (showMultiSshFilters)
                {
                    _cmenTabIncludeInMultiSsh.Checked = selectedConnectionInfo!.IncludeInMultiSsh;
                    _cmenTabExcludeFromMultiSsh.Checked = selectedConnectionInfo.ExcludeFromMultiSsh;
                    _cmenTabIncludeInMultiSsh.Enabled = !selectedConnectionInfo.ExcludeFromMultiSsh;
                    _cmenTabExcludeFromMultiSsh.Enabled = !selectedConnectionInfo.IncludeInMultiSsh;
                }

                cmenTabPuttySettings.Visible = interfaceControl.Protocol is PuttyBase;

                AddExternalApps();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ShowHideMenuButtons (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        #endregion

        #region Tab Actions

        private void TakeScreenshotToFile()
        {
            try
            {
                ConnectionTab? selectedTab = GetSelectedTab();
                if (selectedTab == null) return;

                Image? screenshot = MiscTools.TakeScreenshot(selectedTab);
                if (screenshot == null) return;

                string connectionName = selectedTab.TabText.Replace("&&", "&", StringComparison.Ordinal);
                // Sanitize the connection name for use in a file name
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    connectionName = connectionName.Replace(c, '_');

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                string fileName = $"{connectionName}_{timestamp}.png";

                string screenshotDir = System.IO.Path.Combine(
                    App.Info.SettingsFileInfo.SettingsPath, "Screenshots");
                System.IO.Directory.CreateDirectory(screenshotDir);

                string filePath = System.IO.Path.Combine(screenshotDir, fileName);
                screenshot.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                screenshot.Dispose();

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Screenshot saved: {filePath}");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    "TakeScreenshotToFile (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ToggleSmartSize()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;

                switch (interfaceControl.Protocol)
                {
                    case RdpProtocol rdp:
                        rdp.ToggleSmartSize();
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleSmartSize (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void TransferFile()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;

                if (interfaceControl.Info.Protocol == ProtocolType.SSH1 |
                    interfaceControl.Info.Protocol == ProtocolType.SSH2)
                    SshTransferFile();
                else if (interfaceControl.Info.Protocol == ProtocolType.VNC)
                    VncTransferFile();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("TransferFile (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ToggleMultiSshInclude()
        {
            try
            {
                ConnectionInfo? connectionInfo = GetMultiSshConnectionInfoForTab(GetSelectedTab());
                if (connectionInfo == null)
                    return;

                connectionInfo.IncludeInMultiSsh = !connectionInfo.IncludeInMultiSsh;
                if (connectionInfo.IncludeInMultiSsh)
                    connectionInfo.ExcludeFromMultiSsh = false;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleMultiSshInclude (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ToggleMultiSshExclude()
        {
            try
            {
                ConnectionInfo? connectionInfo = GetMultiSshConnectionInfoForTab(GetSelectedTab());
                if (connectionInfo == null)
                    return;

                connectionInfo.ExcludeFromMultiSsh = !connectionInfo.ExcludeFromMultiSsh;
                if (connectionInfo.ExcludeFromMultiSsh)
                    connectionInfo.IncludeInMultiSsh = false;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleMultiSshExclude (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void SshTransferFile()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;

                AppWindows.Show(WindowType.SSHTransfer);
                ConnectionInfo connectionInfo = interfaceControl.Info;

                AppWindows.SshtransferForm.Hostname = connectionInfo.Hostname;
                AppWindows.SshtransferForm.Username = connectionInfo.Username;
                //App.Windows.SshtransferForm.Password = connectionInfo.Password.ConvertToUnsecureString();
                AppWindows.SshtransferForm.Password = connectionInfo.Password;
                AppWindows.SshtransferForm.Port = Convert.ToString(connectionInfo.Port, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("SSHTransferFile (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private static void VncTransferFile()
        {
            try
            {
                ProtocolVNC.StartFileTransfer();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("VNCTransferFile (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ToggleViewOnly()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (!(interfaceControl?.Protocol is ISupportsViewOnly viewOnly))
                    return;

                cmenTabViewOnly.Checked = !cmenTabViewOnly.Checked;
                viewOnly.ToggleViewOnly();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleViewOnly (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private static void StartChat()
        {
            try
            {
                ProtocolVNC.StartChat();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("StartChat (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void RefreshScreen()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                ProtocolVNC? vnc = interfaceControl?.Protocol as ProtocolVNC;
                vnc?.RefreshScreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RefreshScreen (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void SendSpecialKeys(ProtocolVNC.SpecialKeys keys)
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                ProtocolVNC? vnc = interfaceControl?.Protocol as ProtocolVNC;
                vnc?.SendSpecialKeys(keys);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("SendSpecialKeys (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void SendCtrlAltEnd()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                RdpProtocol? rdp = interfaceControl?.Protocol as RdpProtocol;
                rdp?.SendCtrlAltEnd();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("SendCtrlAltEnd (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ToggleFullscreen()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                RdpProtocol? rdp = interfaceControl?.Protocol as RdpProtocol;
                if (rdp?.RedirectKeysEnabled == true && rdp.Fullscreen)
                    return;
                rdp?.ToggleFullscreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleFullscreen (UI.Window.ConnectionWindow) failed",
                                                             ex);
            }
        }

        private void ShowPuttySettingsDialog()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                PuttyBase? puttyBase = interfaceControl?.Protocol as PuttyBase;
                puttyBase?.ShowSettingsDialog();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                                                             "ShowPuttySettingsDialog (UI.Window.ConnectionWindow) failed",
                                                             ex);
            }
        }

        public void FindInSession()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl?.Protocol is PuttyBase putty)
                {
                    putty.CopyAllToClipboard();

                    Timer timer = new Timer();
                    timer.Interval = 200; // 200ms
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        timer.Dispose();
                        if (Clipboard.ContainsText())
                        {
                            string text = Clipboard.GetText();
                            FrmFind frm = new FrmFind();
                            frm.SetContent(text);
                            frm.Show(this);
                        }
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("FindInSession (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void AddExternalApps()
        {
            try
            {
                //clean up. since new items are added below, we have to dispose of any previous items first
                if (cmenTabExternalApps.DropDownItems.Count > 0)
                {
                    for (int i = cmenTabExternalApps.DropDownItems.Count - 1; i >= 0; i--)
                        cmenTabExternalApps.DropDownItems[i].Dispose();
                    cmenTabExternalApps.DropDownItems.Clear();
                }

                //add ext apps
                foreach (ExternalTool externalTool in Runtime.ExternalToolsService.ExternalTools)
                {
                    ToolStripMenuItem nItem = new()
                    {
                        Text = externalTool.DisplayName,
                        Tag = externalTool,
                        /* rare failure here. While ExternalTool.Image already tries to default this
                         * try again so it's not null/doesn't crash.
                         */
                        Image = externalTool.Image ?? Properties.Resources.mRemoteNG_Icon.ToBitmap()
                    };

                    nItem.Click += (sender, args) =>
                    {
                        if (sender is ToolStripMenuItem menuItem && menuItem.Tag is ExternalTool tool)
                            StartExternalApp(tool);
                    };
                    cmenTabExternalApps.DropDownItems.Add(nItem);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenTreeTools_DropDownOpening failed (UI.Window.ConnectionWindow)", ex);
            }
        }

        private void StartExternalApp(ExternalTool externalTool)
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl?.Info != null)
                    externalTool.Start(interfaceControl.Info);
                else
                    externalTool.Start();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("cmenTabExternalAppsEntry_Click failed (UI.Window.ConnectionWindow)", ex);
            }
        }


        private void CloseTabMenu()
        {
            ConnectionTab? selectedTab = GetSelectedTab();
            if (selectedTab == null) return;

            try
            {
                ConnectionInfo? connectionInfo = GetConnectionInfoForTab(selectedTab);
                if (connectionInfo != null && Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                {
                    string confirmMessage = string.Format(CultureInfo.CurrentCulture, Language.ConfirmDisconnectConnection, connectionInfo.Name);
                    DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                        confirmMessage, "", "", "",
                                                        Language.CheckboxDoNotShowThisMessageAgain,
                                                        ETaskDialogButtons.YesNo, ESysIcons.Question,
                                                        ESysIcons.Question);
                    if (CTaskDialog.VerificationChecked)
                    {
                        Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
                        Settings.Default.Save();
                    }

                    if (result == DialogResult.No)
                        return;
                }

                selectedTab.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("CloseTabMenu (UI.Window.ConnectionWindow) failed", ex);
            }
            finally
            {
                ClosePanelIfEmpty();
            }
        }

        private void CloseOtherTabs()
        {
            ConnectionTab? selectedTab = GetSelectedTab();
            if (selectedTab == null) return;
            if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple)
            {
                DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                    string.Format(CultureInfo.CurrentCulture, Language.ConfirmCloseConnectionOthersInstruction,
                                                                  selectedTab.TabText), "", "", "",
                                                    Language.CheckboxDoNotShowThisMessageAgain,
                                                    ETaskDialogButtons.DisconnectCancel, ESysIcons.Question,
                                                    ESysIcons.Question);
                if (CTaskDialog.VerificationChecked)
                {
                    Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Exit;
                    Settings.Default.Save();
                }

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            foreach (IDockContent dockContent in connDock.Documents.ToArray())
            {
                ConnectionTab tab = (ConnectionTab)dockContent;
                if (selectedTab != tab)
                {
                    tab.Close();
                }
            }
        }

        private void CloseOtherTabsToTheRight()
        {
            try
            {
                ConnectionTab? selectedTab = GetSelectedTab();
                if (selectedTab == null) return;
                DockPane dockPane = selectedTab.Pane;

                bool pastTabToKeepAlive = false;
                List<ConnectionTab> connectionsToClose = new();
                foreach (IDockContent dockContent in dockPane.Contents)
                {
                    ConnectionTab tab = (ConnectionTab)dockContent;
                    if (pastTabToKeepAlive)
                        connectionsToClose.Add(tab);

                    if (selectedTab == tab)
                        pastTabToKeepAlive = true;
                }

                foreach (ConnectionTab tab in connectionsToClose)
                {
                    tab.Close();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("CloseTabMenu (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void DuplicateTab()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;
                // Use OriginalInfo so SSH Jump Mode connections duplicate with the real host/port,
                // not the localhost tunnel endpoint stored in Info (#2135).
                ConnectionInfo infoToDuplicate = interfaceControl.OriginalInfo ?? interfaceControl.Info;
                Runtime.ConnectionInitiator.OpenConnection(infoToDuplicate, ConnectionInfo.Force.DoNotJump);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("DuplicateTab (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        /// <summary>
        /// Tiles all connection tabs in this panel side by side horizontally.
        /// Each tab gets an equal share of the available width.
        /// </summary>
        internal void TileConnectionsHorizontally()
        {
            try
            {
                var tabs = connDock.Contents
                    .OfType<ConnectionTab>()
                    .Where(t => !t.IsDisposed && !t.Disposing)
                    .ToList();

                if (tabs.Count < 2) return;

                _isAddingTab = true;
                try
                {
                    connDock.DocumentStyle = DocumentStyle.DockingWindow;

                    // Collapse all tabs into a single pane first
                    DockPane leftPane = tabs[0].Pane;
                    for (int i = 1; i < tabs.Count; i++)
                    {
                        if (!ReferenceEquals(tabs[i].Pane, leftPane))
                            tabs[i].Show(leftPane, null);
                    }

                    // Peel tabs off right-to-left so the final order matches the tab list (left→right).
                    // At step i: proportion = 1 / (remaining panes including the new one)
                    // This yields equal-width columns for any N.
                    for (int i = 1; i < tabs.Count; i++)
                    {
                        int tabIndex = tabs.Count - i;
                        double proportion = 1.0 / (tabs.Count - i + 1);
                        tabs[tabIndex].Show(leftPane, DockAlignment.Right, proportion);
                    }
                }
                finally
                {
                    _isAddingTab = false;
                    ShowHideConnectionTabs();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("TileConnectionsHorizontally (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        /// <summary>
        /// Tiles all connection tabs in this panel stacked vertically.
        /// Each tab gets an equal share of the available height.
        /// </summary>
        internal void TileConnectionsVertically()
        {
            try
            {
                var tabs = connDock.Contents
                    .OfType<ConnectionTab>()
                    .Where(t => !t.IsDisposed && !t.Disposing)
                    .ToList();

                if (tabs.Count < 2) return;

                _isAddingTab = true;
                try
                {
                    connDock.DocumentStyle = DocumentStyle.DockingWindow;

                    // Collapse all tabs into a single pane first
                    DockPane topPane = tabs[0].Pane;
                    for (int i = 1; i < tabs.Count; i++)
                    {
                        if (!ReferenceEquals(tabs[i].Pane, topPane))
                            tabs[i].Show(topPane, null);
                    }

                    // Peel tabs off bottom-to-top for equal-height rows
                    for (int i = 1; i < tabs.Count; i++)
                    {
                        int tabIndex = tabs.Count - i;
                        double proportion = 1.0 / (tabs.Count - i + 1);
                        tabs[tabIndex].Show(topPane, DockAlignment.Bottom, proportion);
                    }
                }
                finally
                {
                    _isAddingTab = false;
                    ShowHideConnectionTabs();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("TileConnectionsVertically (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        /// <summary>
        /// Collapses all tiled connection panes back into a single tabbed group.
        /// </summary>
        internal void CollapseToTabs()
        {
            try
            {
                var tabs = connDock.Contents
                    .OfType<ConnectionTab>()
                    .Where(t => !t.IsDisposed && !t.Disposing)
                    .ToList();

                if (tabs.Count < 2) return;

                _isAddingTab = true;
                try
                {
                    connDock.DocumentStyle = DocumentStyle.DockingWindow;

                    DockPane targetPane = tabs[0].Pane;
                    for (int i = 1; i < tabs.Count; i++)
                    {
                        if (!ReferenceEquals(tabs[i].Pane, targetPane))
                            tabs[i].Show(targetPane, null);
                    }
                }
                finally
                {
                    _isAddingTab = false;
                    ShowHideConnectionTabs();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("CollapseToTabs (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void Reconnect()
        {
            try
            {
                ConnectionTab? selectedTab = GetSelectedTab();
                ConnectionInfo? connectionInfo = GetConnectionInfoForTab(selectedTab);
                if (connectionInfo == null)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Reconnect (UI.Window.ConnectionWindow) failed. Could not find ConnectionInfo.");
                    return;
                }

                // Show confirmation dialog if the connection is active and setting requires it
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl != null && Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                {
                    string confirmMessage = string.Format(CultureInfo.CurrentCulture, Language.ConfirmReconnectConnection, connectionInfo.Name);
                    DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                        confirmMessage, "", "", "",
                                                        Language.CheckboxDoNotShowThisMessageAgain,
                                                        ETaskDialogButtons.YesNo, ESysIcons.Question,
                                                        ESysIcons.Question);
                    if (CTaskDialog.VerificationChecked)
                    {
                        Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
                        Settings.Default.Save();
                    }

                    if (result == DialogResult.No)
                        return;
                }

                if (interfaceControl != null)
                    HandleProtocolClosed(interfaceControl.Protocol, keepTabOpen: true);

                Runtime.ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump, this);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Reconnect (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void RenameTab()
        {
            try
            {
                InterfaceControl? interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;
                if (interfaceControl.Parent is not ConnectionTab connectionTab) return;
                using (FrmInputBox frmInputBox = new(Language.NewTitle, Language.NewTitle,
                                                         connectionTab.TabText))
                {
                    DialogResult dr = frmInputBox.ShowDialog();
                    if (dr != DialogResult.OK) return;
                    if (!string.IsNullOrEmpty(frmInputBox.returnValue))
                        connectionTab.TabText = frmInputBox.returnValue.Replace("&", "&&", StringComparison.Ordinal);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RenameTab (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        #endregion

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                ConnectionTab? selectedTab = GetSelectedTab();
                if (selectedTab != null && GetInterfaceControl() == null)
                {
                    Reconnect();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == NativeMethods.WM_MOUSEACTIVATE)
            {
                // Dismiss the tab context menu when the user clicks inside the RDP frame.
                // The RDP ActiveX control swallows mouse events, so the context menu never
                // receives a "click elsewhere" notification and stays open (#330).
                if (cmenTab.Visible)
                    cmenTab.Close();

                // Issue #2175: Forward keyboard focus to the active InterfaceControl when
                // the user clicks while the cursor is over the remote session area. Using
                // BeginInvoke defers the focus call until after window activation completes,
                // so the RDP control reliably receives keyboard input on the first click.
                if (IsHandleCreated && !IsDisposed && !Disposing)
                    BeginInvoke((MethodInvoker)FocusInterfaceControlIfMouseOver);
            }

            base.WndProc(ref m);
        }

        private void FocusInterfaceControlIfMouseOver()
        {
            try
            {
                InterfaceControl? ic = GetInterfaceControl();
                if (ic?.Protocol == null || ic.IsDisposed) return;

                Point mouseScreen = Control.MousePosition;
                Rectangle icScreenBounds = ic.RectangleToScreen(ic.ClientRectangle);
                if (icScreenBounds.Contains(mouseScreen))
                {
                    ic.Protocol.Focus();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    "FocusInterfaceControlIfMouseOver (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        #region Protocols

#pragma warning disable CA1707 // Legacy event handler naming; referenced by ConnectionInitiator via delegate
        public void Prot_Event_Closed(object sender)
#pragma warning restore CA1707
        {
            bool keepTabOpen = Properties.OptionsTabsPanelsPage.Default.KeepTabsOpenAfterDisconnect;
            HandleProtocolClosed(sender, keepTabOpen);
        }

        private void HandleProtocolClosed(object sender, bool keepTabOpen)
        {
            try
            {
                if (IsDisposed || Disposing || !IsHandleCreated)
                    return;

                if (InvokeRequired)
                {
                    try
                    {
                        BeginInvoke(new Action<object, bool>(HandleProtocolClosed), sender, keepTabOpen);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Window already disposed while protocol close callback was queued.
                    }
                    catch (InvalidOperationException)
                    {
                        // Window handle is no longer valid.
                    }

                    return;
                }

                ProtocolBase? protocolBase = sender as ProtocolBase;
                if (!(protocolBase?.InterfaceControl?.Parent is ConnectionTab tabPage)) return;
                if (tabPage.Disposing || tabPage.IsDisposed) return;

                ConnectionInfo? closedConnectionInfo =
                    tabPage.TrackedConnectionInfo ??
                    protocolBase.InterfaceControl.OriginalInfo ??
                    GetConnectionInfoForTab(tabPage) ??
                    protocolBase.InterfaceControl.Info;

                if (closedConnectionInfo != null)
                    tabPage.TrackConnection(closedConnectionInfo);

                if (keepTabOpen)
                {
                    if (protocolBase.InterfaceControl != null)
                    {
                        var ic = protocolBase.InterfaceControl;
                        tabPage.Controls.Remove(ic);
                        try
                        {
                            if (!ic.IsDisposed)
                                ic.Dispose();
                        }
                        catch (InvalidOperationException)
                        {
                            // Dispose() cannot be called while CreateHandle() is in progress.
                            // This can happen when a protocol disconnect fires during handle creation
                            // (WinForms pumps messages during CreateWindowEx). Defer to next UI tick.
                            try
                            {
                                BeginInvoke(new MethodInvoker(() =>
                                {
                                    if (!ic.IsDisposed) ic.Dispose();
                                }));
                            }
                            catch (ObjectDisposedException)

                            {

                                _ = 0; // Intentionally empty

                            }
                            catch (InvalidOperationException)

                            {

                                _ = 0; // Intentionally empty

                            }
                        }
                    }

                    tabPage.ShowClosedState();
                    // Re-focus the tab so that disposing the protocol control does not shift
                    // focus to the first tab (issue #1645).
                    tabPage.DockHandler.Activate();
                    if (closedConnectionInfo != null)
                        FrmMain.Default.SelectedConnection = closedConnectionInfo;
                    return;
                }

                tabPage.protocolClose = true;
                try
                {
                    tabPage.Close();
                }
                catch (ObjectDisposedException)
                {
                    // Tab was already disposed by another close path.
                }
                catch (InvalidOperationException)
                {
                    // Handle invalidated during close operation.
                }
                finally
                {
                    ClosePanelIfEmpty();
                }
            }
            catch (ObjectDisposedException)
            {
                // Window/tab disposed while protocol close callback was in progress.
            }
            catch (InvalidOperationException)
            {
                // UI state/layout changed while processing protocol close.
            }
        }

        #endregion

        #region Persistence

        private readonly List<string> _pendingConnectionIds = new();

        protected override string GetPersistString()
        {
            var connectionIds = new List<string>();
            if (connDock != null && !connDock.IsDisposed)
            {
                foreach (var doc in connDock.Contents)
                {
                    if (doc is ConnectionTab tab)
                    {
                        var info = GetConnectionInfoForTab(tab);
                        if (info != null && !string.IsNullOrEmpty(info.ConstantID))
                        {
                            connectionIds.Add(info.ConstantID);
                        }
                    }
                }
            }
            
            // Preserve pending IDs that haven't been processed yet
            connectionIds.AddRange(_pendingConnectionIds);
            
            string joinedIds = string.Join(",", connectionIds.Distinct(StringComparer.Ordinal));
            string titleEncoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Text));

            return $"{typeof(ConnectionWindow)};{titleEncoded};{joinedIds}";
        }

        public void LoadConnections(IEnumerable<string> ids)
        {
            _pendingConnectionIds.Clear();
            _pendingConnectionIds.AddRange(ids);

            if (Runtime.ConnectionsService.IsConnectionsFileLoaded)
            {
                ProcessPendingConnections();
            }
            else
            {
                Runtime.ConnectionsService.ConnectionsLoaded += ConnectionsService_ConnectionsLoaded;
            }
        }

        private void ConnectionsService_ConnectionsLoaded(object? sender, EventArgs e)
        {
            Runtime.ConnectionsService.ConnectionsLoaded -= ConnectionsService_ConnectionsLoaded;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ProcessPendingConnections));
            }
            else
            {
                ProcessPendingConnections();
            }
        }

        private void ProcessPendingConnections()
        {
            if (_pendingConnectionIds.Count == 0) return;
            var tree = Runtime.ConnectionsService.ConnectionTreeModel;
            if (tree == null) return;

            foreach (var id in _pendingConnectionIds)
            {
                var info = tree.FindConnectionById(id);
                if (info != null)
                {
                    Runtime.ConnectionInitiator.OpenConnection(info, ConnectionInfo.Force.DoNotJump, this);
                }
            }
            _pendingConnectionIds.Clear();
        }

        #endregion
    }
}
