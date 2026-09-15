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