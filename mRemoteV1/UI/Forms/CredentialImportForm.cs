using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialImportForm : Form
    {
        private List<CredAndRepoModel> _importedCredentialRecords;

        public CredentialService CredentialService { get; set; }
        public List<ICredentialRecord> ImportedCredentialRecords
        {
            get => _importedCredentialRecords.Select(model => model.CredentialRecord).ToList();
            set
            {
                _importedCredentialRecords = value.Select(r => new CredAndRepoModel(r)).ToList();
                olvCredentials.SetObjects(_importedCredentialRecords);
            }
        }

        public CredentialImportForm()
        {
            InitializeComponent();

            olvCredentials.CellEditStarting += OlvCredentialsOnCellEditStarting;
            olvCredentials.CellEditFinishing += OlvCredentialsOnCellEditFinishing;
            colUsername.AspectGetter = rowObject => (rowObject as CredAndRepoModel)?.CredentialRecord?.Username;
            colDomain.AspectGetter = rowObject => (rowObject as CredAndRepoModel)?.CredentialRecord?.Domain;
            colRepo.AspectGetter = rowObject => (rowObject as CredAndRepoModel)?.AssignedRepository?.Title;
        }

        private void OlvCredentialsOnCellEditFinishing(object sender, CellEditEventArgs e)
        {
            //if (e.Column != colRepo)
            //    return;

            //if ((e.Control as ComboBox)?.SelectedValue is Guid repoId)
            //    e.NewValue = CredentialService.RepositoryList.GetProvider(repoId).FirstOrDefault();
        }

        private void OlvCredentialsOnCellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.Column != colRepo)
                return;

            e.Control = CreateRepoListDropdownControl(e);
        }

        private Control CreateRepoListDropdownControl(CellEditEventArgs e)
        {
            var comboBox = new ComboBox
            {
                Bounds = e.CellBounds,
                Left = e.CellBounds.Left + 1,
                Width = e.CellBounds.Width - 1,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = nameof(ICredentialRepository.Config.Title),
                ValueMember = nameof(ICredentialRepository.Config.Id),
                DataSource = CredentialService.RepositoryList.ToList(),
                AutoCompleteSource = AutoCompleteSource.ListItems,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend
            };

            comboBox.Update();
            return comboBox;
        }

        private void AddCredentialsToAssignedRepos()
        {
            foreach (var importedCred in _importedCredentialRecords)
            {
                importedCred.AssignedRepository.CredentialRecords.Add(importedCred.CredentialRecord);
            }
        }

        private void buttonAccept_Click(object sender, System.EventArgs e)
        {
            AddCredentialsToAssignedRepos();
        }
    }
}
