using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Themes;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class OptionsWindow : BaseWindow
    {
        private FrmOptions? _optionsForm;
        private bool _isInitialized;
        private bool _isFontOverrideApplied;

        #region Public Methods

        public OptionsWindow() : this(new DockContent())
        {
        }

        public OptionsWindow(DockContent panel)
        {
            WindowType = WindowType.Options;
            DockPnl = panel;
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            DoubleBuffered = true;
        }

        #endregion

        #region Form Stuff

        private void Options_Load(object sender, EventArgs e)
        {
            // On reopen of a previously initialized window, nothing to do —
            // FrmOptions is a child control and auto-shows with its parent
            if (_isInitialized && _optionsForm != null && !_optionsForm.IsDisposed)
                return;

            // First-time initialization
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;

            ApplyTheme();
            ApplyLanguage();
            LoadOptionsForm();

            _isInitialized = true;
        }

        private void Options_Shown(object sender, EventArgs e)
        {
            if (_isFontOverrideApplied)
                return;

            BeginInvoke((MethodInvoker)(() =>
            {
                if (IsDisposed || _isFontOverrideApplied)
                    return;

                FontOverrider.FontOverride(this);
                _isFontOverrideApplied = true;
            }));
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            base.ApplyTheme();
        }

        private void ApplyLanguage()
        {
            Text = Language.Options;
            TabText = Language.Options;
        }

        private void LoadOptionsForm()
        {
            // Check if FrmMain.OptionsForm is disposed (source of truth)
            if (FrmMain.OptionsForm != null && FrmMain.OptionsForm.IsDisposed)
                FrmMain.RecreateOptionsForm();

            // If the local reference is disposed, clean up
            if (_optionsForm != null && _optionsForm.IsDisposed)
            {
                if (Controls.Contains(_optionsForm))
                    Controls.Remove(_optionsForm);
                _optionsForm.CloseRequested -= OnOptionsFormCloseRequested;
                _optionsForm = null;
            }

            // Get fresh reference if needed
            if (_optionsForm == null)
            {
                _optionsForm = FrmMain.OptionsForm;

                if (_optionsForm == null || _optionsForm.IsDisposed)
                    return;

                _optionsForm.TopLevel = false;
                _optionsForm.FormBorderStyle = FormBorderStyle.None;
                _optionsForm.Dock = DockStyle.Fill;
                _optionsForm.CloseRequested += OnOptionsFormCloseRequested;
                Controls.Add(_optionsForm);
            }

            if (!_optionsForm.Visible)
                _optionsForm.Show();
        }

        /// <summary>
        /// Handles OK/Cancel from embedded FrmOptions.
        /// Calls DockHandler.Hide() directly — much faster than Close() because
        /// it skips the entire FormClosing event chain.
        /// </summary>
        private void OnOptionsFormCloseRequested(object sender, EventArgs e)
        {
            DockHandler.Hide();
        }

        /// <summary>
        /// Reloads all option-page controls from the stored (on-disk) settings.
        /// Call this before making the panel visible so that any stale edits from a
        /// previous session (e.g. after a Tab-X hide) are discarded.
        /// Safe to call before first initialization — no-op when _optionsForm is null.
        /// </summary>
        internal void RefreshSettings()
        {
            _optionsForm?.ReloadAllSettings();
        }

        /// <summary>
        /// Handles the case where OptionsWindow is closed explicitly (e.g. from code
        /// calling Close(), or DPS's CloseContent which fires FormClosing before hiding).
        /// Checks for unsaved changes in the embedded FrmOptions.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_optionsForm != null && !_optionsForm.IsDisposed && _optionsForm.HasUnsavedChanges())
            {
                DialogResult result = MessageBox.Show(
                    Language.SaveOptionsBeforeClosing,
                    Language.Options,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        _optionsForm.SaveAllOptions();
                        break;
                    case DialogResult.No:
                        _optionsForm.DiscardChanges();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }

            base.OnFormClosing(e);
        }

        public void SetActivatedPage(string pageName)
        {
            _optionsForm?.SetActivatedPage(pageName);
        }

        #endregion

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // OptionsWindow
            // 
            ClientSize = new System.Drawing.Size(800, 600);
            HideOnClose = true;
            Name = "OptionsWindow";
            Text = Language.Options;
            TabText = Language.Options;
            Load += Options_Load;
            Shown += Options_Shown;
            ResumeLayout(false);
        }
    }
}
