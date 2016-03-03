// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using mRemoteNG.Forms.OptionsPages;
//using mRemoteNG.App.Runtime;
using mRemoteNG.My;


namespace mRemoteNG.Forms
{
	public partial class OptionsForm
		{
#region Constructors
			public OptionsForm()
			{
				// This call is required by the designer.
				InitializeComponent();
				// Add any initialization after the InitializeComponent() call.
				
				FontOverride(this);
				
				_pages.Add(new Forms.OptionsPages.StartupExitPage(), new PageInfo());
				_pages.Add(new AppearancePage(), new PageInfo());
				_pages.Add(new TabsPanelsPage(), new PageInfo());
				_pages.Add(new ConnectionsPage(), new PageInfo());
				_pages.Add(new SqlServerPage(), new PageInfo());
				_pages.Add(new UpdatesPage(), new PageInfo());
				_pages.Add(new ThemePage(), new PageInfo());
				_pages.Add(new KeyboardPage(), new PageInfo());
				_pages.Add(new AdvancedPage(), new PageInfo());
				
				_startPage = GetPageFromType(typeof(Forms.OptionsPages.StartupExitPage));
				
				_pageIconImageList.ColorDepth = ColorDepth.Depth32Bit;
				PageListView.LargeImageList = _pageIconImageList;
			}
#endregion
			
#region Public Methods
			public DialogResult ShowDialog(IWin32Window ownerWindow, Type pageType)
			{
				_startPage = GetPageFromType(pageType);
				return ShowDialog(ownerWindow);
			}
#endregion
			
#region Private Fields
			private Dictionary<OptionsPage, PageInfo> _pages = new Dictionary<OptionsPage, PageInfo>();
			private ImageList _pageIconImageList = new ImageList();
			
			private OptionsPage _startPage;
			private OptionsPage _selectedPage = null;
#endregion
			
#region Private Methods
#region Event Handlers
			public void OptionsForm_Load(System.Object sender, EventArgs e)
			{
				foreach (KeyValuePair<OptionsPage, PageInfo> keyValuePair in _pages)
				{
					OptionsPage page = KeyValuePair.Key;
					PageInfo pageInfo = KeyValuePair.Value;
					_pageIconImageList.Images.Add(pageInfo.IconKey, page.PageIcon);
					pageInfo.ListViewItem = PageListView.Items.Add(page.PageName, pageInfo.IconKey);
				}
				
				ApplyLanguage();
				LoadSettings();
				
				ShowPage(_startPage);
			}
			
			public void OptionsForm_FormClosing(System.Object sender, FormClosingEventArgs e)
			{
				if (DialogResult == DialogResult.OK)
				{
					SaveSettings();
				}
				else
				{
					RevertSettings();
				}
			}
			
			public void PageListView_ItemSelectionChanged(System.Object sender, ListViewItemSelectionChangedEventArgs e)
			{
				if (!e.IsSelected)
				{
					return ;
				}
				if (_pages.Count < 1)
				{
					return ;
				}
				OptionsPage page = GetPageFromListViewItem(e.Item);
				if (_selectedPage != page)
				{
					ShowPage(page);
				}
				SelectNextControl(PageListView, true, true, true, true);
			}
			
			public void PageListView_MouseUp(System.Object sender, MouseEventArgs e)
			{
				if (PageListView.SelectedIndices.Count == 0)
				{
					PageInfo pageInfo = _pages[_selectedPage];
					pageInfo.ListViewItem.Selected = true;
				}
				SelectNextControl(PageListView, true, true, true, true);
			}
			
			public void OkButton_Click(System.Object sender, EventArgs e)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
			
			public void CancelButtonControl_Click(System.Object sender, EventArgs e)
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
						MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.ApplyLanguage() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
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
						MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.LoadSettings() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
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
						MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.SaveSettings() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
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
						MessageCollector.AddExceptionMessage(message: string.Format("OptionsPage.RevertSettings() failed for page {0}.", page.PageName), ex: ex, logOnly: true);
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
					OptionsPage page = KeyValuePair.Key;
					PageInfo pageInfo = KeyValuePair.Value;
					if (pageInfo.ListViewItem == listViewItem)
					{
						return page;
					}
				}
				return null;
			}
			
			private void ShowPage(OptionsPage newPage)
			{
				if (_selectedPage != null)
				{
					OptionsPage oldPage = _selectedPage;
					oldPage.Visible = false;
					if (_pages.ContainsKey(oldPage))
					{
						PageInfo oldPageInfo = _pages[oldPage];
						oldPageInfo.ListViewItem.Selected = false;
					}
				}
				
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
