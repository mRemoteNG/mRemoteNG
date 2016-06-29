using System.Collections;
using System.Linq;
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

        public int GetItemCount()
        {
            return Properties.GetItemCount();
        }

        public bool Contains(object targetObject)
        {
            return Properties.Items.Cast<object>().Contains(targetObject);
        }

        public bool Contains(IEnumerable targetObject)
        {
            return Properties.Items.Cast<object>().Contains(targetObject);
        }
    }
}