using System;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.Serial;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class PuttyLogOffTests
    {
        private static readonly IntPtr SomeWindow = new(1);

        [Test]
        public void ShellProtocolsCanLogOffOnceTheirWindowExists()
        {
            // SSH, Telnet and rlogin end in a login shell that "exit" logs off.
            foreach (PuttyBase protocol in new PuttyBase[] { new ProtocolSSH2(), new ProtocolTelnet(), new ProtocolRlogin() })
            {
                Assert.That(protocol.CanLogOff, Is.False, $"{protocol.GetType().Name} before PuTTY started");
                protocol.PuttyHandle = SomeWindow;
                Assert.That(protocol.CanLogOff, Is.True, protocol.GetType().Name);
            }
        }

        [Test]
        public void RawAndSerialCannotLogOff()
        {
            // No shell of their own to end: "exit" would just be sent as data.
            foreach (PuttyBase protocol in new PuttyBase[] { new RawProtocol(), new ProtocolSerial() })
            {
                protocol.PuttyHandle = SomeWindow;
                Assert.That(protocol.CanLogOff, Is.False, protocol.GetType().Name);
            }
        }
    }
}
