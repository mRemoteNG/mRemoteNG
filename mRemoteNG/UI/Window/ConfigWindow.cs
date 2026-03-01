using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
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
using mRemoteNG.UI.Forms;
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
        private System.ComponentModel.Container _components = null!;
        private ToolStripButton _btnShowProperties = null!;
        private ToolStripButton _btnShowDefaultProperties = null!;
        private ToolStripButton _btnShowInheritance = null!;
        private ToolStripButton _btnShowDefaultInheritance = null!;
        private ToolStripButton _btnPresets = null!;
        private ToolStripButton _btnIcon = null!;
        private ToolStripButton _btnHostStatus = null!;
        internal ContextMenuStrip CMenIcons = null!;
        internal ContextMenuStrip PropertyGridContextMenu = null!;
        private ToolStripMenuItem _propertyGridContextMenuShowHelpText = null!;
        private ToolStripMenuItem _propertyGridContextMenuReset = null!;
        private ToolStripSeparator _toolStripSeparator1 = null!;
        private ConnectionInfoPropertyGrid _pGrid = null!;
        private ThemeManager? _themeManager;
        private int _cachedPropertyGridLabelWidth;
        private bool _applySplitterWidthQueued;
        private const int MinPropertyNameColumnWidth = 50;
        private const int MinPropertyValueColumnWidth = 80;
        private const int PropertyNameTextPadding = 24;
        private static readonly BindingFlags NonPublicInstanceBinding = BindingFlags.Instance | BindingFlags.NonPublic;

        private ConnectionInfo? _selectedTreeNode;
        private IEnumerable<ConnectionInfo>? _selectedTreeNodes;

        public ConnectionInfo? SelectedTreeNode
        {
            get => _selectedTreeNode;
            set
            {
                _selectedTreeNode = value;
                SelectedTreeNodes = value != null ? new[] { value } : null;
            }
        }

        public IEnumerable<ConnectionInfo>? SelectedTreeNodes
        {
            get => _selectedTreeNodes;
            set
            {
                _selectedTreeNodes = value;
                _selectedTreeNode = _selectedTreeNodes?.FirstOrDefault();
                _pGrid.SelectedConnectionInfos = value;
                UpdateTopRow();
            }
        }

        private void InitializeComponent()
        {
            _components = new System.ComponentModel.Container();
            Load += Config_Load;
            VisibleChanged += Config_VisibleChanged;
            SystemColorsChanged += Config_SystemColorsChanged;
            _pGrid = new ConnectionInfoPropertyGrid();
            _pGrid.PropertyValueChanged += PGrid_PropertyValueChanged;
            _pGrid.PropertySortChanged += PGrid_PropertySortChanged;
            _pGrid.MouseUp += PGrid_MouseUp;
            _pGrid.Resize += PGrid_Resize;
            PropertyGridContextMenu = new ContextMenuStrip(_components);
            PropertyGridContextMenu.Opening += PropertyGridContextMenu_Opening;
            _propertyGridContextMenuReset = new ToolStripMenuItem();
            _propertyGridContextMenuReset.Click += PropertyGridContextMenuReset_Click;
            _toolStripSeparator1 = new ToolStripSeparator();
            _propertyGridContextMenuShowHelpText = new ToolStripMenuItem();
            _propertyGridContextMenuShowHelpText.Click += PropertyGridContextMenuShowHelpText_Click;
            _propertyGridContextMenuShowHelpText.CheckedChanged += PropertyGridContextMenuShowHelpText_CheckedChanged;
            _btnShowInheritance = new ToolStripButton();
            _btnShowInheritance.Click += BtnShowInheritance_Click;
            _btnShowDefaultInheritance = new ToolStripButton();
            _btnShowDefaultInheritance.Click += BtnShowDefaultInheritance_Click;
            _btnShowProperties = new ToolStripButton();
            _btnShowProperties.Click += BtnShowProperties_Click;
            _btnShowDefaultProperties = new ToolStripButton();
            _btnShowDefaultProperties.Click += BtnShowDefaultProperties_Click;
            _btnPresets = new ToolStripButton();
            _btnPresets.Click += BtnPresets_Click;
            _btnIcon = new ToolStripButton();
            _btnIcon.MouseUp += BtnIcon_Click;
            _btnHostStatus = new ToolStripButton();
            _btnHostStatus.Click += BtnHostStatus_Click;
            CMenIcons = new ContextMenuStrip(_components);
            PropertyGridContextMenu.SuspendLayout();
            SuspendLayout();
            //
            //pGrid
            //
            _pGrid.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                           | AnchorStyles.Left)
                          | AnchorStyles.Right;
            _pGrid.BrowsableProperties = null!;
            _pGrid.ContextMenuStrip = PropertyGridContextMenu;
            _pGrid.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _pGrid.HiddenAttributes = null!;
            _pGrid.HiddenProperties = null!;
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
            //btnPresets
            //
            _btnPresets.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnPresets.Image = Properties.Resources.SchemaObjectProperty_16x;
            _btnPresets.ImageTransparentColor = Color.Magenta;
            _btnPresets.Name = "_btnPresets";
            _btnPresets.Size = new Size(23, 22);
            _btnPresets.Text = @"Presets";
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
        public bool CanShowProperties => SelectedTreeNodes != null && SelectedTreeNodes.Any();

        public bool InheritanceVisible => _btnShowInheritance.Checked;
        public bool CanShowInheritance => SelectedTreeNodes != null && SelectedTreeNodes.Any() &&
                                          SelectedTreeNodes.All(n => n.Parent != null);

        public bool DefaultPropertiesVisible => _btnShowDefaultProperties.Checked;
        public static bool CanShowDefaultProperties => true;

        public bool DefaultInheritanceVisible => _btnShowDefaultInheritance.Checked;
        public static bool CanShowDefaultInheritance => true;

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
            _btnPresets.Text = "Presets";
            _btnIcon.Text = Language.Icon;
            _btnHostStatus.Text = Language.Status;
            Text = Language.Config;
            TabText = Language.Config;
            _propertyGridContextMenuShowHelpText.Text = Language.ShowHelpText;
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            var activeTheme = _themeManager?.ActiveTheme;
            if (activeTheme?.ExtendedPalette == null) return;
            _pGrid.BackColor = activeTheme.ExtendedPalette.getColor("TextBox_Background");
            _pGrid.ForeColor = activeTheme.ExtendedPalette.getColor("TextBox_Foreground");
            _pGrid.ViewBackColor = activeTheme.ExtendedPalette.getColor("List_Item_Background");
            _pGrid.ViewForeColor = activeTheme.ExtendedPalette.getColor("List_Item_Foreground");
            var lineColor = activeTheme.ExtendedPalette.getColor("List_Item_Border");
            _pGrid.LineColor = lineColor == _pGrid.ViewBackColor ? SystemColors.ControlDark : lineColor;
            _pGrid.HelpBackColor = activeTheme.ExtendedPalette.getColor("TextBox_Background");
            _pGrid.HelpForeColor = activeTheme.ExtendedPalette.getColor("TextBox_Foreground");
            _pGrid.CategoryForeColor = activeTheme.ExtendedPalette.getColor("List_Header_Foreground");
            _pGrid.CommandsDisabledLinkColor =
                activeTheme.ExtendedPalette.getColor("List_Item_Disabled_Foreground");
            _pGrid.CommandsBackColor =
                activeTheme.ExtendedPalette.getColor("List_Item_Disabled_Background");
            _pGrid.CommandsForeColor =
                activeTheme.ExtendedPalette.getColor("List_Item_Disabled_Foreground");
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
                UpdatePresetsButton();
                UpdateHostStatusButton();
                UpdateIconButton();
                QueueApplyPropertyGridSplitterWidth();
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

        private void UpdatePresetsButton()
        {
            _btnPresets.Enabled =
                !_pGrid.IsShowingDefaultProperties &&
                !_pGrid.RootNodeSelected &&
                _pGrid.SelectedConnectionInfos?.Any(connection => connection is not RootNodeInfo) == true;
        }

        private void UpdateHostStatusButton()
        {
            _btnHostStatus.Enabled =
                !_pGrid.RootNodeSelected &&
                !_pGrid.IsShowingDefaultProperties &&
                _pGrid.SelectedConnectionInfos?.All(c => !(c is ContainerInfo)) == true;

            SetHostStatus(_pGrid.SelectedObject);
        }

        private void UpdateIconButton()
        {
            _btnIcon.Enabled =
                _pGrid.SelectedConnectionInfos != null &&
                _pGrid.SelectedConnectionInfos.Any() &&
                !_pGrid.IsShowingDefaultProperties &&
                !_pGrid.RootNodeSelected;

            _btnIcon.Image = _btnIcon.Enabled && _pGrid.SelectedConnectionInfo?.Icon != null
                ? ConnectionIcon
                    .FromString(_pGrid.SelectedConnectionInfo.Icon)?
                    .ToBitmap()
                : null;
        }

        private void AddToolStripItems()
        {
            try
            {
                ToolStrip customToolStrip = new();
                customToolStrip.Items.Add(_btnShowProperties);
                customToolStrip.Items.Add(_btnShowInheritance);
                customToolStrip.Items.Add(_btnShowDefaultProperties);
                customToolStrip.Items.Add(_btnShowDefaultInheritance);
                customToolStrip.Items.Add(_btnPresets);
                customToolStrip.Items.Add(_btnHostStatus);
                customToolStrip.Items.Add(_btnIcon);
                customToolStrip.Show();

                ToolStrip propertyGridToolStrip = new();

                ToolStrip? toolStrip = null;
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

                int expectedToolStripItemCount = _originalPropertyGridToolStripItemCount + customToolStrip.Items.Count;
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

        private void Config_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            QueueApplyPropertyGridSplitterWidth();
        }

        private void PGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            CachePropertyGridLabelWidth();
        }

        private void PGrid_Resize(object sender, EventArgs e)
        {
            QueueApplyPropertyGridSplitterWidth();
        }

        private void QueueApplyPropertyGridSplitterWidth()
        {
            if (_applySplitterWidthQueued || !_pGrid.IsHandleCreated || IsDisposed || Disposing)
                return;

            _applySplitterWidthQueued = true;
            try
            {
                BeginInvoke((System.Windows.Forms.MethodInvoker)(() =>
                {
                    _applySplitterWidthQueued = false;
                    TryApplyCachedPropertyGridLabelWidth();
                    EnsureMinimumPropertyGridLabelWidth();
                }));
            }
            catch (ObjectDisposedException)
            {
                _applySplitterWidthQueued = false;
            }
            catch (InvalidOperationException)
            {
                _applySplitterWidthQueued = false;
            }
        }

        private void CachePropertyGridLabelWidth()
        {
            if (!TryGetPropertyGridLabelWidth(_pGrid, out int labelWidth))
                return;

            if (labelWidth <= MinPropertyNameColumnWidth)
                return;

            _cachedPropertyGridLabelWidth = labelWidth;
        }

        private void TryApplyCachedPropertyGridLabelWidth()
        {
            if (_cachedPropertyGridLabelWidth <= 0 || !_pGrid.IsHandleCreated || !_pGrid.Visible)
                return;

            int maxLabelWidth = Math.Max(MinPropertyNameColumnWidth, _pGrid.ClientSize.Width - MinPropertyValueColumnWidth);
            int targetLabelWidth = Math.Min(_cachedPropertyGridLabelWidth, maxLabelWidth);
            if (targetLabelWidth <= MinPropertyNameColumnWidth)
                return;

            TrySetPropertyGridLabelWidth(_pGrid, targetLabelWidth);
        }

        private void EnsureMinimumPropertyGridLabelWidth()
        {
            if (!_pGrid.IsHandleCreated || !_pGrid.Visible || _pGrid.SelectedObject == null)
                return;

            int requiredLabelWidth = CalculateRequiredPropertyGridLabelWidth();
            if (requiredLabelWidth <= MinPropertyNameColumnWidth)
                return;

            int maxLabelWidth = Math.Max(MinPropertyNameColumnWidth, _pGrid.ClientSize.Width - MinPropertyValueColumnWidth);
            int targetLabelWidth = Math.Min(requiredLabelWidth, maxLabelWidth);
            if (targetLabelWidth <= MinPropertyNameColumnWidth)
                return;

            if (TryGetPropertyGridLabelWidth(_pGrid, out int currentLabelWidth) && currentLabelWidth >= targetLabelWidth)
            {
                _cachedPropertyGridLabelWidth = Math.Max(_cachedPropertyGridLabelWidth, currentLabelWidth);
                return;
            }

            if (TrySetPropertyGridLabelWidth(_pGrid, targetLabelWidth))
                _cachedPropertyGridLabelWidth = Math.Max(_cachedPropertyGridLabelWidth, targetLabelWidth);
        }

        private int CalculateRequiredPropertyGridLabelWidth()
        {
            if (_pGrid.SelectedObject == null)
                return 0;

            int requiredLabelWidth = 0;
            PropertyDescriptorCollection objectProperties = TypeDescriptor.GetProperties(_pGrid.SelectedObject);

            foreach (string propertyName in VisibleObjectProperties)
            {
                PropertyDescriptor? descriptor = objectProperties.Find(propertyName, true);
                string? displayName = descriptor?.DisplayName;
                if (string.IsNullOrWhiteSpace(displayName))
                    continue;

                int labelWidth = TextRenderer.MeasureText(displayName, _pGrid.Font).Width + PropertyNameTextPadding;
                if (labelWidth > requiredLabelWidth)
                    requiredLabelWidth = labelWidth;
            }

            return requiredLabelWidth;
        }

        private static bool TryGetPropertyGridLabelWidth(PropertyGrid propertyGrid, out int labelWidth)
        {
            labelWidth = 0;
            if (propertyGrid == null)
                return false;

            try
            {
                object? gridView = typeof(PropertyGrid).GetField("gridView", NonPublicInstanceBinding)?.GetValue(propertyGrid);
                if (gridView == null)
                    return false;

                PropertyInfo? internalLabelWidth = gridView.GetType().GetProperty("InternalLabelWidth", NonPublicInstanceBinding);
                if (internalLabelWidth?.GetValue(gridView) is int widthFromProperty && widthFromProperty > 0)
                {
                    labelWidth = widthFromProperty;
                    return true;
                }

                FieldInfo? labelWidthField = gridView.GetType().GetField("labelWidth", NonPublicInstanceBinding);
                if (labelWidthField?.GetValue(gridView) is int widthFromField && widthFromField > 0)
                {
                    labelWidth = widthFromField;
                    return true;
                }
            }
            catch
            {
                // Ignore reflection differences between runtime versions.
            }

            return false;
        }

        private static bool TrySetPropertyGridLabelWidth(PropertyGrid propertyGrid, int labelWidth)
        {
            if (propertyGrid == null || labelWidth <= 0)
                return false;

            try
            {
                object? gridView = typeof(PropertyGrid).GetField("gridView", NonPublicInstanceBinding)?.GetValue(propertyGrid);
                if (gridView == null)
                    return false;

                MethodInfo? moveSplitterTo = gridView.GetType().GetMethod("MoveSplitterTo", NonPublicInstanceBinding);
                if (moveSplitterTo != null)
                {
                    moveSplitterTo.Invoke(gridView, [labelWidth]);
                    return true;
                }

                PropertyInfo? internalLabelWidth = gridView.GetType().GetProperty("InternalLabelWidth", NonPublicInstanceBinding);
                if (internalLabelWidth is { CanWrite: true })
                {
                    internalLabelWidth.SetValue(gridView, labelWidth);
                    propertyGrid.Invalidate();
                    return true;
                }
            }
            catch
            {
                // Ignore reflection differences between runtime versions.
            }

            return false;
        }

        private void Config_Load(object sender, EventArgs e)
        {
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            ApplyTheme();
            AddToolStripItems();
            _pGrid.HelpVisible = Settings.Default.ShowConfigHelpText;
            CachePropertyGridLabelWidth();
            QueueApplyPropertyGridSplitterWidth();
        }

        private void Config_SystemColorsChanged(object sender, EventArgs e)
        {
            AddToolStripItems();
        }

        private void PGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                if (e.ChangedItem?.Label == Language.Icon)
                {
                    Icon? conIcon = _pGrid.SelectedConnectionInfo != null
                        ? ConnectionIcon.FromString(_pGrid.SelectedConnectionInfo.Icon)
                        : null;
                    if (conIcon != null)
                        _btnIcon.Image = conIcon.ToBitmap();
                }
                else if (e.ChangedItem?.Label == Language.HostnameIp)
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

        private void PGrid_PropertySortChanged(object sender, EventArgs e)
        {
            if (_pGrid.PropertySort == PropertySort.CategorizedAlphabetical)
                _pGrid.PropertySort = PropertySort.Categorized;
        }

        private void BtnShowProperties_Click(object sender, EventArgs e)
        {
            ShowConnectionProperties();
        }

        private void BtnShowInheritance_Click(object sender, EventArgs e)
        {
            ShowInheritanceProperties();
        }

        private void BtnShowDefaultProperties_Click(object sender, EventArgs e)
        {
            ShowDefaultConnectionProperties();
        }

        private void BtnShowDefaultInheritance_Click(object sender, EventArgs e)
        {
            ShowDefaultInheritanceProperties();
        }

        private void BtnPresets_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionInfo[] selectedConnections = _pGrid.SelectedConnectionInfos?
                    .Where(connection => connection is not RootNodeInfo)
                    .ToArray() ?? Array.Empty<ConnectionInfo>();

                if (selectedConnections.Length == 0)
                    return;

                using FrmConnectionPresets presetsForm = new(Runtime.ConnectionPresetService, selectedConnections);
                presetsForm.ShowDialog(this);

                if (!presetsForm.PresetApplied)
                    return;

                SelectedTreeNodes = selectedConnections;
                _pGrid.Refresh();
                UpdateTopRow();
                SetHostStatus(_pGrid.SelectedConnectionInfo);
                Runtime.ConnectionsService.SaveConnectionsAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Opening connection presets dialog failed.", ex);
            }
        }

        private void BtnHostStatus_Click(object sender, EventArgs e)
        {
            SetHostStatus(_pGrid.SelectedObject);
        }

        private void BtnIcon_Click(object sender, MouseEventArgs e)
        {
            try
            {
                if (_pGrid.SelectedConnectionInfos == null || !_pGrid.SelectedConnectionInfos.Any()) return;
                if (_pGrid.SelectedConnectionInfos.Any(c => c is PuttySessionInfo)) return;
                
                CMenIcons.Items.Clear();

                foreach (string iStr in ConnectionIcon.Icons)
                {
                    ToolStripMenuItem tI = new()
                    {
                        Text = iStr,
                        Image = ConnectionIcon.FromString(iStr)?.ToBitmap()
                    };
                    tI.Click += IconMenu_Click;

                    CMenIcons.Items.Add(tI);
                }

                Point mPos = new(new Size(PointToScreen(new Point(e.Location.X + _pGrid.Width - 100, e.Location.Y))));
                CMenIcons.Show(mPos);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ConfigPropertyGridButtonIconClickFailed + Environment.NewLine + ex.Message, true);
            }
        }

        private void IconMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (_pGrid.SelectedObjects == null && _pGrid.SelectedObject == null) return;

                if (sender is not ToolStripMenuItem selectedMenuItem) return;

                string? iconName = selectedMenuItem.Text;
                if (string.IsNullOrEmpty(iconName)) return;

                Icon? connectionIcon = ConnectionIcon.FromString(iconName);
                if (connectionIcon == null) return;

                _btnIcon.Image = connectionIcon.ToBitmap();

                // Update ALL selected objects if they are ConnectionInfo
                var objects = _pGrid.SelectedObjects ?? new[] { _pGrid.SelectedObject };
                if (objects == null) return;
                
                foreach (var obj in objects)
                {
                     if (obj is ConnectionInfo info)
                     {
                         info.Icon = iconName;
                     }
                }
                
                _pGrid.Refresh();

                Runtime.ConnectionsService.SaveConnectionsAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ConfigPropertyGridMenuClickFailed + Environment.NewLine + ex.Message, true);
            }
        }

        #endregion

        #region Host Status

        private Thread? _pThread;
        private const int HostStatusCheckTimeoutMilliseconds = 1000;

        private static bool IsHostReachable(string hostName, int port, int timeoutMilliseconds)
        {
            if (string.IsNullOrWhiteSpace(hostName) || port <= 0)
                return false;

            try
            {
                using TcpClient tcpClient = new();
                var connectTask = tcpClient.ConnectAsync(hostName, port);
                return connectTask.Wait(timeoutMilliseconds) && tcpClient.Connected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CheckHostAlive(object connectionInfo)
        {
            if (connectionInfo is not ConnectionInfo info)
            {
                ShowStatusImage(Properties.Resources.HostStatus_Off);
                return;
            }

            int portToCheck = info.Port == 0 ? info.GetDefaultPort() : info.Port;
            bool isHostReachable = IsHostReachable(
                info.Hostname,
                portToCheck,
                HostStatusCheckTimeoutMilliseconds);

            if (string.Equals(_btnHostStatus.Tag as string, "checking", StringComparison.Ordinal))
            {
                ShowStatusImage(
                    isHostReachable
                        ? Properties.Resources.HostStatus_On
                        : Properties.Resources.HostStatus_Off);
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

        private void SetHostStatus(object? connectionInfo)
        {
            try
            {
                _btnHostStatus.Image = Properties.Resources.HostStatus_Check;
                // To check status, ConnectionInfo must be an mRemoteNG.Connection.Info that is not a container
                if (connectionInfo is not ConnectionInfo info) return;
                if (info.IsContainer) return;

                _btnHostStatus.Tag = "checking";
                _pThread = new Thread(CheckHostAlive);
                _pThread.SetApartmentState(ApartmentState.STA);
                _pThread.IsBackground = true;
                _pThread.Start(info);
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

        private void PropertyGridContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _propertyGridContextMenuShowHelpText.Checked = Settings.Default.ShowConfigHelpText;
                GridItem? gridItem = _pGrid.SelectedGridItem;
                _propertyGridContextMenuReset.Enabled = Convert.ToBoolean(_pGrid.SelectedObject != null &&
                                                                          gridItem?.PropertyDescriptor != null &&
                                                                          gridItem.PropertyDescriptor.CanResetValue(_pGrid.SelectedObject));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.Config.propertyGridContextMenu_Opening() failed.", ex);
            }
        }

        private void PropertyGridContextMenuReset_Click(object sender, EventArgs e)
        {
            try
            {
                GridItem? gridItem = _pGrid.SelectedGridItem;
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

        private void PropertyGridContextMenuShowHelpText_Click(object sender, EventArgs e)
        {
            _propertyGridContextMenuShowHelpText.Checked = !_propertyGridContextMenuShowHelpText.Checked;
        }

        private void PropertyGridContextMenuShowHelpText_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ShowConfigHelpText = _propertyGridContextMenuShowHelpText.Checked;
            _pGrid.HelpVisible = _propertyGridContextMenuShowHelpText.Checked;
        }

        #endregion
    }
}
