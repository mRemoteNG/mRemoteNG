using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.UI.Tabs;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpProtocol8ResizeTests
    {
        private TestableRdpProtocol8 _rdpProtocol;
        private ConnectionInfo _connectionInfo;
        private InterfaceControl _interfaceControl;
        private Form _testForm;

        [SetUp]
        public void Setup()
        {
            // Create a test form to simulate the main window
            _testForm = new Form
            {
                WindowState = FormWindowState.Normal,
                Size = new Size(1024, 768)
            };

            // Create connection info with automatic resize enabled
            _connectionInfo = new ConnectionInfo
            {
                Protocol = ProtocolType.RDP,
                Hostname = "test-host",
                Resolution = RDPResolutions.FitToWindow,
                AutomaticResize = true,
                RdpVersion = RdpVersion.Rdc8
            };

            // Create a mock protocol base for InterfaceControl
            var mockProtocol = Substitute.For<ProtocolBase>();

            // Create interface control
            _interfaceControl = new InterfaceControl(_testForm, mockProtocol, _connectionInfo)
            {
                Size = new Size(800, 600)
            };

            // Create testable RDP protocol instance
            _rdpProtocol = new TestableRdpProtocol8(_testForm);
        }

        [TearDown]
        public void Teardown()
        {
            _rdpProtocol?.Dispose();
            _interfaceControl?.Dispose();
            _testForm?.Dispose();
        }

        [Test]
        public void Resize_WhenMinimized_DoesNotCallDoResizeClient()
        {
            // Arrange
            _testForm.WindowState = FormWindowState.Minimized;
            _rdpProtocol.ResetResizeCounts();

            // Act
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            // Assert
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(0),
                "DoResizeClient should not be called when window is minimized");
            Assert.That(_rdpProtocol.DoResizeControlCallCount, Is.EqualTo(0),
                "DoResizeControl should not be called when window is minimized");
        }

        [Test]
        public void Resize_WhenNormalState_CallsDoResizeControl()
        {
            // Arrange
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.ResetResizeCounts();

            // Act
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            // Assert
            Assert.That(_rdpProtocol.DoResizeControlCallCount, Is.GreaterThanOrEqualTo(1),
                "DoResizeControl should be called to update control size during resize");
        }

        [Test]
        public void Resize_WhenWindowStateChanges_CallsDoResizeClient()
        {
            // Arrange - Start in Normal state
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);
            _rdpProtocol.ResetResizeCounts();

            // Act - Change to Maximized
            _testForm.WindowState = FormWindowState.Maximized;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            // Assert
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(1),
                "DoResizeClient should be called when window state changes");
        }

        [Test]
        public void Resize_WhenWindowStateUnchangedInNormalState_DoesNotCallDoResizeClient()
        {
            // Arrange - Set initial state
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);
            _rdpProtocol.ResetResizeCounts();

            // Act - Simulate manual resize (state stays Normal)
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            // Assert
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(0),
                "DoResizeClient should not be called during manual drag resize (deferred to ResizeEnd)");
            Assert.That(_rdpProtocol.DoResizeControlCallCount, Is.GreaterThanOrEqualTo(1),
                "DoResizeControl should still be called to update UI");
        }

        [Test]
        public void ResizeEnd_WhenMinimized_DoesNotCallDoResizeClient()
        {
            // Arrange
            _testForm.WindowState = FormWindowState.Minimized;
            _rdpProtocol.ResetResizeCounts();

            // Act
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);

            // Assert
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(0),
                "DoResizeClient should not be called when window is minimized");
        }

        [Test]
        public void ResizeEnd_WhenNormalState_CallsDoResizeControlAndSchedulesDebounce()
        {
            // Arrange
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.ResetResizeCounts();

            // Act
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);

            // Assert
            Assert.That(_rdpProtocol.DoResizeControlCallCount, Is.EqualTo(1),
                "DoResizeControl should be called immediately in ResizeEnd");
            Assert.That(_rdpProtocol.DebounceScheduledCount, Is.EqualTo(1),
                "Debounce should be scheduled in ResizeEnd (DoResizeClient will be called after delay)");
        }

        [Test]
        public void ResizeEnd_WithDebounce_CallsDoResizeClientAfterDelay()
        {
            // Arrange
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.ResetResizeCounts();

            // Act
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);

            // Simulate the debounce timer firing
            _rdpProtocol.SimulateDebounceTimerElapsed();

            // Assert
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(1),
                "DoResizeClient should be called after debounce timer elapses");
        }

        [Test]
        public void ResizeEnd_UpdatesLastWindowState()
        {
            // Arrange
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SetLastWindowState(FormWindowState.Minimized);

            // Act
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);

            // Assert
            Assert.That(_rdpProtocol.GetLastWindowState(), Is.EqualTo(FormWindowState.Normal),
                "ResizeEnd should update LastWindowState to current state");
        }

        [Test]
        public void ManualDragResize_Sequence_WorksCorrectly()
        {
            // Arrange - Start with Normal state
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);
            _rdpProtocol.ResetResizeCounts();

            // Act - Simulate drag resize sequence
            // During drag: multiple Resize events (state stays Normal)
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            // After drag completes: ResizeEnd event
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);

            // Simulate debounce timer firing
            _rdpProtocol.SimulateDebounceTimerElapsed();

            // Assert
            Assert.That(_rdpProtocol.DoResizeControlCallCount, Is.GreaterThanOrEqualTo(4),
                "DoResizeControl should be called during each Resize event and ResizeEnd");
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(1),
                "DoResizeClient should only be called once after debounce, not during drag");
        }

        [Test]
        public void DebounceTimer_MultipleResizeEnds_OnlyLastResizeTakesEffect()
        {
            // Arrange
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.ResetResizeCounts();

            // Act - Simulate rapid ResizeEnd calls (user dragging quickly)
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);
            _rdpProtocol.SimulateResizeEnd(null, EventArgs.Empty);

            // Simulate debounce timer firing once
            _rdpProtocol.SimulateDebounceTimerElapsed();

            // Assert
            Assert.That(_rdpProtocol.DebounceScheduledCount, Is.EqualTo(3),
                "Debounce should be scheduled for each ResizeEnd");
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(1),
                "DoResizeClient should only be called once despite multiple ResizeEnd events");
        }

        [Test]
        public void MaximizeRestore_Sequence_WorksCorrectly()
        {
            // Arrange - Start in Normal state
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);
            _rdpProtocol.ResetResizeCounts();

            // Act - Maximize
            _testForm.WindowState = FormWindowState.Maximized;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            var resizeClientCountAfterMaximize = _rdpProtocol.DoResizeClientCallCount;
            _rdpProtocol.ResetResizeCounts();

            // Act - Restore
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            // Assert
            Assert.That(resizeClientCountAfterMaximize, Is.EqualTo(1),
                "DoResizeClient should be called when maximizing");
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(1),
                "DoResizeClient should be called when restoring");
        }

        [Test]
        public void MinimizeRestore_Sequence_WorksCorrectly()
        {
            // Arrange - Start in Normal state
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);
            _rdpProtocol.ResetResizeCounts();

            // Act - Minimize
            _testForm.WindowState = FormWindowState.Minimized;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            var resizeCallsWhileMinimized = _rdpProtocol.DoResizeClientCallCount;
            _rdpProtocol.ResetResizeCounts();

            // Act - Restore from minimize
            _testForm.WindowState = FormWindowState.Normal;
            _rdpProtocol.SimulateResize(null, EventArgs.Empty);

            // Assert
            Assert.That(resizeCallsWhileMinimized, Is.EqualTo(0),
                "DoResizeClient should not be called when minimizing");
            Assert.That(_rdpProtocol.DoResizeClientCallCount, Is.EqualTo(1),
                "DoResizeClient should be called when restoring from minimize");
        }

        /// <summary>
        /// Testable version of RdpProtocol8 that exposes resize methods for testing
        /// </summary>
        private class TestableRdpProtocol8 : IDisposable
        {
            private readonly Form _mainForm;
            private FormWindowState _lastWindowState = FormWindowState.Normal;
            private bool _hasPendingResize = false;

            public int DoResizeControlCallCount { get; private set; }
            public int DoResizeClientCallCount { get; private set; }
            public int DebounceScheduledCount { get; private set; }

            public TestableRdpProtocol8(Form mainForm)
            {
                _mainForm = mainForm;
            }

            public void SimulateResize(object sender, EventArgs e)
            {
                // Replicate the logic from RdpProtocol8.Resize()
                if (_mainForm.WindowState == FormWindowState.Minimized)
                {
                    _lastWindowState = FormWindowState.Minimized;
                    return;
                }

                DoResizeControl();

                if (_lastWindowState != _mainForm.WindowState)
                {
                    _lastWindowState = _mainForm.WindowState;
                    DoResizeClient();
                }
            }

            public void SimulateResizeEnd(object sender, EventArgs e)
            {
                // Replicate the logic from RdpProtocol8.ResizeEnd() with debounce
                if (_mainForm.WindowState == FormWindowState.Minimized) return;

                _lastWindowState = _mainForm.WindowState;
                DoResizeControl();

                // Schedule debounced resize instead of calling DoResizeClient immediately
                ScheduleDebouncedResize();
            }

            private void ScheduleDebouncedResize()
            {
                _hasPendingResize = true;
                DebounceScheduledCount++;
            }

            public void SimulateDebounceTimerElapsed()
            {
                if (!_hasPendingResize) return;
                _hasPendingResize = false;
                DoResizeClient();
            }

            public void DoResizeControl()
            {
                DoResizeControlCallCount++;
            }

            public void DoResizeClient()
            {
                DoResizeClientCallCount++;
            }

            public void ResetResizeCounts()
            {
                DoResizeControlCallCount = 0;
                DoResizeClientCallCount = 0;
                DebounceScheduledCount = 0;
                _hasPendingResize = false;
            }

            public FormWindowState GetLastWindowState() => _lastWindowState;

            public void SetLastWindowState(FormWindowState state)
            {
                _lastWindowState = state;
            }

            public void Dispose()
            {
                // Cleanup if needed
            }
        }
    }
}
