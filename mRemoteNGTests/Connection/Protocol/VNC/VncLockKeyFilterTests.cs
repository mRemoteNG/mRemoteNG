using System;
using System.Reflection;
using System.Windows.Forms;
using NUnit.Framework;
using VncSharpCore;

namespace mRemoteNGTests.Connection.Protocol.VNC
{
    [TestFixture]
    public class VncLockKeyFilterTests
    {
        private RemoteDesktop _remoteDesktop;
        private object _filter;
        private MockVncClient _mockClient;

        public class MockVncClient : VncClient
        {
            public uint LastKeysym;
            public bool LastPressed;
            public int CallCount;

            // Hide the base method to intercept the call.
            // VncLockKeyFilter uses .GetType().GetMethod(), so it will find this method on the instance.
            public new void WriteKeyboardEvent(uint keysym, bool pressed)
            {
                LastKeysym = keysym;
                LastPressed = pressed;
                CallCount++;
            }
        }

        [SetUp]
        public void Setup()
        {
            _remoteDesktop = new RemoteDesktop();
            _mockClient = new MockVncClient();

            // Set the private 'vnc' field
            var vncField = typeof(RemoteDesktop).GetField("vnc", BindingFlags.NonPublic | BindingFlags.Instance);
            if (vncField == null) Assert.Fail("Could not find 'vnc' field on RemoteDesktop");
            
            try 
            {
                vncField.SetValue(_remoteDesktop, _mockClient);
            }
            catch (ArgumentException)
            {
                Assert.Fail($"MockVncClient is not assignable to field type {vncField.FieldType.Name}. Is VncClient sealed?");
            }

            // Create VncLockKeyFilter using reflection
            // It is internal, so we need to find the type in mRemoteNG assembly
            // Use a known public type from mRemoteNG to get the assembly
            var asm = typeof(mRemoteNG.Connection.Protocol.VNC.ProtocolVNC).Assembly; 
            var filterType = asm.GetType("mRemoteNG.Connection.Protocol.VNC.VncLockKeyFilter");
            if (filterType == null) Assert.Fail("Could not find VncLockKeyFilter type");

            _filter = Activator.CreateInstance(filterType, _remoteDesktop);
        }

        [Test]
        public void PreFilterMessage_Intercepts_Cyrillic_Small_a()
        {
            // Cyrillic 'а' is U+0430. Expected keysym 0x06C1.
            char c = '\u0430';
            // WM_CHAR = 0x0102
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0102, (IntPtr)c, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.True, "Should return true to suppress original message");
            Assert.That(_mockClient.CallCount, Is.EqualTo(2), "Should call WriteKeyboardEvent twice (down/up)");
            Assert.That(_mockClient.LastKeysym, Is.EqualTo(0x06C1));
        }

