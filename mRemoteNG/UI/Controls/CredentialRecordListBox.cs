using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Controls
{
    public partial class CredentialRecordListBox : ListBox
    {
        public new ICredentialRecord SelectedItem => (ICredentialRecord)base.SelectedItem;
        public ICredentialRecord NoneSelection { get; } = new CredentialRecord {Title = $"--{Language.None}--"};
        public ICredentialRecord AddNewSelection { get; } = new CredentialRecord {Title = $"--{Language.Add}--"};

        public CredentialRecordListBox(IEnumerable<ICredentialRecord> listOfCredentialRecords)
        {
            InitializeComponent();
            PopulateList(listOfCredentialRecords);
        }

        public CredentialRecordListBox(IEnumerable<ICredentialRecord> listOfCredentialRecords, IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            PopulateList(listOfCredentialRecords);
        }

        private void PopulateList(IEnumerable<ICredentialRecord> listOfCredentialRecords)
        {
            SelectionMode = SelectionMode.One;
            DisplayMember = nameof(ICredentialRecord.Title);
            Items.Add(NoneSelection);
            Items.Add(AddNewSelection);

            foreach (var credential in listOfCredentialRecords)
            {
                Items.Add(credential);
            }
        }
    }
}