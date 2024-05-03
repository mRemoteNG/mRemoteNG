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

        public event AdPathChangedEventHandler AdPathChanged;

        public delegate void AdPathChangedEventHandler(object sender);

        public string AdPath { get; set; }

        public string Domain
        {
            private get => string.IsNullOrEmpty(_domain) == false ? _domain : Environment.UserDomainName;
            set => _domain = value;
        }

        public object SelectedNode { get; internal set; }

        #endregion Public Methods

        #region Private Methods

        private string _domain;

        private void TvActiveDirectory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            try
            {
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
            AdPath = e.Node.Tag.ToString();
            AdPathChangedEventHandler pathChangedEvent = AdPathChanged;
            pathChangedEvent?.Invoke(this);
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
            adhelper.GetChildEntries(tNode.Tag.ToString());
            System.Collections.IDictionaryEnumerator enumerator = adhelper.Children.GetEnumerator();
            tvActiveDirectory.BeginUpdate();
            while (enumerator.MoveNext())
            {
                bool flag1 = false;
                if (enumerator.Key == null) continue;
                TreeNode node1 = new(enumerator.Key.ToString().Substring(3))
                {
                    Tag = RuntimeHelpers.GetObjectValue(enumerator.Value)
                };
                if (!enumerator.Key.ToString().Substring(0, 2).Equals("CN") ||
                    enumerator.Key.ToString().Equals("CN=Computers") ||
                    enumerator.Key.ToString().Equals("CN=Users"))
                    flag1 = true;

                if (flag1)
                {
                    bool flag2 = false;
                    try
                    {
                        foreach (TreeNode node2 in tNode.Nodes)
                        {
                            if (!node2.Text.Equals(node1.Text)) continue;
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

                int imageIndex = GetImageIndex(enumerator.Key.ToString().Substring(0, 2));
                node1.ImageIndex = imageIndex;
                node1.SelectedImageIndex = imageIndex;
            }

            tvActiveDirectory.EndUpdate();
        }

        private static int GetImageIndex(string objType)
        {
            if (objType.Equals("CN"))
                return 2;
            return objType.Equals("OU") ? 1 : 3;
        }

        #endregion Private Methods
    }
}
