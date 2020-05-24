using mRemoteNG.Tools;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    public partial class AdTree : UserControl
    {
        public AdTree()
        {
            InitializeComponent();
        }

        private void TvAD_AfterExpand(object sender, TreeViewEventArgs e)
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

        private void TvAD_AfterSelect(object sender, TreeViewEventArgs e)
        {
            AdPath = e.Node.Tag.ToString();
            var pathChangedEvent = AdPathChanged;
            pathChangedEvent?.Invoke(this);
        }

        public event AdPathChangedEventHandler AdPathChanged;

        public delegate void AdPathChangedEventHandler(object sender);

        private string _domain;

        public string AdPath { get; set; }

        public string Domain
        {
            private get => string.IsNullOrEmpty(_domain) == false ? _domain : Environment.UserDomainName;
            set => _domain = value;
        }
        public BorderStyle BorderStyle { get; internal set; }
        public object SelectedNode { get; internal set; }

        private void ADtree_Load(object sender, EventArgs e)
        {
            TvAd.Nodes.Clear();
            var treeNode = new TreeNode(Domain) { Tag = "" };
            TvAd.Nodes.Add(treeNode);
            AddTreeNodes(treeNode);
            TvAd.Nodes[0].Expand();
        }

        private void AddTreeNodes(TreeNode tNode)
        {
            var adhelper = new ADhelper(Domain);
            adhelper.GetChildEntries(tNode.Tag.ToString());
            var enumerator = adhelper.Children.GetEnumerator();
            TvAd.BeginUpdate();
            while (enumerator.MoveNext())
            {
                var flag1 = false;
                if (enumerator.Key == null) continue;
                var node1 = new TreeNode(enumerator.Key.ToString().Substring(3))
                {
                    Tag = RuntimeHelpers.GetObjectValue(enumerator.Value)
                };
                if (!enumerator.Key.ToString().Substring(0, 2).Equals("CN") ||
                    enumerator.Key.ToString().Equals("CN=Computers") ||
                    enumerator.Key.ToString().Equals("CN=Users"))
                    flag1 = true;

                if (flag1)
                {
                    var flag2 = false;
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

                var imageIndex = GetImageIndex(enumerator.Key.ToString().Substring(0, 2));
                node1.ImageIndex = imageIndex;
                node1.SelectedImageIndex = imageIndex;
            }

            TvAd.EndUpdate();
        }

        private static int GetImageIndex(string objType)
        {
            if (objType.Equals("CN"))
                return 2;
            return objType.Equals("OU") ? 1 : 3;
        }
    }
}
