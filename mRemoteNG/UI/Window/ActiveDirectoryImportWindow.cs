using System;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using mRemoteNG.Container;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class ActiveDirectoryImportWindow : BaseWindow
    {
        private string _currentDomain = "";

        public ActiveDirectoryImportWindow()
        {
            WindowType = WindowType.ActiveDirectoryImport;
            DockPnl = new DockContent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Schema_16x);
            InitializeComponent();
            FontOverrider.FontOverride(this);
            ApplyTheme();
            ApplyLanguage();
            txtDomain.Text = Environment.UserDomainName;
            EnableDisableImportButton();
            ChangeDomain();
        }

        #region Private Methods

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive) return;
            base.ApplyTheme();
            var themeManager = ThemeManager.getInstance();
            if (!themeManager.ActiveAndExtended) return;
            var palette = themeManager.ActiveTheme?.ExtendedPalette;
            if (palette == null) return;
            activeDirectoryTree.BackColor = palette.getColor("List_Background");
            activeDirectoryTree.ForeColor = palette.getColor("List_Item_Foreground");
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            Connection.ConnectionInfo? selectedNode = AppWindows.TreeForm?.SelectedNode;
            ContainerInfo? importDestination;
            if (selectedNode != null)
                importDestination = selectedNode as ContainerInfo ?? selectedNode.Parent;
            else
                importDestination = Runtime.ConnectionsService.ConnectionTreeModel?.RootNodes.First();

            var adPath = activeDirectoryTree.AdPath;
            if (string.IsNullOrEmpty(adPath) || importDestination == null) return;
            Import.ImportFromActiveDirectory(adPath, importDestination, chkSubOU.Checked);
        }

        private void TxtDomain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            ChangeDomain();
            e.SuppressKeyPress = true;
        }

        private void BtnChangeDomain_Click(object sender, EventArgs e)
        {
            ChangeDomain();
        }

        private void ActiveDirectoryTree_ADPathChanged(object sender)
        {
            EnableDisableImportButton();
        }

        private void ApplyLanguage()
        {
            btnImport.Text = Language._Import;
            lblDomain.Text = Language.Domain;
            chkSubOU.Text = Language.ImportSubOUs;
            btnChangeDomain.Text = Language.Change;
            btnClose.Text = Language._Close;
        }

        private void ChangeDomain()
        {
            _currentDomain = txtDomain.Text;
            activeDirectoryTree.Domain = _currentDomain;
            activeDirectoryTree.Refresh();
        }

        private void EnableDisableImportButton()
        {
            btnImport.Enabled = !string.IsNullOrEmpty(activeDirectoryTree.AdPath);
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion Private Methods
    }
}