using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    public class PortScannerTests
    {
        private static readonly int[] Port80 = [80];

        private static IPAddress SingleHost => IPAddress.Parse("192.168.1.1");

        private static List<int> GetScannedPorts(PortScanner scanner)
        {
            FieldInfo field = typeof(PortScanner).GetField("_ports", BindingFlags.NonPublic | BindingFlags.Instance)!;
            return (List<int>)field.GetValue(scanner)!;
        }

        [Test]
        public void NullPorts_Throws()
        {
            Assert.That(() => new PortScanner(SingleHost, SingleHost, null!),
                        Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void EmptyPorts_Throws()
        {
            // An empty list would otherwise ping every host in the range and probe nothing.
            Assert.That(() => new PortScanner(SingleHost, SingleHost, []),
                        Throws.InstanceOf<ArgumentException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(65536)]
        public void PortOutsideValidRange_Throws(int port)
        {
            // Without the up-front guard this only failed later, inside the per-port TcpClient connect.
            Assert.That(() => new PortScanner(SingleHost, SingleHost, new[] { 80, port }),
                        Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ValidPortBoundaries_AreAccepted()
        {
            Assert.DoesNotThrow(() => _ = new PortScanner(
                SingleHost, SingleHost, new[] { PortListParser.MinPort, PortListParser.MaxPort }));
        }

        [Test]
        public void LazyPortSequence_IsEnumeratedOnlyOnce()
        {
            int enumerations = 0;

            IEnumerable<int> CountingPorts()
            {
                enumerations++;
                yield return 80;
            }

            _ = new PortScanner(SingleHost, SingleHost, CountingPorts());

            Assert.That(enumerations, Is.EqualTo(1));
        }

        [Test]
        public void PortRangeCtor_ZeroStartStillMeansASinglePort()
        {
            // (0, 3389) is this overload's "only one port was specified" convention. Validation must
            // run after that has been resolved, or the convention would start throwing.
            var scanner = new PortScanner(SingleHost, SingleHost, 0, 3389);

            Assert.That(GetScannedPorts(scanner), Is.EqualTo(new[] { 3389 }));
        }

        [TestCase(0, 0)]
        [TestCase(-1, 80)]
        [TestCase(80, 65536)]
        public void PortRangeCtor_InvalidBounds_Throw(int port1, int port2)
        {
            Assert.That(() => new PortScanner(SingleHost, SingleHost, port1, port2),
                        Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void PortRangeCtor_DefaultPortsOnly_IgnoresThePortArguments()
        {
            // port1/port2 are unused in this mode, so they must not be validated either.
            var scanner = new PortScanner(SingleHost, SingleHost, 0, 0, checkDefaultPortsOnly: true);

            Assert.That(GetScannedPorts(scanner), Does.Contain(ScanHost.RdpPort));
        }

        [Test]
        public void ExplicitPortList_IsUsedAsGiven()
        {
            var scanner = new PortScanner(SingleHost, SingleHost, Port80);

            Assert.That(GetScannedPorts(scanner), Is.EqualTo(Port80));
        }
    }
}
