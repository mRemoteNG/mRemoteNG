using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Menu;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Menu.msMain
{
    [TestFixture]
    public class FileMenuTests
    {
        [Test]
        public void VerifyNewConnectionMenuItemExists()
        {
            FileMenu? menu = null;
            
            // Run on STA thread just in case
            var thread = new Thread(() =>
            {
                menu = new FileMenu();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.That(menu, Is.Not.Null);
            
            // Find the item
            var items = menu!.DropDownItems.OfType<ToolStripItem>().ToList();
            var newConnectionItem = items.FirstOrDefault(i => string.Equals(i.Name, "mMenNewConnection", StringComparison.Ordinal));

            Assert.That(newConnectionItem, Is.Not.Null, "New Connection menu item should exist");
            Assert.That(newConnectionItem!.Text, Is.EqualTo("New Connection"), "Menu item text should match 'New Connection'");
            
            // Verify position: should be first
            Assert.That(items[0], Is.EqualTo(newConnectionItem), "New Connection menu item should be the first item in the menu");
        }
    }
}
