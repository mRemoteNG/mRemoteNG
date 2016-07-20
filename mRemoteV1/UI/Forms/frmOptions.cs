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

        public frmOptions()
        {
            InitializeComponent();
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
            _pages = new Dictionary<string, OptionsPage>();
            _pages.Add(typeof(StartupExitPage).Name, new StartupExitPage());
            _pages.Add(typeof(AppearancePage).Name, new AppearancePage());
            _pages.Add(typeof(TabsPanelsPage).Name, new TabsPanelsPage());
            _pages.Add(typeof(ConnectionsPage).Name, new ConnectionsPage());
            _pages.Add(typeof(SqlServerPage).Name, new SqlServerPage());
            _pages.Add(typeof(UpdatesPage).Name, new UpdatesPage());
            _pages.Add(typeof(ThemePage).Name, new ThemePage());
            _pages.Add(typeof(AdvancedPage).Name, new AdvancedPage());
            _pages.Add(typeof(SecurityPage).Name, new SecurityPage());
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
                var item = new ListViewItem(page.PageName, page.PageName);
                item.Tag = page.GetType().Name;
                lstOptionPages.Items.Add(item);
            }
        }

        private void SetInitiallyActivatedPage()
        {
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