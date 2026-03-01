using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Controls;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionContextMenuConnectWithCredentialsTests
    {
        [Test]
        public void VerifyConnectWithCredentialsMenuItemExists()
        {
            ConnectionContextMenu? menu = null;
            
            var thread = new Thread(() =>
            {
                // Passing null for connectionTree might be okay for initialization
                // as long as we don't open the menu.
                menu = new ConnectionContextMenu(null!);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.That(menu, Is.Not.Null);
            var items = menu!.Items.Cast<ToolStripItem>().ToList();
            
            // Look for ConnectWithOptions menu item
            var connectWithOptionsItem = items.FirstOrDefault(i => string.Equals(i.Name, "_cMenTreeConnectWithOptions", StringComparison.Ordinal)) as ToolStripMenuItem;
            Assert.That(connectWithOptionsItem, Is.Not.Null, "ConnectWithOptions menu item not found");
            Assert.That(connectWithOptionsItem!.DropDownItems, Is.Not.Empty, "ConnectWithOptions has no dropdown items");
            
            var dropdownItems = connectWithOptionsItem.DropDownItems.Cast<ToolStripItem>().ToList();
            var credentialsItem = dropdownItems.FirstOrDefault(i => string.Equals(i.Name, "_cMenTreeConnectWithOptionsWithCredentials", StringComparison.Ordinal));
            Assert.That(credentialsItem, Is.Not.Null, "Connect with credentials menu item not found");
            Assert.That(credentialsItem!.Text, Is.EqualTo("Connect with credentials..."));
        }
    }
}
