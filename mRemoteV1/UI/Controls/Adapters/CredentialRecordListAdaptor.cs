using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Controls.Adapters
{
    public class CredentialRecordListAdaptor : UITypeEditor
    {
        private IWindowsFormsEditorService _editorService;

        public static ICredentialRepositoryList CredentialRepositoryList { get; set; }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider == null) return value;
            _editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (_editorService == null) return value;

            var listBox = new CredentialRecordListBox(CredentialRepositoryList.GetCredentialRecords());
            listBox.SelectedValueChanged += ListBoxOnSelectedValueChanged;

            _editorService.DropDownControl(listBox);
            if (listBox.SelectedItem == listBox.NoneSelection)
                return null;
            if (listBox.SelectedItem == listBox.AddNewSelection)
            {
                //var newCred = new CredentialRecord();
                //credentialManager.Add(newCred);
                //return newCred;
            }

            return listBox.SelectedItem ?? value;
        }

        private void ListBoxOnSelectedValueChanged(object sender, EventArgs eventArgs)
        {
            _editorService.CloseDropDown();
        }
    }
}