        [Test]
        public void PreFilterMessage_Intercepts_Cyrillic_Capital_A()
        {
            // Cyrillic 'А' is U+0410. Expected keysym 0x06E1.
            char c = '\u0410';
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0102, (IntPtr)c, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.True);
            Assert.That(_mockClient.LastKeysym, Is.EqualTo(0x06E1));
        }

        [Test]
        public void PreFilterMessage_Intercepts_Cyrillic_Yo_Small()
        {
            // 'ё' U+0451 -> 0x06A3
            char c = '\u0451';
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0102, (IntPtr)c, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.True);
            Assert.That(_mockClient.LastKeysym, Is.EqualTo(0x06A3));
        }

        [Test]
        public void PreFilterMessage_Ignores_Latin_Chars()
        {
            // 'a' U+0061 — ASCII, should pass through to VncSharpCore unchanged
            char c = 'a';
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0102, (IntPtr)c, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.False, "Should return false for ASCII characters");
            Assert.That(_mockClient.CallCount, Is.EqualTo(0));
        }

        [Test]
        public void PreFilterMessage_Intercepts_DeadKey_Composed_a_Acute()
        {
            // Dead-key composed character: 'á' U+00E1 (e.g. dead ' + a on many non-US layouts)
            // Expected X11 Unicode keysym: 0x01000000 | 0x00E1 = 0x010000E1
            char c = '\u00E1'; // á
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0102, (IntPtr)c, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.True, "Should intercept and suppress non-ASCII WM_CHAR");
            Assert.That(_mockClient.CallCount, Is.EqualTo(2), "Should send key-down then key-up");
            Assert.That(_mockClient.LastKeysym, Is.EqualTo(0x010000E1u), "Should use X11 Unicode keysym encoding");
        }

        [Test]
        public void PreFilterMessage_Intercepts_DeadKey_Composed_n_Tilde()
        {
            // Dead-key composed character: 'ñ' U+00F1 (e.g. dead ~ + n on Spanish layout)
            // Expected X11 Unicode keysym: 0x01000000 | 0x00F1 = 0x010000F1
            char c = '\u00F1'; // ñ
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0102, (IntPtr)c, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.True);
            Assert.That(_mockClient.LastKeysym, Is.EqualTo(0x010000F1u));
        }

        [Test]
        public void PreFilterMessage_Intercepts_LeftWindowsKey()
        {
            // VK_LWIN = 0x5B
            // WM_KEYDOWN = 0x0100
            // XK_Super_L = 0xFFEB
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0100, (IntPtr)0x5B, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.True, "Should return true to suppress original message");
            Assert.That(_mockClient.CallCount, Is.EqualTo(1));
            Assert.That(_mockClient.LastKeysym, Is.EqualTo(0xFFEB));
            Assert.That(_mockClient.LastPressed, Is.True);
        }

        [Test]
        public void PreFilterMessage_Intercepts_RightWindowsKey()
        {
            // VK_RWIN = 0x5C
            // WM_KEYDOWN = 0x0100
            // XK_Super_R = 0xFFEC
            Message msg = Message.Create(_remoteDesktop.Handle, 0x0100, (IntPtr)0x5C, IntPtr.Zero);

            bool result = InvokePreFilterMessage(ref msg);

            Assert.That(result, Is.True, "Should return true to suppress original message");
            Assert.That(_mockClient.CallCount, Is.EqualTo(1));
            Assert.That(_mockClient.LastKeysym, Is.EqualTo(0xFFEC));
            Assert.That(_mockClient.LastPressed, Is.True);
        }

        [Test]
        public void ReleaseAllModifiers_SendsKeyUpForAllSixModifiers()
        {
            // Arrange: MockVncClient is already wired in SetUp.
            var method = _filter.GetType().GetMethod("ReleaseAllModifiers", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(method, Is.Not.Null, "ReleaseAllModifiers should be a public method on VncLockKeyFilter");

            // Act
            method.Invoke(_filter, null);

            // Assert: six modifier keysyms × 1 call each, all with pressed=false
            // Shift_L, Shift_R, Control_L, Control_R, Alt_L, Alt_R
            Assert.That(_mockClient.CallCount, Is.EqualTo(6),
                "Should send key-up for each of the 6 modifier keysyms");
            Assert.That(_mockClient.LastPressed, Is.False,
                "All calls must be key-up (pressed=false)");
        }

        [Test]
        public void ReleaseAllModifiers_WithNullVncClient_DoesNotThrow()
        {
            // Arrange: clear the vnc field so vncClient resolves to null
            var vncField = typeof(RemoteDesktop).GetField("vnc", BindingFlags.NonPublic | BindingFlags.Instance);
            vncField?.SetValue(_remoteDesktop, null);

            var method = _filter.GetType().GetMethod("ReleaseAllModifiers", BindingFlags.Public | BindingFlags.Instance);

            // Act & Assert: no exception even when not connected
            Assert.DoesNotThrow(() => method.Invoke(_filter, null));
            Assert.That(_mockClient.CallCount, Is.EqualTo(0));
        }

        // Helper to invoke PreFilterMessage
        private bool InvokePreFilterMessage(ref Message m)
        {
            var method = _filter.GetType().GetMethod("PreFilterMessage", BindingFlags.Public | BindingFlags.Instance);
            object[] args = new object[] { m };
            bool result = (bool)method.Invoke(_filter, args);
            m = (Message)args[0]; // Ref update
            return result;
        }
    }
}
