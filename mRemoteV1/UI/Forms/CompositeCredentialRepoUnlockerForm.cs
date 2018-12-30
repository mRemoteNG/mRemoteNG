using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Themes;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms
{
    public sealed partial class CompositeCredentialRepoUnlockerForm : Form
    {
        private readonly DisplayProperties _display = new DisplayProperties();
        private readonly ThemeManager _themeManager = ThemeManager.getInstance();
        private readonly CompositeRepositoryUnlocker _repositoryUnlocker;

        public CompositeCredentialRepoUnlockerForm(CompositeRepositoryUnlocker repositoryUnlocker)
        {
            _repositoryUnlocker = repositoryUnlocker.ThrowIfNull(nameof(repositoryUnlocker));
            InitializeComponent();
            SetupListView();
            ApplyLanguage();
            ApplyTheme();
            chkCloseAfterLastUnlock.Checked = Settings.Default.CloseCredentialUnlockerDialogAfterLastUnlock;
            _themeManager.ThemeChanged += ApplyTheme;
            imgPasswordError.Image = _display.ScaleImage(imgPasswordError.Image);
            imgUnlocked.Image = _display.ScaleImage(imgUnlocked.Image);
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;

            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void buttonUnlock_Click(object sender, EventArgs e)
        {
            try
            {
                _repositoryUnlocker.Unlock(secureTextBoxPassword.SecString);
                SelectNextLockedRepo();

                if (Settings.Default.CloseCredentialUnlockerDialogAfterLastUnlock && _repositoryUnlocker.Repositories.All(r => r.IsLoaded))
                {
                    Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ShowPasswordError(true);
            }
            finally
            {
                secureTextBoxPassword.Clear();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SelectNextLockedRepo()
        {
            _repositoryUnlocker.SelectNextLockedRepository();
            objectListViewRepos.SelectedObject = _repositoryUnlocker.SelectedRepository.FirstOrDefault();
        }

        private void objectListViewRepos_SelectionChanged(object sender, EventArgs e)
        {
            objectListViewRepos.RefreshObjects(_repositoryUnlocker.Repositories.ToList());
            var selectedRepo = objectListViewRepos.SelectedObject as ICredentialRepository;
            _repositoryUnlocker.SelectedRepository = new Optional<ICredentialRepository>(selectedRepo);
            ShowRepoDetails(selectedRepo);
            ShowPasswordError(false);
            UnlockRequired(!selectedRepo?.IsLoaded ?? false);
        }

        private void ShowRepoDetails(ICredentialRepository repo)
        {
            textBoxId.Text = repo?.Config.Id.ToString() ?? "";
            textBoxTitle.Text = repo?.Config.Title ?? "";
            textBoxType.Text = repo?.Config.TypeName ?? "";
            textBoxSource.Text = repo?.Config.Source ?? "";
        }

        private void UnlockRequired(bool isUnlockRequired)
        {
            buttonUnlock.Enabled = isUnlockRequired;
            secureTextBoxPassword.Enabled = isUnlockRequired;
            imgUnlocked.Visible = objectListViewRepos.SelectedObject != null && !isUnlockRequired;
            labelUnlocked.Visible = objectListViewRepos.SelectedObject != null && !isUnlockRequired;
        }

        private void ShowPasswordError(bool shouldErrorBeActive)
        {
            labelPasswordError.Visible = shouldErrorBeActive;
            imgPasswordError.Visible = shouldErrorBeActive;
            secureTextBoxPassword.BackColor = shouldErrorBeActive ? Color.MistyRose : SystemColors.Window;
        }

        private void chkCloseAfterLastUnlock_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CloseCredentialUnlockerDialogAfterLastUnlock = chkCloseAfterLastUnlock.Checked;
            Settings.Default.Save();
        }

        #region Setup
        private void SetupListView()
        {
            olvColumnName.AspectGetter = rowObject => ((ICredentialRepository)rowObject).Config.Title;
            olvColumnStatusIcon.AspectGetter = rowObject => string.Empty;
            olvColumnStatusIcon.ImageGetter = rowObject => ((ICredentialRepository)rowObject).IsLoaded ? "unlocked" : "locked";
            objectListViewRepos.SmallImageList = SetupImageList();
        }

        private ImageList SetupImageList()
        {
            var imageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = _display.ScaleSize(new Size(16, 16)),
                TransparentColor = Color.Transparent
            };
            imageList.Images.Add(_display.ScaleImage(Resources.arrow_left));
            imageList.Images.SetKeyName(0, "unlocking");
            imageList.Images.Add(_display.ScaleImage(Resources.tick));
            imageList.Images.SetKeyName(1, "unlocked");
            imageList.Images.Add(_display.ScaleImage(Resources._lock));
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
            objectListViewRepos.SetObjects(_repositoryUnlocker.Repositories);
        }

        private void ApplyLanguage()
        {
            Text = Language.UnlockCredentialRepository;
            labelUnlocking.Text = Language.Unlocking;
            labelId.Text = Language.strID;
            labelRepoTitle.Text = Language.strTitle;
            labelRepoType.Text = Language.strType;
            labelRepoSource.Text = Language.Source;
            labelPassword.Text = Language.strTitlePassword;
            labelPasswordError.Text = Language.IncorrectPassword;
            labelUnlocked.Text = Language.RepositoryIsUnlocked;
            buttonUnlock.Text = Language.Unlock;
            buttonClose.Text = Language.strButtonClose;
            chkCloseAfterLastUnlock.Text = "Automatically close this dialog after the last repository is unlocked";
        }
        #endregion
    }
}