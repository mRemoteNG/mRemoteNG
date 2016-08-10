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
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Security;
using mRemoteNG.UI.Controls.FilteredPropertyGrid;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
    public class ConfigWindow : BaseWindow
	{
        private bool _originalPropertyGridToolStripItemCountValid;
        private int _originalPropertyGridToolStripItemCount;
        private System.ComponentModel.Container components;
        private ToolStripButton btnShowProperties;
        private ToolStripButton btnShowDefaultProperties;
        private ToolStripButton btnShowInheritance;
        private ToolStripButton btnShowDefaultInheritance;
        private ToolStripButton btnIcon;
        private ToolStripButton btnHostStatus;
        internal ContextMenuStrip cMenIcons;
        internal ContextMenuStrip propertyGridContextMenu;
        private ToolStripMenuItem propertyGridContextMenuShowHelpText;
        private ToolStripMenuItem propertyGridContextMenuReset;
        private ToolStripSeparator ToolStripSeparator1;
        private FilteredPropertyGrid pGrid;


        private void InitializeComponent()
		{
            components = new System.ComponentModel.Container();
            Load += Config_Load;
            SystemColorsChanged += Config_SystemColorsChanged;
            pGrid = new FilteredPropertyGrid();
            pGrid.PropertyValueChanged += pGrid_PropertyValueChanged;
            pGrid.PropertySortChanged += pGrid_PropertySortChanged;
            propertyGridContextMenu = new ContextMenuStrip(components);
            propertyGridContextMenu.Opening += propertyGridContextMenu_Opening;
            propertyGridContextMenuReset = new ToolStripMenuItem();
            propertyGridContextMenuReset.Click += propertyGridContextMenuReset_Click;
            ToolStripSeparator1 = new ToolStripSeparator();
            propertyGridContextMenuShowHelpText = new ToolStripMenuItem();
            propertyGridContextMenuShowHelpText.Click += propertyGridContextMenuShowHelpText_Click;
            propertyGridContextMenuShowHelpText.CheckedChanged += propertyGridContextMenuShowHelpText_CheckedChanged;
            btnShowInheritance = new ToolStripButton();
            btnShowInheritance.Click += btnShowInheritance_Click;
            btnShowDefaultInheritance = new ToolStripButton();
            btnShowDefaultInheritance.Click += btnShowDefaultInheritance_Click;
            btnShowProperties = new ToolStripButton();
            btnShowProperties.Click += btnShowProperties_Click;
            btnShowDefaultProperties = new ToolStripButton();
            btnShowDefaultProperties.Click += btnShowDefaultProperties_Click;
            btnIcon = new ToolStripButton();
            btnIcon.MouseUp += btnIcon_Click;
            btnHostStatus = new ToolStripButton();
            btnHostStatus.Click += btnHostStatus_Click;
            cMenIcons = new ContextMenuStrip(components);
            propertyGridContextMenu.SuspendLayout();
            SuspendLayout();
            //
            //pGrid
            //
            pGrid.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)
                | AnchorStyles.Right;
            pGrid.BrowsableProperties = null;
            pGrid.ContextMenuStrip = propertyGridContextMenu;
            pGrid.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            pGrid.HiddenAttributes = null;
            pGrid.HiddenProperties = null;
            pGrid.Location = new Point(0, 0);
            pGrid.Name = "pGrid";
            pGrid.PropertySort = PropertySort.Categorized;
            pGrid.Size = new Size(226, 530);
            pGrid.TabIndex = 0;
            pGrid.UseCompatibleTextRendering = true;
            //
            //propertyGridContextMenu
            //
            propertyGridContextMenu.Items.AddRange(new ToolStripItem[] { propertyGridContextMenuReset, ToolStripSeparator1, propertyGridContextMenuShowHelpText });
            propertyGridContextMenu.Name = "propertyGridContextMenu";
            propertyGridContextMenu.Size = new Size(157, 76);
            //
            //propertyGridContextMenuReset
            //
            propertyGridContextMenuReset.Name = "propertyGridContextMenuReset";
            propertyGridContextMenuReset.Size = new Size(156, 22);
            propertyGridContextMenuReset.Text = @"&Reset";
            //
            //ToolStripSeparator1
            //
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new Size(153, 6);
            //
            //propertyGridContextMenuShowHelpText
            //
            propertyGridContextMenuShowHelpText.Name = "propertyGridContextMenuShowHelpText";
            propertyGridContextMenuShowHelpText.Size = new Size(156, 22);
            propertyGridContextMenuShowHelpText.Text = @"&Show Help Text";
            //
            //btnShowInheritance
            //
            btnShowInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowInheritance.Image = Resources.Inheritance;
            btnShowInheritance.ImageTransparentColor = Color.Magenta;
            btnShowInheritance.Name = "btnShowInheritance";
            btnShowInheritance.Size = new Size(23, 22);
            btnShowInheritance.Text = @"IInheritable";
            //
            //btnShowDefaultInheritance
            //
            btnShowDefaultInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowDefaultInheritance.Image = Resources.Inheritance_Default;
            btnShowDefaultInheritance.ImageTransparentColor = Color.Magenta;
            btnShowDefaultInheritance.Name = "btnShowDefaultInheritance";
            btnShowDefaultInheritance.Size = new Size(23, 22);
            btnShowDefaultInheritance.Text = @"Default IInheritable";
            //
            //btnShowProperties
            //
            btnShowProperties.Checked = true;
            btnShowProperties.CheckState = CheckState.Checked;
            btnShowProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowProperties.Image = Resources.Properties;
            btnShowProperties.ImageTransparentColor = Color.Magenta;
            btnShowProperties.Name = "btnShowProperties";
            btnShowProperties.Size = new Size(23, 22);
            btnShowProperties.Text = @"Properties";
            //
            //btnShowDefaultProperties
            //
            btnShowDefaultProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowDefaultProperties.Image = Resources.Properties_Default;
            btnShowDefaultProperties.ImageTransparentColor = Color.Magenta;
            btnShowDefaultProperties.Name = "btnShowDefaultProperties";
            btnShowDefaultProperties.Size = new Size(23, 22);
            btnShowDefaultProperties.Text = @"Default Properties";
            //
            //btnIcon
            //
            btnIcon.Alignment = ToolStripItemAlignment.Right;
            btnIcon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnIcon.ImageTransparentColor = Color.Magenta;
            btnIcon.Name = "btnIcon";
            btnIcon.Size = new Size(23, 22);
            btnIcon.Text = @"Icon";
            //
            //btnHostStatus
            //
            btnHostStatus.Alignment = ToolStripItemAlignment.Right;
            btnHostStatus.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnHostStatus.Image = Resources.HostStatus_Check;
            btnHostStatus.ImageTransparentColor = Color.Magenta;
            btnHostStatus.Name = "btnHostStatus";
            btnHostStatus.Size = new Size(23, 22);
            btnHostStatus.Tag = "checking";
            btnHostStatus.Text = @"Status";
            //
            //cMenIcons
            //
            cMenIcons.Name = "cMenIcons";
            cMenIcons.Size = new Size(61, 4);
            //
            //Config
            //
            ClientSize = new Size(226, 530);
            Controls.Add(pGrid);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            HideOnClose = true;
            Icon = Resources.Config_Icon;
            Name = "Config";
            TabText = @"Config";
            Text = @"Config";
            propertyGridContextMenu.ResumeLayout(false);
            ResumeLayout(false);
					
		}
		
        #region Public Properties
        public bool PropertiesVisible
		{
			get
			{
			    return btnShowProperties.Checked;
			}
			set
			{
                btnShowProperties.Checked = value;
				if (value)
				{
                    btnShowInheritance.Checked = false;
                    btnShowDefaultInheritance.Checked = false;
                    btnShowDefaultProperties.Checked = false;
				}
			}
		}
				
        public bool InheritanceVisible
		{
			get
			{
			    return btnShowInheritance.Checked;
			}
			set
			{
                btnShowInheritance.Checked = value;
				if (value)
				{
                    btnShowProperties.Checked = false;
                    btnShowDefaultInheritance.Checked = false;
                    btnShowDefaultProperties.Checked = false;
				}
			}
		}
				
        public bool DefaultPropertiesVisible
		{
			get
			{
			    return btnShowDefaultProperties.Checked;
			}
			set
			{
                btnShowDefaultProperties.Checked = value;
				if (value)
				{
                    btnShowProperties.Checked = false;
                    btnShowDefaultInheritance.Checked = false;
                    btnShowInheritance.Checked = false;
				}
			}
		}
				
        public bool DefaultInheritanceVisible
		{
			get { return btnShowDefaultInheritance.Checked; }
			set
			{
                btnShowDefaultInheritance.Checked = value;
				if (value)
				{
                    btnShowProperties.Checked = false;
                    btnShowDefaultProperties.Checked = false;
                    btnShowInheritance.Checked = false;
				}
			}
		}
        #endregion

        #region Constructors
        public ConfigWindow(DockContent Panel)
        {
            WindowType = WindowType.Config;
            DockPnl = Panel;
            InitializeComponent();
        }
        #endregion

        #region Public Methods
		// Main form handle command key events
		// Adapted from http://kiwigis.blogspot.com/2009/05/adding-tab-key-support-to-propertygrid.html
		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
		{
			if ((keyData & Keys.KeyCode) == Keys.Tab)
			{
				var selectedItem = pGrid.SelectedGridItem;
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
						
				if (keyData == (Keys.Tab | Keys.Shift))
					newItem = FindPreviousGridItemProperty(gridItems, selectedItem);
				else if (keyData == Keys.Tab)
					newItem = FindNextGridItemProperty(gridItems, selectedItem);
						
				pGrid.SelectedGridItem = newItem;
						
				return true; // Handled
			}
			else
			{
				return base.ProcessCmdKey(ref msg, keyData);
			}
		}
				
		private void FindChildGridItems(GridItem item, ref List<GridItem> gridItems)
		{
			gridItems.Add(item);
					
			if (!item.Expandable || item.Expanded)
			{
				foreach (GridItem child in item.GridItems)
				{
					FindChildGridItems(child, ref gridItems);
				}
			}
		}
				
		private bool ContainsGridItemProperty(List<GridItem> gridItems)
		{
			foreach (var item in gridItems)
			{
				if (item.GridItemType == GridItemType.Property)
				{
					return true;
				}
			}
			return false;
		}
				
		private GridItem FindPreviousGridItemProperty(List<GridItem> gridItems, GridItem startItem)
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
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					previousIndex = index;
					previousIndexValid = true;
					break;
				}
			}
			
			if (previousIndexValid)
				return gridItems[previousIndex];
			
			for (var index = gridItems.Count - 1; index >= startIndex + 1; index--)
			{
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					previousIndex = index;
					previousIndexValid = true;
					break;
				}
			}
			
			if (!previousIndexValid)
				return null;
			return gridItems[previousIndex];
		}
				
		private GridItem FindNextGridItemProperty(List<GridItem> gridItems, GridItem startItem)
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
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					nextIndex = index;
					nextIndexValid = true;
					break;
				}
			}
			
			if (nextIndexValid)
				return gridItems[nextIndex];
					
			for (var index = 0; index <= startIndex - 1; index++)
			{
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					nextIndex = index;
					nextIndexValid = true;
					break;
				}
			}
			
			if (!nextIndexValid)
				return null;
			return gridItems[nextIndex];
		}
				
		public void SetPropertyGridObject(object Obj)
		{
			try
			{
                btnShowProperties.Enabled = false;
                btnShowInheritance.Enabled = false;
                btnShowDefaultProperties.Enabled = false;
                btnShowDefaultInheritance.Enabled = false;
                btnIcon.Enabled = false;
                btnHostStatus.Enabled = false;

                btnIcon.Image = null;
						
				if (Obj is ConnectionInfo) //CONNECTION INFO
				{
                    if (((ConnectionInfo)Obj).IsContainer == false) //NO CONTAINER
					{
						if (PropertiesVisible) //Properties selected
						{
                            pGrid.SelectedObject = Obj;

                            btnShowProperties.Enabled = true;
                            if (((ConnectionInfo)Obj).Parent != null)
							{
                                btnShowInheritance.Enabled = true;
							}
							else
							{
                                btnShowInheritance.Enabled = false;
							}
                            btnShowDefaultProperties.Enabled = false;
                            btnShowDefaultInheritance.Enabled = false;
							btnIcon.Enabled = true;
                            btnHostStatus.Enabled = true;
						}
						else if (DefaultPropertiesVisible) //Defaults selected
						{
                            pGrid.SelectedObject = Obj;

                            if (((ConnectionInfo)Obj).IsDefault) //Is the default connection
							{
                                btnShowProperties.Enabled = true;
                                btnShowInheritance.Enabled = false;
                                btnShowDefaultProperties.Enabled = true;
                                btnShowDefaultInheritance.Enabled = true;
                                btnIcon.Enabled = true;
                                btnHostStatus.Enabled = false;
							}
							else //is not the default connection
							{
                                btnShowProperties.Enabled = true;
                                btnShowInheritance.Enabled = true;
                                btnShowDefaultProperties.Enabled = false;
                                btnShowDefaultInheritance.Enabled = false;
                                btnIcon.Enabled = true;
                                btnHostStatus.Enabled = true;

                                PropertiesVisible = true;
							}
						}
						else if (InheritanceVisible) //IInheritable selected
						{
                            pGrid.SelectedObject = ((ConnectionInfo)Obj).Inheritance;

                            btnShowProperties.Enabled = true;
                            btnShowInheritance.Enabled = true;
                            btnShowDefaultProperties.Enabled = false;
                            btnShowDefaultInheritance.Enabled = false;
                            btnIcon.Enabled = true;
                            btnHostStatus.Enabled = true;
						}
						else if (DefaultInheritanceVisible) //Default Inhertiance selected
						{
							pGrid.SelectedObject = Obj;

                            btnShowProperties.Enabled = true;
                            btnShowInheritance.Enabled = true;
                            btnShowDefaultProperties.Enabled = false;
                            btnShowDefaultInheritance.Enabled = false;
                            btnIcon.Enabled = true;
                            btnHostStatus.Enabled = true;

                            PropertiesVisible = true;
						}
					}
                    else if (((ConnectionInfo)Obj).IsContainer) //CONTAINER
					{
                        pGrid.SelectedObject = Obj;

                        btnShowProperties.Enabled = true;
                        if (((ConnectionInfo)Obj).Parent.Parent != null)
						{
                            btnShowInheritance.Enabled = true;
						}
						else
						{
                            btnShowInheritance.Enabled = false;
						}
                        btnShowDefaultProperties.Enabled = false;
                        btnShowDefaultInheritance.Enabled = false;
                        btnIcon.Enabled = true;
                        btnHostStatus.Enabled = false;

                        PropertiesVisible = true;
					}

                    var conIcon = ConnectionIcon.FromString(Convert.ToString(((ConnectionInfo)Obj).Icon));
					if (conIcon != null)
					{
                        btnIcon.Image = conIcon.ToBitmap();
					}
				}
				else if (Obj is RootNodeInfo) //ROOT
				{
					var rootInfo = (RootNodeInfo) Obj;
					switch (rootInfo.Type)
					{
						case RootNodeType.Connection:
							PropertiesVisible = true;
							DefaultPropertiesVisible = false;
							btnShowProperties.Enabled = true;
							btnShowInheritance.Enabled = false;
							btnShowDefaultProperties.Enabled = true;
							btnShowDefaultInheritance.Enabled = true;
							btnIcon.Enabled = false;
							btnHostStatus.Enabled = false;
							break;
						case RootNodeType.Credential:
							throw (new NotImplementedException());
						case RootNodeType.PuttySessions:
							PropertiesVisible = true;
							DefaultPropertiesVisible = false;
							btnShowProperties.Enabled = true;
							btnShowInheritance.Enabled = false;
							btnShowDefaultProperties.Enabled = false;
							btnShowDefaultInheritance.Enabled = false;
							btnIcon.Enabled = false;
							btnHostStatus.Enabled = false;
							break;
					}
					pGrid.SelectedObject = Obj;
				}
				else if (Obj is ConnectionInfoInheritance) //INHERITANCE
				{
                    pGrid.SelectedObject = Obj;
							
					if (InheritanceVisible)
					{
                        InheritanceVisible = true;
                        btnShowProperties.Enabled = true;
                        btnShowInheritance.Enabled = true;
                        btnShowDefaultProperties.Enabled = false;
                        btnShowDefaultInheritance.Enabled = false;
                        btnIcon.Enabled = true;
                        btnHostStatus.Enabled = !((ConnectionInfo)((ConnectionInfoInheritance)Obj).Parent).IsContainer;
                        InheritanceVisible = true;
                        var conIcon = ConnectionIcon.FromString(Convert.ToString(((ConnectionInfo)((ConnectionInfoInheritance)Obj).Parent).Icon));
						if (conIcon != null)
						{
                            btnIcon.Image = conIcon.ToBitmap();
						}
					}
					else if (DefaultInheritanceVisible)
					{
                        btnShowProperties.Enabled = true;
                        btnShowInheritance.Enabled = false;
                        btnShowDefaultProperties.Enabled = true;
                        btnShowDefaultInheritance.Enabled = true;
                        btnIcon.Enabled = false;
                        btnHostStatus.Enabled = false;

                        DefaultInheritanceVisible = true;
					}
							
				}

                ShowHideGridItems();
                SetHostStatus(Obj);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridObjectFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void pGrid_SelectedObjectChanged()
		{
            ShowHideGridItems();
		}
        #endregion
		
        #region Private Methods
		private void ApplyLanguage()
		{
			btnShowInheritance.Text = Language.strButtonInheritance;
			btnShowDefaultInheritance.Text = Language.strButtonDefaultInheritance;
			btnShowProperties.Text = Language.strButtonProperties;
			btnShowDefaultProperties.Text = Language.strButtonDefaultProperties;
			btnIcon.Text = Language.strButtonIcon;
			btnHostStatus.Text = Language.strStatus;
			Text = Language.strMenuConfig;
			TabText = Language.strMenuConfig;
			propertyGridContextMenuShowHelpText.Text = Language.strMenuShowHelpText;
		}
				
		private void ApplyTheme()
		{
			pGrid.BackColor = Themes.ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			pGrid.ForeColor = Themes.ThemeManager.ActiveTheme.ToolbarTextColor;
			pGrid.ViewBackColor = Themes.ThemeManager.ActiveTheme.ConfigPanelBackgroundColor;
			pGrid.ViewForeColor = Themes.ThemeManager.ActiveTheme.ConfigPanelTextColor;
			pGrid.LineColor = Themes.ThemeManager.ActiveTheme.ConfigPanelGridLineColor;
			pGrid.HelpBackColor = Themes.ThemeManager.ActiveTheme.ConfigPanelHelpBackgroundColor;
			pGrid.HelpForeColor = Themes.ThemeManager.ActiveTheme.ConfigPanelHelpTextColor;
			pGrid.CategoryForeColor = Themes.ThemeManager.ActiveTheme.ConfigPanelCategoryTextColor;
		}
		
		private void AddToolStripItems()
		{
			try
			{
				var customToolStrip = new ToolStrip();
				customToolStrip.Items.Add(btnShowProperties);
				customToolStrip.Items.Add(btnShowInheritance);
				customToolStrip.Items.Add(btnShowDefaultProperties);
				customToolStrip.Items.Add(btnShowDefaultInheritance);
				customToolStrip.Items.Add(btnHostStatus);
				customToolStrip.Items.Add(btnIcon);
				customToolStrip.Show();
						
				var propertyGridToolStrip = new ToolStrip();
						
				ToolStrip toolStrip = null;
				foreach (Control control in pGrid.Controls)
				{
                    toolStrip = control as ToolStrip;
                    if (toolStrip != null)
                    {
                        propertyGridToolStrip = toolStrip;
                        break;
                    }
				}
						
				if (toolStrip == null)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strCouldNotFindToolStripInFilteredPropertyGrid, true);
					return ;
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
				if (propertyGridToolStrip.Items.Count != expectedToolStripItemCount)
				{
					propertyGridToolStrip.AllowMerge = true;
					ToolStripManager.Merge(customToolStrip, propertyGridToolStrip);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigUiLoadFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void Config_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
			Themes.ThemeManager.ThemeChanged += ApplyTheme;
			ApplyTheme();
			AddToolStripItems();
			pGrid.HelpVisible = Settings.Default.ShowConfigHelpText;
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
                Runtime.SaveConnectionsBG();
            }
            catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridValueFailed + Environment.NewLine + ex.Message, true);
			}
		}

        private void UpdateConnectionInfoNode(PropertyValueChangedEventArgs e)
        {
            var o = pGrid.SelectedObject as ConnectionInfo;
            if (o != null)
            {
                if (e.ChangedItem.Label == Language.strPropertyNameProtocol)
                {
                    o.SetDefaultPort();
                }
                else if (e.ChangedItem.Label == Language.strPropertyNameName)
                {
                    Windows.treeForm.tvConnections.SelectedNode.Text = Convert.ToString(o.Name);
                    if (Settings.Default.SetHostnameLikeDisplayName)
                    {
                        var connectionInfo = o;
                        if (!string.IsNullOrEmpty(connectionInfo.Name))
                            connectionInfo.Hostname = connectionInfo.Name;
                    }
                }
                else if (e.ChangedItem.Label == Language.strPropertyNameIcon)
                {
                    var conIcon = ConnectionIcon.FromString(Convert.ToString(o.Icon));
                    if (conIcon != null)
                        btnIcon.Image = conIcon.ToBitmap();
                }
                else if (e.ChangedItem.Label == Language.strPropertyNameAddress)
                {
                    SetHostStatus(o);
                }

                if (o.IsDefault)
                    Runtime.DefaultConnectionToSettings();
            }
        }

        private void UpdateRootInfoNode(PropertyValueChangedEventArgs e)
        {
            var o = pGrid.SelectedObject as RootNodeInfo;
            if (o != null)
            {
                var rootInfo = o;
                if (e.ChangedItem.PropertyDescriptor != null)
                    switch (e.ChangedItem.PropertyDescriptor.Name)
                    {
                        case "Password":
                            if (rootInfo.Password)
                            {
                                string passwordName;
                                if (Settings.Default.UseSQLServer)
                                    passwordName = Language.strSQLServer.TrimEnd(':');
                                else
                                    passwordName = Path.GetFileName(Runtime.GetStartupConnectionFileName());

                                var password = MiscTools.PasswordDialog(passwordName);
                                if (password.Length == 0)
                                    rootInfo.Password = false;
                                else
                                    rootInfo.PasswordString = password.ConvertToUnsecureString();
                            }
                            break;
                        case "Name":
                            break;
                    }
            }
        }

        private void UpdateInheritanceNode()
        {
            if (!(pGrid.SelectedObject is DefaultConnectionInheritance)) return;
            DefaultConnectionInheritance.Instance.SaveTo<Settings>(Settings.Default, (a)=>"InhDefault"+a);
        }

        private void pGrid_PropertySortChanged(object sender, EventArgs e)
		{
			if (pGrid.PropertySort == PropertySort.CategorizedAlphabetical)
				pGrid.PropertySort = PropertySort.Categorized;
		}
				
		private void ShowHideGridItems()
		{
			try
			{
                var strHide = new List<string>();
						
				if (pGrid.SelectedObject is ConnectionInfo)
				{
                    var conI = (ConnectionInfo)pGrid.SelectedObject;
							
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
							break;
					}
							
					if (conI.IsDefault == false)
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
                    }
					else
					{
						strHide.Add("Hostname");
						strHide.Add("Name");
					}
				}
				else if (pGrid.SelectedObject is RootNodeInfo)
				{
					var rootInfo = (RootNodeInfo) pGrid.SelectedObject;
					if (rootInfo.Type == RootNodeType.PuttySessions)
					{
						strHide.Add("Password");
					}
				}

                pGrid.HiddenProperties = strHide.ToArray();
                pGrid.Refresh();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridHideItemsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void btnShowProperties_Click(object sender, EventArgs e)
		{
			if (pGrid.SelectedObject is ConnectionInfoInheritance)
			{
				if (pGrid.SelectedObject is DefaultConnectionInheritance)
				{
                    PropertiesVisible = true;
                    InheritanceVisible = false;
                    DefaultPropertiesVisible = false;
                    DefaultInheritanceVisible = false;
                    SetPropertyGridObject((RootNodeInfo)Windows.treeForm.tvConnections.SelectedNode.Tag);
				}
				else
				{
                    PropertiesVisible = true;
                    InheritanceVisible = false;
                    DefaultPropertiesVisible = false;
                    DefaultInheritanceVisible = false;
                    SetPropertyGridObject(((ConnectionInfoInheritance)pGrid.SelectedObject).Parent);
				}
			}
			else if (pGrid.SelectedObject is ConnectionInfo)
			{
			    if (!((ConnectionInfo) pGrid.SelectedObject).IsDefault) return;
			    PropertiesVisible = true;
			    InheritanceVisible = false;
			    DefaultPropertiesVisible = false;
			    DefaultInheritanceVisible = false;
			    SetPropertyGridObject((RootNodeInfo)Windows.treeForm.tvConnections.SelectedNode.Tag);
			}
		}
				
		private void btnShowDefaultProperties_Click(object sender, EventArgs e)
		{
		    if (!(pGrid.SelectedObject is RootNodeInfo) && !(pGrid.SelectedObject is ConnectionInfoInheritance)) return;
		    PropertiesVisible = false;
		    InheritanceVisible = false;
		    DefaultPropertiesVisible = true;
		    DefaultInheritanceVisible = false;
		    SetPropertyGridObject(Runtime.DefaultConnectionFromSettings());
		}
				
		private void btnShowInheritance_Click(object sender, EventArgs e)
		{
		    if (!(pGrid.SelectedObject is ConnectionInfo)) return;
		    PropertiesVisible = false;
		    InheritanceVisible = true;
		    DefaultPropertiesVisible = false;
		    DefaultInheritanceVisible = false;
		    SetPropertyGridObject(((ConnectionInfo)pGrid.SelectedObject).Inheritance);
		}
				
		private void btnShowDefaultInheritance_Click(object sender, EventArgs e)
		{
			if (pGrid.SelectedObject is RootNodeInfo || pGrid.SelectedObject is ConnectionInfo)
			{
                PropertiesVisible = false;
                InheritanceVisible = false;
                DefaultPropertiesVisible = false;
                DefaultInheritanceVisible = true;
                SetPropertyGridObject(DefaultConnectionInheritance.Instance);
			}
		}
				
		private void btnHostStatus_Click(object sender, EventArgs e)
		{
			SetHostStatus(pGrid.SelectedObject);
		}
				
		private void btnIcon_Click(object sender, MouseEventArgs e)
		{
			try
			{
				if (pGrid.SelectedObject is ConnectionInfo && !(pGrid.SelectedObject is PuttySessionInfo))
				{
                    cMenIcons.Items.Clear();
							
					foreach (var iStr in ConnectionIcon.Icons)
					{
					    var tI = new ToolStripMenuItem
					    {
					        Text = iStr,
					        Image = ConnectionIcon.FromString(iStr).ToBitmap()
					    };
					    tI.Click += IconMenu_Click;

                        cMenIcons.Items.Add(tI);
					}
					var mPos = new Point(new Size(PointToScreen(new Point(e.Location.X + pGrid.Width - 100, e.Location.Y))));
                    cMenIcons.Show(mPos);
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
				var connectionInfo = (ConnectionInfo)pGrid.SelectedObject;
				if (connectionInfo == null)
				{
					return ;
				}
						
				var selectedMenuItem = (ToolStripMenuItem)sender;
				if (selectedMenuItem == null)
				{
					return ;
				}
						
				var iconName = selectedMenuItem.Text;
				if (string.IsNullOrEmpty(iconName))
				{
					return ;
				}
						
				var connectionIcon = ConnectionIcon.FromString(iconName);
				if (connectionIcon == null)
				{
					return ;
				}
						
				btnIcon.Image = connectionIcon.ToBitmap();
						
				connectionInfo.Icon = iconName;
				pGrid.Refresh();
						
				Runtime.SaveConnectionsBG();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConfigPropertyGridMenuClickFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Host Status (Ping)
		private string HostName;
		private Thread pThread;
				
		private void CheckHostAlive()
		{
			var pingSender = new Ping();

		    try
			{
			    var pReply = pingSender.Send(HostName);
			    if (pReply?.Status == IPStatus.Success)
				{
					if ((string)btnHostStatus.Tag == "checking")
					{
						ShowStatusImage(Resources.HostStatus_On);
					}
				}
				else
				{
					if ((string)btnHostStatus.Tag == "checking")
					{
						ShowStatusImage(Resources.HostStatus_Off);
					}
				}
			}
			catch (Exception)
			{
				if ((string)btnHostStatus.Tag == "checking")
				{
					ShowStatusImage(Resources.HostStatus_Off);
				}
			}
		}
				
		delegate void ShowStatusImageCB(Image image);
		private void ShowStatusImage(Image image)
		{
			if (pGrid.InvokeRequired)
			{
				ShowStatusImageCB d = ShowStatusImage;
                pGrid.Invoke(d, image);
			}
			else
			{
                btnHostStatus.Image = image;
                btnHostStatus.Tag = "checkfinished";
			}
		}
				
		public void SetHostStatus(object connectionInfo)
		{
			try
			{
                btnHostStatus.Image = Resources.HostStatus_Check;
				// To check status, ConnectionInfo must be an mRemoteNG.Connection.Info that is not a container
				if (connectionInfo is ConnectionInfo)
				{
                    if (((ConnectionInfo)connectionInfo).IsContainer)
					{
						return;
					}
				}
				else
				{
					return;
				}

                btnHostStatus.Tag = "checking";
                HostName = ((ConnectionInfo)connectionInfo).Hostname;
				pThread = new Thread(CheckHostAlive);
				pThread.SetApartmentState(ApartmentState.STA);
				pThread.IsBackground = true;
				pThread.Start();
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
				propertyGridContextMenuShowHelpText.Checked = Settings.Default.ShowConfigHelpText;
				var gridItem = pGrid.SelectedGridItem;
				propertyGridContextMenuReset.Enabled = Convert.ToBoolean(pGrid.SelectedObject != null && gridItem?.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(pGrid.SelectedObject));
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.Config.propertyGridContextMenu_Opening() failed.", ex, MessageClass.ErrorMsg, true);
			}
		}
				
		private void propertyGridContextMenuReset_Click(object sender, EventArgs e)
		{
			try
			{
				var gridItem = pGrid.SelectedGridItem;
				if (pGrid.SelectedObject != null && gridItem?.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(pGrid.SelectedObject))
				{
					pGrid.ResetSelectedProperty();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.Config.propertyGridContextMenuReset_Click() failed.", ex, MessageClass.ErrorMsg, true);
			}
		}
				
		private void propertyGridContextMenuShowHelpText_Click(object sender, EventArgs e)
		{
			propertyGridContextMenuShowHelpText.Checked = !propertyGridContextMenuShowHelpText.Checked;
		}
				
		private void propertyGridContextMenuShowHelpText_CheckedChanged(object sender, EventArgs e)
		{
            Settings.Default.ShowConfigHelpText = propertyGridContextMenuShowHelpText.Checked;
			pGrid.HelpVisible = propertyGridContextMenuShowHelpText.Checked;
        }
        #endregion
    }
}