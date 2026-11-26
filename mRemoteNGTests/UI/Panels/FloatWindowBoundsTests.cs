using System.Drawing;
using mRemoteNG.UI.Panels;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Panels
{
    public class FloatWindowBoundsTests
    {
        // A 1920x1040 primary screen with a second screen of the same size on its left
        private static readonly Rectangle Primary = new(0, 0, 1920, 1040);
        private static readonly Rectangle Left = new(-1920, 0, 1920, 1040);
        private static readonly Rectangle[] Screens = [Primary, Left];

        [Test]
        public void BoundsOnAScreenAreKept()
        {
            Rectangle bounds = new(-1800, 100, 1200, 800);

            Assert.That(FloatWindowBounds.KeepOnScreen(bounds, Screens, Primary), Is.EqualTo(bounds));
        }

        [Test]
        public void BoundsPartlyOffTheScreenAreMovedOnToIt()
        {
            Rectangle bounds = new(1500, 900, 800, 600);

            Assert.That(FloatWindowBounds.KeepOnScreen(bounds, Screens, Primary),
                Is.EqualTo(new Rectangle(1120, 440, 800, 600)));
        }

        [Test]
        public void BoundsLargerThanTheScreenAreShrunk()
        {
            Rectangle bounds = new(100, 100, 2500, 1500);

            Assert.That(FloatWindowBounds.KeepOnScreen(bounds, Screens, Primary), Is.EqualTo(Primary));
        }

        [Test]
        public void BoundsGoToTheScreenTheyOverlapMost()
        {
            // Mostly on the left screen, partly on the primary one
            Rectangle bounds = new(-700, 100, 1000, 600);

            Assert.That(FloatWindowBounds.KeepOnScreen(bounds, Screens, Primary),
                Is.EqualTo(new Rectangle(-1000, 100, 1000, 600)));
        }

        [Test]
        public void BoundsOnADisconnectedScreenAreCentredOnThePrimaryScreen()
        {
            Rectangle bounds = new(4000, 100, 800, 600);

            Assert.That(FloatWindowBounds.KeepOnScreen(bounds, [Primary], Primary),
                Is.EqualTo(new Rectangle(560, 220, 800, 600)));
        }
    }
}
