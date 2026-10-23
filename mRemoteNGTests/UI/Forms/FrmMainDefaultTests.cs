using System;
using System.Threading;
using mRemoteNG.UI.Forms;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms
{
    [Apartment(ApartmentState.STA)]
    public class FrmMainDefaultTests
    {
        [Test]
        public void AskingFromTheWrongThreadDoesNotBreakTheDefaultInstance()
        {
            Exception fromMtaThread = null;
            var thread = new Thread(() =>
            {
                try
                {
                    _ = FrmMain.Default;
                }
                catch (Exception ex)
                {
                    fromMtaThread = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Join();

            // Refused when the form does not exist yet; handed out when an earlier test made it.
            if (fromMtaThread != null)
                Assert.That(fromMtaThread, Is.TypeOf<ThreadStateException>());
            Assert.That(FrmMain.Default, Is.Not.Null);
        }
    }
}
