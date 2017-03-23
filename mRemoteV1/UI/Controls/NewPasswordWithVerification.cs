using System;
using System.ComponentModel;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.Security;

namespace mRemoteNG.UI.Controls
{
    public partial class NewPasswordWithVerification : UserControl
    {
        private bool _useSystemPasswordChar;
        private char _passwordChar;


        [Browsable(false)]
        public SecureString SecureString { get; private set; }

        [Browsable(false)]
        public bool PasswordsMatch { get; private set; }

        [Browsable(true)]
        public char PasswordChar
        {
            get { return _passwordChar; }
            set
            {
                _passwordChar = value;
                secureTextBox1.PasswordChar = _passwordChar;
                secureTextBox2.PasswordChar = _passwordChar;
            }
        }

        [Browsable(true)]
        public bool UseSystemPasswordChar
        {
            get { return _useSystemPasswordChar; }
            set
            {
                _useSystemPasswordChar = value;
                secureTextBox1.UseSystemPasswordChar = _useSystemPasswordChar;
                secureTextBox2.UseSystemPasswordChar = _useSystemPasswordChar;
            }
        }

        public NewPasswordWithVerification()
        {
            InitializeComponent();
            secureTextBox1.TextChanged += OnSecureTextBoxTextChanged;
            secureTextBox2.TextChanged += OnSecureTextBoxTextChanged;
        }

        public void SetPassword(SecureString password)
        {
            var text = password.ConvertToUnsecureString();
            secureTextBox1.Text = text;
            secureTextBox2.Text = text;
        }

        private bool Verify()
        {
            return secureTextBox1.SecString.Length == secureTextBox2.SecString.Length &&
                   secureTextBox1.SecString.ConvertToUnsecureString() ==
                   secureTextBox2.SecString.ConvertToUnsecureString();
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
                SecureString = secureTextBox1.SecString;
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