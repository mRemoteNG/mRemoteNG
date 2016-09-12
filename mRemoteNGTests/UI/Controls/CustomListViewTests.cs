using NUnit.Framework;
using ListView = mRemoteNG.UI.Controls.ListView;

namespace mRemoteNGTests.UI.Controls
{
    public class CustomListViewTests
    {
        ListView _listView;

        [SetUp]
        public void Setup()
        {
            _listView = new ListView
            {
                Name = "myTestListView",
                View = System.Windows.Forms.View.Tile
            };
        }

        [TearDown]
        public void Teardown()
        {
            _listView.Dispose();
            while (_listView.Disposing)
            {
            }
            _listView = null;
        }
    }
}