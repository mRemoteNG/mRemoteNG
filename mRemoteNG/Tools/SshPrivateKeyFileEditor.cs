using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.Tools
{
    /// <summary>
    /// Property-grid editor that shows a "..." button and opens the standard Windows file dialog so the
    /// user can browse for an SSH private key file, filtered to the common private-key file types.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class SshPrivateKeyFileEditor : UITypeEditor
    {
        private const string KeyFileFilter =
            "Private key files (*.pem, *.key, *.ppk, id_*)|*.pem;*.key;*.ppk;*.openssh;*.id_ed25519;id_rsa;id_dsa;id_ecdsa;id_ed25519;id_xmss|" +
            "PEM files (*.pem)|*.pem|" +
            "OpenSSH keys (id_rsa, id_ed25519, ...)|id_rsa;id_dsa;id_ecdsa;id_ed25519;id_xmss;*.openssh|" +
            "PuTTY private keys (*.ppk)|*.ppk|" +
            "Key files (*.key)|*.key|" +
            "All files (*.*)|*.*";

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            => UITypeEditorEditStyle.Modal;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string current = value as string ?? string.Empty;

            using var dialog = new OpenFileDialog
            {
                Title = "Select SSH Private Key File",
                Filter = KeyFileFilter,
                CheckFileExists = true,
                Multiselect = false,
                FileName = current
            };

            if (!string.IsNullOrEmpty(current))
            {
                try
                {
                    string dir = System.IO.Path.GetDirectoryName(current);
                    if (!string.IsNullOrEmpty(dir) && System.IO.Directory.Exists(dir))
                        dialog.InitialDirectory = dir;
                }
                catch (ArgumentException)
                {
                    // Ignore an invalid current path; the dialog just opens at its default location.
                }
            }

            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : value;
        }
    }
}
