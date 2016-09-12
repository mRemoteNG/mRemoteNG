using System;
using System.Collections;
using System.Windows.Forms;

namespace mRemoteNG.Tools.Sorting
{
    public class TreeNodeSorter : IComparer
    {
        public SortOrder Sorting { get; set; }

        public TreeNodeSorter(SortOrder sortOrder = SortOrder.None)
        {
            Sorting = sortOrder;
        }

        public int Compare(object x, object y)
        {
            TreeNode tx = (TreeNode)x;
            TreeNode ty = (TreeNode)y;

            switch (Sorting)
            {
                case SortOrder.Ascending:
                    return String.CompareOrdinal(tx.Text, ty.Text);
                case SortOrder.Descending:
                    return String.CompareOrdinal(ty.Text, tx.Text);
                default:
                    return 0;
            }
        }
    }
}