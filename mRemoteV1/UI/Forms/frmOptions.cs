using mRemoteNG.UI.Forms.OptionsPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
    public partial class frmOptions : Form
    {
        private Dictionary<string, OptionsPage> _pages;
        private ImageList _pageIconImageList;
        private readonly string _pageName;

        public frmOptions()
        {
            InitializeComponent();
            _pageName = Language.strStartupExit;
        }

        public frmOptions(string pn)
        {
            InitializeComponent();
            _pageName = pn;
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            CompileListOfOptionsPages();
            FontOverrider.FontOverride(this);
            SetImageListForListView();
            AddOptionsPagesToListView();
            SetInitiallyActivatedPage();
            ApplyLanguage();
            ApplyTheme();
            Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            lstOptionPages.SelectedIndexChanged += LstOptionPages_SelectedIndexChanged;
            lstOptionPages.SelectedIndex = 0;
        }
        private void ApplyTheme()
        {
            if (!Themes.ThemeManager.getInstance().ThemingActive) return;
            BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void ApplyLanguage()
        {
            Text = Language.strOptionsPageTitle;
            foreach (var optionPage in _pages.Values)
            {
                optionPage.ApplyLanguage();
            }
        }

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

        private void SetImageListForListView()
        {
            _pageIconImageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
            lstOptionPages.LargeImageList = _pageIconImageList;
            lstOptionPages.SmallImageList = _pageIconImageList;
        }

        private void AddOptionsPagesToListView()
        {
            foreach (var page in _pages.Select(keyValuePair => keyValuePair.Value))
            {
                page.LoadSettings();
                _pageIconImageList.Images.Add(page.PageName, page.PageIcon);
                lstOptionPages.AddObject(page);
            }
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