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
    }
}