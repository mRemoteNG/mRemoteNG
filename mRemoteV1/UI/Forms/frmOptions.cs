using mRemoteNG.UI.Forms.OptionsPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmOptions : Form
    {
        private Dictionary<string, OptionsPage> _pages;
        private readonly string _pageName;
        private readonly DisplayProperties _display = new DisplayProperties();

        public FrmOptions(): this(Language.strStartupExit)
        {
        }

        public FrmOptions(string pn)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            InitializeComponent();
            _pageName = pn;
            Cursor.Current = Cursors.Default;
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            CompileListOfOptionsPages();
            FontOverrider.FontOverride(this);
            AddOptionsPagesToListView();
            SetInitiallyActivatedPage();
            // ApplyLanguage();
            // Handle the main page here and the individual pages in
            // AddOptionsPagesToListView()  -- one less foreach loop....
            Text = Language.strOptionsPageTitle;
            btnOK.Text = Language.strButtonOK;
            btnCancel.Text = Language.strButtonCancel;
            btnApply.Text = Language.strButtonApply;
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            lstOptionPages.SelectedIndexChanged += LstOptionPages_SelectedIndexChanged;
            lstOptionPages.SelectedIndex = 0;
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
            Text = Language.strOptionsPageTitle;
            foreach (var optionPage in _pages.Values)
            {
                optionPage.ApplyLanguage();
            }
        }
#endif
        private void CompileListOfOptionsPages()
        {
            _pages = new Dictionary<string, OptionsPage>
            {
                {typeof(StartupExitPage).Name, new StartupExitPage {Dock = DockStyle.Fill}},
                {typeof(AppearancePage).Name, new AppearancePage {Dock = DockStyle.Fill}},
                {typeof(TabsPanelsPage).Name, new TabsPanelsPage {Dock = DockStyle.Fill}},
                {typeof(NotificationsPage).Name, new NotificationsPage {Dock = DockStyle.Fill}},
                {typeof(ConnectionsPage).Name, new ConnectionsPage {Dock = DockStyle.Fill}},
                {typeof(CredentialsPage).Name, new CredentialsPage {Dock = DockStyle.Fill}},
                {typeof(SqlServerPage).Name, new SqlServerPage {Dock = DockStyle.Fill}},
                {typeof(UpdatesPage).Name, new UpdatesPage {Dock = DockStyle.Fill}},
                {typeof(ThemePage).Name, new ThemePage {Dock = DockStyle.Fill}},
                {typeof(SecurityPage).Name, new SecurityPage {Dock = DockStyle.Fill}},
                {typeof(AdvancedPage).Name, new AdvancedPage {Dock = DockStyle.Fill}}
            };
        }

        private void AddOptionsPagesToListView()
        {
            lstOptionPages.RowHeight = _display.ScaleHeight(lstOptionPages.RowHeight);
            lstOptionPages.AllColumns.First().ImageGetter = ImageGetter;

            foreach (var page in _pages.Select(keyValuePair => keyValuePair.Value))
            {
                page.ApplyLanguage();
                page.LoadSettings();
                lstOptionPages.AddObject(page);
            }
        }

        private object ImageGetter(object rowobject)
        {
            var page = rowobject as OptionsPage;
            return page?.PageIcon == null ? _display.ScaleImage(Resources.Help) : _display.ScaleImage(page.PageIcon);
        }

        private void SetInitiallyActivatedPage()
        {
            var isSet = false;
            for (var i = 0; i < lstOptionPages.Items.Count; i++)
            {
                if (!lstOptionPages.Items[i].Text.Equals(_pageName)) continue;
                lstOptionPages.Items[i].Selected = true;
                isSet = true;
                break;
            }

            if(!isSet)
                lstOptionPages.Items[0].Selected = true;
        }

        /*
         * This gets called by both OK and Apply buttons.
         * OK sets DialogResult = OK, Apply does not (None).
         * Apply will no close the dialog.
         */
        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (var page in _pages.Values)
            {
                Debug.WriteLine(page.PageName);
                page.SaveSettings();
            }
            Debug.WriteLine(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            Settings.Default.Save();
        }


        private void LstOptionPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMain.Controls.Clear();

            var page = (OptionsPage)lstOptionPages.SelectedObject;
            if (page != null)
                pnlMain.Controls.Add(page);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (var page in _pages.Values)
            {
                Debug.WriteLine(page.PageName);
                page.RevertSettings();
            }
            Debug.WriteLine(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }
    }
}