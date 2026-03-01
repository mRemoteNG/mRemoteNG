using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Themes;
using mRemoteNG.UI.Tabs;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;

namespace mRemoteNGTests.UI.Tabs
{
    [TestFixture]
    public class DockPaneStripNGTests
    {
        private static void RunWithMessagePump(Action testAction)
        {
            Exception? caught = null;
            var thread = new Thread(() =>
            {
                try
                {
                    testAction();
                }
                catch (Exception ex)
                {
                    caught = ex;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            if (!thread.Join(TimeSpan.FromSeconds(30)))
            {
                thread.Interrupt();
                Assert.Fail("Test timed out after 30 seconds");
            }

            if (caught != null)
                throw caught;
        }

        [Test]
        public void MiddleClick_ClosesSpecificTab_NotAll() => RunWithMessagePump(() =>
        {
            // Arrange
            using var hostForm = new Form
            {
                Width = 800,
                Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };

            var dockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            
            // Apply the factory that creates DockPaneStripNG
            dockPanel.Theme.Extender.DockPaneStripFactory = new MremoteDockPaneStripFactory();

            hostForm.Controls.Add(dockPanel);
            hostForm.Show();

            var doc1 = new DockContent { Text = "Doc1", CloseButton = true, CloseButtonVisible = true };
            var doc2 = new DockContent { Text = "Doc2", CloseButton = true, CloseButtonVisible = true };
            var doc3 = new DockContent { Text = "Doc3", CloseButton = true, CloseButtonVisible = true };

            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(dockPanel, DockState.Document);
            doc3.Show(dockPanel, DockState.Document);

            Application.DoEvents();

            // Find the DockPaneStripNG control
            Control dockPaneStrip = FindDockPaneStripNG(dockPanel);
            Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");
            Assert.That(dockPaneStrip.GetType().Name, Is.EqualTo("DockPaneStripNG"));

            // Get the MiddleClickCloseTab method via reflection
            MethodInfo middleClickMethod = dockPaneStrip.GetType().GetMethod("MiddleClickCloseTab", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(middleClickMethod, Is.Not.Null, "Could not find MiddleClickCloseTab method");

            // Act - Simulate middle click on the second tab (index 1)
            // Assume index 1 is Doc2. Tabs are usually added in order.
            
            middleClickMethod.Invoke(dockPaneStrip, new object[] { 1 });
            
            // Pump events to allow QueueCloseTab -> BeginInvoke to execute
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 2)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                // Check if doc2 is closed (hidden or unknown state)
                if (doc2.DockState == DockState.Unknown || doc2.IsDisposed) break;
            }

            // Assert
            Assert.That(doc1.DockState, Is.EqualTo(DockState.Document), "Doc1 should still be open");
            Assert.That(doc2.DockState, Is.EqualTo(DockState.Unknown).Or.EqualTo(DockState.Hidden), "Doc2 should be closed");
            Assert.That(doc3.DockState, Is.EqualTo(DockState.Document), "Doc3 should still be open");
        });

        [Test]
        public void IsWithinUndockSuppressionZone_ReturnsTrue_WhenPointerIsJustOutsideStrip()
        {
            var tabStripBounds = new Rectangle(0, 0, 120, 24);
            var pointerLocation = new Point(60, -1);
            var dragSize = new Size(8, 8);

            bool isSuppressed = DockPaneStripNG.IsWithinUndockSuppressionZone(tabStripBounds, pointerLocation, dragSize);

            Assert.That(isSuppressed, Is.True);
        }

        [Test]
        public void IsWithinUndockSuppressionZone_ReturnsFalse_WhenPointerIsInsideStrip()
        {
            var tabStripBounds = new Rectangle(0, 0, 120, 24);
            var pointerLocation = new Point(60, 10);
            var dragSize = new Size(8, 8);

            bool isSuppressed = DockPaneStripNG.IsWithinUndockSuppressionZone(tabStripBounds, pointerLocation, dragSize);

            Assert.That(isSuppressed, Is.False);
        }

        [Test]
        public void IsWithinUndockSuppressionZone_ReturnsFalse_WhenPointerIsFarOutsideSuppressionZone()
        {
            var tabStripBounds = new Rectangle(0, 0, 120, 24);
            var pointerLocation = new Point(60, -9);
            var dragSize = new Size(8, 8);

            bool isSuppressed = DockPaneStripNG.IsWithinUndockSuppressionZone(tabStripBounds, pointerLocation, dragSize);

            Assert.That(isSuppressed, Is.False);
        }

        private static Control FindDockPaneStripNG(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (string.Equals(c.GetType().Name, "DockPaneStripNG", StringComparison.Ordinal))
                    return c;
                
                var result = FindDockPaneStripNG(c);
                if (result != null) return result;
            }
            return null;
        }
    }
}
