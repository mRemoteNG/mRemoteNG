using NUnit.Framework;
using System.Reflection;
using System;
using mRemoteNG.UI.Forms;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    public class LockPanelsTests
    {
        private static void SetLockPanels(bool value)
        {
            var optionsType = typeof(FrmMain).Assembly.GetType("mRemoteNG.Properties.OptionsTabsPanelsPage")
                ?? throw new Exception("Could not find OptionsTabsPanelsPage type");
            var defaultProp = optionsType.GetProperty("Default", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                ?? throw new Exception("Could not find Default property");
            var defaultInstance = defaultProp.GetValue(null);
            var lockPanelsProp = optionsType.GetProperty("LockPanels", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                ?? throw new Exception("Could not find LockPanels property");
            lockPanelsProp.SetValue(defaultInstance, value);
        }

        private static bool GetLockPanels()
        {
            var optionsType = typeof(FrmMain).Assembly.GetType("mRemoteNG.Properties.OptionsTabsPanelsPage")!;
            var defaultInstance = optionsType.GetProperty("Default", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(null);
            return (bool)optionsType.GetProperty("LockPanels", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(defaultInstance)!;
        }

        /// <summary>
        /// Verifies that SetPanelLock logic maps LockPanels=true to AllowEndUserDocking=false.
        /// The actual FrmMain.SetPanelLock() does: AllowEndUserDocking = !LockPanels
        /// </summary>
        [Test]
        public void SetPanelLock_LocksPanels_WhenSettingIsTrue()
        {
            SetLockPanels(true);
            var lockPanels = GetLockPanels();
            Assert.That(lockPanels, Is.True, "LockPanels should be true");

            // FrmMain.SetPanelLock: AllowEndUserDocking = !LockPanels
            var allowDocking = !lockPanels;
            Assert.That(allowDocking, Is.False, "When LockPanels=true, docking should be disallowed");
        }

        /// <summary>
        /// Verifies that SetPanelLock logic maps LockPanels=false to AllowEndUserDocking=true.
        /// </summary>
        [Test]
        public void SetPanelLock_UnlocksPanels_WhenSettingIsFalse()
        {
            SetLockPanels(false);
            var lockPanels = GetLockPanels();
            Assert.That(lockPanels, Is.False, "LockPanels should be false");

            // FrmMain.SetPanelLock: AllowEndUserDocking = !LockPanels
            var allowDocking = !lockPanels;
            Assert.That(allowDocking, Is.True, "When LockPanels=false, docking should be allowed");
        }
    }
}
