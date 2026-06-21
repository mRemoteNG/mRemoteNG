#region  Usings
using mRemoteNG.App;
using mRemoteNG.UI.Forms.OptionsPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System.Configuration;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
#endregion

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmOptions : Form
    {
        private readonly List<OptionsPage> _optionPages = [];
        private string _pageName;
        private readonly DisplayProperties _display = new();
        private readonly List<string> _optionPageObjectNames;
        private bool _isLoading = true;
        private bool _isInitialized = false;
        private bool _isFontOverrideApplied = false;
        private bool _isHandlingSelectionChange = false; // Guard flag to prevent recursive event handling

        /// <summary>
        /// Raised when the user clicks OK or Cancel, signalling the host window to hide.
        /// </summary>
        public event EventHandler CloseRequested;

        public FrmOptions() : this(Language.StartupExit)
        {
        }

        private FrmOptions(string pageName)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            _pageName = pageName;
            Cursor.Current = Cursors.Default;
            DoubleBuffered = true;

            _optionPageObjectNames =
            [
                nameof(StartupExitPage),
                nameof(AppearancePage),
                nameof(ConnectionsPage),
                nameof(TabsPanelsPage),
                nameof(NotificationsPage),
                nameof(CredentialsPage),
                nameof(SqlServerPage),
                nameof(UpdatesPage),
                nameof(ThemePage),
                nameof(SecurityPage),
                nameof(AdvancedPage),
                nameof(BackupPage)
            ];

            InitOptionsPagesToListView();
        }

        private void FrmOptions_Load(object sender, EventArgs e)
        {
            Logger.Instance.Log?.Debug($"[FrmOptions_Load] START - IsInitialized: {_isInitialized}, Visible: {this.Visible}, Handle: {this.Handle}");

            // Only initialize once to prevent multiple event subscriptions and page reloading
            if (_isInitialized)
            {
                Logger.Instance.Log?.Debug($"[FrmOptions_Load] Already initialized - fast path");
                return;
            }

            Logger.Instance.Log?.Debug($"[FrmOptions_Load] First initialization");
            SetActivatedPage();
            //ApplyLanguage();
            // Handle the main page here and the individual pages in
            // AddOptionsPagesToListView()  -- one less foreach loop....
            Text = Language.OptionsPageTitle;
            btnOK.Text = Language._Ok;
            btnCancel.Text = Language._Cancel;
            btnApply.Text = Language.Apply;
            //ApplyTheme();
            //ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            lstOptionPages.SelectedIndexChanged += LstOptionPages_SelectedIndexChanged;
            lstOptionPages.SelectedIndex = 0;
            Logger.Instance.Log?.Debug($"[FrmOptions_Load] Selected index set to 0");

            // Mark as initialized
            _isInitialized = true;
            Logger.Instance.Log?.Debug($"[FrmOptions_Load] END (first initialization complete)");
        }

        private void FrmOptions_Shown(object sender, EventArgs e)
        {
            if (_isFontOverrideApplied)
            {
                return;
            }

            BeginInvoke((MethodInvoker)(() =>
            {
                if (IsDisposed || _isFontOverrideApplied)
                {
                    return;
                }

                FontOverrider.FontOverride(this);
                _isFontOverrideApplied = true;
            }));
        }

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

#if false
        private void ApplyLanguage()
        {
            Text = Language.OptionsPageTitle;
            foreach (var optionPage in _pages.Values)
            {
                optionPage.ApplyLanguage();
            }
        }
