using Azuria.Common.Controls;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;
using mRemoteNG.My;
using mRemoteNG.Root;
using mRemoteNG.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
    public class ConfigWindow : BaseWindow
	{
        private bool _originalPropertyGridToolStripItemCountValid;
        private int _originalPropertyGridToolStripItemCount;
        internal ToolStripButton btnShowProperties;
        internal ToolStripButton btnShowDefaultProperties;
        internal ToolStripButton btnShowInheritance;
        internal ToolStripButton btnShowDefaultInheritance;
        internal ToolStripButton btnIcon;
        internal ToolStripButton btnHostStatus;
        internal ContextMenuStrip cMenIcons;
        private System.ComponentModel.Container components = null;
        internal ContextMenuStrip propertyGridContextMenu;
        internal ToolStripMenuItem propertyGridContextMenuShowHelpText;
        internal ToolStripMenuItem propertyGridContextMenuReset;
        internal ToolStripSeparator ToolStripSeparator1;
        internal FilteredPropertyGrid pGrid;


        private void InitializeComponent()
		{
            components = new System.ComponentModel.Container();
            Load += new EventHandler(Config_Load);
			base.SystemColorsChanged += new EventHandler(Config_SystemColorsChanged);
            pGrid = new FilteredPropertyGrid();
            pGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(pGrid_PropertyValueChanged);
            pGrid.PropertySortChanged += new EventHandler(pGrid_PropertySortChanged);
            propertyGridContextMenu = new ContextMenuStrip(components);
            propertyGridContextMenu.Opening += new System.ComponentModel.CancelEventHandler(propertyGridContextMenu_Opening);
            propertyGridContextMenuReset = new ToolStripMenuItem();
            propertyGridContextMenuReset.Click += new EventHandler(propertyGridContextMenuReset_Click);
            ToolStripSeparator1 = new ToolStripSeparator();
            propertyGridContextMenuShowHelpText = new ToolStripMenuItem();
            propertyGridContextMenuShowHelpText.Click += new EventHandler(propertyGridContextMenuShowHelpText_Click);
            propertyGridContextMenuShowHelpText.CheckedChanged += new EventHandler(propertyGridContextMenuShowHelpText_CheckedChanged);
            btnShowInheritance = new ToolStripButton();
            btnShowInheritance.Click += new EventHandler(btnShowInheritance_Click);
            btnShowDefaultInheritance = new ToolStripButton();
            btnShowDefaultInheritance.Click += new EventHandler(btnShowDefaultInheritance_Click);
            btnShowProperties = new ToolStripButton();
            btnShowProperties.Click += new EventHandler(btnShowProperties_Click);
            btnShowDefaultProperties = new ToolStripButton();
            btnShowDefaultProperties.Click += new EventHandler(btnShowDefaultProperties_Click);
            btnIcon = new ToolStripButton();
            btnIcon.MouseUp += new MouseEventHandler(btnIcon_Click);
            btnHostStatus = new ToolStripButton();
            btnHostStatus.Click += new EventHandler(btnHostStatus_Click);
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
            pGrid.Font = new Font("Microsoft Sans Serif", (float) (8.25F), FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
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
            propertyGridContextMenuReset.Text = "&Reset";
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
            propertyGridContextMenuShowHelpText.Text = "&Show Help Text";
            //
            //btnShowInheritance
            //
            btnShowInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowInheritance.Image = My.Resources.Inheritance;
            btnShowInheritance.ImageTransparentColor = Color.Magenta;
            btnShowInheritance.Name = "btnShowInheritance";
            btnShowInheritance.Size = new Size(23, 22);
            btnShowInheritance.Text = "Inheritance";
            //
            //btnShowDefaultInheritance
            //
            btnShowDefaultInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowDefaultInheritance.Image = My.Resources.Inheritance_Default;
            btnShowDefaultInheritance.ImageTransparentColor = Color.Magenta;
            btnShowDefaultInheritance.Name = "btnShowDefaultInheritance";
            btnShowDefaultInheritance.Size = new Size(23, 22);
            btnShowDefaultInheritance.Text = "Default Inheritance";
            //
            //btnShowProperties
            //
            btnShowProperties.Checked = true;
            btnShowProperties.CheckState = CheckState.Checked;
            btnShowProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowProperties.Image = My.Resources.Properties;
            btnShowProperties.ImageTransparentColor = Color.Magenta;
            btnShowProperties.Name = "btnShowProperties";
            btnShowProperties.Size = new Size(23, 22);
            btnShowProperties.Text = "Properties";
            //
            //btnShowDefaultProperties
            //
            btnShowDefaultProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnShowDefaultProperties.Image = My.Resources.Properties_Default;
            btnShowDefaultProperties.ImageTransparentColor = Color.Magenta;
            btnShowDefaultProperties.Name = "btnShowDefaultProperties";
            btnShowDefaultProperties.Size = new Size(23, 22);
            btnShowDefaultProperties.Text = "Default Properties";
            //
            //btnIcon
            //
            btnIcon.Alignment = ToolStripItemAlignment.Right;
            btnIcon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnIcon.ImageTransparentColor = Color.Magenta;
            btnIcon.Name = "btnIcon";
            btnIcon.Size = new Size(23, 22);
            btnIcon.Text = "Icon";
            //
            //btnHostStatus
            //
            btnHostStatus.Alignment = ToolStripItemAlignment.Right;
            btnHostStatus.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnHostStatus.Image = My.Resources.HostStatus_Check;
            btnHostStatus.ImageTransparentColor = Color.Magenta;
            btnHostStatus.Name = "btnHostStatus";
            btnHostStatus.Size = new Size(23, 22);
            btnHostStatus.Tag = "checking";
            btnHostStatus.Text = "Status";
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
            Font = new Font("Microsoft Sans Serif", (float) (8.25F), FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            HideOnClose = true;
            Icon = My.Resources.Config_Icon;
            Name = "Config";
            TabText = "Config";
            Text = "Config";
            propertyGridContextMenu.ResumeLayout(false);
            ResumeLayout(false);
					
		}
		
        #region Public Properties
        public bool PropertiesVisible
		{
			get
			{
				if (btnShowProperties.Checked)
					return true;
				else
					return false;
			}
			set
			{
                btnShowProperties.Checked = value;
				if (value == true)
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
				if (btnShowInheritance.Checked)
					return true;
				else
					return false;
			}
			set
			{
                btnShowInheritance.Checked = value;
				if (value == true)
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
				if (btnShowDefaultProperties.Checked)
					return true;
				else
					return false;
			}
			set
			{
                btnShowDefaultProperties.Checked = value;
				if (value == true)
				{
                    btnShowProperties.Checked = false;
                    btnShowDefaultInheritance.Checked = false;
                    btnShowInheritance.Checked = false;
				}
			}
		}
				
        public bool DefaultInheritanceVisible
		{
			get
			{
				if (btnShowDefaultInheritance.Checked)
					return true;
				else
					return false;
			}
			set
			{
                btnShowDefaultInheritance.Checked = value;
				if (value == true)
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
		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
		{
			if ((keyData & Keys.KeyCode) == Keys.Tab)
			{
				GridItem selectedItem = pGrid.SelectedGridItem;
				GridItem gridRoot = selectedItem;
				while (gridRoot.GridItemType != GridItemType.Root)
				{
					gridRoot = gridRoot.Parent;
				}
						
				List<GridItem> gridItems = new List<GridItem>();
				FindChildGridItems(gridRoot, ref gridItems);
						
				if (!ContainsGridItemProperty(gridItems))
				{
					return true;
				}
						
				GridItem newItem = selectedItem;
						
				if (keyData == (Keys.Tab | Keys.Shift))
				{
					newItem = FindPreviousGridItemProperty(gridItems, selectedItem);
				}
				else if (keyData == Keys.Tab)
				{
					newItem = FindNextGridItemProperty(gridItems, selectedItem);
				}
						
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
			foreach (GridItem item in gridItems)
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
			if (gridItems.Count == 0)
			{
				return null;
			}
			if (startItem == null)
			{
				return null;
			}
					
			int startIndex = gridItems.IndexOf(startItem);
					
			if (startItem.GridItemType == GridItemType.Property)
			{
				startIndex--;
				if (startIndex < 0)
				{
					startIndex = gridItems.Count - 1;
				}
			}
					
			int previousIndex = 0;
			bool previousIndexValid = false;
			for (int index = startIndex; index >= 0; index--)
			{
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					previousIndex = index;
					previousIndexValid = true;
					break;
				}
			}
					
			if (previousIndexValid)
			{
				return gridItems[previousIndex];
			}
					
			for (int index = gridItems.Count - 1; index >= startIndex + 1; index--)
			{
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					previousIndex = index;
					previousIndexValid = true;
					break;
				}
			}
					
			if (!previousIndexValid)
			{
				return null;
			}
					
			return gridItems[previousIndex];
		}
				
		private GridItem FindNextGridItemProperty(List<GridItem> gridItems, GridItem startItem)
		{
			if (gridItems.Count == 0)
			{
				return null;
			}
			if (startItem == null)
			{
				return null;
			}
					
			int startIndex = gridItems.IndexOf(startItem);
					
			if (startItem.GridItemType == GridItemType.Property)
			{
				startIndex++;
				if (startIndex >= gridItems.Count)
				{
					startIndex = 0;
				}
			}
					
			int nextIndex = 0;
			bool nextIndexValid = false;
			for (int index = startIndex; index <= gridItems.Count - 1; index++)
			{
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					nextIndex = index;
					nextIndexValid = true;
					break;
				}
			}
					
			if (nextIndexValid)
			{
				return gridItems[nextIndex];
			}
					
			for (int index = 0; index <= startIndex - 1; index++)
			{
				if (gridItems[index].GridItemType == GridItemType.Property)
				{
					nextIndex = index;
					nextIndexValid = true;
					break;
				}
			}
					
			if (!nextIndexValid)
			{
				return null;
			}
					
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
						else if (InheritanceVisible) //Inheritance selected
						{
                            pGrid.SelectedObject = ((ConnectionInfo)Obj).Inherit;

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
                        if (((Container.ContainerInfo)((ConnectionInfo)Obj).Parent).Parent != null)
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

                    Icon conIcon = ConnectionIcon.FromString(Convert.ToString(((ConnectionInfo)Obj).Icon));
					if (conIcon != null)
					{
                        btnIcon.Image = conIcon.ToBitmap();
					}
				}
				else if (Obj is Root.Info) //ROOT
				{
					Root.Info rootInfo = (Root.Info) Obj;
					switch (rootInfo.Type)
					{
						case Root.Info.RootType.Connection:
							PropertiesVisible = true;
							DefaultPropertiesVisible = false;
							btnShowProperties.Enabled = true;
							btnShowInheritance.Enabled = false;
							btnShowDefaultProperties.Enabled = true;
							btnShowDefaultInheritance.Enabled = true;
							btnIcon.Enabled = false;
							btnHostStatus.Enabled = false;
							break;
						case Root.Info.RootType.Credential:
							throw (new NotImplementedException());
						case Root.Info.RootType.PuttySessions:
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
                        Icon conIcon = ConnectionIcon.FromString(Convert.ToString(((ConnectionInfo)((ConnectionInfoInheritance)Obj).Parent).Icon));
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConfigPropertyGridObjectFailed + Environment.NewLine + ex.Message, true);
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
			btnShowInheritance.Text = My.Language.strButtonInheritance;
			btnShowDefaultInheritance.Text = My.Language.strButtonDefaultInheritance;
			btnShowProperties.Text = My.Language.strButtonProperties;
			btnShowDefaultProperties.Text = My.Language.strButtonDefaultProperties;
			btnIcon.Text = My.Language.strButtonIcon;
			btnHostStatus.Text = My.Language.strStatus;
			Text = My.Language.strMenuConfig;
			TabText = My.Language.strMenuConfig;
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
				ToolStrip customToolStrip = new ToolStrip();
				customToolStrip.Items.Add(btnShowProperties);
				customToolStrip.Items.Add(btnShowInheritance);
				customToolStrip.Items.Add(btnShowDefaultProperties);
				customToolStrip.Items.Add(btnShowDefaultInheritance);
				customToolStrip.Items.Add(btnHostStatus);
				customToolStrip.Items.Add(btnIcon);
				customToolStrip.Show();
						
				ToolStrip propertyGridToolStrip = new ToolStrip();
						
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
						
				int expectedToolStripItemCount = _originalPropertyGridToolStripItemCount + customToolStrip.Items.Count;
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
			pGrid.HelpVisible = My.Settings.Default.ShowConfigHelpText;
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConfigPropertyGridValueFailed + Environment.NewLine + ex.Message, true);
			}
		}

        private void UpdateConnectionInfoNode(PropertyValueChangedEventArgs e)
        {
            if (pGrid.SelectedObject is ConnectionInfo)
            {
                if (e.ChangedItem.Label == My.Language.strPropertyNameProtocol)
                {
                    ((ConnectionInfo)pGrid.SelectedObject).SetDefaultPort();
                }
                else if (e.ChangedItem.Label == My.Language.strPropertyNameName)
                {
                    Windows.treeForm.tvConnections.SelectedNode.Text = Convert.ToString(((ConnectionInfo)pGrid.SelectedObject).Name);
                    if (My.Settings.Default.SetHostnameLikeDisplayName && pGrid.SelectedObject is ConnectionInfo)
                    {
                        ConnectionInfo connectionInfo = (ConnectionInfo)pGrid.SelectedObject;
                        if (!string.IsNullOrEmpty(connectionInfo.Name))
                            connectionInfo.Hostname = connectionInfo.Name;
                    }
                }
                else if (e.ChangedItem.Label == My.Language.strPropertyNameIcon)
                {
                    Icon conIcon = ConnectionIcon.FromString(Convert.ToString(((ConnectionInfo)pGrid.SelectedObject).Icon));
                    if (conIcon != null)
                        btnIcon.Image = conIcon.ToBitmap();
                }
                else if (e.ChangedItem.Label == My.Language.strPropertyNameAddress)
                {
                    SetHostStatus(pGrid.SelectedObject);
                }

                if (((ConnectionInfo)pGrid.SelectedObject).IsDefault)
                    Runtime.DefaultConnectionToSettings();
            }
        }

        private void UpdateRootInfoNode(PropertyValueChangedEventArgs e)
        {
            if (pGrid.SelectedObject is Info)
            {
                Info rootInfo = (Info)pGrid.SelectedObject;
                switch (e.ChangedItem.PropertyDescriptor.Name)
                {
                    case "Password":
                        if (rootInfo.Password == true)
                        {
                            string passwordName = "";
                            if (My.Settings.Default.UseSQLServer)
                                passwordName = Language.strSQLServer.TrimEnd(':');
                            else
                                passwordName = Path.GetFileName(Runtime.GetStartupConnectionFileName());

                            string password = MiscTools.PasswordDialog(passwordName);
                            if (string.IsNullOrEmpty(password))
                                rootInfo.Password = false;
                            else
                                rootInfo.PasswordString = password;
                        }
                        break;
                    case "Name":
                        break;
                }
            }
        }

        private void UpdateInheritanceNode()
        {
            if (pGrid.SelectedObject is ConnectionInfoInheritance)
            {
                if (((ConnectionInfoInheritance)pGrid.SelectedObject).IsDefault)
                {
                    Runtime.DefaultInheritanceToSettings();
                }
            }
        }

        private void pGrid_PropertySortChanged(object sender, EventArgs e)
		{
			if (pGrid.PropertySort == PropertySort.CategorizedAlphabetical)
			{
				pGrid.PropertySort = PropertySort.Categorized;
			}
		}
				
		private void ShowHideGridItems()
		{
			try
			{
				System.Collections.Generic.List<string> strHide = new System.Collections.Generic.List<string>();
						
				if (pGrid.SelectedObject is ConnectionInfo)
				{
                    ConnectionInfo conI = (ConnectionInfo)pGrid.SelectedObject;
							
					switch (conI.Protocol)
					{
						case mRemoteNG.Connection.Protocol.ProtocolType.RDP:
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.VNC:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.SSH1:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.SSH2:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.Telnet:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.Rlogin:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.RAW:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.HTTP:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.HTTPS:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ExtApp");
							strHide.Add("ICAEncryption");
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
						case mRemoteNG.Connection.Protocol.ProtocolType.ICA:
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
						case mRemoteNG.Connection.Protocol.ProtocolType.IntApp:
							strHide.Add("CacheBitmaps");
							strHide.Add("Colors");
							strHide.Add("DisplayThemes");
							strHide.Add("DisplayWallpaper");
							strHide.Add("EnableFontSmoothing");
							strHide.Add("EnableDesktopComposition");
							strHide.Add("Domain");
							strHide.Add("ICAEncryption");
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
						if (conI.Inherit.CacheBitmaps)
						{
							strHide.Add("CacheBitmaps");
						}
								
						if (conI.Inherit.Colors)
						{
							strHide.Add("Colors");
						}
								
						if (conI.Inherit.Description)
						{
							strHide.Add("Description");
						}
								
						if (conI.Inherit.DisplayThemes)
						{
							strHide.Add("DisplayThemes");
						}
								
						if (conI.Inherit.DisplayWallpaper)
						{
							strHide.Add("DisplayWallpaper");
						}
								
						if (conI.Inherit.EnableFontSmoothing)
						{
							strHide.Add("EnableFontSmoothing");
						}
								
						if (conI.Inherit.EnableDesktopComposition)
						{
							strHide.Add("EnableDesktopComposition");
						}
								
						if (conI.Inherit.Domain)
						{
							strHide.Add("Domain");
						}
								
						if (conI.Inherit.Icon)
						{
							strHide.Add("Icon");
						}
								
						if (conI.Inherit.Password)
						{
							strHide.Add("Password");
						}
								
						if (conI.Inherit.Port)
						{
							strHide.Add("Port");
						}
								
						if (conI.Inherit.Protocol)
						{
							strHide.Add("Protocol");
						}
								
						if (conI.Inherit.PuttySession)
						{
							strHide.Add("PuttySession");
						}
								
						if (conI.Inherit.RedirectDiskDrives)
						{
							strHide.Add("RedirectDiskDrives");
						}
								
						if (conI.Inherit.RedirectKeys)
						{
							strHide.Add("RedirectKeys");
						}
								
						if (conI.Inherit.RedirectPorts)
						{
							strHide.Add("RedirectPorts");
						}
								
						if (conI.Inherit.RedirectPrinters)
						{
							strHide.Add("RedirectPrinters");
						}
								
						if (conI.Inherit.RedirectSmartCards)
						{
							strHide.Add("RedirectSmartCards");
						}
								
						if (conI.Inherit.RedirectSound)
						{
							strHide.Add("RedirectSound");
						}
								
						if (conI.Inherit.Resolution)
						{
							strHide.Add("Resolution");
						}
								
						if (conI.Inherit.AutomaticResize)
						{
							strHide.Add("AutomaticResize");
						}
								
						if (conI.Inherit.UseConsoleSession)
						{
							strHide.Add("UseConsoleSession");
						}
								
						if (conI.Inherit.UseCredSsp)
						{
							strHide.Add("UseCredSsp");
						}
								
						if (conI.Inherit.RenderingEngine)
						{
							strHide.Add("RenderingEngine");
						}
								
						if (conI.Inherit.ICAEncryption)
						{
							strHide.Add("ICAEncryption");
						}
								
						if (conI.Inherit.RDPAuthenticationLevel)
						{
							strHide.Add("RDPAuthenticationLevel");
						}
								
						if (conI.Inherit.LoadBalanceInfo)
						{
							strHide.Add("LoadBalanceInfo");
						}
								
						if (conI.Inherit.Username)
						{
							strHide.Add("Username");
						}
								
						if (conI.Inherit.Panel)
						{
							strHide.Add("Panel");
						}
								
						if (conI.IsContainer)
						{
							strHide.Add("Hostname");
						}
								
						if (conI.Inherit.PreExtApp)
						{
							strHide.Add("PreExtApp");
						}
								
						if (conI.Inherit.PostExtApp)
						{
							strHide.Add("PostExtApp");
						}
								
						if (conI.Inherit.MacAddress)
						{
							strHide.Add("MacAddress");
						}
								
						if (conI.Inherit.UserField)
						{
							strHide.Add("UserField");
						}
								
						if (conI.Inherit.VNCAuthMode)
						{
							strHide.Add("VNCAuthMode");
						}
								
						if (conI.Inherit.VNCColors)
						{
							strHide.Add("VNCColors");
						}
								
						if (conI.Inherit.VNCCompression)
						{
							strHide.Add("VNCCompression");
						}
								
						if (conI.Inherit.VNCEncoding)
						{
							strHide.Add("VNCEncoding");
						}
								
						if (conI.Inherit.VNCProxyIP)
						{
							strHide.Add("VNCProxyIP");
						}
								
						if (conI.Inherit.VNCProxyPassword)
						{
							strHide.Add("VNCProxyPassword");
						}
								
						if (conI.Inherit.VNCProxyPort)
						{
							strHide.Add("VNCProxyPort");
						}
								
						if (conI.Inherit.VNCProxyType)
						{
							strHide.Add("VNCProxyType");
						}
								
						if (conI.Inherit.VNCProxyUsername)
						{
							strHide.Add("VNCProxyUsername");
						}
								
						if (conI.Inherit.VNCViewOnly)
						{
							strHide.Add("VNCViewOnly");
						}
								
						if (conI.Inherit.VNCSmartSizeMode)
						{
							strHide.Add("VNCSmartSizeMode");
						}
								
						if (conI.Inherit.ExtApp)
						{
							strHide.Add("ExtApp");
						}
								
						if (conI.Inherit.RDGatewayUsageMethod)
						{
							strHide.Add("RDGatewayUsageMethod");
						}
								
						if (conI.Inherit.RDGatewayHostname)
						{
							strHide.Add("RDGatewayHostname");
						}
								
						if (conI.Inherit.RDGatewayUsername)
						{
							strHide.Add("RDGatewayUsername");
						}
								
						if (conI.Inherit.RDGatewayPassword)
						{
							strHide.Add("RDGatewayPassword");
						}
								
						if (conI.Inherit.RDGatewayDomain)
						{
							strHide.Add("RDGatewayDomain");
						}
								
						if (conI.Inherit.RDGatewayUseConnectionCredentials)
						{
							strHide.Add("RDGatewayUseConnectionCredentials");
						}
								
						if (conI.Inherit.RDGatewayHostname)
						{
							strHide.Add("RDGatewayHostname");
						}
					}
					else
					{
						strHide.Add("Hostname");
						strHide.Add("Name");
					}
				}
				else if (pGrid.SelectedObject is Root.Info)
				{
					Root.Info rootInfo = (Root.Info) pGrid.SelectedObject;
					if (rootInfo.Type == Root.Info.RootType.PuttySessions)
					{
						strHide.Add("Password");
					}
				}

                pGrid.HiddenProperties = strHide.ToArray();

                pGrid.Refresh();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConfigPropertyGridHideItemsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void btnShowProperties_Click(object sender, EventArgs e)
		{
			if (pGrid.SelectedObject is ConnectionInfoInheritance)
			{
				if (((ConnectionInfoInheritance)pGrid.SelectedObject).IsDefault)
				{
                    PropertiesVisible = true;
                    InheritanceVisible = false;
                    DefaultPropertiesVisible = false;
                    DefaultInheritanceVisible = false;
                    SetPropertyGridObject((mRemoteNG.Root.Info)Windows.treeForm.tvConnections.SelectedNode.Tag);
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
                if (((ConnectionInfo)pGrid.SelectedObject).IsDefault)
				{
                    PropertiesVisible = true;
                    InheritanceVisible = false;
                    DefaultPropertiesVisible = false;
                    DefaultInheritanceVisible = false;
                    SetPropertyGridObject((mRemoteNG.Root.Info)Windows.treeForm.tvConnections.SelectedNode.Tag);
				}
			}
		}
				
		private void btnShowDefaultProperties_Click(object sender, EventArgs e)
		{
			if (pGrid.SelectedObject is mRemoteNG.Root.Info || pGrid.SelectedObject is ConnectionInfoInheritance)
			{
                PropertiesVisible = false;
                InheritanceVisible = false;
                DefaultPropertiesVisible = true;
                DefaultInheritanceVisible = false;
                SetPropertyGridObject(Runtime.DefaultConnectionFromSettings());
			}
		}
				
		private void btnShowInheritance_Click(object sender, EventArgs e)
		{
			if (pGrid.SelectedObject is ConnectionInfo)
			{
                PropertiesVisible = false;
                InheritanceVisible = true;
                DefaultPropertiesVisible = false;
                DefaultInheritanceVisible = false;
                SetPropertyGridObject(((ConnectionInfo)pGrid.SelectedObject).Inherit);
			}
		}
				
		private void btnShowDefaultInheritance_Click(object sender, EventArgs e)
		{
			if (pGrid.SelectedObject is mRemoteNG.Root.Info || pGrid.SelectedObject is ConnectionInfo)
			{
                PropertiesVisible = false;
                InheritanceVisible = false;
                DefaultPropertiesVisible = false;
                DefaultInheritanceVisible = true;
                SetPropertyGridObject(Runtime.DefaultInheritanceFromSettings());
			}
		}
				
		private void btnHostStatus_Click(object sender, EventArgs e)
		{
			SetHostStatus(pGrid.SelectedObject);
		}
				
		private void btnIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (pGrid.SelectedObject is ConnectionInfo&& !(pGrid.SelectedObject is PuttySessionInfo))
				{
                    cMenIcons.Items.Clear();
							
					foreach (string iStr in ConnectionIcon.Icons)
					{
						ToolStripMenuItem tI = new ToolStripMenuItem();
						tI.Text = iStr;
						tI.Image = ConnectionIcon.FromString(iStr).ToBitmap();
						tI.Click += IconMenu_Click;

                        cMenIcons.Items.Add(tI);
					}
					Point mPos = new Point(new Size(PointToScreen(new Point(e.Location.X + pGrid.Width - 100, e.Location.Y))));
                    cMenIcons.Show(mPos);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConfigPropertyGridButtonIconClickFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void IconMenu_Click(object sender, EventArgs e)
		{
			try
			{
				ConnectionInfo connectionInfo = (ConnectionInfo)pGrid.SelectedObject;
				if (connectionInfo == null)
				{
					return ;
				}
						
				ToolStripMenuItem selectedMenuItem = (ToolStripMenuItem)sender;
				if (selectedMenuItem == null)
				{
					return ;
				}
						
				string iconName = selectedMenuItem.Text;
				if (string.IsNullOrEmpty(iconName))
				{
					return ;
				}
						
				Icon connectionIcon = ConnectionIcon.FromString(iconName);
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
		private System.Threading.Thread pThread;
				
		private void CheckHostAlive()
		{
			Ping pingSender = new Ping();
			PingReply pReply;
					
			try
			{
				pReply = pingSender.Send(HostName);
				if (pReply.Status == IPStatus.Success)
				{
					if ((string)btnHostStatus.Tag == "checking")
					{
						ShowStatusImage(My.Resources.HostStatus_On);
					}
				}
				else
				{
					if ((string)btnHostStatus.Tag == "checking")
					{
						ShowStatusImage(My.Resources.HostStatus_Off);
					}
				}
			}
			catch (Exception)
			{
				if ((string)btnHostStatus.Tag == "checking")
				{
					ShowStatusImage(My.Resources.HostStatus_Off);
				}
			}
		}
				
		delegate void ShowStatusImageCB(Image Image);
		private void ShowStatusImage(Image Image)
		{
			if (pGrid.InvokeRequired)
			{
				ShowStatusImageCB d = new ShowStatusImageCB(ShowStatusImage);
                pGrid.Invoke(d, new object[] {Image});
			}
			else
			{
                btnHostStatus.Image = Image;
                btnHostStatus.Tag = "checkfinished";
			}
		}
				
		public void SetHostStatus(object ConnectionInfo)
		{
			try
			{
                btnHostStatus.Image = My.Resources.HostStatus_Check;
				// To check status, ConnectionInfo must be an mRemoteNG.Connection.Info that is not a container
				if (ConnectionInfo is ConnectionInfo)
				{
                    if (((ConnectionInfo)ConnectionInfo).IsContainer)
					{
						return;
					}
				}
				else
				{
					return;
				}

                btnHostStatus.Tag = "checking";
                HostName = ((ConnectionInfo)ConnectionInfo).Hostname;
				pThread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckHostAlive));
				pThread.SetApartmentState(System.Threading.ApartmentState.STA);
				pThread.IsBackground = true;
				pThread.Start();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConfigPropertyGridSetHostStatusFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion

        #region Event Handlers
        private void propertyGridContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				propertyGridContextMenuShowHelpText.Checked = My.Settings.Default.ShowConfigHelpText;
				GridItem gridItem = pGrid.SelectedGridItem;
				propertyGridContextMenuReset.Enabled = Convert.ToBoolean(pGrid.SelectedObject != null && gridItem != null && gridItem.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(pGrid.SelectedObject));
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
				GridItem gridItem = pGrid.SelectedGridItem;
				if (pGrid.SelectedObject != null && gridItem != null && gridItem.PropertyDescriptor != null && gridItem.PropertyDescriptor.CanResetValue(pGrid.SelectedObject))
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
			My.Settings.Default.ShowConfigHelpText = propertyGridContextMenuShowHelpText.Checked;
			pGrid.HelpVisible = propertyGridContextMenuShowHelpText.Checked;
        }
        #endregion
    }
}