﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.UI.Forms.OptionsPages;

namespace mRemoteNG.UI.Forms
{
    public partial class frmOptions : Form
    {
        private readonly string pageName;
        private ImageList _pageIconImageList;
        private Dictionary<string, OptionsPage> _pages;

        public frmOptions()
        {
            InitializeComponent();
            pageName = Language.strStartupExit;
        }

        public frmOptions(string pn)
        {
            InitializeComponent();
            pageName = pn;
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
                optionPage.ApplyLanguage();
        }

        private void CompileListOfOptionsPages()
        {
            _pages = new Dictionary<string, OptionsPage>
            {
                {typeof(StartupExitPage).Name, new StartupExitPage()},
                {typeof(AppearancePage).Name, new AppearancePage()},
                {typeof(TabsPanelsPage).Name, new TabsPanelsPage()},
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
            _pageIconImageList = new ImageList {ColorDepth = ColorDepth.Depth32Bit};
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
            var isSet = false;
            for (var i = 0; i < lstOptionPages.Items.Count; i++)
            {
                if (!lstOptionPages.Items[i].Text.Equals(pageName)) continue;
                lstOptionPages.Items[i].Selected = true;
                isSet = true;
                break;
            }

            if (!isSet)
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