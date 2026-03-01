using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Menu;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Menu.msMain
{
    [TestFixture]
    public class ConnectionsMenuTests
    {
        [Test]
        public void VerifyConnectionsMenuInitialization()
        {
            ConnectionsMenu? menu = null;
            
            var thread = new Thread(() =>
            {
                menu = new ConnectionsMenu();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.That(menu, Is.Not.Null);
            Assert.That(menu!.Name, Is.EqualTo("mMenConnections"));
            Assert.That(menu.Text, Is.EqualTo("Connections"));
        }
    }
}
