using mRemoteNG.Tools;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class MrngAdTree : UserControl
    {
        #region Public Methods

        public MrngAdTree()
        {
            InitializeComponent();
        }

        public event AdPathChangedEventHandler? AdPathChanged;

        public delegate void AdPathChangedEventHandler(object sender);

        public string? AdPath { get; set; }

        public string Domain
        {
            private get => string.IsNullOrEmpty(_domain) == false ? _domain : Environment.UserDomainName;
            set => _domain = value;
        }

        public object? SelectedNode { get; internal set; }

        #endregion Public Methods

        #region Private Methods

        private string? _domain;

        private void TvActiveDirectory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node is null) return;
                foreach (TreeNode node in e.Node.Nodes)
                    AddTreeNodes(node);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void TvActiveDirectory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            AdPath = e.Node?.Tag?.ToString();
            AdPathChanged?.Invoke(this);
        }

        private void AdTree_Load(object sender, EventArgs e)
        {
            tvActiveDirectory.Nodes.Clear();
            TreeNode treeNode = new(Domain) { Tag = "" };
            tvActiveDirectory.Nodes.Add(treeNode);
            AddTreeNodes(treeNode);
            tvActiveDirectory.Nodes[0].Expand();
        }

        private void AddTreeNodes(TreeNode tNode)
        {
            AdHelper adhelper = new(Domain);
            adhelper.GetChildEntries(tNode.Tag?.ToString() ?? "");
            System.Collections.IDictionaryEnumerator enumerator = adhelper.Children.GetEnumerator();
            tvActiveDirectory.BeginUpdate();
            while (enumerator.MoveNext())
            {
                bool flag1 = false;
                if (enumerator.Key == null) continue;
                string keyStr = enumerator.Key.ToString() ?? "";
                TreeNode node1 = new(keyStr.Substring(3))
                {
                    Tag = RuntimeHelpers.GetObjectValue(enumerator.Value)
                };
                if (!keyStr.Substring(0, 2).Equals("CN", StringComparison.Ordinal) ||
                    keyStr.Equals("CN=Computers", StringComparison.Ordinal) ||
                    keyStr.Equals("CN=Users", StringComparison.Ordinal))
                    flag1 = true;

                if (flag1)
                {
                    bool flag2 = false;
                    try
                    {
                        foreach (TreeNode node2 in tNode.Nodes)
                        {
                            if (!node2.Text.Equals(node1.Text, StringComparison.Ordinal)) continue;
                            flag2 = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }

                    if (!flag2)
                        tNode.Nodes.Add(node1);
                }

                int imageIndex = GetImageIndex(keyStr.Substring(0, 2));
                node1.ImageIndex = imageIndex;
                node1.SelectedImageIndex = imageIndex;
            }

            tvActiveDirectory.EndUpdate();
        }

        private static int GetImageIndex(string objType)
        {
            if (objType.Equals("CN", StringComparison.Ordinal))
                return 2;
            return objType.Equals("OU", StringComparison.Ordinal) ? 1 : 3;
        }

        #endregion Private Methods
    }
}
