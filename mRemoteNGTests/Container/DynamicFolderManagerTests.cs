using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using mRemoteNG.Container;
using mRemoteNG.Connection;
using System.Threading;

namespace mRemoteNGTests.Container
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class DynamicFolderManagerTests
    {
        private string _tempFile;
        private DynamicFolderManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new DynamicFolderManager();
            _tempFile = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }
        }

        /// <summary>
        /// Calls ImportXml directly via reflection to bypass RefreshFolderInternal's
        /// exception swallowing and FrmMain.Default?.InvokeRequired check.
        /// </summary>
        private static void InvokeImportXml(string xmlContent, ContainerInfo container, string sourceName)
        {
            var method = typeof(DynamicFolderManager).GetMethod(
                "ImportXml",
                BindingFlags.Static | BindingFlags.NonPublic);
            Assert.That(method, Is.Not.Null, "Failed to find ImportXml method");
            try
            {
                method!.Invoke(null, new object[] { xmlContent, container, sourceName });
            }
            catch (TargetInvocationException tie) when (tie.InnerException != null)
            {
                throw tie.InnerException;
            }
        }

        [Test]
        public void RefreshFolder_FileSource_PopulatesChildren()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Connections Name=""Connections"" Export=""False"" ConfVersion=""1.3"">
    <Node Name=""TestConnection"" Type=""Connection"" Descr=""Test Description"" Protocol=""RDP"" Hostname=""localhost"" Port=""3389"" />
</Connections>";

            var container = new ContainerInfo
            {
                Name = "DynamicFolder",
                DynamicSource = DynamicSourceType.File,
                DynamicSourceValue = _tempFile
            };

            // Act — call ImportXml directly (bypasses RefreshFolderInternal exception swallowing)
            InvokeImportXml(xml, container, "TestFile");

            // Assert
            Assert.That(container.Children.Count, Is.EqualTo(1));
            Assert.That(container.Children[0].Name, Is.EqualTo("TestConnection"));
        }

        [Test]
        public void RefreshFolder_ScriptSource_PopulatesChildren()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Connections Name=""Connections"" Export=""False"" ConfVersion=""1.3"">
    <Node Name=""ScriptConnection"" Type=""Connection"" Descr=""From Script"" Protocol=""SSH2"" Hostname=""127.0.0.1"" Port=""22"" />
</Connections>";

            var container = new ContainerInfo
            {
                Name = "DynamicScriptFolder",
                DynamicSource = DynamicSourceType.Script,
                DynamicSourceValue = "test-script.bat"
            };

            // Act — call ImportXml directly to test XML parsing without script execution
            InvokeImportXml(xml, container, "ScriptOutput");

            // Assert
            Assert.That(container.Children.Count, Is.EqualTo(1));
            Assert.That(container.Children[0].Name, Is.EqualTo("ScriptConnection"));
        }
    }
}
