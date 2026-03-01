using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Tabs;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [NonParallelizable]
    public class PuttyBaseTests
    {
        private const string FallbackTabText = "SSH2: Connection Name";

        private TestablePuttyBase _puttyProtocol = null!;
        private ConnectionTab _connectionTab = null!;
        private InterfaceControl _interfaceControl = null!;

        [SetUp]
        public void Setup()
        {
            _puttyProtocol = new TestablePuttyBase();
            _connectionTab = new ConnectionTab
            {
                TabText = FallbackTabText
            };

            ConnectionInfo connectionInfo = new()
            {
                Protocol = ProtocolType.SSH2,
                Name = "Connection Name",
                Hostname = "example-host"
            };

            _interfaceControl = new InterfaceControl(_connectionTab, _puttyProtocol, connectionInfo);
            _puttyProtocol.InterfaceControl = _interfaceControl;
        }

        [TearDown]
        public void TearDown()
        {
            _puttyProtocol?.StopTrackingForTest();
            _interfaceControl?.Dispose();
            _connectionTab?.Dispose();
        }

        [Test]
        public void UpdateTabTitleFromTerminalTitle_UsesDynamicTitleWhenWindowTitleChanges()
        {
            _puttyProtocol.QueueTerminalTitle("example-host - PuTTY");
            _puttyProtocol.StartTrackingForTest();

            _puttyProtocol.QueueTerminalTitle("deploy-session");
            _puttyProtocol.RefreshTitleForTest();

            Assert.That(_connectionTab.TabText, Is.EqualTo("deploy-session"));
        }

        [Test]
        public void UpdateTabTitleFromTerminalTitle_UsesFallbackWhenWindowTitleIsInitialOrEmpty()
        {
            _puttyProtocol.QueueTerminalTitle("example-host - PuTTY");
            _puttyProtocol.StartTrackingForTest();

            _puttyProtocol.QueueTerminalTitle("deploy-session");
            _puttyProtocol.RefreshTitleForTest();
            Assert.That(_connectionTab.TabText, Is.EqualTo("deploy-session"));

            _puttyProtocol.QueueTerminalTitle("example-host - PuTTY");
            _puttyProtocol.RefreshTitleForTest();
            Assert.That(_connectionTab.TabText, Is.EqualTo(FallbackTabText));

            _puttyProtocol.QueueTerminalTitle(string.Empty);
            _puttyProtocol.RefreshTitleForTest();
            Assert.That(_connectionTab.TabText, Is.EqualTo(FallbackTabText));
        }

        [Test]
        public void StopTerminalTitleTracking_RestoresFallbackAndPreventsFurtherUpdates()
        {
            _puttyProtocol.QueueTerminalTitle("example-host - PuTTY");
            _puttyProtocol.StartTrackingForTest();

            _puttyProtocol.QueueTerminalTitle("deploy-session");
            _puttyProtocol.RefreshTitleForTest();
            Assert.That(_connectionTab.TabText, Is.EqualTo("deploy-session"));

            _puttyProtocol.StopTrackingForTest();
            Assert.That(_connectionTab.TabText, Is.EqualTo(FallbackTabText));

            _puttyProtocol.QueueTerminalTitle("another-session");
            _puttyProtocol.RefreshTitleForTest();
            Assert.That(_connectionTab.TabText, Is.EqualTo(FallbackTabText));
        }

        [Test]
        public void SchedulePostOpenLayoutResizePass_WaitsForHandleCreationThenResizes()
        {
            Assert.That(_interfaceControl.IsHandleCreated, Is.False);

            _puttyProtocol.SchedulePostOpenLayoutResizePassForTest();
            Assert.That(_puttyProtocol.DeferredResizeCallCount, Is.EqualTo(0));

            _ = _interfaceControl.Handle;
            Assert.That(_puttyProtocol.DeferredResizeCallCount, Is.EqualTo(1));
        }

        [Test]
        public void OnPowerModeChanged_Resume_TriggersResizeOnUIThread()
        {
            // Ensure handle is created so BeginInvoke works
            _ = _interfaceControl.Handle;

            int initialCount = _puttyProtocol.DeferredResizeCallCount;

            _puttyProtocol.OnPowerModeChanged(PowerModes.Resume);

            // Allow UI message loop to process the BeginInvoke
            Application.DoEvents();

            Assert.That(_puttyProtocol.DeferredResizeCallCount, Is.GreaterThan(initialCount));
        }

        private sealed class TestablePuttyBase : PuttyBase
        {
            private readonly Queue<string> _queuedTitles = new();
            private string _currentTitle = string.Empty;

            public int DeferredResizeCallCount { get; private set; }

            protected override bool UseTerminalTitlePollingTimer => false;

            protected override int PowerModeChangedResizeDelay => 0;

            protected override void QueuePostOpenLayoutResizePass(MethodInvoker resizeAction)
            {
                resizeAction();
            }

            protected override void Resize(object sender, EventArgs e)
            {
                DeferredResizeCallCount++;
            }

            protected override string ReadTerminalWindowTitle()
            {
                if (_queuedTitles.Count > 0)
                    _currentTitle = _queuedTitles.Dequeue();

                return _currentTitle;
            }

            public void QueueTerminalTitle(string title)
            {
                _queuedTitles.Enqueue(title);
            }

            public void SchedulePostOpenLayoutResizePassForTest()
            {
                SchedulePostOpenLayoutResizePass();
            }

            public void StartTrackingForTest()
            {
                StartTerminalTitleTracking();
            }

            public void RefreshTitleForTest()
            {
                UpdateTabTitleFromTerminalTitle();
            }

            public void StopTrackingForTest()
            {
                StopTerminalTitleTracking();
            }
        }
    }
}
