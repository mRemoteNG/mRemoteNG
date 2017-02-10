using System;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.Security;

namespace mRemoteNG.UI.Controls
{
    public partial class NewPasswordWithVerification : UserControl
    {
        public SecureString SecureString { get; private set; }
        public bool PasswordsMatch { get; private set; }

        public NewPasswordWithVerification()
        {
            InitializeComponent();
            secureTextBox1.TextChanged += OnSecureTextBoxTextChanged;
            secureTextBox2.TextChanged += OnSecureTextBoxTextChanged;
        }

        private bool Verify()
        {
            return secureTextBox1.SecureString.Length == secureTextBox2.SecureString.Length &&
                   secureTextBox1.SecureString.ConvertToUnsecureString() ==
                   secureTextBox2.SecureString.ConvertToUnsecureString();
        }

        private void TogglePasswordMatchIndicator(bool passwordsMatch)
        {
            labelPasswordsDontMatch.Visible = !passwordsMatch;
            imgError.Visible = !passwordsMatch;
            if (passwordsMatch)
            {
                secureTextBox1.BackColor = SystemColors.Window;
                secureTextBox2.BackColor = SystemColors.Window;
            }
            else
            {
                secureTextBox1.BackColor = Color.MistyRose;
                secureTextBox2.BackColor = Color.MistyRose;
            }
        }

        private void OnSecureTextBoxTextChanged(object sender, EventArgs eventArgs)
        {
            if (Verify() && !PasswordsMatch)
            {
                PasswordsMatch = true;
                SecureString = secureTextBox1.SecureString;
                RaiseVerifiedEvent();
            }
            else
            {
                PasswordsMatch = false;
                SecureString = null;
            }
            TogglePasswordMatchIndicator(PasswordsMatch);
        }

        public event EventHandler Verified;

        private void RaiseVerifiedEvent()
        {
            Verified?.Invoke(this, EventArgs.Empty);
        }
    }
}