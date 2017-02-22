﻿using System.Security;
using System.Windows.Forms;
using mRemoteNG.Security;


namespace mRemoteNG.UI.Controls
{
    public partial class SecureTextBox : TextBox
    {
        public SecureString SecureString { get; private set; } = new SecureString();

        public SecureTextBox()
        {
            InitializeComponent();
            TextChanged += SecureTextBox_TextChanged;
        }

        private void SecureTextBox_TextChanged(object sender, System.EventArgs e)
        {
            SecureString = Text.ConvertToSecureString();
        }
    }
}