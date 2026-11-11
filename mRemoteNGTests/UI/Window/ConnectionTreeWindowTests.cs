using System.Collections.Generic;
using System.Linq;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls.ConnectionTree;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNGTests.UI.Window
{
    [Apartment(ApartmentState.STA)]
    public class ConnectionTreeWindowTests
    {
        private ConnectionTreeWindow _connectionTreeWindow;

        [SetUp]
        public void Setup()
        {
            _connectionTreeWindow = new ConnectionTreeWindow(new DockContent());
        }

        [TearDown]
        public void Teardown()
        {
            _connectionTreeWindow.Close();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void CanShowWindow()
        {
            _connectionTreeWindow.Show();
            Assert.That(_connectionTreeWindow.Visible);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void PreviousSessionsAreReopenedWhenAutomaticReconnectIsTicked()
        {
            // OptionsAdvancedPage.NoReconnect is the "Automatically try to reconnect when disconnected
            // from server" checkbox; it used to turn off reopening the previous sessions (#3178)
            List<IConnectionTreeDelegate> actions = PostSetupActions(reopenPreviousSessions: true, automaticReconnect: true, noReconnectSwitch: false);

            Assert.That(actions, Has.Some.InstanceOf<PreviousSessionOpener>());
        }

        [Test, Apartment(ApartmentState.STA)]
        public void NoReconnectSwitchSkipsThePreviousSessions()
        {
            List<IConnectionTreeDelegate> actions = PostSetupActions(reopenPreviousSessions: true, automaticReconnect: false, noReconnectSwitch: true);

            Assert.That(actions, Has.None.InstanceOf<PreviousSessionOpener>());
        }

        private static List<IConnectionTreeDelegate> PostSetupActions(bool reopenPreviousSessions, bool automaticReconnect, bool noReconnectSwitch)
        {
            bool originalReopenPreviousSessions = OptionsStartupExitPage.Default.OpenConsFromLastSession;
            bool originalNoReconnect = OptionsAdvancedPage.Default.NoReconnect;
            bool originalNoReconnectSwitch = Runtime.NoReconnectToPreviousSessions;
            try
            {
                OptionsStartupExitPage.Default.OpenConsFromLastSession = reopenPreviousSessions;
                OptionsAdvancedPage.Default.NoReconnect = automaticReconnect;
                Runtime.NoReconnectToPreviousSessions = noReconnectSwitch;

                using ConnectionTreeWindow window = new(new DockContent());
                return window.ConnectionTree.PostSetupActions.ToList();
            }
            finally
            {
                OptionsStartupExitPage.Default.OpenConsFromLastSession = originalReopenPreviousSessions;
                OptionsAdvancedPage.Default.NoReconnect = originalNoReconnect;
                Runtime.NoReconnectToPreviousSessions = originalNoReconnectSwitch;
            }
        }
    }
}