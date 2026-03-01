using NUnit.Framework;
using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using mRemoteNGTests.TestHelpers;
using System.Linq;

namespace mRemoteNGTests.UI.Forms
{
    /// <summary>
    /// Tests for FrmOptions. CRITICAL: Only ONE test allowed per fixture.
    /// FrmOptions + ObjectListView leaks native Win32 resources (GDI handles,
    /// window class registrations). Even 2 tests that touch FrmOptions in the
    /// same testhost process crash it. All assertions are in a single test.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsFormTests
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
        /// Single test that validates FrmOptions construction, controls, ListView,
        /// page selection, OK button, and change tracking.
        /// </summary>
        [Test]
        public void FormBehavior() => RunWithMessagePump(optionsForm =>
        {
            // 1. Controls are created
            var pnlMain = optionsForm.FindControl<Panel>("pnlMain");
            Assert.That(pnlMain, Is.Not.Null, "pnlMain should exist");
            Assert.That(pnlMain.Controls.Count, Is.GreaterThan(0), "pnlMain should have child controls");

            // 2. First page is not disposed
            var firstPage = pnlMain.Controls[0];
            Assert.That(firstPage.IsDisposed, Is.False, "Page should not be disposed");

            // 3. ListView has all 13 options pages
            ListViewTester listViewTester = new("lstOptionPages", optionsForm);
            Assert.That(listViewTester.Items.Count, Is.EqualTo(13));

            // 4. SelectedObject is set
            var lstOptionPages = optionsForm.GetType()
                .GetField("lstOptionPages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(optionsForm);
            Assert.That(lstOptionPages, Is.Not.Null, "lstOptionPages should exist");
            var selectedObject = lstOptionPages.GetType()
                .GetProperty("SelectedObject")
                ?.GetValue(lstOptionPages);
            Assert.That(selectedObject, Is.Not.Null, "SelectedObject should not be null");

            // 5. OK button sets DialogResult
            Button okButton = optionsForm.FindControl<Button>("btnOK");
            okButton.PerformClick();
            Assert.That(optionsForm.DialogResult, Is.EqualTo(DialogResult.OK));

            // 6. Change tracking - toggle a checkbox
            Application.DoEvents();
            var optionsPage = pnlMain.Controls[0] as mRemoteNG.UI.Forms.OptionsPages.OptionsPage;
            Assert.That(optionsPage, Is.Not.Null, "First control in pnlMain should be an OptionsPage");
            var checkBoxes = optionsPage.GetAllControls().OfType<CheckBox>().ToList();
            Assert.That(checkBoxes.Count, Is.GreaterThan(0), "Options page should have at least one checkbox");
            var checkBox = checkBoxes[0];
            checkBox.Checked = !checkBox.Checked;
            Application.DoEvents();
            Assert.That(optionsPage.HasChanges, Is.True, "Toggling a checkbox should mark the page as having changes");
        });
    }
}
