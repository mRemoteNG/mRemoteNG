using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Window;
using NSubstitute;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    [NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
    public class ConnectionInitiatorSshTunnelTests
    {
        private ConnectionInitiator _connectionInitiator;
        private IProtocolFactory _mockProtocolFactory;
        private ITunnelPortValidator _mockTunnelPortValidator;
        private ConnectionInfo _targetConnection;
        private ConnectionInfo _tunnelConnection;
        private string _tempFilePath;
        private TestHelpers.TestScope? _scope;

        [SetUp]
        public void Setup()
        {
            _scope = TestHelpers.TestScope.Begin();
            _mockProtocolFactory = Substitute.For<IProtocolFactory>();
            _mockTunnelPortValidator = Substitute.For<ITunnelPortValidator>();
            _connectionInitiator = new ConnectionInitiator(_mockProtocolFactory, _mockTunnelPortValidator);

            _tunnelConnection = new ConnectionInfo
            {
                Name = "MyTunnel",
                Hostname = "tunnel.host.com",
                Protocol = ProtocolType.SSH2,
                Panel = "General"
            };

            _targetConnection = new ConnectionInfo
            {
                Name = "TargetViaTunnel",
                Hostname = "target.internal.com",
                Port = 80,
                Protocol = ProtocolType.HTTP,
                SSHTunnelConnectionName = "MyTunnel",
                Panel = "General"
            };

            // Setup a real ConnectionTreeModel via ConnectionsService
            _tempFilePath = Path.Combine(Path.GetTempPath(), "test_connections.mxs");

            // Create a minimal tree model
            var treeModel = new ConnectionTreeModel();
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            var container = new ContainerInfo { Name = "Root" };
            rootNode.Children.Add(container);
            container.Children.Add(_tunnelConnection);
            container.Children.Add(_targetConnection);
            treeModel.AddRootNode(rootNode);

            // Use reflection to set the model if LoadConnections is too heavy
            var connectionsService = Runtime.ConnectionsService;
            var backingField = typeof(ConnectionsService).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.Name.Contains("<ConnectionTreeModel>k__BackingField"));

            if (backingField != null)
            {
                backingField.SetValue(connectionsService, treeModel);
            }
            else
            {
                // Fallback to property setter if it exists (it's private set, so we need reflection)
                var treeModelProperty = typeof(ConnectionsService).GetProperty(nameof(ConnectionsService.ConnectionTreeModel));
                treeModelProperty!.GetSetMethod(true)!.Invoke(connectionsService, new object[] { treeModel });
            }

            Runtime.MessageCollector.ClearMessages();
        }

        [TearDown]
        public void Cleanup()
        {
            _scope?.Dispose();
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        /// <summary>
        /// Runs the given action on a dedicated STA thread with a WinForms message pump.
        /// Required because OpenConnection is async void and needs a SynchronizationContext
        /// to post continuations. The message pump also provides the DockPanel infrastructure
        /// needed for ConnectionWindow tab operations.
        /// </summary>
        private static void RunWithMessagePump(Action testAction)
        {
            Exception caught = null;
            var thread = new Thread(() =>
            {
                var form = new Form
                {
                    Width = 400, Height = 300,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.Manual,
                    Location = new System.Drawing.Point(-10000, -10000)
                };

                form.Load += (_, _) =>
                {
                    try
                    {
                        testAction();
                    }
                    catch (Exception ex)
                    {
                        caught = ex;
                    }
                    finally
                    {
                        form.Close();
                    }
                };

                Application.Run(form);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            if (!thread.Join(TimeSpan.FromSeconds(30)))
            {
                thread.Interrupt();
                Assert.Fail("Test timed out after 30 seconds (message pump deadlock)");
            }

            if (caught != null)
                throw caught;
        }

        [Test]
        public void OpenConnection_RetriesSshTunnel_OnFailure() => RunWithMessagePump(() =>
        {
            // Arrange - create a lightweight DockPanel host (avoids FrmMain.Default)
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };

            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            // Ensure Runtime.WindowList is initialized (normally done by FrmMain)
            Runtime.WindowList ??= new mRemoteNG.UI.WindowList();

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "SSH Tunnel Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();
                Application.DoEvents();

                var mockTunnelProtocol = Substitute.For<ProtocolSSH2>();
                var mockTargetProtocol = Substitute.For<ProtocolBase>();

                mockTunnelProtocol.InitializeAsync().Returns(Task.FromResult(true));
                mockTunnelProtocol.Connect().Returns(true);
                mockTunnelProtocol.isRunning().Returns(true);

                int validationCallCount = 0;
                _mockTunnelPortValidator.ValidatePortAsync(Arg.Any<int>()).Returns(_ =>
                {
                    validationCallCount++;
                    return Task.FromResult(validationCallCount > 2);
                });

                _mockProtocolFactory.CreateProtocol(Arg.Is<ConnectionInfo>(c => string.Equals(c.Name, "MyTunnel", StringComparison.Ordinal)))
                                   .Returns(mockTunnelProtocol);

                _mockProtocolFactory.CreateProtocol(Arg.Is<ConnectionInfo>(c => string.Equals(c.Hostname, "localhost", StringComparison.OrdinalIgnoreCase)))
                                   .Returns(mockTargetProtocol);

                mockTargetProtocol.InitializeAsync().Returns(Task.FromResult(true));
                mockTargetProtocol.Connect().Returns(true);

                // Act - pass DoNotJump to avoid FrmMain.Default.pnlDock and
                // provide the ConnectionWindow to bypass PanelAdder.AddPanel
                _connectionInitiator.OpenConnection(
                    _targetConnection,
                    ConnectionInfo.Force.DoNotJump,
                    connectionWindow);

                // Wait for the async void OpenConnection to complete by pumping events.
                // The async continuations are posted to the WindowsFormsSynchronizationContext.
                bool completed = SpinWait.SpinUntil(() =>
                {
                    Application.DoEvents();
                    return validationCallCount >= 3;
                }, TimeSpan.FromSeconds(10));

                // Pump a few more times to let the final target connection complete
                for (int i = 0; i < 20; i++)
                {
                    Application.DoEvents();
                    Thread.Sleep(50);
                }

                // Assert
                Assert.That(completed, Is.True, "Tunnel port validation should have been called 3 times (2 failures + 1 success)");
                _mockTunnelPortValidator.Received(3).ValidatePortAsync(Arg.Any<int>());
                _mockProtocolFactory.Received(1).CreateProtocol(Arg.Is<ConnectionInfo>(c => string.Equals(c.Hostname, "localhost", StringComparison.OrdinalIgnoreCase)));
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });
    }
}
