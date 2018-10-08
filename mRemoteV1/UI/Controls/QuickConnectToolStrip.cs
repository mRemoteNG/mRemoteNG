using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Themes;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Controls
{
    public class QuickConnectToolStrip : ToolStrip
    {
        private IContainer components;
        private ToolStripLabel _lblQuickConnect;
        private ToolStripDropDownButton _btnConnections;
        private ToolStripSplitButton _btnQuickConnect;
        private ContextMenuStrip _mnuQuickConnectProtocol;
        private QuickConnectComboBox _cmbQuickConnect;
        private ContextMenuStrip _mnuConnections;
        private IConnectionInitiator _connectionInitiator = new ConnectionInitiator();
        private ThemeManager _themeManager;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;

        public IConnectionInitiator ConnectionInitiator
        {
            get { return _connectionInitiator; }
            set
            {
                if (value == null)
                    return;
                _connectionInitiator = value;
            }
        }

        public QuickConnectToolStrip()
        {
            Initialize();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            PopulateQuickConnectProtocolMenu();
            ApplyTheme();
        }

        private void Initialize()
        {
            components = new System.ComponentModel.Container();
            _lblQuickConnect = new ToolStripLabel();
            _cmbQuickConnect = new QuickConnectComboBox();
            _btnQuickConnect = new ToolStripSplitButton();
            _mnuQuickConnectProtocol = new ContextMenuStrip(components);
            _btnConnections = new ToolStripDropDownButton();
            _mnuConnections = new ContextMenuStrip(components);
            SuspendLayout();
            //
            //Theming support
            //
            vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(components);
            // 
            // lblQuickConnect
            // 
            _lblQuickConnect.Name = "lblQuickConnect";
            _lblQuickConnect.Size = new System.Drawing.Size(55, 22);
            _lblQuickConnect.Text = Language.strLabelConnect;
            _lblQuickConnect.Click += lblQuickConnect_Click;
            // 
            // cmbQuickConnect
            // 
            _cmbQuickConnect.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _cmbQuickConnect.AutoCompleteSource = AutoCompleteSource.ListItems;
            _cmbQuickConnect.Margin = new Padding(1, 0, 3, 0);
            _cmbQuickConnect.Name = "cmbQuickConnect";
            _cmbQuickConnect.Size = new System.Drawing.Size(200, 25);
            _cmbQuickConnect.ConnectRequested += cmbQuickConnect_ConnectRequested;
            _cmbQuickConnect.ProtocolChanged += cmbQuickConnect_ProtocolChanged;
            // 
            // tsQuickConnect
            // 
            Dock = DockStyle.None;
            Items.AddRange(new ToolStripItem[] {
            _lblQuickConnect,
            _cmbQuickConnect,
            _btnQuickConnect,
            _btnConnections});
            Location = new System.Drawing.Point(3, 24);
            MaximumSize = new System.Drawing.Size(0, 25);
            Name = "tsQuickConnect";
            Size = new System.Drawing.Size(387, 25);
            TabIndex = 18;
            // 
            // btnQuickConnect
            // 
            _btnQuickConnect.DropDown = _mnuQuickConnectProtocol;
            _btnQuickConnect.Image = Resources.Play_Quick;
            _btnQuickConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnQuickConnect.Margin = new Padding(0, 1, 3, 2);
            _btnQuickConnect.Name = "btnQuickConnect";
            _btnQuickConnect.Size = new System.Drawing.Size(84, 22);
            _btnQuickConnect.Text = Language.strMenuConnect;
            _btnQuickConnect.ButtonClick += btnQuickConnect_ButtonClick;
            _btnQuickConnect.DropDownItemClicked += btnQuickConnect_DropDownItemClicked;
            // 
            // mnuQuickConnectProtocol
            // 
            _mnuQuickConnectProtocol.Name = "mnuQuickConnectProtocol";
            _mnuQuickConnectProtocol.OwnerItem = _btnQuickConnect;
            _mnuQuickConnectProtocol.ShowCheckMargin = true;
            _mnuQuickConnectProtocol.ShowImageMargin = false;
            _mnuQuickConnectProtocol.Size = new System.Drawing.Size(61, 4);
            // 
            // btnConnections
            // 
            _btnConnections.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnConnections.DropDown = _mnuConnections;
            _btnConnections.Image = Resources.Root;
            _btnConnections.ImageScaling = ToolStripItemImageScaling.None;
            _btnConnections.ImageTransparentColor = System.Drawing.Color.Magenta;
            _btnConnections.Name = "btnConnections";
            _btnConnections.Size = new System.Drawing.Size(29, 22);
            _btnConnections.Text = Language.strMenuConnections;
            _btnConnections.DropDownOpening += btnConnections_DropDownOpening;
            // 
            // mnuConnections
            // 
            _mnuConnections.Name = "mnuConnections";
            _mnuConnections.OwnerItem = _btnConnections;
            _mnuConnections.Size = new System.Drawing.Size(61, 4);

            ResumeLayout();
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive) return;
            vsToolStripExtender.SetStyle(_mnuQuickConnectProtocol, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
            vsToolStripExtender.SetStyle(_mnuConnections, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
            _cmbQuickConnect.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            _cmbQuickConnect.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
        }

        #region Quick Connect
        private void PopulateQuickConnectProtocolMenu()
        {
            try
            {
                _mnuQuickConnectProtocol.Items.Clear();
                foreach (var fieldInfo in typeof(ProtocolType).GetFields())
                {
                    if (fieldInfo.Name == "value__" || fieldInfo.Name == "IntApp") continue;
                    var menuItem = new ToolStripMenuItem(fieldInfo.Name);
                    if (fieldInfo.Name == Settings.Default.QuickConnectProtocol)
                    {
                        menuItem.Checked = true;
                        _btnQuickConnect.Text = Settings.Default.QuickConnectProtocol;
                    }
                    _mnuQuickConnectProtocol.Items.Add(menuItem);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("PopulateQuickConnectProtocolMenu() failed.", ex);
            }
        }

        private void lblQuickConnect_Click(object sender, EventArgs e)
        {
            _cmbQuickConnect.Focus();
        }

        private void cmbQuickConnect_ConnectRequested(object sender, QuickConnectComboBox.ConnectRequestedEventArgs e)
        {
            btnQuickConnect_ButtonClick(sender, e);
        }

        private void btnQuickConnect_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var connectionInfo = Runtime.ConnectionsService.CreateQuickConnect(_cmbQuickConnect.Text.Trim(), Converter.StringToProtocol(Settings.Default.QuickConnectProtocol));
                if (connectionInfo == null)
                {
                    _cmbQuickConnect.Focus();
                    return;
                }
                _cmbQuickConnect.Add(connectionInfo);
                ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("btnQuickConnect_ButtonClick() failed.", ex);
            }
        }

        private void cmbQuickConnect_ProtocolChanged(object sender, QuickConnectComboBox.ProtocolChangedEventArgs e)
        {
            SetQuickConnectProtocol(Converter.ProtocolToString(e.Protocol));
        }

        private void btnQuickConnect_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SetQuickConnectProtocol(e.ClickedItem.Text);
            btnQuickConnect_ButtonClick(this, e);
        }

        private void SetQuickConnectProtocol(string protocol)
        {
            Settings.Default.QuickConnectProtocol = protocol;
            _btnQuickConnect.Text = protocol;
            foreach (ToolStripMenuItem menuItem in _mnuQuickConnectProtocol.Items)
            {
                menuItem.Checked = menuItem.Text.Equals(protocol);
            }
        }
        #endregion

        #region Connections DropDown
        private void btnConnections_DropDownOpening(object sender, EventArgs e)
        {
            _btnConnections.DropDownItems.Clear();
            var menuItemsConverter = new ConnectionsTreeToMenuItemsConverter
            {
                MouseUpEventHandler = ConnectionsMenuItem_MouseUp
            };

            // ReSharper disable once CoVariantArrayConversion
            ToolStripItem[] rootMenuItems = menuItemsConverter.CreateToolStripDropDownItems(Runtime.ConnectionsService.ConnectionTreeModel).ToArray();
            _btnConnections.DropDownItems.AddRange(rootMenuItems);

        }

        private void ConnectionsMenuItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var menuItem = (ToolStripMenuItem) sender;

            // While we can connect to a whole folder at once, it is
            // probably not the expected behavior when navigating through
            // a nested menu. Just return
            var containerInfo = menuItem.Tag as ContainerInfo;
            if (containerInfo != null)
                return;

            var connectionInfo = menuItem.Tag as ConnectionInfo;
            if (connectionInfo != null)
            {
                ConnectionInitiator.OpenConnection(connectionInfo);
            }
        }
        #endregion

        // CodeAyalysis doesn't like null propagation
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "components")]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!disposing) return;
                components?.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}