#endif

        private void InitOptionsPagesToListView()
        {
            Logger.Instance.Log?.Debug($"[InitOptionsPagesToListView] START - Loading {_optionPageObjectNames.Count} pages");

            lstOptionPages.RowHeight = _display.ScaleHeight(lstOptionPages.RowHeight);
            lstOptionPages.AllColumns.First().ImageGetter = ImageGetter;

            // Suspend layout to prevent flickering during batch loading
            lstOptionPages.BeginUpdate();
            try
            {
                // Load all pages synchronously for faster, more responsive loading
                // This is especially important when the form is recreated (second+ open)
                foreach (var pageName in _optionPageObjectNames)
                {
                    Logger.Instance.Log?.Debug($"[InitOptionsPagesToListView] Loading page: {pageName}");
                    InitOptionsPage(pageName);
                }

                // All pages loaded, now start tracking changes
                _isLoading = false;
                Logger.Instance.Log?.Debug($"[InitOptionsPagesToListView] All {_optionPageObjectNames.Count} pages loaded");
            }
            finally
            {
                lstOptionPages.EndUpdate();
            }

            Logger.Instance.Log?.Debug($"[InitOptionsPagesToListView] END");
        }

        private void InitOptionsPage(string pageName)
        {
            OptionsPage page = null;

            switch (pageName)
            {
                case "StartupExitPage":
                    {
                        if (Properties.OptionsStartupExitPage.Default.cbStartupExitPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new StartupExitPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "AppearancePage":
                    {
                        if (Properties.OptionsAppearancePage.Default.cbAppearancePageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new AppearancePage { Dock = DockStyle.Fill };
                        break;
                    }
                case "ConnectionsPage":
                    {
                        if (Properties.OptionsConnectionsPage.Default.cbConnectionsPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new ConnectionsPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "TabsPanelsPage":
                    {
                        if (Properties.OptionsTabsPanelsPage.Default.cbTabsPanelsPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new TabsPanelsPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "NotificationsPage":
                    {
                        if (Properties.OptionsNotificationsPage.Default.cbNotificationsPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new NotificationsPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "CredentialsPage":
                    {
                        if (Properties.OptionsCredentialsPage.Default.cbCredentialsPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new CredentialsPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "SqlServerPage":
                    {
                        if (Properties.OptionsDBsPage.Default.cbDBsPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new SqlServerPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "UpdatesPage":
                    {
                        if (Properties.OptionsUpdatesPage.Default.cbUpdatesPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new UpdatesPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "ThemePage":
                    {
                        if (Properties.OptionsThemePage.Default.cbThemePageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new ThemePage { Dock = DockStyle.Fill };
                        break;
                    }
                case "SecurityPage":
                    {
                        if (Properties.OptionsSecurityPage.Default.cbSecurityPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new SecurityPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "AdvancedPage":
                    {
                        if (Properties.OptionsAdvancedPage.Default.cbAdvancedPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new AdvancedPage { Dock = DockStyle.Fill };
                        break;
                    }
                case "BackupPage":
                    {
                        if (Properties.OptionsBackupPage.Default.cbBacupPageInOptionMenu ||
                            Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
                            page = new BackupPage { Dock = DockStyle.Fill };
                        break;
                    }
            }

            if (page == null) return;
            page.ApplyLanguage();
            page.LoadRegistrySettings();
            page.LoadSettings();
            _optionPages.Add(page);
            lstOptionPages.AddObject(page);
            
            // Track changes in all controls on the page
            TrackChangesInControls(page);
        }

        private object ImageGetter(object rowobject)
        {
            OptionsPage page = rowobject as OptionsPage;
            return page?.PageIcon == null ? _display.ScaleImage(Properties.Resources.F1Help_16x) : _display.ScaleImage(page.PageIcon);
        }

        public void SetActivatedPage(string pageName = default)
        {
            _pageName = pageName ?? Language.StartupExit;

            // Ensure we have items loaded before trying to access them
            if (lstOptionPages.Items.Count == 0)
            {
                Logger.Instance.Log?.Warn($"[SetActivatedPage] No items in lstOptionPages, cannot set active page to '{_pageName}'");
                return;
            }

            // Skip if the requested page is already selected (avoid redundant layout work)
            if (lstOptionPages.SelectedObject is OptionsPage selectedPage && selectedPage.PageName == _pageName)
            {
                Logger.Instance.Log?.Debug($"[SetActivatedPage] Page '{_pageName}' already selected - skipping");
                return;
            }

            bool isSet = false;
            for (int i = 0; i < lstOptionPages.Items.Count; i++)
            {
                if (!lstOptionPages.Items[i].Text.Equals(_pageName)) continue;
                lstOptionPages.Items[i].Selected = true;
                isSet = true;
                break;
            }

            if (!isSet && lstOptionPages.Items.Count > 0)
                lstOptionPages.Items[0].Selected = true;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Logger.Instance.Log?.Debug($"[BtnOK_Click] START");
            SaveOptions();
            ClearChangeFlags();
            CloseRequested?.Invoke(this, EventArgs.Empty);
            Logger.Instance.Log?.Debug($"[BtnOK_Click] END");
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            Logger.Instance.Log?.Debug($"[BtnApply_Click] START");
            SaveOptions();
            // Clear change flags after applying
            ClearChangeFlags();
            Logger.Instance.Log?.Debug($"[BtnApply_Click] END");
        }

        private void SaveOptions()
        {
            foreach (OptionsPage page in _optionPages)
            {
                Logger.Instance.Log?.Debug($"[SaveOptions] Saving page: {page.PageName}");
                page.SaveSettings();
            }

            Logger.Instance.Log?.Debug($"[SaveOptions] Configuration file: {(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)).FilePath}");
            Settings.Default.Save();
        }

        private void LstOptionPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guard against recursive calls that can cause infinite loops
            if (_isHandlingSelectionChange)
            {
                Logger.Instance.Log?.Warn($"[LstOptionPages_SelectedIndexChanged] RECURSIVE CALL BLOCKED - Preventing infinite loop");
                return;
            }

            _isHandlingSelectionChange = true;
            try
            {
                Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] START - IsLoading: {_isLoading}, SelectedIndex: {lstOptionPages.SelectedIndex}, Items.Count: {lstOptionPages.Items.Count}");

                if (lstOptionPages.SelectedObject is OptionsPage page)
                {
                    Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] SelectedObject: {page.PageName}");
                }
                else
                {
                    Logger.Instance.Log?.Warn($"[LstOptionPages_SelectedIndexChanged] Page is NULL - cannot display. This may indicate a selection issue.");
                    return;
                }

                // Skip if this page is already displayed in the panel
                if (pnlMain.Controls.Count == 1 && pnlMain.Controls[0] == page)
                {
                    Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] Page '{page.PageName}' already displayed - skipping");
                    return;
                }

                pnlMain.SuspendLayout();
                pnlMain.Controls.Clear();
                Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] pnlMain.Controls cleared");

                if (page.IsDisposed)
                {
                    Logger.Instance.Log?.Error($"[LstOptionPages_SelectedIndexChanged] Page '{page.PageName}' is disposed - cannot display");
                    return;
                }

                // Ensure the page has a valid window handle
                if (!page.IsHandleCreated)
                {
                    Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] Page '{page.PageName}' has no handle - creating handle");
                    var handle = page.Handle; // This creates the handle
                    Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] Handle created: {handle}");
                }

                Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] Adding page '{page.PageName}' to pnlMain");
                pnlMain.Controls.Add(page);
                pnlMain.ResumeLayout(true);
                Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] Page added successfully. pnlMain.Controls.Count: {pnlMain.Controls.Count}");

                Logger.Instance.Log?.Debug($"[LstOptionPages_SelectedIndexChanged] END");
            }
            finally
            {
                _isHandlingSelectionChange = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Logger.Instance.Log?.Debug($"[BtnCancel_Click] START");
            ReloadAllSettings();
            CloseRequested?.Invoke(this, EventArgs.Empty);
            Logger.Instance.Log?.Debug($"[BtnCancel_Click] END");
        }

        /// <summary>
        /// Returns true if any options page has been modified.
        /// </summary>
        internal bool HasUnsavedChanges() => _optionPages.Any(page => page.HasChanges);

        /// <summary>
        /// Saves all option page settings to disk.
        /// </summary>
        internal void SaveAllOptions()
        {
            SaveOptions();
            ClearChangeFlags();
        }

        /// <summary>
        /// Reloads all pages from the stored settings, discarding any pending control edits.
        /// Call this on Cancel or any close path that should not persist changes.
        /// </summary>
        internal void ReloadAllSettings()
        {
            // Suppress HasChanges tracking while we programmatically restore control values
            var wasLoading = _isLoading;
            _isLoading = true;
            try
            {
                foreach (OptionsPage page in _optionPages)
                    page.LoadSettings();
            }
            finally
            {
                _isLoading = wasLoading;
            }
            ClearChangeFlags();
        }

        /// <summary>
        /// Discards any pending change flags and reloads control values from stored settings.
        /// </summary>
        internal void DiscardChanges() => ReloadAllSettings();

        private void TrackChangesInControls(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                // Track changes for common input controls
                if (childControl is TextBox textBox)
                {
                    textBox.TextChanged += (s, e) => MarkPageAsChanged(control);
                }
                else if (childControl is CheckBox checkBox)
                {
                    checkBox.CheckedChanged += (s, e) => MarkPageAsChanged(control);
                }
                else if (childControl is RadioButton radioButton)
                {
                    radioButton.CheckedChanged += (s, e) => MarkPageAsChanged(control);
                }
                else if (childControl is ComboBox comboBox)
                {
                    comboBox.SelectedIndexChanged += (s, e) => MarkPageAsChanged(control);
                }
                else if (childControl is NumericUpDown numericUpDown)
                {
                    numericUpDown.ValueChanged += (s, e) => MarkPageAsChanged(control);
                }
                else if (childControl is ListBox listBox)
                {
                    listBox.SelectedIndexChanged += (s, e) => MarkPageAsChanged(control);
                }
                
                // Recursively track changes in nested controls
                if (childControl.Controls.Count > 0)
                {
                    TrackChangesInControls(childControl);
                }
            }
        }

        private void MarkPageAsChanged(Control control)
        {
            // Don't track changes during initial loading
            if (_isLoading) return;
            
            // Find the parent OptionsPage
            Control current = control;
            while (current != null && !(current is OptionsPage))
            {
                current = current.Parent;
            }
            
            if (current is OptionsPage page)
            {
                page.HasChanges = true;
            }
        }

        private void ClearChangeFlags()
        {
            foreach (OptionsPage page in _optionPages)
            {
                page.HasChanges = false;
            }
        }
    }
}