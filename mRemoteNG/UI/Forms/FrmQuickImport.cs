using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection;
using mRemoteNG.Container;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmQuickImport : Form
    {
        public FrmQuickImport()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                var input = txtInput.Text;
                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }

                var destination = GetDestinationContainer();
                if (destination == null)
                {
                    MessageBox.Show("Could not determine destination folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var importer = new TextImporter();
                // We need to capture the added connections if we want to connect to them.
                // The TextImporter adds directly to the container.
                // So we can check the container children count before and after, or modify TextImporter to return added items.
                // For now, let's just count on the fact that they are added at the end or use a temporary container?
                // No, TextImporter adds to the destination.
                
                // Better approach: TextImporter adds them. We can iterate over the added items if we knew them.
                // Let's modify TextImporter to return the list of added connections, but I already wrote it to return void (interface).
                // I can just parse them here locally using the same logic or cast the importer to something else?
                // Or I can just check the last N children of the destination container.
                
                // Actually, I'll just use the TextImporter and then if "Connect Immediately" is checked, I need to know which ones.
                // Maybe I should subclass TextImporter or just put the logic here since it's "Quick Import" specific.
                // But keeping it in TextImporter is cleaner.
                
                // Let's count children before.
                int childrenCountBefore = destination.Children.Count;
                
                importer.Import(input, destination);
                
                int childrenCountAfter = destination.Children.Count;
                
                if (chkConnectImmediate.Checked)
                {
                    // Connect to the newly added connections
                    for (int i = childrenCountBefore; i < childrenCountAfter; i++)
                    {
                        var newConn = destination.Children[i];
                        Runtime.ConnectionInitiator.OpenConnection(newConn);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private static ContainerInfo? GetDestinationContainer()
        {
            var selectedNode = AppWindows.TreeForm?.SelectedNode;

            if (selectedNode == null)
            {
                return Runtime.ConnectionsService.ConnectionTreeModel?.RootNodes.FirstOrDefault();
            }

            if (selectedNode is ContainerInfo container)
            {
                return container;
            }

            return selectedNode.Parent ?? Runtime.ConnectionsService.ConnectionTreeModel?.RootNodes.FirstOrDefault();
        }
    }
}
