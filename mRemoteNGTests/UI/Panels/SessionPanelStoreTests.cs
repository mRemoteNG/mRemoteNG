using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;

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
            Dictionary<string, string> connections = new()
            {
                ["id-1"] = "Prod",
                ["id-2"] = "General",
                ["id-3"] = "R&D <lab>"
            };
            Dictionary<string, FloatingPanelPlacement> floating = new()
            {
                ["Prod"] = new FloatingPanelPlacement(new Rectangle(-1900, 40, 1600, 900), false),
                ["R&D <lab>"] = new FloatingPanelPlacement(new Rectangle(100, 200, 800, 600), true)
            };

            new SessionPanelStore(_filePath).Save(new SessionPanels(connections, floating));
            SessionPanels loaded = new SessionPanelStore(_filePath).Load();

            Assert.That(loaded.ConnectionPanels, Is.EquivalentTo(connections));
            Assert.That(loaded.FloatingPanels, Is.EquivalentTo(floating));
        }

        [Test]
        public void NoFileLoadsNothing()
        {
            SessionPanels loaded = new SessionPanelStore(_filePath).Load();

            Assert.That(loaded.ConnectionPanels, Is.Empty);
            Assert.That(loaded.FloatingPanels, Is.Empty);
        }

        [Test]
        public void DamagedFileLoadsNothing()
        {
            Directory.CreateDirectory(_folder);
            File.WriteAllText(_filePath, "<SessionPanels><Connection Id=");

            SessionPanels loaded = new SessionPanelStore(_filePath).Load();

            Assert.That(loaded.ConnectionPanels, Is.Empty);
            Assert.That(loaded.FloatingPanels, Is.Empty);
        }

        [Test]
        public void IncompleteEntriesAreSkipped()
        {
            Directory.CreateDirectory(_folder);
            File.WriteAllText(_filePath,
                "<SessionPanels>" +
                "<Panel Name=\"Prod\" Bounds=\"10,20,300,200\" Maximized=\"false\" />" +
                "<Panel Name=\"NoBounds\" />" +
                "<Panel Name=\"BadBounds\" Bounds=\"10,20,abc,200\" />" +
                "<Panel Name=\"EmptySize\" Bounds=\"10,20,0,200\" />" +
                "<Panel Bounds=\"10,20,300,200\" />" +
                "<Connection Id=\"id-1\" Panel=\"Prod\" />" +
                "<Connection Panel=\"NoId\" />" +
                "<Connection Id=\"id-2\" />" +
                "<Connection Id=\"id-3\" Panel=\"\" />" +
                "</SessionPanels>");

            SessionPanels loaded = new SessionPanelStore(_filePath).Load();

            Assert.That(loaded.ConnectionPanels, Is.EquivalentTo(new Dictionary<string, string> { ["id-1"] = "Prod" }));
            Assert.That(loaded.FloatingPanels, Is.EquivalentTo(new Dictionary<string, FloatingPanelPlacement>
            {
                ["Prod"] = new(new Rectangle(10, 20, 300, 200), false)
            }));
        }

        [Test]
        public void SavingNothingLeavesAnEmptyStore()
        {
            SessionPanelStore store = new(_filePath);
            store.Save(new SessionPanels(new Dictionary<string, string> { ["id-1"] = "Prod" },
                                         new Dictionary<string, FloatingPanelPlacement>()));

            store.Save(SessionPanels.Empty);

            Assert.That(store.Load().ConnectionPanels, Is.Empty);
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

            SessionPanels panels = SessionPanelStore.Collect([prod, lab]);

            Assert.That(panels.ConnectionPanels, Is.EquivalentTo(new Dictionary<string, string>
            {
                [server1.ConstantID] = "Prod",
                [server2.ConstantID] = "Prod",
                [server3.ConstantID] = "R&D"
            }));
            Assert.That(panels.FloatingPanels, Is.Empty, "docked panels have no placement");
        }

        [Test]
        public void CollectKeepsTheFirstPanelOfAConnectionOpenTwice()
        {
            using Form hostForm = new() { Width = 800, Height = 600, ShowInTaskbar = false };
            hostForm.Show();
            ConnectionInfo server = new();
            using ConnectionWindow first = ShowPanel(hostForm, "First", server);
            using ConnectionWindow second = ShowPanel(hostForm, "Second", server);

            SessionPanels panels = SessionPanelStore.Collect([first, second]);

            Assert.That(panels.ConnectionPanels[server.ConstantID], Is.EqualTo("First"));
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

            Assert.That(SessionPanelStore.Collect([general]).ConnectionPanels, Is.Empty);
        }

        [Test]
        public void CollectRecordsWhereAFloatingPanelIs()
        {
            using Form hostForm = new()
            {
                Width = 800, Height = 600, ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual, Location = new Point(0, 0)
            };
            DockPanel mainDock = new()
            {
                Dock = DockStyle.Fill, DocumentStyle = DocumentStyle.DockingWindow, Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(mainDock);
            hostForm.Show();

            Rectangle workingArea = Screen.PrimaryScreen!.WorkingArea;
            Rectangle bounds = new(workingArea.Left + 20, workingArea.Top + 30, 640, 480);
            ConnectionInfo server = new();
            using ConnectionWindow floating = new(new DockContent(), "Floating");
            floating.ShowFloating(mainDock, new FloatingPanelPlacement(bounds, false));
            new ConnectionTab { Tag = server }.Show(floating.connDock, DockState.Document);
            Application.DoEvents();

            SessionPanels panels = SessionPanelStore.Collect([floating]);

            Assert.That(floating.DockState, Is.EqualTo(DockState.Float));
            Assert.That(panels.ConnectionPanels[server.ConstantID], Is.EqualTo("Floating"));
            Assert.That(panels.FloatingPanels["Floating"], Is.EqualTo(new FloatingPanelPlacement(bounds, false)));
        }

        [Test]
        public void FloatingPanelWithoutConnectionsIsNotRecorded()
        {
            using Form hostForm = new() { Width = 800, Height = 600, ShowInTaskbar = false };
            DockPanel mainDock = new()
            {
                Dock = DockStyle.Fill, DocumentStyle = DocumentStyle.DockingWindow, Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(mainDock);
            hostForm.Show();

            Rectangle workingArea = Screen.PrimaryScreen!.WorkingArea;
            using ConnectionWindow empty = new(new DockContent(), "Empty");
            empty.ShowFloating(mainDock,
                new FloatingPanelPlacement(new Rectangle(workingArea.Location, new Size(400, 300)), false));
            Application.DoEvents();

            Assert.That(SessionPanelStore.Collect([empty]).FloatingPanels, Is.Empty);
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
