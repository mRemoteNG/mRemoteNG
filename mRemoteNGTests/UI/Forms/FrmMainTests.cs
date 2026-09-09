using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class FrmMainTests
    {
        private Form _parentForm;
        private Panel _childPanel;

        [SetUp]
        public void Setup()
        {
            _parentForm = new Form
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(200, 150),
                Size = new Size(300, 200),
            };

            _childPanel = new Panel
            {
                Location = new Point(30, 40),
                Size = new Size(100, 60),
            };

            _parentForm.Controls.Add(_childPanel);
            _parentForm.Show();
        }

        [TearDown]
        public void Teardown()
        {
            _parentForm.Dispose();
            _parentForm = null;
            _childPanel = null;
        }

        [Test]
        public void GetChildAtScreenPoint_ReturnsChildWhenPointFallsInsideChildBounds()
        {
            Point pointInsideChild = _childPanel.PointToScreen(new Point(10, 10));

            Control result = frmMain.GetChildAtScreenPoint(_parentForm, pointInsideChild);

            Assert.That(result, Is.SameAs(_childPanel));
        }

        [Test]
        public void GetChildAtScreenPoint_ReturnsNullWhenPointFallsOutsideAnyChildControl()
        {
            Point pointOutsideChild = _parentForm.PointToScreen(new Point(5, 5));

            Control result = frmMain.GetChildAtScreenPoint(_parentForm, pointOutsideChild);

            Assert.That(result, Is.Null);
        }
    }
}
