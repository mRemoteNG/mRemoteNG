using System;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class ExternalToolsToolStrip : ToolStrip
    {
        private IContainer components;
        private ContextMenuStrip _cMenExtAppsToolbar;
        internal ToolStripMenuItem CMenToolbarShowText;

        public ExternalToolsToolStrip()
        {
            Initialize();
            Runtime.ExternalToolsService.ExternalTools.CollectionUpdated += (sender, args) => AddExternalToolsToToolBar();
        }

        private void Initialize()
        {
            components = new System.ComponentModel.Container();
            _cMenExtAppsToolbar = new ContextMenuStrip(components);
            CMenToolbarShowText = new ToolStripMenuItem();

            // 
            // tsExternalTools
            // 
            ContextMenuStrip = _cMenExtAppsToolbar;
            Dock = DockStyle.None;
            Location = new System.Drawing.Point(39, 49);
            Name = "tsExternalTools";
            Size = new System.Drawing.Size(111, 25);
            TabIndex = 17;
            // 
            // cMenExtAppsToolbar
            // 
            _cMenExtAppsToolbar.Items.Add(CMenToolbarShowText);
            _cMenExtAppsToolbar.Name = "cMenToolbar";
            _cMenExtAppsToolbar.Size = new System.Drawing.Size(129, 26);
            // 
            // cMenToolbarShowText
            // 
            CMenToolbarShowText.Checked = true;
            CMenToolbarShowText.CheckState = CheckState.Checked;
            CMenToolbarShowText.Name = "cMenToolbarShowText";
            CMenToolbarShowText.Size = new System.Drawing.Size(128, 22);
            CMenToolbarShowText.Text = Language.ShowText;
            CMenToolbarShowText.Click += cMenToolbarShowText_Click;
        }

        #region Ext Apps Toolbar

        private void cMenToolbarShowText_Click(object sender, EventArgs e)
        {
            SwitchToolBarText(!CMenToolbarShowText.Checked);
        }

        public void AddExternalToolsToToolBar()
        {
            try
            {
                SuspendLayout();

                for (var index = Items.Count - 1; index >= 0; index--)
                    Items[index].Dispose();
                Items.Clear();

                foreach (var tool in Runtime.ExternalToolsService.ExternalTools)
                {
                    if (!tool.ShowOnToolbar) continue;
                    var button = (ToolStripButton)Items.Add(tool.DisplayName, tool.Image, tsExtAppEntry_Click);
                    if (CMenToolbarShowText.Checked)
                        button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    else
                        button.DisplayStyle = button.Image != null
                            ? ToolStripItemDisplayStyle.Image
                            : ToolStripItemDisplayStyle.ImageAndText;

                    button.Tag = tool;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ErrorAddExternalToolsToToolBarFailed, ex);
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        private static void tsExtAppEntry_Click(object sender, EventArgs e)
        {
            var extA = (ExternalTool)((ToolStripButton)sender).Tag;

            var selectedTreeNode = Windows.TreeForm.SelectedNode;
            if (selectedTreeNode != null && selectedTreeNode.GetTreeNodeType() == TreeNodeType.Connection ||
                selectedTreeNode.GetTreeNodeType() == TreeNodeType.PuttySession)
                extA.Start(selectedTreeNode);
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "No connection was selected, external tool may return errors.", true);
                extA.Start();
            }
        }

        public void SwitchToolBarText(bool show)
        {
            foreach (ToolStripButton tItem in Items)
            {
                if (show)
                    tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                else
                    tItem.DisplayStyle = tItem.Image != null
                        ? ToolStripItemDisplayStyle.Image
                        : ToolStripItemDisplayStyle.ImageAndText;
            }

            CMenToolbarShowText.Checked = show;
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