using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Credential;


namespace mRemoteNG.UI.Controls
{
    public partial class CredentialRecordListBox : ListBox
    {
        public ICredentialRecord NoneSelection { get; } = new CredentialRecord { Title = $"--{Language.strNone}--" };
        public ICredentialRecord AddNewSelection { get; } = new CredentialRecord { Title = $"--{Language.strAdd}--" };

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