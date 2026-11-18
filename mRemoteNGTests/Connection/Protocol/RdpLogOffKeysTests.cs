using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection.Protocol.RDP;
using MSTSCLib;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpLogOffKeysTests
    {
        private const int ScanLeftWindowsExtended = 0x15B;
        private const int ScanR = 0x13;

        [Test]
        public void OpenRunBox_HoldsWindowsWhileRIsPressed()
        {
            RdpLogOffKeys.Key[] keys = RdpLogOffKeys.OpenRunBox.ToArray();

            Assert.That(keys, Is.EqualTo(new[]
            {
                new RdpLogOffKeys.Key(ScanLeftWindowsExtended, false),
                new RdpLogOffKeys.Key(ScanR, false),
                new RdpLogOffKeys.Key(ScanR, true),
                new RdpLogOffKeys.Key(ScanLeftWindowsExtended, true),
            }));
        }

        [Test]
        public void TypeLogOffAndEnter_TypesLogoffThenPressesEnter()
        {
            // Scan codes (set 1) of l o g o f f, then Enter.
            int[] expectedScanCodes = [0x26, 0x18, 0x22, 0x18, 0x21, 0x21, 0x1C];

            RdpLogOffKeys.Key[] keys = RdpLogOffKeys.TypeLogOffAndEnter.ToArray();

            Assert.That(keys.Where(key => !key.KeyUp).Select(key => key.KeyData), Is.EqualTo(expectedScanCodes));
        }

        [Test]
        public void EveryKeyPressedIsReleased()
        {
            // A key left down in the session (e.g. Windows) would change what the user types next.
            foreach (IReadOnlyList<RdpLogOffKeys.Key> sequence in new[] { RdpLogOffKeys.OpenRunBox, RdpLogOffKeys.TypeLogOffAndEnter })
            {
                Dictionary<int, int> down = new();
                foreach (RdpLogOffKeys.Key key in sequence)
                    down[key.KeyData] = down.GetValueOrDefault(key.KeyData) + (key.KeyUp ? -1 : 1);

                Assert.That(down.Values, Is.All.EqualTo(0));
            }
        }

        [Test]
        public void RunBoxGetsTimeToOpenBeforeTyping()
        {
            Assert.That(RdpLogOffKeys.RunBoxOpenDelay.TotalMilliseconds, Is.GreaterThanOrEqualTo(500));
        }

        [Test]
        public void SendKeys_SendsEachKeyOnItsOwnInOrder()
        {
            // The generated interop passes SendKeys' arrays as single values, so each call must
            // carry exactly one key or every key after the first would be lost.
            IMsRdpClientNonScriptable keyboard = Substitute.For<IMsRdpClientNonScriptable>();
            List<(int Count, bool KeyUp, int KeyData)> sent = new();
            keyboard.When(k => k.SendKeys(Arg.Any<int>(), ref Arg.Any<bool>(), ref Arg.Any<int>()))
                    .Do(call => sent.Add(((int)call[0], (bool)call[1], (int)call[2])));

            RdpProtocol.SendKeys(keyboard, RdpLogOffKeys.OpenRunBox);

            Assert.That(sent.Select(s => s.Count), Is.All.EqualTo(1));
            Assert.That(sent.Select(s => (s.KeyUp, s.KeyData)),
                        Is.EqualTo(RdpLogOffKeys.OpenRunBox.Select(k => (k.KeyUp, k.KeyData))));
        }
    }
}
