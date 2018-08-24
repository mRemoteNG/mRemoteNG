using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Window;
using NSubstitute;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    public abstract class ConfigWindowSpecialTestsBase
    {
        protected abstract ProtocolType Protocol { get; }
        protected bool TestAgainstContainerInfo { get; set; } = false;
        protected ConfigWindow ConfigWindow;
        protected ConnectionInfo ConnectionInfo;
        protected List<string> ExpectedPropertyList;

        [SetUp]
        public virtual void Setup()
        {
            ConnectionInfo = ConfigWindowGeneralTests.ConstructConnectionInfo(Protocol, TestAgainstContainerInfo);
            ExpectedPropertyList = ConfigWindowGeneralTests.BuildExpectedConnectionInfoPropertyList(Protocol, TestAgainstContainerInfo);

            ConfigWindow = new ConfigWindow(new DockContent(), Substitute.For<IConnectionsService>())
            {
                PropertiesVisible = true,
            };
        }

        public void RunVerification()
        {
            ConfigWindow.SelectedTreeNode = ConnectionInfo;
            Assert.That(
                ConfigWindow.VisibleObjectProperties,
                Is.EquivalentTo(ExpectedPropertyList));
        }
    }
}
