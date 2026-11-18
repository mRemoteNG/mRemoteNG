using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
        private const int ScanLeftShift = 0x2A;

        private static IntPtr CurrentLayout => InputLanguage.CurrentInputLanguage.Handle;

        /// <summary>The handle of an installed keyboard layout, or the test is skipped.</summary>
        private static IntPtr InstalledLayout(string cultureName)
        {
            // Only installed layouts can be looked up; loading one would change the user's own list.
            InputLanguage[] forCulture = InputLanguage.InstalledInputLanguages.Cast<InputLanguage>()
                .Where(l => l.Culture.Name == cultureName).ToArray();
            // en-US also has layouts such as "United States-International"; the plain one is "US".
            InputLanguage language = cultureName == "en-US"
                ? forCulture.FirstOrDefault(l => l.LayoutName == "US")
                : forCulture.FirstOrDefault();
            Assume.That(language, Is.Not.Null, $"keyboard layout {cultureName} is not installed");
            return language.Handle;
        }

        [Test]
        public void LogOffCommand_IsForced()
        {
            // Plain "logoff" lets open programs stop the log off to ask about unsaved work.
            Assert.That(RdpLogOffKeys.LogOffCommand, Is.EqualTo("shutdown /l /f"));
        }

        [Test]
        public void OpenRunBox_HoldsWindowsWhileRIsPressed()
        {
            RdpLogOffKeys.Key[] keys = RdpLogOffKeys.OpenRunBox(InstalledLayout("en-US")).ToArray();

            Assert.That(keys, Is.EqualTo(new[]
            {
                new RdpLogOffKeys.Key(ScanLeftWindowsExtended, false),
                new RdpLogOffKeys.Key(0x13, false),
                new RdpLogOffKeys.Key(0x13, true),
                new RdpLogOffKeys.Key(ScanLeftWindowsExtended, true),
            }));
        }

        [Test]
        public void TypeLogOffCommand_OnUsLayout_TypesTheCommandThenEnter()
        {
            // Scan codes (set 1) of "shutdown /l /f" on a US keyboard, then Enter; no Shift needed.
            int[] expected = [0x1F, 0x23, 0x16, 0x14, 0x20, 0x18, 0x11, 0x31, 0x39, 0x35, 0x26, 0x39, 0x35, 0x21, 0x1C];

            RdpLogOffKeys.Key[] keys = RdpLogOffKeys.TypeLogOffCommand(InstalledLayout("en-US")).ToArray();

            Assert.That(keys.Where(key => !key.KeyUp).Select(key => key.KeyData), Is.EqualTo(expected));
        }

        [Test]
        public void TypeLogOffCommand_OnGermanLayout_TypesSlashAsShift7()
        {
            // On a German keyboard "/" is Shift+7: the reason each character is looked up in the layout.
            RdpLogOffKeys.Key[] keys = RdpLogOffKeys.TypeLogOffCommand(InstalledLayout("de-DE")).ToArray();

            int slash = Array.IndexOf(keys, new RdpLogOffKeys.Key(ScanLeftShift, false));
            Assert.That(keys.Skip(slash).Take(4), Is.EqualTo(new[]
            {
                new RdpLogOffKeys.Key(ScanLeftShift, false),
                new RdpLogOffKeys.Key(0x08, false),
                new RdpLogOffKeys.Key(0x08, true),
                new RdpLogOffKeys.Key(ScanLeftShift, true),
            }));
        }

        [Test]
        public void EveryKeyPressedIsReleased()
        {
            // A key left down in the session (Windows, Shift) would change what the user types next.
            foreach (IReadOnlyList<RdpLogOffKeys.Key> sequence in new[] { RdpLogOffKeys.OpenRunBox(CurrentLayout), RdpLogOffKeys.TypeLogOffCommand(CurrentLayout) })
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
            IReadOnlyList<RdpLogOffKeys.Key> keys = RdpLogOffKeys.OpenRunBox(CurrentLayout);

            RdpProtocol.SendKeys(keyboard, keys);

            Assert.That(sent.Select(s => s.Count), Is.All.EqualTo(1));
            Assert.That(sent.Select(s => (s.KeyUp, s.KeyData)), Is.EqualTo(keys.Select(k => (k.KeyUp, k.KeyData))));
        }
    }
}
