using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Config;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Forms
{
    /// <summary>
    /// Exiting asks "Do you want to close all open connections?" only when connections are open.
    /// The Options window is shown in the connection panel and stays there when hidden.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class FrmMainExitConfirmationTests
    {
        [Test]
        public void HiddenOptionsWindowIsNotCountedAsAConnection()
        {
            using var hostForm = new Form { Width = 800, Height = 600, ShowInTaskbar = false };
            hostForm.Show();
            using var connectionWindow = new ConnectionWindow(new DockContent());
            connectionWindow.TopLevel = false;
            hostForm.Controls.Add(connectionWindow);
            connectionWindow.Show();
            DockPanel connDock = connectionWindow.Controls.OfType<DockPanel>().First();

            using var optionsLikeWindow = new DockContent { HideOnClose = true };
            optionsLikeWindow.Show(connDock, DockState.Document);
            optionsLikeWindow.Hide();
            Application.DoEvents();

            Assert.That(FrmMain.CountOpenConnections([connectionWindow]), Is.Zero);
        }

        [Test]
        public void ConnectionTabsAreCounted()
        {
            using var hostForm = new Form { Width = 800, Height = 600, ShowInTaskbar = false };
            hostForm.Show();
            using var connectionWindow = new ConnectionWindow(new DockContent());
            connectionWindow.TopLevel = false;
            hostForm.Controls.Add(connectionWindow);
            connectionWindow.Show();
            DockPanel connDock = connectionWindow.Controls.OfType<DockPanel>().First();

            // shown directly: AddConnectionTab would dock the window into FrmMain.Default
            using var server1 = new ConnectionTab { TabText = "Server1" };
            using var server2 = new ConnectionTab { TabText = "Server2" };
            server1.Show(connDock, DockState.Document);
            server2.Show(connDock, DockState.Document);
            Application.DoEvents();

            Assert.That(FrmMain.CountOpenConnections([connectionWindow]), Is.EqualTo(2));
        }

        [TestCase(ConfirmCloseEnum.All)]
        [TestCase(ConfirmCloseEnum.Multiple)]
        [TestCase(ConfirmCloseEnum.Exit)]
        [TestCase(ConfirmCloseEnum.Never)]
        public void NoQuestionWithoutOpenConnections(ConfirmCloseEnum setting)
        {
            Assert.That(FrmMain.ShouldConfirmExit((int)setting, 0), Is.False);
        }

        [TestCase(ConfirmCloseEnum.All, 1, true)]
        [TestCase(ConfirmCloseEnum.Exit, 1, true)]
        [TestCase(ConfirmCloseEnum.Multiple, 1, false)]
        [TestCase(ConfirmCloseEnum.Multiple, 2, true)]
        [TestCase(ConfirmCloseEnum.Never, 3, false)]
        public void QuestionFollowsTheSetting(ConfirmCloseEnum setting, int openConnections, bool expected)
        {
            Assert.That(FrmMain.ShouldConfirmExit((int)setting, openConnections), Is.EqualTo(expected));
        }
    }
}
