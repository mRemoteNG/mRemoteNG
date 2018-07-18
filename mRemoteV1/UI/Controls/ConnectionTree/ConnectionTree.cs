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
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.UI.Controls
{
    public partial class ConnectionTree : TreeListView, IConnectionTree
    {
        private readonly ConnectionTreeDragAndDropHandler _dragAndDropHandler = new ConnectionTreeDragAndDropHandler();
        private readonly PuttySessionsManager _puttySessionsManager = PuttySessionsManager.Instance;
	    private readonly StatusImageList _statusImageList = new StatusImageList();
		private bool _nodeInEditMode;
        private bool _allowEdit;
        private ConnectionContextMenu _contextMenu;
        private ConnectionTreeModel _connectionTreeModel;

        public ConnectionInfo SelectedNode => (ConnectionInfo) SelectedObject;

        public NodeSearcher NodeSearcher { get; private set; }

        public IConfirm<ConnectionInfo> NodeDeletionConfirmer { get; set; } = new AlwaysConfirmYes();

        public IEnumerable<IConnectionTreeDelegate> PostSetupActions { get; set; } = new IConnectionTreeDelegate[0];

        public ITreeNodeClickHandler<ConnectionInfo> DoubleClickHandler { get; set; } = new TreeNodeCompositeClickHandler();

        public ITreeNodeClickHandler<ConnectionInfo> SingleClickHandler { get; set; } = new TreeNodeCompositeClickHandler();

        public ConnectionTreeModel ConnectionTreeModel
        {
            get { return _connectionTreeModel; }
            set
            {
                _connectionTreeModel = value;
                PopulateTreeView();
            }
        }

        public ConnectionTree()
        {
            InitializeComponent();
            SetupConnectionTreeView();
            UseOverlays = false;
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _statusImageList?.Dispose();
            }
            base.Dispose(disposing);
        }


        #region ConnectionTree Setup
        private void SetupConnectionTreeView()
        {
            SmallImageList = _statusImageList.ImageList;
            AddColumns(_statusImageList.ImageGetter);
            LinkModelToView();
            _contextMenu = new ConnectionContextMenu(this);
            ContextMenuStrip = _contextMenu;
            SetupDropSink();
            SetEventHandlers();
        }

        private void AddColumns(ImageGetterDelegate imageGetterDelegate)
        {
            Columns.Add(new NameColumn(imageGetterDelegate));
        }

        private void LinkModelToView()
        {
            CanExpandGetter = item =>
            {
                var itemAsContainer = item as ContainerInfo;
                return itemAsContainer?.Children.Count > 0;
            };
            ChildrenGetter = item => ((ContainerInfo)item).Children;
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
            Collapsed += (sender, args) =>
            {
                var container = args.Model as ContainerInfo;
                if (container == null) return;
                container.IsExpanded = false;
				AutoResizeColumn(Columns[0]);
			};
            Expanded += (sender, args) =>
            {
                var container = args.Model as ContainerInfo;
                if (container == null) return;
                container.IsExpanded = true;
				AutoResizeColumn(Columns[0]);
			};
            SelectionChanged += tvConnections_AfterSelect;
            MouseDoubleClick += OnMouse_DoubleClick;
            MouseClick += OnMouse_SingleClick;
            CellToolTipShowing += tvConnections_CellToolTipShowing;
            ModelCanDrop += _dragAndDropHandler.HandleEvent_ModelCanDrop;
            ModelDropped += _dragAndDropHandler.HandleEvent_ModelDropped;
            BeforeLabelEdit += OnBeforeLabelEdit;
            AfterLabelEdit += OnAfterLabelEdit;
        }

		/// <summary>
		/// Resizes the given column to ensure that all content is shown
		/// </summary>
	    private void AutoResizeColumn(ColumnHeader column)
	    {
		    if (InvokeRequired)
		    {
			    Invoke((MethodInvoker) (() => AutoResizeColumn(column)));
			    return;
		    }

		    var longestIndentationAndTextWidth = int.MinValue;
		    var horizontalScrollOffset = LowLevelScrollPosition.X;
		    const int padding = 10;

		    for (var i = 0; i < Items.Count; i++)
		    {
			    var rowIndentation = Items[i].Position.X;
			    var rowTextWidth = TextRenderer.MeasureText(Items[i].Text, Font).Width;

				longestIndentationAndTextWidth = Math.Max(rowIndentation + rowTextWidth, longestIndentationAndTextWidth);
		    }

		    column.Width = longestIndentationAndTextWidth +
		                   SmallImageSize.Width +
		                   horizontalScrollOffset +
		                   padding;
		}

        private void PopulateTreeView()
        {
            UnregisterModelUpdateHandlers();
            SetObjects(ConnectionTreeModel.RootNodes);
            RegisterModelUpdateHandlers();
            NodeSearcher = new NodeSearcher(ConnectionTreeModel);
            ExecutePostSetupActions();
			AutoResizeColumn(Columns[0]);
		}

        private void RegisterModelUpdateHandlers()
        {
            _puttySessionsManager.PuttySessionsCollectionChanged += OnPuttySessionsCollectionChanged;
            ConnectionTreeModel.CollectionChanged += HandleCollectionChanged;
            ConnectionTreeModel.PropertyChanged += HandleCollectionPropertyChanged;
        }

        private void UnregisterModelUpdateHandlers()
        {
            _puttySessionsManager.PuttySessionsCollectionChanged -= OnPuttySessionsCollectionChanged;
            ConnectionTreeModel.CollectionChanged -= HandleCollectionChanged;
            ConnectionTreeModel.PropertyChanged -= HandleCollectionPropertyChanged;
        }

        private void OnPuttySessionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshObjects(GetRootPuttyNodes().ToList());
        }

        private void HandleCollectionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // for some reason property changed events are getting triggered twice for each changed property. should be just once. cant find source of duplication
            // Removed "TO DO" from above comment. Per #142 it apperas that this no longer occurs with ObjectListView 2.9.1
            var property = propertyChangedEventArgs.PropertyName;
            if (property != nameof(ConnectionInfo.Name)
                && property != nameof(ConnectionInfo.OpenConnections)
                && property != nameof(ConnectionInfo.Icon))
            {
                return;
            }

            var senderAsConnectionInfo = sender as ConnectionInfo;
            if (senderAsConnectionInfo == null)
                return;

            RefreshObject(senderAsConnectionInfo);
			AutoResizeColumn(Columns[0]);
		}

        private void ExecutePostSetupActions()
        {
            foreach (var action in PostSetupActions)
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

        public void InvokeExpand(object model)
        {
            Invoke((MethodInvoker)(() => Expand(model)));
        }

        public void InvokeRebuildAll(bool preserveState)
        {
            Invoke((MethodInvoker)(() => RebuildAll(preserveState)));
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strErrorAddFolderFailed, ex);
            }
        }

        private void AddNode(ConnectionInfo newNode)
        {
            if (SelectedNode?.GetTreeNodeType() == TreeNodeType.PuttyRoot || SelectedNode?.GetTreeNodeType() == TreeNodeType.PuttySession)
                return;

            // use root node if no node is selected
            ConnectionInfo parentNode = SelectedNode ?? GetRootConnectionNode();
            DefaultConnectionInfo.Instance.SaveTo(newNode);
            DefaultConnectionInheritance.Instance.SaveTo(newNode.Inheritance);
            var selectedContainer = parentNode as ContainerInfo;
            var parent = selectedContainer ?? parentNode?.Parent;
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

            var selectedNodeType = SelectedNode.GetTreeNodeType();
            if (selectedNodeType != TreeNodeType.Connection && selectedNodeType != TreeNodeType.Container)
                return;

            var newNode = SelectedNode.Clone();
            SelectedNode.Parent.AddChildBelow(newNode, SelectedNode);
            newNode.Parent.SetChildBelow(newNode, SelectedNode);
        }

        public void RenameSelectedNode()
        {
            _allowEdit = true;
            SelectedItem.BeginEdit();
        }

        public void DeleteSelectedNode()
        {
            if (SelectedNode is RootNodeInfo || SelectedNode is PuttySessionInfo) return;
            if (!NodeDeletionConfirmer.Confirm(SelectedNode)) return;
            ConnectionTreeModel.DeleteNode(SelectedNode);
        }

        public void SortRecursive(ConnectionInfo sortTarget, ListSortDirection sortDirection)
        {
            if (sortTarget == null)
                sortTarget = GetRootConnectionNode();

            Runtime.ConnectionsService.BeginBatchingSaves();

            var sortTargetAsContainer = sortTarget as ContainerInfo;
            if (sortTargetAsContainer != null)
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

        protected override void UpdateFiltering()
        {
            base.UpdateFiltering();
            AutoResizeColumn(Columns[0]);
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
			// disable filtering if necessary. prevents RefreshObjects from
			// throwing an exception
			var filteringEnabled = IsFiltering;
			var filter = ModelFilter;
			if (filteringEnabled)
			{
				ResetColumnFiltering();
			}

			RefreshObject(sender);
			AutoResizeColumn(Columns[0]);

			// turn filtering back on
			if (filteringEnabled)
			{
				ModelFilter = filter;
				UpdateFiltering();
			}
		}

        private void tvConnections_AfterSelect(object sender, EventArgs e)
        {
            try
            {
                Windows.ConfigForm.SelectedTreeNode = SelectedNode;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterSelect (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void OnMouse_DoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks < 2) return;
            OLVColumn column;
            var listItem = GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out column);
	        var clickedNode = listItem?.RowObject as ConnectionInfo;
            if (clickedNode == null) return;
            DoubleClickHandler.Execute(clickedNode);
        }

        private void OnMouse_SingleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks > 1) return;
            OLVColumn column;
            var listItem = GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out column);
            var clickedNode = listItem?.RowObject as ConnectionInfo;
            if (clickedNode == null) return;
            SingleClickHandler.Execute(clickedNode);
        }

        private void tvConnections_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
        {
            try
            {
                var nodeProducingTooltip = (ConnectionInfo)e.Model;
                e.Text = nodeProducingTooltip.Description;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_MouseMove (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void OnBeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (_nodeInEditMode || !(sender is ConnectionTree))
                return;

            if (!_allowEdit || SelectedNode is PuttySessionInfo || SelectedNode is RootPuttySessionsNodeInfo)
            {
                e.CancelEdit = true;
                return;
            }

            _nodeInEditMode = true;
            _contextMenu.DisableShortcutKeys();
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
                Windows.ConfigForm.SelectedTreeNode = SelectedNode;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterLabelEdit (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }
        #endregion
    }
}