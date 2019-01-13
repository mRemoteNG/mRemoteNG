using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Tools.CustomCollections;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms.CredentialManager.RepoProviders.Xml;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public partial class CredentialManagerForm : Form
    {
        private readonly ThemeManager _themeManager;
        private readonly CredentialService _credentialService;
        private ImageList _repoImageList;
        private ImageList _buttonImages;
        private readonly ISelectionTarget<ICredentialRepositoryConfig>[] _selectionTargets;
        private readonly UnlockerFormFactory _unlockerFactory;
        private readonly CredentialListPage _credentialListPage;

        public CredentialManagerForm(CredentialService credentialService, UnlockerFormFactory unlockerFactory)
        {
            _unlockerFactory = unlockerFactory;
            _credentialService = credentialService.ThrowIfNull(nameof(credentialService));
            InitializeComponent();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            ApplyTheme();
            ApplyLanguage();
            _selectionTargets = new ISelectionTarget<ICredentialRepositoryConfig>[]
            {
                new XmlCredentialRepositorySelector(_credentialService),
                //new KeePassRepositorySelector()
            };

            _credentialListPage = new CredentialListPage(credentialService.RepositoryList)
            {
                DeletionConfirmer = new CredentialDeletionMsgBoxConfirmer(MessageBox.Show)
            };

            SetupListView();
            ShowPage(_credentialListPage);
        }

        private void ApplyLanguage()
        {
            Text = Language.strCredentialManager;
            buttonClose.Text = Language.strButtonClose;
        }

        #region Cred Repo Listview
        private void SetupListView()
        {
            var display = new DisplayProperties();
            _repoImageList = new ImageList
            {
                ImageSize = new Size(display.ScaleWidth(16), display.ScaleHeight(16)),
            };
            _selectionTargets.ForEach(t => _repoImageList.Images.Add(t.DefaultConfig.TypeName, t.Image));

            _buttonImages = new ImageList
            {
                ImageSize = new Size(display.ScaleWidth(16), display.ScaleHeight(16)),
            };
            _buttonImages.Images.Add("Locked", Resources._lock);
            _buttonImages.Images.Add("Unlocked", Resources.tick);
            _buttonImages.Images.Add("Edit", Resources.Config);

            colCredRepoTitle.ImageGetter = ImageGetter;
            colCredRepoTitle.AspectGetter = o => (o as ICredentialRepository)?.Title;

            olvCredRepos.SetObjects(_credentialService.RepositoryList);
            olvCredRepos.SelectedIndex = 0;
            olvCredRepos.SelectionChanged += (sender, args) => UpdateUi();
            olvCredRepos.CellClick += OlvCredReposOnCellClick;
            _credentialService.RepositoryList.RepositoriesUpdated += RepositoryListOnRepositoriesUpdated;
        }

        private void OlvCredReposOnCellClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount < 2)
                return;

            // edit repo on double click of tree item
            EditRepository(e.Model as ICredentialRepository);
        }

        private object ImageGetter(object rowObject)
        {
            if (!(rowObject is ICredentialRepository repository))
                return null;

            var key = repository.Config.TypeName;
            return _repoImageList.Images.ContainsKey(key) 
                ? _repoImageList.Images[key] 
                : null;
        }

        private void olvCredRepos_FormatRow(object sender, FormatRowEventArgs e)
        {
            if (!(e.Model is ICredentialRepository repo))
                return;

            e.Item.Decorations.Clear();

            if (repo.IsLoaded)
                return;

            e.Item.Decorations.Add(new TintedRowDecoration(e.Item)
            {
                Tint = Color.FromArgb(150, Color.Black)
            });
            e.Item.Decorations.Add(new TextDecoration("Locked", ContentAlignment.MiddleCenter)
            {
                TextColor = Color.AliceBlue,
            });

            var imgDecoration = new ImageDecoration(_buttonImages.Images["Locked"], ContentAlignment.MiddleRight);
            e.Item.Decorations.Add(imgDecoration);
        }
        #endregion

        private void ShowPage(Control page)
        {
            if (page == null)
                return;

            panelMain.Controls.Clear();
            panelMain.Controls.Add(page);
            page.Dock = DockStyle.Fill;
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;

            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddRepo_Click(object sender, EventArgs e)
        {
            var pageWorkflowController = new PageWorkflowController(ShowPage, _credentialListPage);
            var repoTypeSelection = new CredentialRepositoryTypeSelectionPage(
                _selectionTargets, 
                _credentialService.RepositoryList,
                pageWorkflowController);
            
            ShowPage(repoTypeSelection);
        }

        private void btnRemoveRepo_Click(object sender, EventArgs e)
        {
            if (!(olvCredRepos.SelectedObject is ICredentialRepository selectedRepository))
                return;

            _credentialService.RemoveRepository(selectedRepository);
        }

        private void btnToggleUnlock_Click(object sender, EventArgs e)
        {
            ToggleRepositoryLockStatus(olvCredRepos.SelectedObject as ICredentialRepository);
        }

        private void btnEditRepo_Click(object sender, EventArgs e)
        {
            EditRepository(olvCredRepos?.SelectedObject as ICredentialRepository);
        }

        private void ToggleRepositoryLockStatus(ICredentialRepository repository)
        {
            if (repository == null)
                return;

            if (repository.IsLoaded)
                repository.UnloadCredentials();
            else
                _unlockerFactory.Build(new[] { repository }).ShowDialog(this);

            olvCredRepos.RefreshObject(repository);
            UpdateUi();
        }

        private void EditRepository(ICredentialRepository repository)
        {
            if (repository == null)
                return;

            var pageWorkflowController = new PageWorkflowController(ShowPage, _credentialListPage);

            var editorPage = _selectionTargets
                .FirstOrDefault(t => t.DefaultConfig.TypeName.Equals(repository.Config.TypeName))?
                .BuildEditorPage(repository.Config.ToOptional(), _credentialService.RepositoryList, pageWorkflowController);

            ShowPage(editorPage);
        }

        private void UpdateUi()
        {
            var selectedRepository = olvCredRepos.SelectedObject as ICredentialRepository;
            var repoIsSelected = selectedRepository != null;

            btnRemoveRepo.Enabled = repoIsSelected;
            btnEditRepo.Enabled = repoIsSelected;
            btnToggleUnlock.Enabled = repoIsSelected;

            if (repoIsSelected)
                btnToggleUnlock.Text = selectedRepository.IsLoaded ? "Lock" : "Unlock";
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _credentialService.RepositoryList.RepositoriesUpdated -= RepositoryListOnRepositoriesUpdated;
            }
            base.Dispose(disposing);
        }

        private void RepositoryListOnRepositoriesUpdated(object sender, CollectionUpdatedEventArgs<ICredentialRepository> e)
        {
            olvCredRepos.SetObjects(_credentialService.RepositoryList, true);
        }
    }
}