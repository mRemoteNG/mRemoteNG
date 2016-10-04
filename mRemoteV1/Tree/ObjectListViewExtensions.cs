using System.Windows.Forms;
using BrightIdeasSoftware;


namespace mRemoteNG.Tree
{
    public static class ObjectListViewExtensions
    {
        public static void Invoke(this Control control, MethodInvoker action)
        {
            control.Invoke(action);
        }

        public static void InvokeExpand(this TreeListView control, object model)
        {
            control.Invoke(() => control.Expand(model));
        }

        public static void InvokeRebuildAll(this TreeListView control, bool preserveState)
        {
            control.Invoke(() => control.RebuildAll(preserveState));
        }
    }
}