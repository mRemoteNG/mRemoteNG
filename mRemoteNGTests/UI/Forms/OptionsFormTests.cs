using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using System.Linq;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsFormTests : OptionsFormSetupAndTeardown
    {
        // The form no longer closes itself: OK and Cancel ask whoever hosts it to close it.
        [Test]
        public void ClickingCancelButtonRequestsClose()
        {
            bool closeRequested = false;
            _optionsForm.CloseRequested += (o, e) => closeRequested = true;
            Button cancelButton = _optionsForm.FindControl<Button>("btnCancel");
            cancelButton.PerformClick();
            Assert.That(closeRequested, Is.True);
        }

        [Test]
        public void ClickingOKButtonRequestsClose()
        {
            bool closeRequested = false;
            _optionsForm.CloseRequested += (o, e) => closeRequested = true;
            Button okButton = _optionsForm.FindControl<Button>("btnOK");
            okButton.PerformClick();
            Assert.That(closeRequested, Is.True);
        }

        [Test]
        public void ListViewContainsOptionsPages()
        {
#if DEBUG
            // Options Management is added to debug builds only.
            const int pages = 14;
#else
            const int pages = 13;
#endif
            ListViewTester listViewTester = new("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items.Count, Is.EqualTo(pages));
        }

        [Test]
        public void ChangingOptionMarksPageAsChanged()
        {
            // Wait for all pages to load
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();

            // Get the options panel
            var pnlMain = _optionsForm.FindControl<Panel>("pnlMain");
            Assert.That(pnlMain, Is.Not.Null);

            if (pnlMain.Controls.Count > 0)
            {
                var optionsPage = pnlMain.Controls[0] as mRemoteNG.UI.Forms.OptionsPages.OptionsPage;
                Assert.That(optionsPage, Is.Not.Null);

                // Find a checkbox in the options page (Controls.Find throws on an empty key)
                var checkBoxes = AllControls(optionsPage).OfType<CheckBox>().ToList();
                
                if (checkBoxes.Count > 0)
                {
                    var checkBox = checkBoxes[0];
                    bool originalValue = checkBox.Checked;
                    checkBox.Checked = !originalValue;
                    Application.DoEvents();
                    
                    // Verify the page is marked as changed
                    Assert.That(optionsPage.HasChanges, Is.True);
                }
            }
        }

        [Test]
        public void MultipleOpenCloseWithoutConnectionsDoesNotFreeze()
        {
            // Test for issue #2907: Options panel should not freeze after multiple open/close cycles
            for (int i = 0; i < 25; i++)
            {
                // Show the form
                _optionsForm.Show();
                Application.DoEvents();
                System.Threading.Thread.Sleep(50);

                // Verify panel has content
                var pnlMain = _optionsForm.FindControl<Panel>("pnlMain");
                Assert.That(pnlMain, Is.Not.Null, $"pnlMain is null on iteration {i}");
                Assert.That(pnlMain.Controls.Count, Is.GreaterThan(0), $"pnlMain has no controls on iteration {i}");

                // Hide the form (simulating OK/Cancel)
                _optionsForm.Visible = false;
                Application.DoEvents();
            }

            // Final check - form should still be responsive
            _optionsForm.Show();
            Application.DoEvents();
            var finalPanel = _optionsForm.FindControl<Panel>("pnlMain");
            Assert.That(finalPanel.Controls.Count, Is.GreaterThan(0), "Final pnlMain has no controls after 25 cycles");
        }

        [Test]
        public void OptionsFormHasValidSelectedPageAfterMultipleShows()
        {
            // Test for issue #2907: lstOptionPages.SelectedObject should remain valid
            for (int i = 0; i < 10; i++)
            {
                _optionsForm.Show();
                Application.DoEvents();

                // Use reflection to check lstOptionPages.SelectedObject
                var lstOptionPages = _optionsForm.GetType()
                    .GetField("lstOptionPages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(_optionsForm);

                Assert.That(lstOptionPages, Is.Not.Null, $"lstOptionPages is null on iteration {i}");

                var selectedObject = lstOptionPages.GetType()
                    .GetProperty("SelectedObject")
                    ?.GetValue(lstOptionPages);

                Assert.That(selectedObject, Is.Not.Null, $"SelectedObject is null on iteration {i}");

                _optionsForm.Visible = false;
                Application.DoEvents();
            }
        }

        [Test]
        public void OptionsFormControlStateRemainsValidAfterHideShow()
        {
            // Test for issue #2907: Control handles should remain valid after hide/show
            _optionsForm.Show();
            Application.DoEvents();
            System.Threading.Thread.Sleep(500); // Wait for all pages to load

            var pnlMain = _optionsForm.FindControl<Panel>("pnlMain");
            Assert.That(pnlMain.Controls.Count, Is.GreaterThan(0));

            var firstPage = pnlMain.Controls[0];
            Assert.That(firstPage.IsHandleCreated, Is.True, "Page handle should be created initially");

            // Hide and show multiple times
            for (int i = 0; i < 5; i++)
            {
                _optionsForm.Visible = false;
                Application.DoEvents();
                _optionsForm.Show();
                Application.DoEvents();

                var currentPanel = _optionsForm.FindControl<Panel>("pnlMain");
                Assert.That(currentPanel.Controls.Count, Is.GreaterThan(0), $"Panel should have controls on iteration {i}");

                var currentPage = currentPanel.Controls[0];
                Assert.That(currentPage.IsHandleCreated, Is.True, $"Page handle should remain valid on iteration {i}");
                Assert.That(currentPage.IsDisposed, Is.False, $"Page should not be disposed on iteration {i}");
            }
        }

        [Test]
        public void RapidOpenCloseDoesNotCauseNullReference()
        {
            // Test for issue #2907: Rapid open/close should not cause null reference exceptions
            for (int i = 0; i < 50; i++)
            {
                _optionsForm.Show();
                _optionsForm.Visible = false;
                Application.DoEvents();
            }

            // Should be able to show normally after rapid cycles
            Assert.DoesNotThrow(() =>
            {
                _optionsForm.Show();
                Application.DoEvents();
            });
        }

        private static IEnumerable<Control> AllControls(Control parent) =>
            parent.Controls.Cast<Control>().SelectMany(control => new[] { control }.Concat(AllControls(control)));
    }
}