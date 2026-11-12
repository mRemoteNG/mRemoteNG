using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms
{
	[TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PasswordFormTests
    {
        FrmPassword _passwordForm;

        [SetUp]
        public void Setup()
        {
            _passwordForm = new FrmPassword();
            _passwordForm.Show();
        }

        [TearDown]
        public void Teardown()
        {
            _passwordForm.Dispose();
            while (_passwordForm.Disposing)
            {
            }
            _passwordForm = null;
        }

        [Test]
		[SetUICulture("en-US")]
        public void PasswordFormText()
        {
            Assert.That(_passwordForm.Text, Does.Match("mRemoteNG password"));
        }

        [Test]
        public void ClickingCancelClosesPasswordForm()
        {
            bool eventFired = false;
            _passwordForm.FormClosed += (o, e) => eventFired = true;
            Button cancelButton = _passwordForm.FindControl<Button>("btnCancel");
            cancelButton.PerformClick();
            Assert.That(eventFired, Is.True);
        }

        [TestCase("", "")]
        [TestCase("ab", "ab")]
        [TestCase("abc", "abc")]
        [TestCase("new password", "different password")]
        public void InvalidNewPasswordDoesNotAcceptDialog(string password, string confirmation)
        {
            _passwordForm.FindControl<TextBox>("txtPassword").Text = password;
            _passwordForm.FindControl<TextBox>("txtVerify").Text = confirmation;

            _passwordForm.FindControl<Button>("btnOK").PerformClick();

            Assert.That(_passwordForm.DialogResult, Is.EqualTo(DialogResult.None));
            Assert.That(_passwordForm.Visible, Is.True);
            Assert.That(_passwordForm.FindControl<Label>("lblStatus").Visible, Is.True);
        }

        [Test]
        public void MatchingNewPasswordAcceptsDialog()
        {
            _passwordForm.FindControl<TextBox>("txtPassword").Text = "new password";
            _passwordForm.FindControl<TextBox>("txtVerify").Text = "new password";

            _passwordForm.FindControl<Button>("btnOK").PerformClick();

            Assert.That(_passwordForm.DialogResult, Is.EqualTo(DialogResult.OK));
        }

        [Test]
        public void ExistingPasswordDoesNotRequireConfirmationOrNewPasswordLength()
        {
            using FrmPassword existingPasswordForm = new(newPasswordMode: false);
            existingPasswordForm.Show();
            existingPasswordForm.FindControl<TextBox>("txtPassword").Text = "a";

            existingPasswordForm.FindControl<Button>("btnOK").PerformClick();

            Assert.That(existingPasswordForm.DialogResult, Is.EqualTo(DialogResult.OK));
        }

        [Test]
        public void PasswordIsHiddenUntilTheEyeIsClicked()
        {
            TextBox passwordTextBox = _passwordForm.FindControl<TextBox>("txtPassword");
            Button showPasswordButton = _passwordForm.FindControl<Button>("btnShowPassword");

            Assert.That(passwordTextBox.UseSystemPasswordChar, Is.True);
            showPasswordButton.PerformClick();
            Assert.That(passwordTextBox.UseSystemPasswordChar, Is.False);
            showPasswordButton.PerformClick();
            Assert.That(passwordTextBox.UseSystemPasswordChar, Is.True);
        }

        [Test]
        public void ShownNewPasswordDoesNotNeedVerification()
        {
            _passwordForm.FindControl<Button>("btnShowPassword").PerformClick();
            _passwordForm.FindControl<TextBox>("txtPassword").Text = "new password";

            _passwordForm.FindControl<Button>("btnOK").PerformClick();

            Assert.That(_passwordForm.FindControl<TextBox>("txtVerify").Enabled, Is.False);
            Assert.That(_passwordForm.DialogResult, Is.EqualTo(DialogResult.OK));
        }

        [Test]
        public void ShownNewPasswordMustStillBeLongEnough()
        {
            _passwordForm.FindControl<Button>("btnShowPassword").PerformClick();
            _passwordForm.FindControl<TextBox>("txtPassword").Text = "ab";

            _passwordForm.FindControl<Button>("btnOK").PerformClick();

            Assert.That(_passwordForm.DialogResult, Is.EqualTo(DialogResult.None));
        }

        [Test]
        public void HidingThePasswordAgainNeedsVerificationAgain()
        {
            Button showPasswordButton = _passwordForm.FindControl<Button>("btnShowPassword");
            showPasswordButton.PerformClick();
            _passwordForm.FindControl<TextBox>("txtPassword").Text = "new password";
            showPasswordButton.PerformClick();

            _passwordForm.FindControl<Button>("btnOK").PerformClick();

            Assert.That(_passwordForm.FindControl<TextBox>("txtVerify").Enabled, Is.True);
            Assert.That(_passwordForm.DialogResult, Is.EqualTo(DialogResult.None));
        }

        [Test]
        public void ShowingThePasswordKeepsWhatWasTyped()
        {
            TextBox passwordTextBox = _passwordForm.FindControl<TextBox>("txtPassword");
            passwordTextBox.Text = "typed before";

            _passwordForm.FindControl<Button>("btnShowPassword").PerformClick();

            Assert.That(passwordTextBox.Text, Is.EqualTo("typed before"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ButtonsAreFullyVisibleBeforeAndAfterAStatusMessage(bool newPasswordMode)
        {
            using FrmPassword form = new(newPasswordMode: newPasswordMode);
            form.Show();
            AssertButtonsVisible(form);

            form.FindControl<TextBox>("txtPassword").Text = newPasswordMode ? "abc" : "";
            if (newPasswordMode)
                form.FindControl<Button>("btnOK").PerformClick();
            AssertButtonsVisible(form);
        }

        private static void AssertButtonsVisible(Form form)
        {
            foreach (string name in new[] { "btnOK", "btnCancel" })
            {
                Button button = form.FindControl<Button>(name);
                System.Drawing.Rectangle bounds = form.RectangleToClient(button.RectangleToScreen(button.ClientRectangle));
                Assert.That(form.ClientRectangle.Contains(bounds), Is.True, $"{name} is cut off: {bounds} in {form.ClientRectangle}");
            }
        }

        [Test]
        public void FourCharacterNewPasswordIsAccepted()
        {
            _passwordForm.FindControl<TextBox>("txtPassword").Text = "abcd";
            _passwordForm.FindControl<TextBox>("txtVerify").Text = "abcd";

            _passwordForm.FindControl<Button>("btnOK").PerformClick();

            Assert.That(_passwordForm.DialogResult, Is.EqualTo(DialogResult.OK));
        }

        [Test]
        [SetUICulture("en-US")]
        public void TypingANewPasswordShowsItsStrength()
        {
            Label strengthLabel = _passwordForm.FindControl<Label>("lblStrength");
            Assert.That(strengthLabel.Visible, Is.False);

            _passwordForm.FindControl<TextBox>("txtPassword").Text = "password";
            WaitForStrengthLabel(strengthLabel);

            Assert.That(strengthLabel.Visible, Is.True);
            Assert.That(strengthLabel.Text, Is.EqualTo("Strength: Weak"));
        }

        [Test]
        [SetUICulture("en-US")]
        public void VeryLongPasswordIsNotScoredAndDialogRemainsResponsive()
        {
            _passwordForm.FindControl<TextBox>("txtPassword").Text = new string('a', 10000);

            Label strengthLabel = _passwordForm.FindControl<Label>("lblStrength");
            Assert.That(strengthLabel.Text, Does.Contain("Not estimated"));

            _passwordForm.FindControl<Button>("btnCancel").PerformClick();

            Assert.That(_passwordForm.Visible, Is.False);
        }

        [Test]
        [SetUICulture("en-US")]
        public void StrengthEvaluationFailureShowsUnavailableWithoutCrashing()
        {
            _passwordForm.FindControl<TextBox>("txtPassword").Text = "password";
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            typeof(FrmPassword).GetField("_activeStrengthVersion", flags).SetValue(_passwordForm, 1L);
            var completed = new RunWorkerCompletedEventArgs(null, new InvalidOperationException(), false);

            typeof(FrmPassword).GetMethod("StrengthWorker_RunWorkerCompleted", flags)
                .Invoke(_passwordForm, new object[] { null, completed });

            Assert.That(_passwordForm.FindControl<Label>("lblStrength").Text, Is.EqualTo("Strength: Unable to estimate"));
        }

        private static void WaitForStrengthLabel(Label strengthLabel)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (string.IsNullOrEmpty(strengthLabel.Text) && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }

        [Test]
        public void ExistingPasswordDoesNotShowStrength()
        {
            using FrmPassword existingPasswordForm = new(newPasswordMode: false);
            existingPasswordForm.Show();

            existingPasswordForm.FindControl<TextBox>("txtPassword").Text = "password";

            Assert.That(existingPasswordForm.FindControl<Label>("lblStrength").Visible, Is.False);
        }

        [Test]
        public void PasswordFieldHasAutofocus()
        {
            // Find the password textbox control
            TextBox passwordTextBox = _passwordForm.FindControl<TextBox>("txtPassword");
            
            // Verify that the password field has focus when the form is loaded
            Assert.That(passwordTextBox.Focused, Is.True, "Password field should have autofocus when form loads");
        }

        [Test]
        public void GetKey_UsesProvidedOwnerWindow()
        {
            using Form ownerForm = new();
            ownerForm.Show();

            using FrmPassword passwordForm = new();
            Form capturedOwner = null;
            passwordForm.Shown += (_, _) =>
            {
                capturedOwner = passwordForm.Owner;
                passwordForm.DialogResult = DialogResult.Cancel;
                passwordForm.Close();
            };

            _ = passwordForm.GetKey(ownerForm);

            Assert.That(capturedOwner, Is.SameAs(ownerForm));
        }
    }
}