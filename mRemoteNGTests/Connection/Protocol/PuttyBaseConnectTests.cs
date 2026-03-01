using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Tabs;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PuttyBaseConnectTests
    {
        private PuttyBase _puttyProtocol;
        private ConnectionTab _connectionTab;
        private InterfaceControl _interfaceControl;
        private string _originalPuttyPath;

        [SetUp]
        public void Setup()
        {
            _originalPuttyPath = PuttyBase.PuttyPath;
            _puttyProtocol = new PuttyBase();
            _connectionTab = new ConnectionTab();
            ConnectionInfo connectionInfo = new ConnectionInfo
            {
                Protocol = ProtocolType.SSH2,
                Name = "Test Connection",
                Hostname = "localhost"
            };
            _interfaceControl = new InterfaceControl(_connectionTab, _puttyProtocol, connectionInfo);
            _puttyProtocol.InterfaceControl = _interfaceControl;
            
            // Set PuttyPath to cmd.exe to simulate a process starting
            PuttyBase.PuttyPath = "cmd.exe"; 
        }

        [TearDown]
        public void TearDown()
        {
            _puttyProtocol?.Close(); 
            _interfaceControl?.Dispose();
            _connectionTab?.Dispose();
            PuttyBase.PuttyPath = _originalPuttyPath;
        }

        [Test]
        public void Connect_ReturnsTrueImmediately()
        {
            // This test verifies that Connect() returns true without waiting for the window
            bool result = _puttyProtocol.Connect();
            Assert.That(result, Is.True, "Connect should return true immediately");
        }
    }
}
