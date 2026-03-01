using System;
using System.IO;
using System.Linq;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Putty
{
    [TestFixture]
    public class PuttySessionsFileProviderTests
    {
        private string _tempDir;
        private string _kittyPath;
        private string _sessionsDir;
        private string _originalPuttyPath;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "mRemoteNG_KiTTY_Tests_" + Guid.NewGuid());
            Directory.CreateDirectory(_tempDir);
            _kittyPath = Path.Combine(_tempDir, "KiTTY.exe");
            // Create dummy exe
            File.WriteAllText(_kittyPath, "dummy");
            
            _sessionsDir = Path.Combine(_tempDir, "Sessions");
            Directory.CreateDirectory(_sessionsDir);

            _originalPuttyPath = PuttyBase.PuttyPath;
            PuttyBase.PuttyPath = _kittyPath;
        }

        [TearDown]
        public void TearDown()
        {
            PuttyBase.PuttyPath = _originalPuttyPath;
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { }
            }
        }

        [Test]
        public void GetSessionNames_ReturnsSessionNames()
        {
            File.WriteAllText(Path.Combine(_sessionsDir, "Session%201"), "Protocol=ssh");
            File.WriteAllText(Path.Combine(_sessionsDir, "Session2"), "Protocol=raw");

            var provider = new PuttySessionsFileProvider();
            var names = provider.GetSessionNames(false);

            Assert.That(names, Contains.Item("Session 1"));
            Assert.That(names, Contains.Item("Session2"));
        }

        [Test]
        public void GetSession_ReturnsCorrectSessionInfo()
        {
            var sessionName = "Test%20Session";
            var filePath = Path.Combine(_sessionsDir, sessionName);
            File.WriteAllText(filePath, "Protocol=ssh" + Environment.NewLine + "HostName=example.com" + Environment.NewLine + "PortNumber=2222" + Environment.NewLine + "UserName=user");

            var provider = new PuttySessionsFileProvider();
            
            var session = provider.GetSession(sessionName);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.Name, Is.EqualTo("Test Session"));
            Assert.That(session.Hostname, Is.EqualTo("example.com"));
            Assert.That(session.Port, Is.EqualTo(2222));
            Assert.That(session.Username, Is.EqualTo("user"));
            Assert.That(session.Protocol, Is.EqualTo(ProtocolType.SSH2));
        }
    }
}
