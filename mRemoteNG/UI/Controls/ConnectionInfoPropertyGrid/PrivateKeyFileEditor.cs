using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid
{
    [SupportedOSPlatform("windows")]
    public class PrivateKeyFileEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            if (provider?.GetService(typeof(IWindowsFormsEditorService)) is not IWindowsFormsEditorService)
                return value;

            using var dialog = new OpenFileDialog
            {
                Title = "Select Private Key File",
                Filter = "PuTTY Private Key Files (*.ppk)|*.ppk|All Files (*.*)|*.*",
                CheckFileExists = true
            };

            string currentPath = value as string ?? string.Empty;
            if (!string.IsNullOrEmpty(currentPath))
                dialog.FileName = currentPath;

            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : value;
        }
    }
}
