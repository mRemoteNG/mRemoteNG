using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Panels
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SessionPanelStoreTests
    {
        private string _folder;
        private string _filePath;

        [SetUp]
        public void Setup()
        {
            _folder = Path.Combine(Path.GetTempPath(), "mRemoteNGTests-" + Guid.NewGuid().ToString("N"));
            _filePath = Path.Combine(_folder, SessionPanelStore.DefaultFileName);
        }

        [TearDown]
        public void Teardown()
        {
            if (Directory.Exists(_folder))
                Directory.Delete(_folder, true);
        }

        [Test]
        public void SavedPanelsAreLoadedBack()
        {
            SessionPanelStore store = new(_filePath);
            Dictionary<string, string> panels = new()
            {
                ["id-1"] = "Prod",
                ["id-2"] = "General",
                ["id-3"] = "R&D <lab>"
            };

            store.Save(panels);

            Assert.That(new SessionPanelStore(_filePath).Load(), Is.EquivalentTo(panels));
        }

        [Test]
        public void NoFileLoadsNothing()
        {
            Assert.That(new SessionPanelStore(_filePath).Load(), Is.Empty);
        }

        [Test]
        public void DamagedFileLoadsNothing()
        {
            Directory.CreateDirectory(_folder);
            File.WriteAllText(_filePath, "<SessionPanels><Connection Id=");

            Assert.That(new SessionPanelStore(_filePath).Load(), Is.Empty);
        }

        [Test]
        public void EntriesWithoutIdOrPanelAreSkipped()
        {
            Directory.CreateDirectory(_folder);
            File.WriteAllText(_filePath,
                "<SessionPanels>" +
                "<Connection Id=\"id-1\" Panel=\"Prod\" />" +
                "<Connection Panel=\"NoId\" />" +
                "<Connection Id=\"id-2\" />" +
                "<Connection Id=\"id-3\" Panel=\"\" />" +
                "</SessionPanels>");

            IReadOnlyDictionary<string, string> panels = new SessionPanelStore(_filePath).Load();

            Assert.That(panels, Is.EquivalentTo(new Dictionary<string, string> { ["id-1"] = "Prod" }));
        }

        [Test]
        public void SavingNothingLeavesAnEmptyStore()
        {
            SessionPanelStore store = new(_filePath);
            store.Save(new Dictionary<string, string> { ["id-1"] = "Prod" });

            store.Save(new Dictionary<string, string>());

            Assert.That(store.Load(), Is.Empty);
        }

        [Test]
        public void CollectFindsTheConnectionsInEachPanel()
        {
            using Form hostForm = new() { Width = 800, Height = 600, ShowInTaskbar = false };
            hostForm.Show();
            ConnectionInfo server1 = new();
            ConnectionInfo server2 = new();
            ConnectionInfo server3 = new();
            using ConnectionWindow prod = ShowPanel(hostForm, "Prod", server1, server2);
            using ConnectionWindow lab = ShowPanel(hostForm, "R&&D", server3);

            Dictionary<string, string> panels = SessionPanelStore.Collect([prod, lab]);

            Assert.That(panels, Is.EquivalentTo(new Dictionary<string, string>
            {
                [server1.ConstantID] = "Prod",
                [server2.ConstantID] = "Prod",
                [server3.ConstantID] = "R&D"
            }));
        }

        [Test]
        public void CollectKeepsTheFirstPanelOfAConnectionOpenTwice()
        {
            using Form hostForm = new() { Width = 800, Height = 600, ShowInTaskbar = false };
            hostForm.Show();
            ConnectionInfo server = new();
            using ConnectionWindow first = ShowPanel(hostForm, "First", server);
            using ConnectionWindow second = ShowPanel(hostForm, "Second", server);

            Dictionary<string, string> panels = SessionPanelStore.Collect([first, second]);

            Assert.That(panels[server.ConstantID], Is.EqualTo("First"));
        }

        [Test]
        public void CollectSkipsDocumentsThatAreNotConnections()
        {
            using Form hostForm = new() { Width = 800, Height = 600, ShowInTaskbar = false };
            hostForm.Show();
            using ConnectionWindow general = ShowPanel(hostForm, "General");
            using DockContent optionsLikeWindow = new();
            optionsLikeWindow.Show(general.connDock, DockState.Document);
            Application.DoEvents();

            Assert.That(SessionPanelStore.Collect([general]), Is.Empty);
        }

        private static ConnectionWindow ShowPanel(Form hostForm, string tabText, params ConnectionInfo[] connections)
        {
            ConnectionWindow panel = new(new DockContent(), tabText) { TopLevel = false };
            hostForm.Controls.Add(panel);
            panel.Show();

            // Shown directly: AddConnectionTab would dock the panel into FrmMain.Default
            foreach (ConnectionInfo connection in connections)
                new ConnectionTab { Tag = connection, TabText = connection.Name }.Show(panel.connDock, DockState.Document);

            Application.DoEvents();
            return panel;
        }
    }
}
