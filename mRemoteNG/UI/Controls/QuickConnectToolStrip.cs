using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class QuickConnectToolStrip : ToolStrip
    {
        private IContainer components;
        private ToolStripLabel _lblQuickConnect;
        private ToolStripDropDownButton _btnConnections;
        private MrngToolStripSplitButton _btnQuickConnect;
        private ContextMenuStrip _mnuQuickConnectProtocol;
        private QuickConnectComboBox _cmbQuickConnect;
        private ContextMenuStrip _mnuConnections;
        private readonly ThemeManager _themeManager;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private readonly DisplayProperties _display;


        public QuickConnectToolStrip()
        {
            _display = new DisplayProperties();
            Initialize();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            PopulateQuickConnectProtocolMenu();
            ApplyTheme();
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            _lblQuickConnect.Text = Language.QuickConnect;
        }

        private void Initialize()
        {
            components = new System.ComponentModel.Container();
            _lblQuickConnect = new ToolStripLabel();
            _cmbQuickConnect = new QuickConnectComboBox();
            _btnQuickConnect = new MrngToolStripSplitButton();
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
            _lblQuickConnect.Size = new Size(55, 22);
            _lblQuickConnect.Text = Language.Connect;
            _lblQuickConnect.Click += lblQuickConnect_Click;
            // 
            // cmbQuickConnect
            // 
            _cmbQuickConnect.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _cmbQuickConnect.AutoCompleteSource = AutoCompleteSource.ListItems;
            _cmbQuickConnect.Margin = new Padding(1, 0, 3, 0);
            _cmbQuickConnect.Name = "cmbQuickConnect";
            _cmbQuickConnect.Size = new Size(_display.ScaleWidth(200), 25);
            _cmbQuickConnect.ConnectRequested += cmbQuickConnect_ConnectRequested;
            _cmbQuickConnect.ProtocolChanged += cmbQuickConnect_ProtocolChanged;
            // 
            // tsQuickConnect
            // 
            Dock = DockStyle.None;
            Items.AddRange(new ToolStripItem[]
            {
                _lblQuickConnect,
                _cmbQuickConnect,
                _btnQuickConnect,
                _btnConnections
            });
            Location = new Point(3, 24);
            Name = "tsQuickConnect";
            Size = new Size(_display.ScaleWidth(387), 25);
            TabIndex = 18;
            // 
            // btnQuickConnect
            // 
            _btnQuickConnect.DropDown = _mnuQuickConnectProtocol;
            _btnQuickConnect.Image = Properties.Resources.Run_16x;
            _btnQuickConnect.ImageTransparentColor = Color.Magenta;
            _btnQuickConnect.Margin = new Padding(0, 1, 3, 2);
            _btnQuickConnect.Name = "btnQuickConnect";
            _btnQuickConnect.Size = new Size(84, 22);
            _btnQuickConnect.Text = Language.Connect;
            _btnQuickConnect.ButtonClick += btnQuickConnect_ButtonClick;
            _btnQuickConnect.DropDownItemClicked += btnQuickConnect_DropDownItemClicked;
            // 
            // mnuQuickConnectProtocol
            // 
            _mnuQuickConnectProtocol.Name = "mnuQuickConnectProtocol";
            _mnuQuickConnectProtocol.OwnerItem = _btnQuickConnect;
            _mnuQuickConnectProtocol.ShowCheckMargin = true;
            _mnuQuickConnectProtocol.ShowImageMargin = false;
            _mnuQuickConnectProtocol.Size = new Size(61, 4);
            // 
            // btnConnections
            // 
            _btnConnections.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnConnections.DropDown = _mnuConnections;
            _btnConnections.Image = Properties.Resources.ASPWebSite_16x;
            _btnConnections.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            _btnConnections.ImageTransparentColor = Color.Magenta;
            _btnConnections.Name = "btnConnections";
            _btnConnections.Size = new Size(29, 22);
            _btnConnections.Text = Language.Connections;
            _btnConnections.DropDownOpening += btnConnections_DropDownOpening;
            // 
            // mnuConnections
            // 
            _mnuConnections.Name = "mnuConnections";
            _mnuConnections.OwnerItem = _btnConnections;
            _mnuConnections.Size = new Size(61, 4);

            ResumeLayout();
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive) return;
            vsToolStripExtender.SetStyle(_mnuQuickConnectProtocol, _themeManager.ActiveTheme.Version,
                                         _themeManager.ActiveTheme.Theme);
            vsToolStripExtender.SetStyle(_mnuConnections, _themeManager.ActiveTheme.Version,
                                         _themeManager.ActiveTheme.Theme);

            if (!_themeManager.ActiveAndExtended) return;
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
                var connectionInfo = Runtime.ConnectionsService.CreateQuickConnect(_cmbQuickConnect.Text.Trim(),
                                                                                   Converter.StringToProtocol(Settings
                                                                                                              .Default
                                                                                                              .QuickConnectProtocol));
                if (connectionInfo == null)
                {
                    _cmbQuickConnect.Focus();
                    return;
                }

                _cmbQuickConnect.Add(connectionInfo);
                Runtime.ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
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
            if (string.IsNullOrEmpty(_cmbQuickConnect.Text))
                _cmbQuickConnect.Focus();
            else
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
            ToolStripItem[] rootMenuItems = menuItemsConverter
                                            .CreateToolStripDropDownItems(Runtime.ConnectionsService
                                                                                 .ConnectionTreeModel).ToArray();
            _btnConnections.DropDownItems.AddRange(rootMenuItems);

            ToolStripMenuItem favorites = new ToolStripMenuItem(Language.Favorites, Properties.Resources.Favorite_16x);
            var rootNodes = Runtime.ConnectionsService.ConnectionTreeModel.RootNodes;
            List<ToolStripMenuItem> favoritesList = new List<ToolStripMenuItem>();

            foreach (var node in rootNodes)
            {
                foreach (var containerInfo in Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveFavoriteChildList(node))
                {
                    var favoriteMenuItem = new ToolStripMenuItem
                    {
                        Text = containerInfo.Name,
                        Tag = containerInfo,
                        Image = containerInfo.OpenConnections.Count > 0 ? Properties.Resources.Run_16x : Properties.Resources.Stop_16x
                    };
                    favoriteMenuItem.MouseUp += ConnectionsMenuItem_MouseUp;
                    favoritesList.Add(favoriteMenuItem);
                }
            }
            favorites.DropDownItems.AddRange(favoritesList.ToArray());
            _btnConnections.DropDownItems.Add(favorites);
        }

        private void ConnectionsMenuItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var menuItem = (ToolStripMenuItem)sender;

            switch (menuItem.Tag)
            {
                // While we can connect to a whole folder at once, it is
                // probably not the expected behavior when navigating through
                // a nested menu. Just return
                case ContainerInfo _:
                    return;
                case ConnectionInfo connectionInfo:
                    Runtime.ConnectionInitiator.OpenConnection(connectionInfo);
                    break;
            }
        }

        #endregion

        // CodeAyalysis doesn't like null propagation
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
            MessageId = "components")]
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