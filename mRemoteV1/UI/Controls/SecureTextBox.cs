using System.Security;
using mRemoteNG.Security;
using mRemoteNG.UI.Controls.Base;

namespace mRemoteNG.UI.Controls
{
    public partial class SecureTextBox : NGTextBox
    {
        public SecureString SecString { get; private set; } = new SecureString();

        public SecureTextBox()
        {
            InitializeComponent();
            TextChanged += SecureTextBox_TextChanged;
        }

        private void SecureTextBox_TextChanged(object sender, System.EventArgs e)
        {
            SecString = Text.ConvertToSecureString();
        }

    
    }
}