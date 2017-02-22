using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Themes;
using mRemoteNG.UI.Controls.FilteredPropertyGrid;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
    public class ConfigWindow : BaseWindow
	{
        private bool _originalPropertyGridToolStripItemCountValid;
        private int _originalPropertyGridToolStripItemCount;
        private System.ComponentModel.Container _components;
        private ToolStripButton _btnShowProperties;
        private ToolStripButton _btnShowDefaultProperties;
        private ToolStripButton _btnShowInheritance;
        private ToolStripButton _btnShowDefaultInheritance;
        private ToolStripButton _btnIcon;
        private ToolStripButton _btnHostStatus;
        internal ContextMenuStrip CMenIcons;
        internal ContextMenuStrip PropertyGridContextMenu;
        private ToolStripMenuItem _propertyGridContextMenuShowHelpText;
        private ToolStripMenuItem _propertyGridContextMenuReset;
        private ToolStripSeparator _toolStripSeparator1;
        private FilteredPropertyGrid _pGrid;

        private AbstractConnectionRecord _selectedTreeNode;
        public AbstractConnectionRecord SelectedTreeNode
        {
            get { return _selectedTreeNode; }
            set
            {
                _selectedTreeNode = value;
                SetPropertyGridObject(_selectedTreeNode);
            }
        }

        private void InitializeComponent()
		{
            _components = new System.ComponentModel.Container();
            Load += Config_Load;
            SystemColorsChanged += Config_SystemColorsChanged;
            _pGrid = new FilteredPropertyGrid();
            _pGrid.PropertyValueChanged += pGrid_PropertyValueChanged;
            _pGrid.PropertySortChanged += pGrid_PropertySortChanged;
            PropertyGridContextMenu = new ContextMenuStrip(_components);
            PropertyGridContextMenu.Opening += propertyGridContextMenu_Opening;
            _propertyGridContextMenuReset = new ToolStripMenuItem();
            _propertyGridContextMenuReset.Click += propertyGridContextMenuReset_Click;
            _toolStripSeparator1 = new ToolStripSeparator();
            _propertyGridContextMenuShowHelpText = new ToolStripMenuItem();
            _propertyGridContextMenuShowHelpText.Click += propertyGridContextMenuShowHelpText_Click;
            _propertyGridContextMenuShowHelpText.CheckedChanged += propertyGridContextMenuShowHelpText_CheckedChanged;
            _btnShowInheritance = new ToolStripButton();
            _btnShowInheritance.Click += btnShowInheritance_Click;
            _btnShowDefaultInheritance = new ToolStripButton();
            _btnShowDefaultInheritance.Click += btnShowDefaultInheritance_Click;
            _btnShowProperties = new ToolStripButton();
            _btnShowProperties.Click += btnShowProperties_Click;
            _btnShowDefaultProperties = new ToolStripButton();
            _btnShowDefaultProperties.Click += btnShowDefaultProperties_Click;
            _btnIcon = new ToolStripButton();
            _btnIcon.MouseUp += btnIcon_Click;
            _btnHostStatus = new ToolStripButton();
            _btnHostStatus.Click += btnHostStatus_Click;
            CMenIcons = new ContextMenuStrip(_components);
            PropertyGridContextMenu.SuspendLayout();
            SuspendLayout();
            //
            //pGrid
            //
            _pGrid.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)
                | AnchorStyles.Right;
            _pGrid.BrowsableProperties = null;
            _pGrid.ContextMenuStrip = PropertyGridContextMenu;
            _pGrid.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _pGrid.HiddenAttributes = null;
            _pGrid.HiddenProperties = null;
            _pGrid.Location = new Point(0, 0);
            _pGrid.Name = "_pGrid";
            _pGrid.PropertySort = PropertySort.Categorized;
            _pGrid.Size = new Size(226, 530);
            _pGrid.TabIndex = 0;
            _pGrid.UseCompatibleTextRendering = true;
            //
            //propertyGridContextMenu
            //
            PropertyGridContextMenu.Items.AddRange(new ToolStripItem[] { _propertyGridContextMenuReset, _toolStripSeparator1, _propertyGridContextMenuShowHelpText });
            PropertyGridContextMenu.Name = "PropertyGridContextMenu";
            PropertyGridContextMenu.Size = new Size(157, 76);
            //
            //propertyGridContextMenuReset
            //
            _propertyGridContextMenuReset.Name = "_propertyGridContextMenuReset";
            _propertyGridContextMenuReset.Size = new Size(156, 22);
            _propertyGridContextMenuReset.Text = @"&Reset";
            //
            //ToolStripSeparator1
            //
            _toolStripSeparator1.Name = "_toolStripSeparator1";
            _toolStripSeparator1.Size = new Size(153, 6);
            //
            //propertyGridContextMenuShowHelpText
            //
            _propertyGridContextMenuShowHelpText.Name = "_propertyGridContextMenuShowHelpText";
            _propertyGridContextMenuShowHelpText.Size = new Size(156, 22);
            _propertyGridContextMenuShowHelpText.Text = @"&Show Help Text";
            //
            //btnShowInheritance
            //
            _btnShowInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnShowInheritance.Image = Resources.Inheritance;
            _btnShowInheritance.ImageTransparentColor = Color.Magenta;
            _btnShowInheritance.Name = "_btnShowInheritance";
            _btnShowInheritance.Size = new Size(23, 22);
            _btnShowInheritance.Text = @"Inheritance";
            //
            //btnShowDefaultInheritance
            //
            _btnShowDefaultInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnShowDefaultInheritance.Image = Resources.Inheritance_Default;
            _btnShowDefaultInheritance.ImageTransparentColor = Color.Magenta;
            _btnShowDefaultInheritance.Name = "_btnShowDefaultInheritance";
            _btnShowDefaultInheritance.Size = new Size(23, 22);
            _btnShowDefaultInheritance.Text = @"Default Inheritance";
            //
            //btnShowProperties
            //
            _btnShowProperties.Checked = true;
            _btnShowProperties.CheckState = CheckState.Checked;
            _btnShowProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnShowProperties.Image = Resources.Properties;
            _btnShowProperties.ImageTransparentColor = Color.Magenta;
            _btnShowProperties.Name = "_btnShowProperties";
            _btnShowProperties.Size = new Size(23, 22);
            _btnShowProperties.Text = @"Properties";
            //
            //btnShowDefaultProperties
            //
            _btnShowDefaultProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnShowDefaultProperties.Image = Resources.Properties_Default;
            _btnShowDefaultProperties.ImageTransparentColor = Color.Magenta;
            _btnShowDefaultProperties.Name = "_btnShowDefaultProperties";
            _btnShowDefaultProperties.Size = new Size(23, 22);
            _btnShowDefaultProperties.Text = @"Default Properties";
            //
            //btnIcon
            //
            _btnIcon.Alignment = ToolStripItemAlignment.Right;
            _btnIcon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnIcon.ImageTransparentColor = Color.Magenta;
            _btnIcon.Name = "_btnIcon";
            _btnIcon.Size = new Size(23, 22);
            _btnIcon.Text = @"Icon";
            //
            //btnHostStatus
            //
            _btnHostStatus.Alignment = ToolStripItemAlignment.Right;
            _btnHostStatus.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnHostStatus.Image = Resources.HostStatus_Check;
            _btnHostStatus.ImageTransparentColor = Color.Magenta;
            _btnHostStatus.Name = "_btnHostStatus";
            _btnHostStatus.Size = new Size(23, 22);
            _btnHostStatus.Tag = "checking";
            _btnHostStatus.Text = @"Status";
            //
            //cMenIcons
            //
            CMenIcons.Name = "CMenIcons";
            CMenIcons.Size = new Size(61, 4);
            //
            //Config
            //
            ClientSize = new Size(226, 530);
            Controls.Add(_pGrid);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            HideOnClose = true;
            Icon = Resources.Config_Icon;
            Name = "ConfigWindow";
            TabText = @"Config";
            Text = @"Config";
            PropertyGridContextMenu.ResumeLayout(false);
            ResumeLayout(false);
					
		}
		
        #region Public Properties
        public bool PropertiesVisible
		{
			get
			{
			    return _btnShowProperties.Checked;
			}
			set
			{
                _btnShowProperties.Checked = value;
			    if (!value) return;
			    _btnShowInheritance.Checked = false;
			    _btnShowDefaultInheritance.Checked = false;
			    _btnShowDefaultProperties.Checked = false;
			}
		}
		
        public bool InheritanceVisible
		{
			get
			{
			    return _btnShowInheritance.Checked;
			}
			set
			{
                _btnShowInheritance.Checked = value;
			    if (!value) return;
			    _btnShowProperties.Checked = false;
			    _btnShowDefaultInheritance.Checked = false;
			    _btnShowDefaultProperties.Checked = false;
			}
		}
		
        public bool DefaultPropertiesVisible
		{
			get
			{
			    return _btnShowDefaultProperties.Checked;
			}
			set
			{
                _btnShowDefaultProperties.Checked = value;
			    if (!value) return;
			    _btnShowProperties.Checked = false;
			    _btnShowDefaultInheritance.Checked = false;
			    _btnShowInheritance.Checked = false;
			}
		}
		
        public bool DefaultInheritanceVisible
		{
			get { return _btnShowDefaultInheritance.Checked; }
			set
			{
                _btnShowDefaultInheritance.Checked = value;
			    if (!value) return;
			    _btnShowProperties.Checked = false;
			    _btnShowDefaultProperties.Checked = false;
			    _btnShowInheritance.Checked = false;
			}
		}
        #endregion

        #region Constructors

        public ConfigWindow() : this(new DockContent())
        {
        }

        public ConfigWindow(DockContent panel)
        {
            WindowType = WindowType.Config;
            DockPnl = panel;
            InitializeComponent();
        }
        #endregion

        #region Public Methods
		
		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
		{
		    // Main form handle command key events
            // Adapted from http://kiwigis.blogspot.com/2009/05/adding-tab-key-support-to-propertygrid.html
            if ((keyData & Keys.KeyCode) == Keys.Tab)
			{
				var selectedItem = _pGrid.SelectedGridItem;
				var gridRoot = selectedItem;
				while (gridRoot.GridItemType != GridItemType.Root)
				{
					gridRoot = gridRoot.Parent;
				}
						
				var gridItems = new List<GridItem>();
				FindChildGridItems(gridRoot, ref gridItems);
						
				if (!ContainsGridItemProperty(gridItems))
					return true;
						
				var newItem = selectedItem;
						
			    // ReSharper disable once SwitchStatementMissingSomeCases
				switch (keyData)
				{
				    case (Keys.Tab | Keys.Shift):
				        newItem = FindPreviousGridItemProperty(gridItems, selectedItem);
				        break;
				    case Keys.Tab:
				        newItem = FindNextGridItemProperty(gridItems, selectedItem);
				        break;
				}
						
				_pGrid.SelectedGridItem = newItem;
						
				return true; // Handled
			}

		    return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void FindChildGridItems(GridItem item, ref List<GridItem> gridItems)
		{
			gridItems.Add(item);

		    if (item.Expandable && !item.Expanded) return;
		    foreach (GridItem child in item.GridItems)
		    {
		        FindChildGridItems(child, ref gridItems);
		    }
		}
		
		private bool ContainsGridItemProperty(IEnumerable<GridItem> gridItems)
		{
		    return gridItems.Any(item => item.GridItemType == GridItemType.Property);
		}

        private GridItem FindPreviousGridItemProperty(IList<GridItem> gridItems, GridItem startItem)
		{
			if (gridItems.Count == 0 || startItem == null)
				return null;
			
			var startIndex = gridItems.IndexOf(startItem);
			if (startItem.GridItemType == GridItemType.Property)
			{
				startIndex--;
				if (startIndex < 0)
				{
					startIndex = gridItems.Count - 1;
				}
			}
			
			var previousIndex = 0;
			var previousIndexValid = false;
			for (var index = startIndex; index >= 0; index--)
			{
			    if (gridItems[index].GridItemType != GridItemType.Property) continue;
			    previousIndex = index;
			    previousIndexValid = true;
			    break;
			}
			
			if (previousIndexValid)
				return gridItems[previousIndex];
			
			for (var index = gridItems.Count - 1; index >= startIndex + 1; index--)
			{
			    if (gridItems[index].GridItemType != GridItemType.Property) continue;
			    previousIndex = index;
			    previousIndexValid = true;
			    break;
			}
			
			return !previousIndexValid ? null : gridItems[previousIndex];
		}
		
		private GridItem FindNextGridItemProperty(IList<GridItem> gridItems, GridItem startItem)
		{
			if (gridItems.Count == 0 || startItem == null)
				return null;
					
			var startIndex = gridItems.IndexOf(startItem);
			if (startItem.GridItemType == GridItemType.Property)
			{
				startIndex++;
				if (startIndex >= gridItems.Count)
				{
					startIndex = 0;
				}
			}
			
			var nextIndex = 0;
			var nextIndexValid = false;
			for (var index = startIndex; index <= gridItems.Count - 1; index++)
			{
			    if (gridItems[index].GridItemType != GridItemType.Property) continue;
			    nextIndex = index;
			    nextIndexValid = true;
			    break;
			}
			
			if (nextIndexValid)
				return gridItems[nextIndex];
					
			for (var index = 0; index <= startIndex - 1; index++)
			{
			    if (gridItems[index].GridItemType != GridItemType.Property) continue;
			    nextIndex = index;
			    nextIndexValid = true;
			    break;
			}
			
			return !nextIndexValid ? null : gridItems[nextIndex];
		}
		
		public void SetPropertyGridObject(object propertyGridObject)
		{
			try
			{
                _btnShowProperties.Enabled = false;
                _btnShowInheritance.Enabled = false;
                _btnShowDefaultProperties.Enabled = false;
                _btnShowDefaultInheritance.Enabled = false;
                _btnIcon.Enabled = false;
                _btnHostStatus.Enabled = false;

                _btnIcon.Image = null;

			    var gridObjectAsConnectionInfo = propertyGridObject as ConnectionInfo;
			    if (gridObjectAsConnectionInfo != null) //CONNECTION INFO
				{
                    var gridObjectAsContainerInfo = propertyGridObject as ContainerInfo;
				    if (gridObjectAsContainerInfo != null) //CONTAINER
                    {
                        var gridObjectAsRootNodeInfo = propertyGridObject as RootNodeInfo;
                        if (gridObjectAsRootNodeInfo != null) // ROOT
					    {
					        // ReSharper disable once SwitchStatementMissingSomeCases
                            switch (gridObjectAsRootNodeInfo.Type)
					        {
					            case RootNodeType.Connection:
					                PropertiesVisible = true;
					                DefaultPropertiesVisible = false;
					                _btnShowProperties.Enabled = true;
					                _btnShowInheritance.Enabled = false;
					                _btnShowDefaultProperties.Enabled = true;
					                _btnShowDefaultInheritance.Enabled = true;
					                _btnIcon.Enabled = false;
					                _btnHostStatus.Enabled = false;
					                break;
					            case RootNodeType.PuttySessions:
					                PropertiesVisible = true;
					                DefaultPropertiesVisible = false;
					                _btnShowProperties.Enabled = true;
					                _btnShowInheritance.Enabled = false;
					                _btnShowDefaultProperties.Enabled = false;
					                _btnShowDefaultInheritance.Enabled = false;
					                _btnIcon.Enabled = false;
					                _btnHostStatus.Enabled = false;
					                break;
					        }
					        
					        _pGrid.SelectedObject = propertyGridObject;
					    }
					    else
                        {
					        _pGrid.SelectedObject = propertyGridObject;

					        _btnShowProperties.Enabled = true;
					        _btnShowInheritance.Enabled = gridObjectAsContainerInfo.Parent != null;
					        _btnShowDefaultProperties.Enabled = false;
					        _btnShowDefaultInheritance.Enabled = false;
					        _btnIcon.Enabled = true;
					        _btnHostStatus.Enabled = false;

					        PropertiesVisible = true;
					    }
                    }
                    else //NO CONTAINER
				    {
                        if (PropertiesVisible) //Properties selected
                        {
                            _pGrid.SelectedObject = propertyGridObject;

                            _btnShowProperties.Enabled = true;
                            _btnShowInheritance.Enabled = gridObjectAsConnectionInfo.Parent != null;
                            _btnShowDefaultProperties.Enabled = false;
                            _btnShowDefaultInheritance.Enabled = false;
                            _btnIcon.Enabled = true;
                            _btnHostStatus.Enabled = true;
                        }
                        else if (DefaultPropertiesVisible) //Defaults selected
                        {
                            _pGrid.SelectedObject = propertyGridObject;

                            if (propertyGridObject is DefaultConnectionInfo) //Is the default connection
                            {
                                _btnShowProperties.Enabled = true;
                                _btnShowInheritance.Enabled = false;
                                _btnShowDefaultProperties.Enabled = true;
                                _btnShowDefaultInheritance.Enabled = true;
                                _btnIcon.Enabled = true;
                                _btnHostStatus.Enabled = false;
                            }
                            else //is not the default connection
                            {
                                _btnShowProperties.Enabled = true;
                                _btnShowInheritance.Enabled = true;
                                _btnShowDefaultProperties.Enabled = false;
                                _btnShowDefaultInheritance.Enabled = false;
                                _btnIcon.Enabled = true;
                                _btnHostStatus.Enabled = true;

                                PropertiesVisible = true;
                            }
                        }
                        else if (InheritanceVisible) //Inheritance selected
                        {
                            _pGrid.SelectedObject = gridObjectAsConnectionInfo.Inheritance;

                            _btnShowProperties.Enabled = true;
                            _btnShowInheritance.Enabled = true;
                            _btnShowDefaultProperties.Enabled = false;
                            _btnShowDefaultInheritance.Enabled = false;
                            _btnIcon.Enabled = true;
                            _btnHostStatus.Enabled = true;
                        }
                        else if (DefaultInheritanceVisible) //Default Inhertiance selected
                        {
                            _pGrid.SelectedObject = propertyGridObject;

                            _btnShowProperties.Enabled = true;
                            _btnShowInheritance.Enabled = true;
                            _btnShowDefaultProperties.Enabled = false;
                            _btnShowDefaultInheritance.Enabled = false;
                            _btnIcon.Enabled = true;
                            _btnHostStatus.Enabled = true;

                            PropertiesVisible = true;
                        }
                    }

                    var conIcon = ConnectionIcon.FromString(Convert.ToString(gridObjectAsConnectionInfo.Icon));
					if (conIcon != null)
					{
                        _btnIcon.Image = conIcon.ToBitmap();
					}
				}
				else if (propertyGridObject is ConnectionInfoInheritance) //INHERITANCE
				{
                    _pGrid.SelectedObject = propertyGridObject;
							
					if (InheritanceVisible)
					{
                        InheritanceVisible = true;
                        _btnShowProperties.Enabled = true;
                        _btnShowInheritance.Enabled = true;
                        _btnShowDefaultProperties.Enabled = false;
                        _btnShowDefaultInheritance.Enabled = false;
                        _btnIcon.Enabled = true;
                        _btnHostStatus.Enabled = !((ConnectionInfo)((ConnectionInfoInheritance)propertyGridObject).Parent).IsContainer;
                        InheritanceVisible = true;
                        var conIcon = ConnectionIcon.FromString(Convert.ToString(((ConnectionInfo)((ConnectionInfoInheritance)propertyGridObject).Parent).Icon));
						if (conIcon != null)
						{
                            _btnIcon.Image = conIcon.ToBitmap();
						}
					}
					else if (DefaultInheritanceVisible)
					{
                        _btnShowProperties.Enabled = true;
                        _btnShowInheritance.Enabled = false;
                        _btnShowDefaultProperties.Enabled = true;
                        _btnShowDefaultInheritance.Enabled = true;
                        _btnIcon.Enabled = false;
                        _btnHostStatus.Enabled = false;

                        DefaultInheritanceVisible = true;
					}
							
				}

                ShowHideGridItems();
                SetHostStatus(propertyGridObject);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridObjectFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Private Methods
		private void ApplyLanguage()
		{
			_btnShowInheritance.Text = Language.strButtonInheritance;
			_btnShowDefaultInheritance.Text = Language.strButtonDefaultInheritance;
			_btnShowProperties.Text = Language.strButtonProperties;
			_btnShowDefaultProperties.Text = Language.strButtonDefaultProperties;
			_btnIcon.Text = Language.strButtonIcon;
			_btnHostStatus.Text = Language.strStatus;
			Text = Language.strMenuConfig;
			TabText = Language.strMenuConfig;
			_propertyGridContextMenuShowHelpText.Text = Language.strMenuShowHelpText;
		}
		
		private void ApplyTheme()
		{
			_pGrid.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			_pGrid.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			_pGrid.ViewBackColor = ThemeManager.ActiveTheme.ConfigPanelBackgroundColor;
			_pGrid.ViewForeColor = ThemeManager.ActiveTheme.ConfigPanelTextColor;
			_pGrid.LineColor = ThemeManager.ActiveTheme.ConfigPanelGridLineColor;
			_pGrid.HelpBackColor = ThemeManager.ActiveTheme.ConfigPanelHelpBackgroundColor;
			_pGrid.HelpForeColor = ThemeManager.ActiveTheme.ConfigPanelHelpTextColor;
			_pGrid.CategoryForeColor = ThemeManager.ActiveTheme.ConfigPanelCategoryTextColor;
		}
		
		private void AddToolStripItems()
		{
			try
			{
				var customToolStrip = new ToolStrip();
				customToolStrip.Items.Add(_btnShowProperties);
				customToolStrip.Items.Add(_btnShowInheritance);
				customToolStrip.Items.Add(_btnShowDefaultProperties);
				customToolStrip.Items.Add(_btnShowDefaultInheritance);
				customToolStrip.Items.Add(_btnHostStatus);
				customToolStrip.Items.Add(_btnIcon);
				customToolStrip.Show();
						
				var propertyGridToolStrip = new ToolStrip();
						
				ToolStrip toolStrip = null;
				foreach (Control control in _pGrid.Controls)
				{
                    toolStrip = control as ToolStrip;
				    if (toolStrip == null) continue;
				    propertyGridToolStrip = toolStrip;
				    break;
				}
						
				if (toolStrip == null)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strCouldNotFindToolStripInFilteredPropertyGrid, true);
					return;
				}
						
				if (!_originalPropertyGridToolStripItemCountValid)
				{
					_originalPropertyGridToolStripItemCount = propertyGridToolStrip.Items.Count;
					_originalPropertyGridToolStripItemCountValid = true;
				}
				Debug.Assert(_originalPropertyGridToolStripItemCount == 5);
						
				// Hide the "Property Pages" button
				propertyGridToolStrip.Items[_originalPropertyGridToolStripItemCount - 1].Visible = false;
						
				var expectedToolStripItemCount = _originalPropertyGridToolStripItemCount + customToolStrip.Items.Count;
			    if (propertyGridToolStrip.Items.Count == expectedToolStripItemCount) return;
			    propertyGridToolStrip.AllowMerge = true;
			    ToolStripManager.Merge(customToolStrip, propertyGridToolStrip);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigUiLoadFailed + Environment.NewLine + ex.Message, true);
			}
		}
		
		private void Config_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
			ThemeManager.ThemeChanged += ApplyTheme;
			ApplyTheme();
			AddToolStripItems();
			_pGrid.HelpVisible = Settings.Default.ShowConfigHelpText;
		}
		
		private void Config_SystemColorsChanged(object sender, EventArgs e)
		{
			AddToolStripItems();
		}
		
		private void pGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			try
            {
                UpdateConnectionInfoNode(e);
                UpdateRootInfoNode(e);
                UpdateInheritanceNode();
                ShowHideGridItems();
                Runtime.SaveConnectionsAsync();
            }
            catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridValueFailed + Environment.NewLine + ex.Message, true);
			}
		}

        private void UpdateConnectionInfoNode(PropertyValueChangedEventArgs e)
        {
            Debug.WriteLine("update config");
            var selectedGridObject = _pGrid.SelectedObject as ConnectionInfo;
            if (selectedGridObject == null) return;
            if (e.ChangedItem.Label == Language.strPropertyNameProtocol)
            {
                selectedGridObject.SetDefaultPort();
            }
            else if (e.ChangedItem.Label == Language.strPropertyNameName)
            {
                if (Settings.Default.SetHostnameLikeDisplayName)
                {
                    var connectionInfo = selectedGridObject;
                    if (!string.IsNullOrEmpty(connectionInfo.Name))
                        connectionInfo.Hostname = connectionInfo.Name;
                }
            }
            else if (e.ChangedItem.Label == Language.strPropertyNameIcon)
            {
                var conIcon = ConnectionIcon.FromString(Convert.ToString(selectedGridObject.Icon));
                if (conIcon != null)
                    _btnIcon.Image = conIcon.ToBitmap();
            }
            else if (e.ChangedItem.Label == Language.strPropertyNameAddress)
            {
                SetHostStatus(selectedGridObject);
            }

            if (selectedGridObject is DefaultConnectionInfo)
                DefaultConnectionInfo.Instance.SaveTo(Settings.Default, a=>"ConDefault"+a);
        }

        private void UpdateRootInfoNode(PropertyValueChangedEventArgs e)
        {
            var rootInfo = _pGrid.SelectedObject as RootNodeInfo;
            if (rootInfo == null) return;
            if (e.ChangedItem.PropertyDescriptor == null) return;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (e.ChangedItem.PropertyDescriptor.Name)
            {
                case "Password":
                    if (rootInfo.Password)
                    {
                        var passwordName = Settings.Default.UseSQLServer ? Language.strSQLServer.TrimEnd(':') : Path.GetFileName(Runtime.GetStartupConnectionFileName());

                        var password = MiscTools.PasswordDialog(passwordName);
                        if (password.Length == 0)
                            rootInfo.Password = false;
                        else
                            rootInfo.PasswordString = password.ConvertToUnsecureString();
                    }
                    else
                    {
                        rootInfo.PasswordString = "";
                    }
                    break;
                case "Name":
                    break;
            }
        }

        private void UpdateInheritanceNode()
        {
            if (!(_pGrid.SelectedObject is DefaultConnectionInheritance)) return;
            DefaultConnectionInheritance.Instance.SaveTo(Settings.Default, a=>"InhDefault"+a);
        }

        private void pGrid_PropertySortChanged(object sender, EventArgs e)
		{
			if (_pGrid.PropertySort == PropertySort.CategorizedAlphabetical)
				_pGrid.PropertySort = PropertySort.Categorized;
		}
		
		private void ShowHideGridItems()
		{
			try
			{
                var strHide = new List<string>();
			    var o = _pGrid.SelectedObject as RootNodeInfo;
			    if (o != null)
                {
                    var rootInfo = o;
                    if (rootInfo.Type == RootNodeType.PuttySessions)
                    {
                        strHide.Add("Password");
                    }
                    strHide.Add("CacheBitmaps");
                    strHide.Add("Colors");
                    strHide.Add("DisplayThemes");
                    strHide.Add("DisplayWallpaper");
                    strHide.Add("EnableFontSmoothing");
                    strHide.Add("EnableDesktopComposition");
                    strHide.Add("Domain");
                    strHide.Add("ExtApp");
                    strHide.Add("ICAEncryptionStrength");
                    strHide.Add("RDGatewayDomain");
                    strHide.Add("RDGatewayHostname");
                    strHide.Add("RDGatewayPassword");
                    strHide.Add("RDGatewayUsageMethod");
                    strHide.Add("RDGatewayUseConnectionCredentials");
                    strHide.Add("RDGatewayUsername");
                    strHide.Add("RDPAuthenticationLevel");
                    strHide.Add("RDPMinutesToIdleTimeout");
                    strHide.Add("RDPAlertIdleTimeout");
                    strHide.Add("LoadBalanceInfo");
                    strHide.Add("RedirectDiskDrives");
                    strHide.Add("RedirectKeys");
                    strHide.Add("RedirectPorts");
                    strHide.Add("RedirectPrinters");
                    strHide.Add("RedirectSmartCards");
                    strHide.Add("RedirectSound");
                    strHide.Add("RenderingEngine");
                    strHide.Add("Resolution");
                    strHide.Add("AutomaticResize");
                    strHide.Add("UseConsoleSession");
                    strHide.Add("UseCredSsp");
                    strHide.Add("VNCAuthMode");
                    strHide.Add("VNCColors");
                    strHide.Add("VNCCompression");
                    strHide.Add("VNCEncoding");
                    strHide.Add("VNCProxyIP");
                    strHide.Add("VNCProxyPassword");
                    strHide.Add("VNCProxyPort");
                    strHide.Add("VNCProxyType");
                    strHide.Add("VNCProxyUsername");
                    strHide.Add("VNCSmartSizeMode");
                    strHide.Add("VNCViewOnly");
                    strHide.Add("Icon");
                    strHide.Add("Panel");
                    strHide.Add("Hostname");
                    strHide.Add("Username");
                    strHide.Add("Protocol");
                    strHide.Add("Port");
                    strHide.Add("PuttySession");
                    strHide.Add("PreExtApp");
                    strHide.Add("PostExtApp");
                    strHide.Add("MacAddress");
                    strHide.Add("UserField");
                    strHide.Add("Description");
                    strHide.Add("SoundQuality");
                    strHide.Add("CredentialRecord");
                }
                else if (_pGrid.SelectedObject is ConnectionInfo)
				{
                    var conI = (ConnectionInfo)_pGrid.SelectedObject;
				    // ReSharper disable once SwitchStatementMissingSomeCases
					switch (conI.Protocol)
					{
						case ProtocolType.RDP:
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("PuttySession");
							strHide.Add("RenderingEngine");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            if (conI.RDPMinutesToIdleTimeout <= 0)
                            {
                                strHide.Add("RDPAlertIdleTimeout");
                            }
							if (conI.RDGatewayUsageMethod == ProtocolRDP.RDGatewayUsageMethod.Never)
							{
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayHostname");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUseConnectionCredentials");
								strHide.Add("RDGatewayUsername");
							}
                            else if (conI.RDGatewayUseConnectionCredentials == ProtocolRDP.RDGatewayUseConnectionCredentials.Yes)
							{
								strHide.Add("RDGatewayDomain");
								strHide.Add("RDGatewayPassword");
								strHide.Add("RDGatewayUsername");
							}
							if (!(conI.Resolution == ProtocolRDP.RDPResolutions.FitToWindow || conI.Resolution == ProtocolRDP.RDPResolutions.Fullscreen))
							{
								strHide.Add("AutomaticResize");
							}
					        if (conI.RedirectSound != ProtocolRDP.RDPSounds.BringToThisComputer)
					        {
                                strHide.Add("SoundQuality");
                            }
							break;
						case ProtocolType.VNC:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("PuttySession");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							if (conI.VNCAuthMode == ProtocolVNC.AuthMode.AuthVNC)
							{
								strHide.Add("Username");
								strHide.Add("Domain");
							}
							if (conI.VNCProxyType == ProtocolVNC.ProxyType.ProxyNone)
							{
								strHide.Add("VNCProxyIP");
								strHide.Add("VNCProxyPassword");
								strHide.Add("VNCProxyPort");
								strHide.Add("VNCProxyUsername");
							}
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.SSH1:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.SSH2:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.Telnet:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("Password");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("Username");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.Rlogin:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("Password");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("Username");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.RAW:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("Password");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("Username");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.HTTP:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("PuttySession");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.HTTPS:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("PuttySession");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound;Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.ICA:
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("ExtApp");
							strHide.Add("Port");
							strHide.Add("PuttySession");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
						case ProtocolType.IntApp:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ICAEncryptionStrength");
							strHide.Add("PuttySession");
							strHide.Add("RDGatewayDomain");
							strHide.Add("RDGatewayHostname");
							strHide.Add("RDGatewayPassword");
							strHide.Add("RDGatewayUsageMethod");
							strHide.Add("RDGatewayUseConnectionCredentials");
							strHide.Add("RDGatewayUsername");
							strHide.Add("RDPAuthenticationLevel");
                            strHide.Add("RDPMinutesToIdleTimeout");
                            strHide.Add("RDPAlertIdleTimeout");
                            strHide.Add("LoadBalanceInfo");
							strHide.Add("RedirectDiskDrives");
							strHide.Add("RedirectKeys");
							strHide.Add("RedirectPorts");
							strHide.Add("RedirectPrinters");
							strHide.Add("RedirectSmartCards");
							strHide.Add("RedirectSound");
							strHide.Add("RenderingEngine");
							strHide.Add("Resolution");
							strHide.Add("AutomaticResize");
							strHide.Add("UseConsoleSession");
							strHide.Add("UseCredSsp");
							strHide.Add("VNCAuthMode");
							strHide.Add("VNCColors");
							strHide.Add("VNCCompression");
							strHide.Add("VNCEncoding");
							strHide.Add("VNCProxyIP");
							strHide.Add("VNCProxyPassword");
							strHide.Add("VNCProxyPort");
							strHide.Add("VNCProxyType");
							strHide.Add("VNCProxyUsername");
							strHide.Add("VNCSmartSizeMode");
							strHide.Add("VNCViewOnly");
                            strHide.Add("SoundQuality");
                            break;
					}
							
					if (!(conI is DefaultConnectionInfo))
					{
						if (conI.Inheritance.CacheBitmaps)
							strHide.Add("CacheBitmaps");
						if (conI.Inheritance.Colors)
							strHide.Add("Colors");
						if (conI.Inheritance.Description)
							strHide.Add("Description");
						if (conI.Inheritance.DisplayThemes)
							strHide.Add("DisplayThemes");
						if (conI.Inheritance.DisplayWallpaper)
							strHide.Add("DisplayWallpaper");
						if (conI.Inheritance.EnableFontSmoothing)
							strHide.Add("EnableFontSmoothing");
						if (conI.Inheritance.EnableDesktopComposition)
							strHide.Add("EnableDesktopComposition");
						if (conI.Inheritance.Domain)
							strHide.Add("Domain");
						if (conI.Inheritance.Icon)
							strHide.Add("Icon");
						if (conI.Inheritance.Password)
							strHide.Add("Password");
						if (conI.Inheritance.Port)
							strHide.Add("Port");
						if (conI.Inheritance.Protocol)
							strHide.Add("Protocol");
						if (conI.Inheritance.PuttySession)
							strHide.Add("PuttySession");
						if (conI.Inheritance.RedirectDiskDrives)
                            strHide.Add("RedirectDiskDrives");
                        if (conI.Inheritance.RedirectKeys)
                            strHide.Add("RedirectKeys");
                        if (conI.Inheritance.RedirectPorts)
                            strHide.Add("RedirectPorts");
                        if (conI.Inheritance.RedirectPrinters)
                            strHide.Add("RedirectPrinters");
                        if (conI.Inheritance.RedirectSmartCards)
                            strHide.Add("RedirectSmartCards");
                        if (conI.Inheritance.RedirectSound)
                            strHide.Add("RedirectSound");
                        if (conI.Inheritance.Resolution)
                            strHide.Add("Resolution");
                        if (conI.Inheritance.AutomaticResize)
                            strHide.Add("AutomaticResize");
                        if (conI.Inheritance.UseConsoleSession)
                            strHide.Add("UseConsoleSession");
                        if (conI.Inheritance.UseCredSsp)
                            strHide.Add("UseCredSsp");
                        if (conI.Inheritance.RenderingEngine)
                            strHide.Add("RenderingEngine");
                        if (conI.Inheritance.ICAEncryptionStrength)
                            strHide.Add("ICAEncryptionStrength");
                        if (conI.Inheritance.RDPAuthenticationLevel)
                            strHide.Add("RDPAuthenticationLevel");
                        if (conI.Inheritance.RDPMinutesToIdleTimeout)
                            strHide.Add("RDPMinutesToIdleTimeout");
                        if (conI.Inheritance.RDPAlertIdleTimeout)
                            strHide.Add("RDPAlertIdleTimeout");
                        if (conI.Inheritance.LoadBalanceInfo)
                            strHide.Add("LoadBalanceInfo");
                        if (conI.Inheritance.Username)
                            strHide.Add("Username");
                        if (conI.Inheritance.Panel)
                            strHide.Add("Panel");
                        if (conI.IsContainer)
                            strHide.Add("Hostname");
                        if (conI.Inheritance.PreExtApp)
                            strHide.Add("PreExtApp");
                        if (conI.Inheritance.PostExtApp)
                            strHide.Add("PostExtApp");
                        if (conI.Inheritance.MacAddress)
                            strHide.Add("MacAddress");
                        if (conI.Inheritance.UserField)
                            strHide.Add("UserField");
                        if (conI.Inheritance.VNCAuthMode)
                            strHide.Add("VNCAuthMode");
                        if (conI.Inheritance.VNCColors)
                            strHide.Add("VNCColors");
                        if (conI.Inheritance.VNCCompression)
                            strHide.Add("VNCCompression");
                        if (conI.Inheritance.VNCEncoding)
                            strHide.Add("VNCEncoding");
                        if (conI.Inheritance.VNCProxyIP)
                            strHide.Add("VNCProxyIP");
                        if (conI.Inheritance.VNCProxyPassword)
                            strHide.Add("VNCProxyPassword");
                        if (conI.Inheritance.VNCProxyPort)
                            strHide.Add("VNCProxyPort");
                        if (conI.Inheritance.VNCProxyType)
                            strHide.Add("VNCProxyType");
                        if (conI.Inheritance.VNCProxyUsername)
                            strHide.Add("VNCProxyUsername");
                        if (conI.Inheritance.VNCViewOnly)
                            strHide.Add("VNCViewOnly");
                        if (conI.Inheritance.VNCSmartSizeMode)
                            strHide.Add("VNCSmartSizeMode");
                        if (conI.Inheritance.ExtApp)
                            strHide.Add("ExtApp");
                        if (conI.Inheritance.RDGatewayUsageMethod)
                            strHide.Add("RDGatewayUsageMethod");
                        if (conI.Inheritance.RDGatewayHostname)
                            strHide.Add("RDGatewayHostname");
                        if (conI.Inheritance.RDGatewayUsername)
                            strHide.Add("RDGatewayUsername");
                        if (conI.Inheritance.RDGatewayPassword)
                            strHide.Add("RDGatewayPassword");
                        if (conI.Inheritance.RDGatewayDomain)
                            strHide.Add("RDGatewayDomain");
                        if (conI.Inheritance.RDGatewayUseConnectionCredentials)
                            strHide.Add("RDGatewayUseConnectionCredentials");
                        if (conI.Inheritance.RDGatewayHostname)
                            strHide.Add("RDGatewayHostname");
                        if(conI.Inheritance.SoundQuality)
                            strHide.Add("SoundQuality");
                        if(conI.Inheritance.CredentialRecord)
                            strHide.Add("CredentialRecord");
                    }
					else
					{
						strHide.Add("Hostname");
						strHide.Add("Name");
					}
				}

                _pGrid.HiddenProperties = strHide.ToArray();
                _pGrid.Refresh();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridHideItemsFailed + Environment.NewLine + ex.Message, true);
			}
		}
		
		private void btnShowProperties_Click(object sender, EventArgs e)
		{
		    var o = _pGrid.SelectedObject as ConnectionInfoInheritance;
		    if (o != null)
			{
				if (_pGrid.SelectedObject is DefaultConnectionInheritance)
				{
                    PropertiesVisible = true;
                    InheritanceVisible = false;
                    DefaultPropertiesVisible = false;
                    DefaultInheritanceVisible = false;
                    SetPropertyGridObject((RootNodeInfo)_selectedTreeNode);
				}
				else
				{
                    PropertiesVisible = true;
                    InheritanceVisible = false;
                    DefaultPropertiesVisible = false;
                    DefaultInheritanceVisible = false;
                    SetPropertyGridObject(o.Parent);
				}
			}
			else if (_pGrid.SelectedObject is ConnectionInfo)
			{
			    if (!((ConnectionInfo) _pGrid.SelectedObject).IsDefault) return;
			    PropertiesVisible = true;
			    InheritanceVisible = false;
			    DefaultPropertiesVisible = false;
			    DefaultInheritanceVisible = false;
			    SetPropertyGridObject((RootNodeInfo)_selectedTreeNode);
			}
		}
		
		private void btnShowDefaultProperties_Click(object sender, EventArgs e)
		{
		    if (!(_pGrid.SelectedObject is RootNodeInfo) && !(_pGrid.SelectedObject is ConnectionInfoInheritance)) return;
		    PropertiesVisible = false;
		    InheritanceVisible = false;
		    DefaultPropertiesVisible = true;
		    DefaultInheritanceVisible = false;
		    SetPropertyGridObject(DefaultConnectionInfo.Instance);
		}
		
		private void btnShowInheritance_Click(object sender, EventArgs e)
		{
		    if (!(_pGrid.SelectedObject is ConnectionInfo)) return;
		    PropertiesVisible = false;
		    InheritanceVisible = true;
		    DefaultPropertiesVisible = false;
		    DefaultInheritanceVisible = false;
		    SetPropertyGridObject(((ConnectionInfo)_pGrid.SelectedObject).Inheritance);
		}
		
		private void btnShowDefaultInheritance_Click(object sender, EventArgs e)
		{
		    if (!(_pGrid.SelectedObject is RootNodeInfo) && !(_pGrid.SelectedObject is ConnectionInfo)) return;
		    PropertiesVisible = false;
		    InheritanceVisible = false;
		    DefaultPropertiesVisible = false;
		    DefaultInheritanceVisible = true;
		    SetPropertyGridObject(DefaultConnectionInheritance.Instance);
		}
		
		private void btnHostStatus_Click(object sender, EventArgs e)
		{
			SetHostStatus(_pGrid.SelectedObject);
		}
		
		private void btnIcon_Click(object sender, MouseEventArgs e)
		{
			try
			{
				if (_pGrid.SelectedObject is ConnectionInfo && !(_pGrid.SelectedObject is PuttySessionInfo))
				{
                    CMenIcons.Items.Clear();
							
					foreach (var iStr in ConnectionIcon.Icons)
					{
					    var tI = new ToolStripMenuItem
					    {
					        Text = iStr,
					        Image = ConnectionIcon.FromString(iStr).ToBitmap()
					    };
					    tI.Click += IconMenu_Click;

                        CMenIcons.Items.Add(tI);
					}
					var mPos = new Point(new Size(PointToScreen(new Point(e.Location.X + _pGrid.Width - 100, e.Location.Y))));
                    CMenIcons.Show(mPos);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridButtonIconClickFailed + Environment.NewLine + ex.Message, true);
			}
		}
		
		private void IconMenu_Click(object sender, EventArgs e)
		{
			try
			{
				var connectionInfo = (ConnectionInfo)_pGrid.SelectedObject;
				if (connectionInfo == null) return;
						
				var selectedMenuItem = (ToolStripMenuItem)sender;

			    var iconName = selectedMenuItem?.Text;
				if (string.IsNullOrEmpty(iconName)) return;
						
				var connectionIcon = ConnectionIcon.FromString(iconName);
				if (connectionIcon == null) return;
						
				_btnIcon.Image = connectionIcon.ToBitmap();
						
				connectionInfo.Icon = iconName;
				_pGrid.Refresh();
						
				Runtime.SaveConnectionsAsync();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridMenuClickFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Host Status (Ping)
		private string _hostName;
		private Thread _pThread;
		
		private void CheckHostAlive()
		{
			var pingSender = new Ping();

		    try
			{
			    var pReply = pingSender.Send(_hostName);
			    if (pReply?.Status == IPStatus.Success)
				{
					if ((string)_btnHostStatus.Tag == "checking")
					{
						ShowStatusImage(Resources.HostStatus_On);
					}
				}
				else
				{
					if ((string)_btnHostStatus.Tag == "checking")
					{
						ShowStatusImage(Resources.HostStatus_Off);
					}
				}
			}
			catch (Exception)
			{
				if ((string)_btnHostStatus.Tag == "checking")
				{
					ShowStatusImage(Resources.HostStatus_Off);
				}
			}
		}

        private delegate void ShowStatusImageCb(Image image);
		private void ShowStatusImage(Image image)
		{
			if (_pGrid.InvokeRequired)
			{
				ShowStatusImageCb d = ShowStatusImage;
                _pGrid.Invoke(d, image);
			}
			else
			{
                _btnHostStatus.Image = image;
                _btnHostStatus.Tag = "checkfinished";
			}
		}

	    private void SetHostStatus(object connectionInfo)
		{
			try
			{
                _btnHostStatus.Image = Resources.HostStatus_Check;
				// To check status, ConnectionInfo must be an mRemoteNG.Connection.Info that is not a container
			    var info = connectionInfo as ConnectionInfo;
                if (info == null) return;

                if (info.IsContainer) return;

                _btnHostStatus.Tag = "checking";
                _hostName = ((ConnectionInfo)connectionInfo).Hostname;
				_pThread = new Thread(CheckHostAlive);
				_pThread.SetApartmentState(ApartmentState.STA);
				_pThread.IsBackground = true;
				_pThread.Start();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridSetHostStatusFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion

        #region Event Handlers
        private void propertyGridContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				_propertyGridContextMenuShowHelpText.Checked = Settings.Default.ShowConfigHelpText;
				var gridItem = _pGrid.SelectedGridItem;
				_propertyGridContextMenuReset.Enabled = Convert.ToBoolean(_pGrid.SelectedObject != null && gridItem?.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(_pGrid.SelectedObject));
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.Config.propertyGridContextMenu_Opening() failed.", ex);
			}
		}
		
		private void propertyGridContextMenuReset_Click(object sender, EventArgs e)
		{
			try
			{
				var gridItem = _pGrid.SelectedGridItem;
				if (_pGrid.SelectedObject != null && gridItem?.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(_pGrid.SelectedObject))
				{
					_pGrid.ResetSelectedProperty();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.Config.propertyGridContextMenuReset_Click() failed.", ex);
			}
		}
		
		private void propertyGridContextMenuShowHelpText_Click(object sender, EventArgs e)
		{
			_propertyGridContextMenuShowHelpText.Checked = !_propertyGridContextMenuShowHelpText.Checked;
		}
		
		private void propertyGridContextMenuShowHelpText_CheckedChanged(object sender, EventArgs e)
		{
            Settings.Default.ShowConfigHelpText = _propertyGridContextMenuShowHelpText.Checked;
			_pGrid.HelpVisible = _propertyGridContextMenuShowHelpText.Checked;
        }
        #endregion
    }
}