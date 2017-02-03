using mRemoteNG.App;
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
            Runtime.FontOverride(this);
            SetImageListForListView();
            AddOptionsPagesToListView();
            SetInitiallyActivatedPage();
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            foreach (var optionPage in _pages.Values)
            {
                optionPage.ApplyLanguage();
            }
        }

        private void CompileListOfOptionsPages()
        {
            _pages = new Dictionary<string, OptionsPage>
            {
                {typeof(StartupExitPage).Name, new StartupExitPage()},
                {typeof(AppearancePage).Name, new AppearancePage()},
                {typeof(TabsPanelsPage).Name, new TabsPanelsPage()},
                {typeof(NotificationsPage).Name, new NotificationsPage()},
                {typeof(ConnectionsPage).Name, new ConnectionsPage()},
                {typeof(SqlServerPage).Name, new SqlServerPage()},
                {typeof(UpdatesPage).Name, new UpdatesPage()},
                {typeof(ThemePage).Name, new ThemePage()},
                {typeof(SecurityPage).Name, new SecurityPage()},
                {typeof(AdvancedPage).Name, new AdvancedPage()}
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
                var item = new ListViewItem(page.PageName, page.PageName) {Tag = page.GetType().Name};
                lstOptionPages.Items.Add(item);
            }
        }

        private void SetInitiallyActivatedPage()
        {
            bool isSet = false;
            for (int i = 0; i < lstOptionPages.Items.Count; i++)
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

        private void lstOptionPages_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            pnlMain.Controls.Clear();

            var page = _pages[(string) e.Item.Tag];
            if (page != null)
                pnlMain.Controls.Add(page);
        }
    }
}