using System;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
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

            

            SetupListView();
            ShowPage(new CredentialListPage(credentialService.RepositoryList)
            {
                DeletionConfirmer = new CredentialDeletionMsgBoxConfirmer(MessageBox.Show)
            });
        }

        private void SetupListView()
        {
            var display = new DisplayProperties();
            _repoImageList = new ImageList
            {
                ImageSize = new Size(display.ScaleWidth(16), display.ScaleHeight(16)),
            };
            _selectionTargets.ForEach(t => _repoImageList.Images.Add(t.Config.TypeName, t.Image));

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
        }

        private void ShowPage(Control page)
        {
            if (page == null)
                return;

            panelMain.Controls.Clear();
            panelMain.Controls.Add(page);
            page.Dock = DockStyle.Fill;
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

        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;

            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void ApplyLanguage()
        {
            Text = Language.strCredentialManager;
            buttonClose.Text = Language.strButtonClose;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
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

        private void btnAddRepo_Click(object sender, EventArgs e)
        {
        }

        private void btnRemoveRepo_Click(object sender, EventArgs e)
        {

        }

        private void btnToggleUnlock_Click(object sender, EventArgs e)
        {
            var selectedRepository = olvCredRepos.SelectedObject as ICredentialRepository;
            if (selectedRepository.IsLoaded)
                selectedRepository.UnloadCredentials();
            else
                _unlockerFactory.Build(new[] { selectedRepository }).ShowDialog(this);

            olvCredRepos.RefreshObject(selectedRepository);
            UpdateUi();
        }

        private void btnEditRepo_Click(object sender, EventArgs e)
        {
            
        }

        private void UpdateUi()
        {
            var selectedRepository = olvCredRepos.SelectedObject as ICredentialRepository;
            btnRemoveRepo.Enabled = selectedRepository != null;
            btnEditRepo.Enabled = selectedRepository != null;
            btnToggleUnlock.Enabled = selectedRepository != null;
            UpdateLoadToggleButton(selectedRepository);
        }

        private void UpdateLoadToggleButton(ICredentialRepository selectedRepository)
        {
            if (selectedRepository == null)
                return;

            btnToggleUnlock.Text = selectedRepository.IsLoaded ? "Lock" : "Unlock";
        }
    }
}