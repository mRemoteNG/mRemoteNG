using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mRemoteNG.Controls;
using NUnit.Framework;
using NUnit.Extensions.Forms;
using System.Windows.Forms;

namespace mRemoteNGTests.UI.Controls
{
    public class CustomListViewTests
    {
        TestForm _testForm;
        mRemoteNG.Controls.ListView _listView;

        [SetUp]
        public void Setup()
        {
            _listView = new mRemoteNG.Controls.ListView();
            _listView.Name = "myTestListView";
            _listView.View = System.Windows.Forms.View.Tile;
        }

        [TearDown]
        public void Teardown()
        {
            _listView.Dispose();
            while (_listView.Disposing) ;
            _listView = null;
        }

        [Test]
        public void a()
        {
            _testForm = new TestForm();
            _testForm.Controls.Add(_listView);
            _listView.Items.Add(new ListViewItem("mytestitem"));
            _testForm.Show();
            ListViewTester listviewTester = new ListViewTester("myTestListView", _testForm);
            while (true) ;
            Assert.That(listviewTester.Items.Count, Is.EqualTo(1));

        }
    }
}