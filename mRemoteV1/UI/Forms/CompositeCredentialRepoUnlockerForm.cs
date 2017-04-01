using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Forms
{
    public partial class CompositeCredentialRepoUnlockerForm : Form
    {
        private readonly List<ICredentialRepository> _credentialRepositories;

        private ICredentialRepository CurrentCredentialRepositoryUnlocking => (ICredentialRepository)objectListViewRepos.SelectedObject;

        public CompositeCredentialRepoUnlockerForm(IEnumerable<ICredentialRepository> credentialRepositories)
        {
            if (credentialRepositories == null)
                throw new ArgumentNullException(nameof(credentialRepositories));
            _credentialRepositories = credentialRepositories.ToList();
            InitializeComponent();
            SetupListView();
        }

        private void SetupListView()
        {
            olvColumnName.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Title;
            olvColumnStatusIcon.AspectGetter = rowObject => string.Empty;
            olvColumnStatusIcon.ImageGetter = rowObject =>
            {
                if (rowObject.Equals(CurrentCredentialRepositoryUnlocking))
                    return "unlocking";
                return ((ICredentialRepository) rowObject).IsLoaded ? "unlocked" : "locked";
            };
            objectListViewRepos.SmallImageList = SetupImageList();
        }

        private ImageList SetupImageList()
        {
            var imageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16),
                TransparentColor = Color.Transparent
            };
            imageList.Images.Add(Resources.arrow_left);
            imageList.Images.SetKeyName(0, "unlocking");
            imageList.Images.Add(Resources.tick);
            imageList.Images.SetKeyName(1, "unlocked");
            imageList.Images.Add(Resources._lock);
            imageList.Images.SetKeyName(2, "locked");

            return imageList;
        }

        private void CompositeCredentialRepoUnlockerForm_Shown(object sender, EventArgs e)
        {
            PopulateListView();
            objectListViewRepos.SelectedIndex = 0;
        }

        private void PopulateListView()
        {
            objectListViewRepos.SetObjects(_credentialRepositories);
        }

        private void buttonUnlock_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            objectListViewRepos.SelectedIndex++;
        }

        private void objectListViewRepos_SelectedIndexChanged(object sender, EventArgs e)
        {
            objectListViewRepos.RefreshObjects(_credentialRepositories);
            var selectedRepo = objectListViewRepos.SelectedObject as ICredentialRepository;
            textBoxId.Text = selectedRepo?.Config.Id.ToString() ?? "";
            textBoxTitle.Text = selectedRepo?.Config.Title ?? "";
            textBoxType.Text = selectedRepo?.Config.TypeName ?? "";
            textBoxSource.Text = selectedRepo?.Config.Source ?? "";
        }
    }
}