using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Themes;
using mRemoteNG.Tools.Clipboard;
using mRemoteNG.Tree;
using mRemoteNG.Tree.ClickHandlers;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    [SupportedOSPlatform("windows")]
    public partial class ConnectionTree : TreeListView, IConnectionTree
    {
        private readonly ConnectionTreeDragAndDropHandler _dragAndDropHandler = new();
        private readonly PuttySessionsManager _puttySessionsManager = PuttySessionsManager.Instance;
        private readonly StatusImageList _statusImageList = new();
        private ThemeManager _themeManager;

        private readonly ConnectionTreeSearchTextFilter _connectionTreeSearchTextFilter = new();

        private bool _nodeInEditMode;
        private bool _allowEdit;
        private ConnectionContextMenu _contextMenu;
        private ConnectionTreeModel _connectionTreeModel;
        private ISlowClickRenameHandler? _slowClickRenameHandler;

        // Number of direct children currently allowed to be shown for a container
        // that is mid-animation. Containers not present in this dictionary show
        // all of their children (the normal, non-animated state).
        private readonly Dictionary<ContainerInfo, int> _revealLimits = new();

        // Containers currently animating. Value is true while expanding (growing
        // the reveal limit towards the full child count) and false while
        // collapsing (shrinking the reveal limit towards zero).
        private readonly Dictionary<ContainerInfo, bool> _activeAnimations = new();

        // Guards against our own programmatic Collapse() call (issued once the
        // shrink animation reaches zero) being cancelled a second time by
        // ConnectionTree_Collapsing.
        private readonly HashSet<ContainerInfo> _collapseFinalizing = new();

        private const int RowsRevealedPerTick = 1;
        private readonly Timer _expandCollapseAnimationTimer = new() { Interval = 15 };

        public ConnectionInfo SelectedNode => (ConnectionInfo)SelectedObject;

        public NodeSearcher NodeSearcher { get; private set; }

        public IConfirm<ConnectionInfo> NodeDeletionConfirmer { get; set; } = new AlwaysConfirmYes();

        public IEnumerable<IConnectionTreeDelegate> PostSetupActions { get; set; } = Array.Empty<IConnectionTreeDelegate>();

        public ITreeNodeClickHandler<ConnectionInfo> DoubleClickHandler { get; set; } = new TreeNodeCompositeClickHandler();

        public ITreeNodeClickHandler<ConnectionInfo> SingleClickHandler { get; set; } = new TreeNodeCompositeClickHandler();

        public ConnectionTreeModel ConnectionTreeModel
        {
            get { return _connectionTreeModel; }
            set
            {
                if (_connectionTreeModel == value)
                {
                    return;
                }
                
                UnregisterModelUpdateHandlers(_connectionTreeModel);
                _connectionTreeModel = value;
                PopulateTreeView(value);
            }
        }

        public ConnectionTree()
        {
            InitializeComponent();
            SetupConnectionTreeView();
            UseOverlays = false;
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ThemeManagerOnThemeChanged;
            ApplyTheme();
            _expandCollapseAnimationTimer.Tick += ExpandCollapseAnimationTimer_Tick;
        }

        private void ThemeManagerOnThemeChanged()
        {
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ActiveAndExtended)
                return;

            ExtendedColorPalette themePalette = _themeManager.ActiveTheme.ExtendedPalette;

            BackColor = themePalette.getColor("TreeView_Background");
            ForeColor = themePalette.getColor("TreeView_Foreground");
            SelectedBackColor = themePalette.getColor("Treeview_SelectedItem_Active_Background");
            SelectedForeColor = themePalette.getColor("Treeview_SelectedItem_Active_Foreground");
            UnfocusedSelectedBackColor = themePalette.getColor("Treeview_SelectedItem_Inactive_Background");
            UnfocusedSelectedForeColor = themePalette.getColor("Treeview_SelectedItem_Inactive_Foreground");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _statusImageList?.Dispose();
                _slowClickRenameHandler?.Dispose();

                _themeManager.ThemeChanged -= ThemeManagerOnThemeChanged;

                _expandCollapseAnimationTimer.Tick -= ExpandCollapseAnimationTimer_Tick;
                _expandCollapseAnimationTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        #region ConnectionTree Setup

        private void SetupConnectionTreeView()
        {
            SetSmallImageList(_statusImageList.ImageList);
            AddColumns(_statusImageList.ImageGetter);
            LinkModelToView();
            _contextMenu = new ConnectionContextMenu(this);
            ContextMenuStrip = _contextMenu;
            SetupDropSink();
            SetEventHandlers();
            SetupSlowClickRename();
        }

        internal void SetupSlowClickRename()
        {
            _slowClickRenameHandler?.Dispose();
            _slowClickRenameHandler = Settings.Default.SlowClickRenameEnabled
                ? new SlowClickRenameHandler(
                    new SlowClickRenameTimer(SystemInformation.DoubleClickTime),
                    RenameSelectedNode,
                    () => SelectedNode)
                : null;
        }

        private void AddColumns(ImageGetterDelegate imageGetterDelegate)
        {
            Columns.Add(new NameColumn(imageGetterDelegate));
        }

        private void LinkModelToView()
        {
            CanExpandGetter = item =>
            {
                ContainerInfo itemAsContainer = item as ContainerInfo;
                return itemAsContainer?.Children.Count > 0;
            };
            ChildrenGetter = item =>
            {
                ContainerInfo container = (ContainerInfo)item;
                if (_revealLimits.TryGetValue(container, out int limit))
                    return container.Children.Take(limit).ToList();

                return container.Children;
            };
        }

        private void SetupDropSink()
        {
            DropSink = new SimpleDropSink
            {
                CanDropBetween = true
            };
        }

        private void SetEventHandlers()
        {
            Expanding += ConnectionTree_Expanding;
            Collapsing += ConnectionTree_Collapsing;
            Collapsed += (sender, args) =>
            {
                if (args.Model is not ContainerInfo container) return;
                container.IsExpanded = false;
                AutoResizeColumn(Columns[0]);
            };
            Expanded += (sender, args) =>
            {
                if (args.Model is not ContainerInfo container) return;
                container.IsExpanded = true;
                AutoResizeColumn(Columns[0]);
            };
            SelectionChanged += TvConnections_AfterSelect;
            MouseDoubleClick += OnMouse_DoubleClick;
            MouseClick += OnMouse_SingleClick;
            CellToolTipShowing += TvConnections_CellToolTipShowing;
            ModelCanDrop += _dragAndDropHandler.HandleEvent_ModelCanDrop;
            ModelDropped += _dragAndDropHandler.HandleEvent_ModelDropped;
            BeforeLabelEdit += OnBeforeLabelEdit;
            AfterLabelEdit += OnAfterLabelEdit;
            FormatCell += ConnectionTree_FormatCell;
        }

        /// <summary>
        /// Instead of letting the branch insert all of its children at once,
        /// start with a single child visible and grow the reveal limit on a
        /// timer so children slide into view progressively.
        /// </summary>
        private void ConnectionTree_Expanding(object sender, TreeBranchExpandingEventArgs e)
        {
            if (!ExpandCollapseAnimationsAllowed)
                return;

            if (e.Model is not ContainerInfo container || container.Children.Count == 0)
                return;

            _revealLimits[container] = Math.Min(1, container.Children.Count);
            _activeAnimations[container] = true;
            StartAnimationTimerIfNeeded();
        }

        /// <summary>
        /// Cancels the default (instant) collapse and instead shrinks the
        /// reveal limit down to zero on a timer, so children disappear
        /// progressively before the branch is actually collapsed.
        /// </summary>
        private void ConnectionTree_Collapsing(object sender, TreeBranchCollapsingEventArgs e)
        {
            if (e.Model is not ContainerInfo container)
                return;

            // This is our own programmatic Collapse() call, issued once the
            // shrink animation has finished. Let it proceed normally.
            if (_collapseFinalizing.Remove(container))
                return;

            if (!ExpandCollapseAnimationsAllowed || container.Children.Count == 0)
                return;

            e.Canceled = true;

            _revealLimits[container] = container.Children.Count;
            _activeAnimations[container] = false;
            StartAnimationTimerIfNeeded();
        }

        /// <summary>
        /// Determines whether the connection tree's expand/collapse animation
        /// should run. Disabled when the user has High Contrast mode enabled
        /// (a signal that reduced/no motion effects are preferred), or when
        /// the user has turned off connection tree animations in
        /// Options > Appearance.
        /// </summary>
        private static bool ExpandCollapseAnimationsAllowed =>
            !SystemInformation.HighContrast &&
            Properties.OptionsAppearancePage.Default.EnableConnectionTreeAnimations;

        private void StartAnimationTimerIfNeeded()
        {
            if (!_expandCollapseAnimationTimer.Enabled)
                _expandCollapseAnimationTimer.Start();
        }

        private void ExpandCollapseAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (_activeAnimations.Count == 0)
            {
                _expandCollapseAnimationTimer.Stop();
                return;
            }

            List<ContainerInfo> finishedExpansions = new();
            List<ContainerInfo> finishedCollapses = new();

            foreach (KeyValuePair<ContainerInfo, bool> entry in _activeAnimations)
            {
                ContainerInfo container = entry.Key;
                bool isExpanding = entry.Value;
                int totalChildren = container.Children.Count;
                int currentLimit = _revealLimits.TryGetValue(container, out int limit) ? limit : 0;

                if (isExpanding)
                {
                    currentLimit = Math.Min(totalChildren, currentLimit + RowsRevealedPerTick);
                    _revealLimits[container] = currentLimit;
                    RefreshObject(container);

                    if (currentLimit >= totalChildren)
                        finishedExpansions.Add(container);
                }
                else
                {
                    currentLimit = Math.Max(0, currentLimit - RowsRevealedPerTick);
                    _revealLimits[container] = currentLimit;
                    RefreshObject(container);

                    if (currentLimit <= 0)
                        finishedCollapses.Add(container);
                }
            }

            foreach (ContainerInfo container in finishedExpansions)
            {
                _activeAnimations.Remove(container);
                _revealLimits.Remove(container);
                RefreshObject(container);
            }

            foreach (ContainerInfo container in finishedCollapses)
            {
                _activeAnimations.Remove(container);
                _collapseFinalizing.Add(container);
                Collapse(container);
                _revealLimits.Remove(container);
            }

            AutoResizeColumn(Columns[0]);

            if (_activeAnimations.Count == 0)
                _expandCollapseAnimationTimer.Stop();
        }

        /// <summary>
        /// Resizes the given column to ensure that all content is shown
        /// </summary>
        private void AutoResizeColumn(ColumnHeader column)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => AutoResizeColumn(column)));
                return;
            }

            int longestIndentationAndTextWidth = int.MinValue;
            int horizontalScrollOffset = LowLevelScrollPosition.X;
            const int padding = 10;

            for (int i = 0; i < Items.Count; i++)
            {
                int rowIndentation = Items[i].Position.X;
                int rowTextWidth = TextRenderer.MeasureText(Items[i].Text, Font).Width;

                longestIndentationAndTextWidth = Math.Max(rowIndentation + rowTextWidth, longestIndentationAndTextWidth);
            }

            column.Width = longestIndentationAndTextWidth + SmallImageSize.Width + horizontalScrollOffset + padding;
        }

        private void PopulateTreeView(ConnectionTreeModel newModel)
        {
            _expandCollapseAnimationTimer.Stop();
            _revealLimits.Clear();
            _activeAnimations.Clear();
            _collapseFinalizing.Clear();

            SetObjects(newModel.RootNodes);
            RegisterModelUpdateHandlers(newModel);
            NodeSearcher = new NodeSearcher(newModel);
            ExecutePostSetupActions();
            AutoResizeColumn(Columns[0]);
        }

        private void RegisterModelUpdateHandlers(ConnectionTreeModel newModel)
        {
            _puttySessionsManager.PuttySessionsCollectionChanged += OnPuttySessionsCollectionChanged;
            newModel.CollectionChanged += HandleCollectionChanged;
            newModel.PropertyChanged += HandleCollectionPropertyChanged;
        }

        private void UnregisterModelUpdateHandlers(ConnectionTreeModel oldConnectionTreeModel)
        {
            _puttySessionsManager.PuttySessionsCollectionChanged -= OnPuttySessionsCollectionChanged;

            if (oldConnectionTreeModel == null)
                return;

            oldConnectionTreeModel.CollectionChanged -= HandleCollectionChanged;
            oldConnectionTreeModel.PropertyChanged -= HandleCollectionPropertyChanged;
        }

        private void OnPuttySessionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshObjects(GetRootPuttyNodes().ToList());
        }

        private void HandleCollectionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // for some reason property changed events are getting triggered twice for each changed property. should be just once. cant find source of duplication
            // Removed "TO DO" from above comment. Per #142 it apperas that this no longer occurs with ObjectListView 2.9.1
            string property = propertyChangedEventArgs.PropertyName;
            if (property != nameof(ConnectionInfo.Name)
             && property != nameof(ConnectionInfo.OpenConnections)
             && property != nameof(ConnectionInfo.Icon))
            {
                return;
            }

            if (sender is not ConnectionInfo senderAsConnectionInfo)
                return;

            RefreshObject(senderAsConnectionInfo);
            AutoResizeColumn(Columns[0]);
        }

        private void ExecutePostSetupActions()
        {
            foreach (IConnectionTreeDelegate action in PostSetupActions)
            {
                action.Execute(this);
            }
        }

        #endregion

        #region ConnectionTree Behavior

        public RootNodeInfo GetRootConnectionNode()
        {
            return (RootNodeInfo)ConnectionTreeModel.RootNodes.First(item => item is RootNodeInfo);
        }

        public new void Invoke(Action action)
        {
            Invoke((Delegate)action);
        }

        public void InvokeExpand(object model)
        {
            Invoke(() => Expand(model));
        }

        public void InvokeRebuildAll(bool preserveState)
        {
            Invoke(() => RebuildAll(preserveState));
        }

        public IEnumerable<RootPuttySessionsNodeInfo> GetRootPuttyNodes()
        {
            return Objects.OfType<RootPuttySessionsNodeInfo>();
        }

        public void AddConnection()
        {
            try
            {
                AddNode(new ConnectionInfo());
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("UI.Window.Tree.AddConnection() failed.", ex);
            }
        }

        public void AddFolder()
        {
            try
            {
                AddNode(new ContainerInfo());
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ErrorAddFolderFailed, ex);
            }
        }

        private void AddNode(ConnectionInfo newNode)
        {
            if (SelectedNode?.GetTreeNodeType() == TreeNodeType.PuttyRoot ||
                SelectedNode?.GetTreeNodeType() == TreeNodeType.PuttySession)
                return;

            // the new node will survive filtering if filtering is active
            _connectionTreeSearchTextFilter.SpecialInclusionList.Add(newNode);

            // use root node if no node is selected
            ConnectionInfo parentNode = SelectedNode ?? GetRootConnectionNode();
            DefaultConnectionInfo.Instance.SaveTo(newNode);
            DefaultConnectionInheritance.Instance.SaveTo(newNode.Inheritance);
            ContainerInfo selectedContainer = parentNode as ContainerInfo;
            ContainerInfo parent = selectedContainer ?? parentNode?.Parent;
            newNode.SetParent(parent);
            Expand(parent);
            SelectObject(newNode, true);
            EnsureModelVisible(newNode);
            _allowEdit = true;
            SelectedItem.BeginEdit();
        }

        public void DuplicateSelectedNode()
        {
            if (SelectedNode == null)
                return;

            TreeNodeType selectedNodeType = SelectedNode.GetTreeNodeType();
            if (selectedNodeType != TreeNodeType.Connection && selectedNodeType != TreeNodeType.Container)
                return;

            ConnectionInfo newNode = SelectedNode.Clone();
            SelectedNode.Parent.AddChildBelow(newNode, SelectedNode);
            newNode.Parent.SetChildBelow(newNode, SelectedNode);
        }

        public void RenameSelectedNode()
        {
            if (SelectedItem == null) return;
            _slowClickRenameHandler?.Cancel();
            _allowEdit = true;
            SelectedItem.BeginEdit();
        }

        public void DeleteSelectedNode()
        {
            if (SelectedNode is RootNodeInfo || SelectedNode is PuttySessionInfo) return;
            if (!NodeDeletionConfirmer.Confirm(SelectedNode)) return;
            ConnectionTreeModel.DeleteNode(SelectedNode);
        }

        /// <summary>
        /// Copies the Hostname of the selected connection (or the Name of
        /// the selected container) to the given <see cref="IClipboard"/>.
        /// </summary>
        /// <param name="clipboard"></param>
        public void CopyHostnameSelectedNode(IClipboard clipboard)
        {
            if (SelectedNode == null)
                return;

            string textToCopy = SelectedNode.IsContainer ? SelectedNode.Name : SelectedNode.Hostname;

            if (string.IsNullOrEmpty(textToCopy))
                return;

            clipboard.SetText(textToCopy);
        }

        public void SortRecursive(ConnectionInfo sortTarget, ListSortDirection sortDirection)
        {
            sortTarget ??= GetRootConnectionNode();

            Runtime.ConnectionsService.BeginBatchingSaves();

            if (sortTarget is ContainerInfo sortTargetAsContainer)
                sortTargetAsContainer.SortRecursive(sortDirection);
            else
                SelectedNode.Parent.SortRecursive(sortDirection);

            Runtime.ConnectionsService.EndBatchingSaves();
        }

        /// <summary>
        /// Expands all tree objects and recalculates the
        /// column widths.
        /// </summary>
        public override void ExpandAll()
        {
            base.ExpandAll();
            AutoResizeColumn(Columns[0]);
        }

        /// <summary>
        /// Filters tree items based on the given <see cref="filterText"/>
        /// </summary>
        /// <param name="filterText">The text to filter by</param>
        public void ApplyFilter(string filterText)
        {
            UseFiltering = true;
            _connectionTreeSearchTextFilter.FilterText = filterText;
            ModelFilter = _connectionTreeSearchTextFilter;
        }

        /// <summary>
        /// Removes all item filtering from the connection tree
        /// </summary>
        public void RemoveFilter()
        {
            UseFiltering = false;
            ResetColumnFiltering();
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            // disable filtering if necessary. prevents RefreshObjects from
            // throwing an exception
            bool filteringEnabled = IsFiltering;
            IModelFilter filter = ModelFilter;
            if (filteringEnabled)
            {
                ResetColumnFiltering();
            }

            RefreshObject(sender);
            AutoResizeColumn(Columns[0]);

            // turn filtering back on
            if (!filteringEnabled) return;
            ModelFilter = filter;
            UpdateFiltering();
        }

        protected override void UpdateFiltering()
        {
            base.UpdateFiltering();
            AutoResizeColumn(Columns[0]);
        }

        private void TvConnections_AfterSelect(object sender, EventArgs e)
        {
            try
            {
                _slowClickRenameHandler?.CancelIfDifferentNode(SelectedNode);
                AppWindows.ConfigForm.SelectedTreeNode = SelectedNode;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterSelect (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void OnMouse_DoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks < 2) return;
            OLVListItem listItem = GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out _);
            if (listItem?.RowObject is not ConnectionInfo clickedNode) return;
            _slowClickRenameHandler?.Cancel();
            DoubleClickHandler.Execute(clickedNode);
        }

        private void OnMouse_SingleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks > 1) return;
            OLVListItem listItem = GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out _);
            if (listItem?.RowObject is not ConnectionInfo clickedNode) return;
            _slowClickRenameHandler?.Execute(clickedNode);
            SingleClickHandler.Execute(clickedNode);
        }

        private void TvConnections_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
        {
            try
            {
                if (!Properties.OptionsAppearancePage.Default.ShowDescriptionTooltipsInTree)
                {
                    // setting text to null prevents the tooltip from being shown
                    e.Text = null;
                    return;
                }

                ConnectionInfo nodeProducingTooltip = (ConnectionInfo)e.Model;
                e.Text = nodeProducingTooltip.Description;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                "tvConnections_MouseMove (UI.Window.ConnectionTreeWindow) failed",
                                                                ex);
            }
        }

        private void OnBeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (_nodeInEditMode || sender is not ConnectionTree)
                return;

            if (!_allowEdit || SelectedNode is PuttySessionInfo || SelectedNode is RootPuttySessionsNodeInfo)
            {
                e.CancelEdit = true;
                return;
            }

            _nodeInEditMode = true;
            _contextMenu.DisableShortcutKeys();
        }

        private void ConnectionTree_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Model is not ConnectionInfo connectionInfo)
                return;

            string colorString = connectionInfo.Color;
            if (string.IsNullOrEmpty(colorString))
                return;

            try
            {
                System.Drawing.ColorConverter converter = new();
                System.Drawing.Color color = (System.Drawing.Color)converter.ConvertFromString(colorString);
                e.SubItem.ForeColor = color;
            }
            catch
            {
                // If color parsing fails, just ignore and use default color
            }
        }

        private void OnAfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (!_nodeInEditMode)
                return;

            try
            {
                _contextMenu.EnableShortcutKeys();
                ConnectionTreeModel.RenameNode(SelectedNode, e.Label);
                _nodeInEditMode = false;
                _allowEdit = false;
                _slowClickRenameHandler?.Cancel();
                // ensures that if we are filtering and a new item is added that doesn't match the filter, it will be filtered out
                _connectionTreeSearchTextFilter.SpecialInclusionList.Clear();
                UpdateFiltering();
                AppWindows.ConfigForm.SelectedTreeNode = SelectedNode;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterLabelEdit (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        #endregion
    }
}