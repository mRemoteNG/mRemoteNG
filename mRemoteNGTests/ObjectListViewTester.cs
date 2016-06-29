using System.Windows.Forms;
using BrightIdeasSoftware;

namespace NUnit.Extensions.Forms
{
    public class ObjectListViewTester : ControlTester
    {
        public ObjectListViewTester(string name, string formName) : base(name, formName)
        {
        }

        public ObjectListViewTester(string name, Form form) : base(name, form)
        {
        }

        public ObjectListViewTester(string name) : base(name)
        {
        }

        public ObjectListView Properties => (ObjectListView) Control;

        public void Select(int index)
        {
            Properties.SelectedIndex = index;
        }

        public void Select(OLVListItem selectedOlvListItem)
        {
            Properties.SelectedItem = selectedOlvListItem;
        }

        public void SelectAll()
        {
            Properties.SelectAll();
        }
    }
}