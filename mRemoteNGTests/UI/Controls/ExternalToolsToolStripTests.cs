using System;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    public class ExternalToolsToolStripTests
    {
        private ExternalToolsToolStrip _externalToolsToolStrip;

        [SetUp]
        public void Setup()
        {
            _externalToolsToolStrip = new ExternalToolsToolStrip();
        }

        [TearDown]
        public void Teardown()
        {
            _externalToolsToolStrip.Dispose();
        }

        [Test]
        public void SettingExternalToolsServiceToNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _externalToolsToolStrip.ExternalToolsService = null);
        }

        [Test]
        public void AddExternalToolsToToolBarCreatesControlsForAllExternalTools()
        {
            var externaltoolsService = new ExternalToolsService();
            externaltoolsService.ExternalTools.Add(BuildExternalTool());
            externaltoolsService.ExternalTools.Add(BuildExternalTool());

            _externalToolsToolStrip.ExternalToolsService = externaltoolsService;
            _externalToolsToolStrip.AddExternalToolsToToolBar();
            Assert.That(_externalToolsToolStrip.Items.Count, Is.EqualTo(2));
        }

        private ExternalTool BuildExternalTool()
        {
            return new ExternalTool(Substitute.For<IConnectionInitiator>(), Substitute.For<ConnectionsService>());
        }
    }
}
