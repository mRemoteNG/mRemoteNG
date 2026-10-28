using System;
using System.IO;
using mRemoteNG.App.Initialization;
using NUnit.Framework;

namespace mRemoteNGTests.App
{
    [TestFixture]
    public class DataFolder176CopierTests
    {
        private string _root;
        private string _old;
        private string _new;

        [SetUp]
        public void Setup()
        {
            _root = Path.Combine(Path.GetTempPath(), $"mrngdata_{Guid.NewGuid():N}");
            _old = Path.Combine(_root, "mRemoteNG");
            _new = Path.Combine(_root, "mRemoteNG Connection Manager");
            Directory.CreateDirectory(_old);
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                Directory.Delete(_root, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        [Test]
        public void ConnectionsToolsLayoutAndThemesAreCopied()
        {
            Write(_old, "confCons.xml", "connections");
            Write(_old, "confCons.xml.20240101-1200000000.backup", "backup");
            Write(_old, "extApps.xml", "tools");
            Write(_old, "pnlLayout.xml", "layout");
            Write(Path.Combine(_old, "Themes"), "mine.vstheme", "theme");

            int copied = DataFolder176Copier.Copy(_old, _new);

            Assert.That(copied, Is.EqualTo(5));
            Assert.That(File.ReadAllText(Path.Combine(_new, "confCons.xml")), Is.EqualTo("connections"));
            Assert.That(File.ReadAllText(Path.Combine(_new, "confCons.xml.20240101-1200000000.backup")), Is.EqualTo("backup"));
            Assert.That(File.ReadAllText(Path.Combine(_new, "extApps.xml")), Is.EqualTo("tools"));
            Assert.That(File.ReadAllText(Path.Combine(_new, "pnlLayout.xml")), Is.EqualTo("layout"));
            Assert.That(File.ReadAllText(Path.Combine(_new, "Themes", "mine.vstheme")), Is.EqualTo("theme"));
        }

        [Test]
        public void TheOldFolderIsLeftAsItWas()
        {
            Write(_old, "confCons.xml", "connections");

            DataFolder176Copier.Copy(_old, _new);

            Assert.That(File.ReadAllText(Path.Combine(_old, "confCons.xml")), Is.EqualTo("connections"));
        }

        [Test]
        public void LogFilesStayBehind()
        {
            Write(_old, "confCons.xml", "connections");
            Write(_old, "mRemoteNG.log", "log");

            DataFolder176Copier.Copy(_old, _new);

            Assert.That(File.Exists(Path.Combine(_new, "mRemoteNG.log")), Is.False);
        }

        [Test]
        public void NothingIsCopiedWhenTheNewFolderHasConnections()
        {
            Write(_old, "confCons.xml", "old connections");
            Write(_old, "extApps.xml", "old tools");
            Write(_new, "confCons.xml", "new connections");

            int copied = DataFolder176Copier.Copy(_old, _new);

            Assert.That(copied, Is.Zero);
            Assert.That(File.ReadAllText(Path.Combine(_new, "confCons.xml")), Is.EqualTo("new connections"));
            Assert.That(File.Exists(Path.Combine(_new, "extApps.xml")), Is.False);
        }

        [Test]
        public void NothingIsCopiedWithoutConnectionsIn176()
        {
            Write(_old, "extApps.xml", "tools");

            Assert.That(DataFolder176Copier.Copy(_old, _new), Is.Zero);
            Assert.That(Directory.Exists(_new), Is.False);
        }

        [Test]
        public void FilesTheNewFolderAlreadyHasAreKept()
        {
            Write(_old, "confCons.xml", "connections");
            Write(Path.Combine(_old, "Themes"), "darcula.vstheme", "old theme");
            Write(Path.Combine(_new, "Themes"), "darcula.vstheme", "new theme");

            int copied = DataFolder176Copier.Copy(_old, _new);

            Assert.That(copied, Is.EqualTo(1));
            Assert.That(File.ReadAllText(Path.Combine(_new, "Themes", "darcula.vstheme")), Is.EqualTo("new theme"));
        }

        [Test]
        public void TheSameFolderIsLeftAlone()
        {
            Write(_old, "confCons.xml", "connections");

            Assert.That(DataFolder176Copier.Copy(_old, _old + Path.DirectorySeparatorChar), Is.Zero);
        }

        private static void Write(string folder, string name, string content)
        {
            Directory.CreateDirectory(folder);
            File.WriteAllText(Path.Combine(folder, name), content);
        }
    }
}
