using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using mRemoteNG.App;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.UI.Controls
{
    public class CredentialRecordTypeEditorAdapter : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            using (var form = new CredentialManagerForm(Runtime.CredentialManager))
            {
                svc.ShowDialog(form);
            }
            return value; // can also replace the wrapper object here
        }
    }
}