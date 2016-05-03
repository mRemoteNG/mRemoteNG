using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.My;
using mRemoteNG.UI.Forms.OptionsPages;

namespace mRemoteNG.UI.Forms
{
	public partial class OptionsForm
	{
        private Dictionary<OptionsPage, PageInfo> _pages;
        private ImageList _pageIconImageList;
        private OptionsPage _startPage;
        private OptionsPage _selectedPage;


		public OptionsForm()
		{
			// This call is required by the designer.
			InitializeComponent();
			// Add any initialization after the InitializeComponent() call.
            _selectedPage = null;
            Runtime.FontOverride(this);
            CompileListOfOptionsPages();
            SetImageListForListView();
            SetStartPage();
		}

        private void CompileListOfOptionsPages()
        {
            _pages = new Dictionary<OptionsPage, PageInfo>();
            _pages.Add(new StartupExitPage(), new PageInfo());
            _pages.Add(new AppearancePage(), new PageInfo());
            _pages.Add(new TabsPanelsPage(), new PageInfo());
            _pages.Add(new ConnectionsPage(), new PageInfo());
            _pages.Add(new SqlServerPage(), new PageInfo());
            _pages.Add(new UpdatesPage(), new PageInfo());
            _pages.Add(new ThemePage(), new PageInfo());
            _pages.Add(new KeyboardPage(), new PageInfo());
            _pages.Add(new AdvancedPage(), new PageInfo());
        }

        private void SetImageListForListView()
        {
            _pageIconImageList = new ImageList();
            _pageIconImageList.ColorDepth = ColorDepth.Depth32Bit;
            PageListView.LargeImageList = _pageIconImageList;
            PageListView.SmallImageList = _pageIconImageList;
        }

        private void SetStartPage()
        {
            _startPage = GetPageFromType(typeof(StartupExitPage));
        }

        #region Public Methods
		public DialogResult ShowDialog(IWin32Window ownerWindow, Type pageType)
		{
			_startPage = GetPageFromType(pageType);
			return ShowDialog(ownerWindow);
		}
        #endregion
	    
        #region Private Methods
        #region Event Handlers
		public void OptionsForm_Load(Object sender, EventArgs e)
		{
            AddOptionsPagesToListView();
			ApplyLanguage();
			LoadSettings();
			ShowPage(_startPage);
		}

        private void AddOptionsPagesToListView()
        {
            foreach (KeyValuePair<OptionsPage, PageInfo> keyValuePair in _pages)
            {
                OptionsPage page = keyValuePair.Key;
                PageInfo pageInfo = keyValuePair.Value;
                _pageIconImageList.Images.Add(pageInfo.IconKey, page.PageIcon);
                ListViewItem item = new ListViewItem(page.PageName, pageInfo.IconKey);
                pageInfo.ListViewItem = PageListView.Items.Add(item);
            }
        }
		
		public void OptionsForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
				SaveSettings();
			else
				RevertSettings();
		}
			
		public void PageListView_ItemSelectionChanged(Object sender, ListViewItemSelectionChangedEventArgs e)
		{
            if (!e.IsSelected || _pages.Count < 1)
				return ;
			OptionsPage page = GetPageFromListViewItem(e.Item);
			if (_selectedPage != page)
				ShowPage(page);
			SelectNextControl(PageListView, true, true, true, true);
		}
		
		public void PageListView_MouseUp(Object sender, MouseEventArgs e)
		{
			if (PageListView.SelectedIndices.Count == 0)
			{
				PageInfo pageInfo = _pages[_selectedPage];
				pageInfo.ListViewItem.Selected = true;
			}
			SelectNextControl(PageListView, true, true, true, true);
		}
		
		public void OkButton_Click(Object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
		
		public void CancelButtonControl_Click(Object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
        #endregion
		
		private void ApplyLanguage()
		{
			Text = Language.strMenuOptions;
			OkButton.Text = Language.strButtonOK;
			CancelButtonControl.Text = Language.strButtonCancel;
				
			foreach (OptionsPage page in _pages.Keys)
			{
				try
				{
					page.ApplyLanguage();
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.ApplyLanguage() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
				}
			}
		}
		
		private void LoadSettings()
		{
			foreach (OptionsPage page in _pages.Keys)
			{
				try
				{
					page.LoadSettings();
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.LoadSettings() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
				}
			}
		}
		
		private void SaveSettings()
		{
			foreach (OptionsPage page in _pages.Keys)
			{
				try
				{
					page.SaveSettings();
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.SaveSettings() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
				}
			}
		}
		
		private void RevertSettings()
		{
			foreach (OptionsPage page in _pages.Keys)
			{
				try
				{
					page.RevertSettings();
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.RevertSettings() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
				}
			}
		}
		
		private OptionsPage GetPageFromType(Type pageType)
		{
			foreach (OptionsPage page in _pages.Keys)
			{
				if (page.GetType() == pageType)
				{
					return page;
				}
			}
			return null;
		}
		
		private OptionsPage GetPageFromListViewItem(ListViewItem listViewItem)
		{
			foreach (KeyValuePair<OptionsPage, PageInfo> keyValuePair in _pages)
			{
				OptionsPage page = keyValuePair.Key;
				PageInfo pageInfo = keyValuePair.Value;
				if (pageInfo.ListViewItem == listViewItem)
				{
					return page;
				}
			}
			return null;
		}
		
		private void ShowPage(OptionsPage newPage)
		{
            DeactivateOldPage();
            ActivateNewPage(newPage);
		}

        private void DeactivateOldPage()
        {
            if (_selectedPage != null)
            {
                var oldPage = _selectedPage;
                oldPage.Visible = false;
                if (_pages.ContainsKey(oldPage))
                {
                    var oldPageInfo = _pages[oldPage];
                    oldPageInfo.ListViewItem.Selected = false;
                }
            }
        }

        private void ActivateNewPage(OptionsPage newPage)
        {
            _selectedPage = newPage;
            if (newPage != null)
            {
                newPage.Parent = PagePanel;
                newPage.Dock = DockStyle.Fill;
                newPage.Visible = true;
                if (_pages.ContainsKey(newPage))
                {
                    PageInfo newPageInfo = _pages[newPage];
                    newPageInfo.ListViewItem.Selected = true;
                }
            }
        }
        #endregion
		
        #region Private Classes
		private class PageInfo
		{
			public string IconKey {get; set;}
			public ListViewItem ListViewItem {get; set;}
				
			public PageInfo()
			{
				IconKey = Guid.NewGuid().ToString();
			}
		}
        #endregion
	}
}