using System.Threading;
using mRemoteNG.UI.Forms;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms
{
    public class FrmMainTests
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        public void CanCreateFrmMain()
        {
            var frmMain = FrmMain.Default;
        }
    }
}
