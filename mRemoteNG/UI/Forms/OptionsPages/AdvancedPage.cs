using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class AdvancedPage
    {
        private readonly Dictionary<RootPuttySessionsNodeInfo, int> _puttyRootOriginalIndices = [];

        public AdvancedPage()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            DisplayProperties display = new();
            System.Drawing.Bitmap img = display.ScaleImage(Properties.Resources.PuttyConfig);
            btnLaunchPutty.Image = img;
        }

        #region Public Methods

        public override string PageName
        {
            get => Language.Advanced;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblSeconds.Text = Language.Seconds;
            lblMaximumPuttyWaitTime.Text = Language.PuttyTimeout;
            chkAutomaticReconnect.Text = Language.DisplayReconnectionDialog;
            chkNoReconnect.Text = Language.CheckboxAutomaticReconnect;
            chkLoadBalanceInfoUseUtf8.Text = Language.LoadBalanceInfoUseUtf8;
            lblConfigurePuttySessions.Text = Language.PuttySessionsConfig;
            btnLaunchPutty.Text = Language.ButtonLaunchPutty;
            btnBrowseCustomPuttyPath.Text = Language._Browse;
            chkUseCustomPuttyPath.Text = Language.CheckboxPuttyPath;
            chkShowPuttySessionsInTree.Text = Language.ShowPuttySessionsInTree;
            lblUVNCSCPort.Text = Language.UltraVNCSCListeningPort;
        }

        public override void LoadSettings()
        {
            chkAutomaticReconnect.Checked = Properties.OptionsAdvancedPage.Default.ReconnectOnDisconnect;
            chkNoReconnect.Checked = Properties.OptionsAdvancedPage.Default.NoReconnect;
            chkNoReconnect.Enabled = Properties.OptionsAdvancedPage.Default.ReconnectOnDisconnect;

            chkLoadBalanceInfoUseUtf8.Checked = Properties.OptionsAdvancedPage.Default.RdpLoadBalanceInfoUseUtf8;
            numPuttyWaitTime.Value = Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime;

            chkUseCustomPuttyPath.Checked = Properties.OptionsAdvancedPage.Default.UseCustomPuttyPath;
            txtCustomPuttyPath.Text = Properties.OptionsAdvancedPage.Default.CustomPuttyPath;
            chkShowPuttySessionsInTree.Checked = Properties.OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            SetPuttyLaunchButtonEnabled();

            numUVNCSCPort.Value = Properties.OptionsAdvancedPage.Default.UVNCSCPort;
        }

        public override void SaveSettings()
        {
            Properties.OptionsAdvancedPage.Default.ReconnectOnDisconnect = chkAutomaticReconnect.Checked;
            Properties.OptionsAdvancedPage.Default.NoReconnect = chkNoReconnect.Checked;
            Properties.OptionsAdvancedPage.Default.RdpLoadBalanceInfoUseUtf8 = chkLoadBalanceInfoUseUtf8.Checked;

            bool puttyPathChanged = false;
            if (Properties.OptionsAdvancedPage.Default.CustomPuttyPath != txtCustomPuttyPath.Text)
            {
                puttyPathChanged = true;
                Properties.OptionsAdvancedPage.Default.CustomPuttyPath = txtCustomPuttyPath.Text;
            }

            if (Properties.OptionsAdvancedPage.Default.UseCustomPuttyPath != chkUseCustomPuttyPath.Checked)
            {
                puttyPathChanged = true;
                Properties.OptionsAdvancedPage.Default.UseCustomPuttyPath = chkUseCustomPuttyPath.Checked;
            }

            bool puttySessionsVisibilityChanged = Properties.OptionsAdvancedPage.Default.ShowPuttySessionsInTree != chkShowPuttySessionsInTree.Checked;
            Properties.OptionsAdvancedPage.Default.ShowPuttySessionsInTree = chkShowPuttySessionsInTree.Checked;

            if (puttySessionsVisibilityChanged && !chkShowPuttySessionsInTree.Checked)
            {
                UpdatePuttySessionsVisibility();
            }

            if (puttyPathChanged || puttySessionsVisibilityChanged)
            {
                PuttyBase.PuttyPath = Properties.OptionsAdvancedPage.Default.UseCustomPuttyPath ? Properties.OptionsAdvancedPage.Default.CustomPuttyPath : GeneralAppInfo.PuttyPath;
                PuttySessionsManager.Instance.AddSessions();
            }

            if (puttySessionsVisibilityChanged && chkShowPuttySessionsInTree.Checked)
            {
                UpdatePuttySessionsVisibility();
            }

            Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime = (int)numPuttyWaitTime.Value;
            Properties.OptionsAdvancedPage.Default.UVNCSCPort = (int)numUVNCSCPort.Value;
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        private void chkUseCustomPuttyPath_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
            btnBrowseCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
            SetPuttyLaunchButtonEnabled();
        }

        private void txtCustomPuttyPath_TextChanged(object sender, EventArgs e)
        {
            SetPuttyLaunchButtonEnabled();
        }

        private void btnBrowseCustomPuttyPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new())
            {
                openFileDialog.Filter = $@"{Language.FilterApplication}|*.exe|{Language.FilterAll}|*.*";
                openFileDialog.FileName = Path.GetFileName(GeneralAppInfo.PuttyPath);
                openFileDialog.CheckFileExists = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                txtCustomPuttyPath.Text = openFileDialog.FileName;
                SetPuttyLaunchButtonEnabled();
            }
        }

        private void btnLaunchPutty_Click(object sender, EventArgs e)
        {
            try
            {
                PuttyProcessController puttyProcess = new();
                string fileName = chkUseCustomPuttyPath.Checked ? txtCustomPuttyPath.Text : GeneralAppInfo.PuttyPath;
                puttyProcess.Start(fileName);
                puttyProcess.SetControlText("Button", "&Cancel", "&Close");
                puttyProcess.SetControlVisible("Button", "&Open", false);
                puttyProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.ErrorCouldNotLaunchPutty, Application.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Runtime.MessageCollector.AddExceptionMessage(Language.ErrorCouldNotLaunchPutty, ex);
            }
        }

        #endregion

        private void SetPuttyLaunchButtonEnabled()
        {
            string puttyPath = chkUseCustomPuttyPath.Checked ? txtCustomPuttyPath.Text : GeneralAppInfo.PuttyPath;

            bool exists = false;
            try
            {
                exists = File.Exists(puttyPath);
            }
            catch
            {
                // ignored
            }

            lblConfigurePuttySessions.Enabled = exists;
            btnLaunchPutty.Enabled = exists;
        }

        private void UpdatePuttySessionsVisibility()
        {
            if (Runtime.ConnectionsService.ConnectionTreeModel is not { } connectionTreeModel)
                return;

            if (chkShowPuttySessionsInTree.Checked)
            {
                foreach (RootPuttySessionsNodeInfo puttyRoot in PuttySessionsManager.Instance.RootPuttySessionsNodes)
                {
                    if (connectionTreeModel.RootNodes.Contains(puttyRoot))
                        continue;

                    connectionTreeModel.AddRootNode(puttyRoot);
                }

                List<ContainerInfo> desiredRootOrder = connectionTreeModel.RootNodes
                    .Where(root => root is not RootPuttySessionsNodeInfo)
                    .Cast<ContainerInfo>()
                    .ToList();
                RootPuttySessionsNodeInfo[] visiblePuttyRoots = PuttySessionsManager.Instance.RootPuttySessionsNodes
                    .Where(connectionTreeModel.RootNodes.Contains)
                    .ToArray();
                foreach (RootPuttySessionsNodeInfo puttyRoot in visiblePuttyRoots
                             .Where(root => _puttyRootOriginalIndices.ContainsKey(root))
                             .OrderBy(root => _puttyRootOriginalIndices[root]))
                {
                    int targetIndex = _puttyRootOriginalIndices[puttyRoot];
                    int clampedTargetIndex = targetIndex < 0
                        ? 0
                        : targetIndex > desiredRootOrder.Count
                            ? desiredRootOrder.Count
                            : targetIndex;
                    desiredRootOrder.Insert(clampedTargetIndex, puttyRoot);
                }

                foreach (RootPuttySessionsNodeInfo puttyRoot in visiblePuttyRoots.Where(root => !_puttyRootOriginalIndices.ContainsKey(root)))
                {
                    desiredRootOrder.Add(puttyRoot);
                }

                for (int index = 0; index < desiredRootOrder.Count; index++)
                {
                    connectionTreeModel.MoveRootNode(desiredRootOrder[index], index);
                }
            }
            else
            {
                RootPuttySessionsNodeInfo[] puttyRoots = connectionTreeModel.RootNodes
                    .OfType<RootPuttySessionsNodeInfo>()
                    .ToArray();
                _puttyRootOriginalIndices.Clear();
                foreach (RootPuttySessionsNodeInfo puttyRoot in puttyRoots)
                {
                    _puttyRootOriginalIndices[puttyRoot] = connectionTreeModel.RootNodes.IndexOf(puttyRoot);
                }

                foreach (RootPuttySessionsNodeInfo puttyRoot in puttyRoots)
                {
                    connectionTreeModel.RemoveRootNode(puttyRoot);
                }
            }

            RefreshConnectionTreeRoots();
        }

        private static void RefreshConnectionTreeRoots()
        {
            if (AppWindows.ExistingTreeForm is not { IsDisposed: false } treeForm)
                return;

            if (treeForm.ConnectionTree.IsDisposed)
                return;

            treeForm.ConnectionTree.RefreshVisibleRoots();
        }

        private void chkNoReconnect_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkAutomaticReconnect_CheckedChanged(object sender, EventArgs e)
        {
            chkNoReconnect.Enabled = chkAutomaticReconnect.Checked;
        }

        #endregion

    }
}