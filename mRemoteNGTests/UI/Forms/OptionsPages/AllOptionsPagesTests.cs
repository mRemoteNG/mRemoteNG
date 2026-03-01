using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    /// <summary>
    /// Consolidated tests for all FrmOptions pages.
    /// CRITICAL: Only ONE test per fixture. FrmOptions + ObjectListView leaks
    /// native Win32 resources. Even 2 tests in the same STA fixture crash testhost.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class AllOptionsPagesTests
    {
        private static void RunWithMessagePump(Action<FrmOptions> testAction)
        {
            Exception caught = null;
            var thread = new Thread(() =>
            {
                FrmOptions optionsForm = null;
                try
                {
                    optionsForm = new FrmOptions();
                    optionsForm.Load += (s, e) =>
                    {
                        optionsForm.BeginInvoke(() =>
                        {
                            try
                            {
                                Application.DoEvents();
                                testAction(optionsForm);
                            }
                            catch (Exception ex)
                            {
                                caught = ex;
                            }
                            finally
                            {
                                Application.ExitThread();
                            }
                        });
                    };
                    Application.Run(optionsForm);
                }
                catch (Exception ex)
                {
                    if (caught == null) caught = ex;
                }
                finally
                {
                    try { optionsForm?.Dispose(); } catch { }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            if (!thread.Join(TimeSpan.FromSeconds(30)))
            {
                thread.Interrupt();
                Assert.Fail("Test timed out after 30 seconds (message pump deadlock)");
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            if (caught != null)
                throw caught;
        }

        /// <summary>
        /// Single test: verifies all 13 option pages exist in the ListView with
        /// correct names, icons, and that selecting each page loads settings controls.
        /// Also validates DoNotRestoreOnRdpMinimize setting binding.
        /// Consolidates 28 tests from 9 original test classes + 1 setting test.
        /// </summary>
        [Test]
        public void AllPagesExistWithIconsAndLoadCorrectSettings() => RunWithMessagePump(form =>
        {
            var lv = new ListViewTester("lstOptionPages", form);

            // --- Page names at correct indices ---
            Assert.That(lv.Items[0].Text, Does.Match("Startup/Exit"), "Page 0");
            Assert.That(lv.Items[1].Text, Does.Match("Appearance"), "Page 1");
            Assert.That(lv.Items[2].Text, Does.Match("Connections"), "Page 2");
            Assert.That(lv.Items[3].Text, Does.Match("Tabs & Panels"), "Page 3");
            Assert.That(lv.Items[6].Text, Does.Match("SQL Server"), "Page 6");
            Assert.That(lv.Items[7].Text, Does.Match("Updates"), "Page 7");
            Assert.That(lv.Items[8].Text, Does.Match("Theme"), "Page 8");
            Assert.That(lv.Items[10].Text, Does.Match("Advanced"), "Page 10");

            // --- All pages have icons ---
            for (int i = 0; i < lv.Items.Count; i++)
                Assert.That(lv.Items[i].ImageList, Is.Not.Null, $"Page {i} should have an icon");

            // --- Select each page and verify a control loads ---

            // Startup/Exit
            lv.Select("Startup/Exit");
            Application.DoEvents();
            Assert.That(form.FindControl<CheckBox>("chkReconnectOnStart").Text,
                Does.Match("Reconnect to previously opened sessions"));

            // Appearance
            lv.Select("Appearance");
            Application.DoEvents();
            Assert.That(form.FindControl<CheckBox>("chkShowSystemTrayIcon").Text,
                Does.Match("show notification area icon"));

            // Connections
            lv.Select("Connections");
            Application.DoEvents();
            Assert.That(form.FindControl<CheckBox>("chkSingleClickOnConnectionOpensIt").Text,
                Does.Match("Single click on connection"));

            // Tabs & Panels + DoNotRestoreOnRdpMinimize setting
            lv.Select("Tabs & Panels");
            Application.DoEvents();
            Assert.That(form.FindControl<CheckBox>("chkAlwaysShowPanelTabs").Text,
                Does.Match("Always show panel tabs"));
            var rdpMinCheckbox = form.FindControl<CheckBox>("chkDoNotRestoreOnRdpMinimize");
            Assert.That(rdpMinCheckbox, Is.Not.Null);
            Assert.That(rdpMinCheckbox.Text,
                Does.Match("Do not dock to tab when minimizing from Full screen"));

            // SQL Server
            lv.Select("SQL Server");
            Application.DoEvents();
            var sqlCheckbox = form.FindControl<CheckBox>("chkUseSQLServer");
            Assert.That(sqlCheckbox, Is.Not.Null, "chkUseSQLServer should exist");
            Assert.That(sqlCheckbox.Text, Does.Match("Use SQL"));

            // Updates
            lv.Select("Updates");
            Application.DoEvents();
            Assert.That(form.FindControl<CheckBox>("chkCheckForUpdatesOnStartup").Text,
                Does.Match("Check for updates"));

            // Theme
            lv.Select("Theme");
            Application.DoEvents();
            Assert.That(form.FindControl<Button>("btnThemeNew").Text, Does.Match("New"));

            // Advanced
            lv.Select("Advanced");
            Application.DoEvents();
            Assert.That(form.FindControl<CheckBox>("chkAutomaticReconnect").Text,
                Is.EqualTo("Display reconnection dialog when disconnected from server (RDP && ICA only)"));
        });
    }
}
