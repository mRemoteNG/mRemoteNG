using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Themes;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;


namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
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
        private ConnectionInfoPropertyGrid _pGrid;
        private ThemeManager _themeManager;

        private ConnectionInfo _selectedTreeNode;

        public ConnectionInfo SelectedTreeNode
        {
            get => _selectedTreeNode;
            set
            {
                _selectedTreeNode = value;
                _pGrid.SelectedConnectionInfo = value;
                UpdateTopRow();
            }
        }

        private void InitializeComponent()
        {
            _components = new System.ComponentModel.Container();
            Load += Config_Load;
            SystemColorsChanged += Config_SystemColorsChanged;
            _pGrid = new ConnectionInfoPropertyGrid();
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
            PropertyGridContextMenu.Items.AddRange(new ToolStripItem[]
            {
                _propertyGridContextMenuReset, _toolStripSeparator1, _propertyGridContextMenuShowHelpText
            });
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
            _btnShowInheritance.Image = Properties.Resources.Schema_16x;
            _btnShowInheritance.ImageTransparentColor = Color.Magenta;
            _btnShowInheritance.Name = "_btnShowInheritance";
            _btnShowInheritance.Size = new Size(23, 22);
            _btnShowInheritance.Text = @"Inheritance";
            //
            //btnShowDefaultInheritance
            //
            _btnShowDefaultInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnShowDefaultInheritance.Image = Properties.Resources.ViewDownBySchema_16x;
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
            _btnShowProperties.Image = Properties.Resources.Property_16x;
            _btnShowProperties.ImageTransparentColor = Color.Magenta;
            _btnShowProperties.Name = "_btnShowProperties";
            _btnShowProperties.Size = new Size(23, 22);
            _btnShowProperties.Text = @"Properties";
            //
            //btnShowDefaultProperties
            //
            _btnShowDefaultProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnShowDefaultProperties.Image = Properties.Resources.ExtendedProperty_16x;
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
            _btnHostStatus.Image = Properties.Resources.HostStatus_Check;
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
            Name = "ConfigWindow";
            TabText = @"Config";
            Text = @"Config";
            PropertyGridContextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #region Public Properties

        public bool PropertiesVisible => _btnShowProperties.Checked;
        public bool CanShowProperties => SelectedTreeNode != null;

        public bool InheritanceVisible => _btnShowInheritance.Checked;
        public bool CanShowInheritance => SelectedTreeNode != null &&
                                          _pGrid.SelectedConnectionInfo?.Parent != null;

        public bool DefaultPropertiesVisible => _btnShowDefaultProperties.Checked;
        public bool CanShowDefaultProperties => true;

        public bool DefaultInheritanceVisible => _btnShowDefaultInheritance.Checked;
        public bool CanShowDefaultInheritance => true;

        /// <summary>
        /// A list of properties being shown for the current object.
        /// </summary>
        public IEnumerable<string> VisibleObjectProperties => _pGrid.VisibleProperties;

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
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            ApplyLanguage();
        }

        #endregion

        #region Public Methods

        public void ShowConnectionProperties()
        {
            _pGrid.PropertyMode = PropertyMode.Connection;
            UpdateTopRow();
        }

        public void ShowInheritanceProperties()
        {
            _pGrid.PropertyMode = PropertyMode.Inheritance;
            UpdateTopRow();
        }

        public void ShowDefaultConnectionProperties()
        {
            _pGrid.PropertyMode = PropertyMode.DefaultConnection;
            UpdateTopRow();
        }

        public void ShowDefaultInheritanceProperties()
        {
            _pGrid.PropertyMode = PropertyMode.DefaultInheritance;
            UpdateTopRow();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            // Main form handle command key events
            // Adapted from http://kiwigis.blogspot.com/2009/05/adding-tab-key-support-to-propertygrid.html
            if ((keyData & Keys.KeyCode) != Keys.Tab)
                return base.ProcessCmdKey(ref msg, keyData);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (keyData)
            {
                case (Keys.Tab | Keys.Shift):
                    _pGrid.SelectPreviousGridItem();
                    break;
                case Keys.Tab:
                    _pGrid.SelectNextGridItem();
                    break;
            }

            return true; // Handled
        }
        #endregion

        #region Private Methods
        private void ApplyLanguage()
        {
            _btnShowInheritance.Text = Language.Inheritance;
            _btnShowDefaultInheritance.Text = Language.ButtonDefaultInheritance;
            _btnShowProperties.Text = Language.Properties;
            _btnShowDefaultProperties.Text = Language.ButtonDefaultProperties;
            _btnIcon.Text = Language.Icon;
            _btnHostStatus.Text = Language.Status;
            Text = Language.Config;
            TabText = Language.Config;
            _propertyGridContextMenuShowHelpText.Text = Language.ShowHelpText;
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            _pGrid.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            _pGrid.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
            _pGrid.ViewBackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Background");
            _pGrid.ViewForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Foreground");
            _pGrid.LineColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Border");
            _pGrid.HelpBackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            _pGrid.HelpForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
            _pGrid.CategoryForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Header_Foreground");
            _pGrid.CommandsDisabledLinkColor =
                _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Disabled_Foreground");
            _pGrid.CommandsBackColor =
                _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Disabled_Background");
            _pGrid.CommandsForeColor =
                _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Disabled_Foreground");
        }

        private void UpdateTopRow()
        {
            try
            {
                // if we are on the show inheritance tab but it isn't a
                // valid choice, switch to the properties tab
                if (_pGrid.PropertyMode == PropertyMode.Inheritance && !CanShowInheritance)
                {
                    ShowConnectionProperties();
                    return;
                }

                UpdatePropertiesButton();
                UpdateShowInheritanceButton();
                UpdateShowDefaultPropertiesButton();
                UpdateShowDefaultInheritanceButton();
                UpdateHostStatusButton();
                UpdateIconButton();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(
                    MessageClass.ErrorMsg,
                    Language.ConfigPropertyGridObjectFailed + Environment.NewLine +
                    ex.Message, true);
            }
        }

        private void UpdatePropertiesButton()
        {
            _btnShowProperties.Enabled = CanShowProperties;
            _btnShowProperties.Checked =
                _pGrid.PropertyMode == PropertyMode.Connection;
        }

        private void UpdateShowInheritanceButton()
        {
            _btnShowInheritance.Enabled = CanShowInheritance;
            _btnShowInheritance.Checked =
                _pGrid.PropertyMode == PropertyMode.Inheritance;
        }

        private void UpdateShowDefaultPropertiesButton()
        {
            _btnShowDefaultProperties.Enabled = CanShowDefaultProperties;
            _btnShowDefaultProperties.Checked =
                _pGrid.PropertyMode == PropertyMode.DefaultConnection;
        }

        private void UpdateShowDefaultInheritanceButton()
        {
            _btnShowDefaultInheritance.Enabled = CanShowDefaultInheritance;
            _btnShowDefaultInheritance.Checked =
                _pGrid.PropertyMode == PropertyMode.DefaultInheritance;
        }

        private void UpdateHostStatusButton()
        {
            _btnHostStatus.Enabled =
                !_pGrid.RootNodeSelected &&
                !_pGrid.IsShowingDefaultProperties &&
                !(_pGrid.SelectedConnectionInfo is ContainerInfo);

            SetHostStatus(_pGrid.SelectedObject);
        }

        private void UpdateIconButton()
        {
            _btnIcon.Enabled =
                _pGrid.SelectedConnectionInfo != null &&
                !_pGrid.IsShowingDefaultProperties &&
                !_pGrid.RootNodeSelected;

            _btnIcon.Image = _btnIcon.Enabled
                ? ConnectionIcon
                    .FromString(_pGrid.SelectedConnectionInfo?.Icon)?
                    .ToBitmap()
                : null;
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
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.CouldNotFindToolStripInFilteredPropertyGrid, true);
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.ConfigUiLoadFailed + Environment.NewLine + ex.Message,
                                                    true);
            }
        }

        private void Config_Load(object sender, EventArgs e)
        {
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
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
                if (e.ChangedItem.Label == Language.Icon)
                {
                    var conIcon = ConnectionIcon.FromString(_pGrid.SelectedConnectionInfo.Icon);
                    if (conIcon != null)
                        _btnIcon.Image = conIcon.ToBitmap();
                }
                else if (e.ChangedItem.Label == Language.HostnameIp)
                {
                    SetHostStatus(_pGrid.SelectedConnectionInfo);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.ConfigPropertyGridValueFailed + Environment.NewLine +
                                                    ex.Message, true);
            }
        }

        private void pGrid_PropertySortChanged(object sender, EventArgs e)
        {
            if (_pGrid.PropertySort == PropertySort.CategorizedAlphabetical)
                _pGrid.PropertySort = PropertySort.Categorized;
        }

        private void btnShowProperties_Click(object sender, EventArgs e)
        {
            ShowConnectionProperties();
        }

        private void btnShowInheritance_Click(object sender, EventArgs e)
        {
            ShowInheritanceProperties();
        }

        private void btnShowDefaultProperties_Click(object sender, EventArgs e)
        {
            ShowDefaultConnectionProperties();
        }

        private void btnShowDefaultInheritance_Click(object sender, EventArgs e)
        {
            ShowDefaultInheritanceProperties();
        }

        private void btnHostStatus_Click(object sender, EventArgs e)
        {
            SetHostStatus(_pGrid.SelectedObject);
        }

        private void btnIcon_Click(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(_pGrid.SelectedObject is ConnectionInfo) || _pGrid.SelectedObject is PuttySessionInfo) return;
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

                var mPos = new Point(
                                     new Size(PointToScreen(new Point(e.Location.X + _pGrid.Width - 100,
                                                                      e.Location.Y))));
                CMenIcons.Show(mPos);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.ConfigPropertyGridButtonIconClickFailed +
                                                    Environment.NewLine + ex.Message, true);
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

                Runtime.ConnectionsService.SaveConnectionsAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.ConfigPropertyGridMenuClickFailed +
                                                    Environment.NewLine + ex.Message, true);
            }
        }

        #endregion

        #region Host Status (Ping)

        private Thread _pThread;

        private void CheckHostAlive(object hostName)
        {
            if (string.IsNullOrEmpty(hostName as string))
            {
                ShowStatusImage(Properties.Resources.HostStatus_Off);
                return;
            }

            var pingSender = new Ping();

            try
            {
                var pReply = pingSender.Send((string)hostName);
                if (pReply?.Status == IPStatus.Success)
                {
                    if ((string)_btnHostStatus.Tag == "checking")
                        ShowStatusImage(Properties.Resources.HostStatus_On);
                }
                else
                {
                    if ((string)_btnHostStatus.Tag == "checking")
                        ShowStatusImage(Properties.Resources.HostStatus_Off);
                }
            }
            catch (Exception)
            {
                if ((string)_btnHostStatus.Tag == "checking")
                    ShowStatusImage(Properties.Resources.HostStatus_Off);
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
                _btnHostStatus.Image = Properties.Resources.HostStatus_Check;
                // To check status, ConnectionInfo must be an mRemoteNG.Connection.Info that is not a container
                if (!(connectionInfo is ConnectionInfo info)) return;
                if (info.IsContainer) return;

                _btnHostStatus.Tag = "checking";
                _pThread = new Thread(CheckHostAlive);
                _pThread.SetApartmentState(ApartmentState.STA);
                _pThread.IsBackground = true;
                _pThread.Start(((ConnectionInfo)connectionInfo).Hostname);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    Language.ConfigPropertyGridSetHostStatusFailed +
                                                    Environment.NewLine + ex.Message, true);
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
                _propertyGridContextMenuReset.Enabled = Convert.ToBoolean(_pGrid.SelectedObject != null &&
                                                                          gridItem?.PropertyDescriptor != null &&
                                                                          gridItem.PropertyDescriptor.CanResetValue(_pGrid.SelectedObject));
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
                if (_pGrid.SelectedObject != null && gridItem?.PropertyDescriptor != null &&
                    gridItem.PropertyDescriptor.CanResetValue(_pGrid.SelectedObject))
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