using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App.Update;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms.OptionsPages;

namespace mRemoteNG.UI.Forms
{
	public partial class frmOptions : Form
    {
        private Dictionary<string, OptionsPage> _pages;
        private ImageList _pageIconImageList;
        private readonly string _pageName;
        private readonly IConnectionInitiator _connectionInitiator;
	    private readonly Action<WindowType> _showWindowAction;
        private readonly Func<NotificationAreaIcon> _notificationAreaIconBuilder;
        private readonly ConnectionsService _connectionsService;
        private readonly AppUpdater _appUpdater;

        public frmOptions(IConnectionInitiator connectionInitiator, Action<WindowType> showWindowAction, Func<NotificationAreaIcon> notificationAreaIconBuilder, ConnectionsService connectionsService, AppUpdater appUpdater) 
            : this(connectionInitiator, showWindowAction, notificationAreaIconBuilder, connectionsService, appUpdater, Language.strStartupExit)
        {
        }

        public frmOptions(IConnectionInitiator connectionInitiator, Action<WindowType> showWindowAction, Func<NotificationAreaIcon> notificationAreaIconBuilder, ConnectionsService connectionsService, AppUpdater appUpdater, string pageName)
        {
            _connectionInitiator = connectionInitiator.ThrowIfNull(nameof(connectionInitiator));
	        _showWindowAction = showWindowAction.ThrowIfNull(nameof(showWindowAction));
            _notificationAreaIconBuilder = notificationAreaIconBuilder;
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
            _appUpdater = appUpdater.ThrowIfNull(nameof(appUpdater));
            _pageName = pageName.ThrowIfNull(nameof(pageName));
	        InitializeComponent();
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
            if(Themes.ThemeManager.getInstance().ThemingActive)
            {
                BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            }
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
                {typeof(AppearancePage).Name, new AppearancePage(_connectionInitiator, _notificationAreaIconBuilder)},
                {typeof(TabsPanelsPage).Name, new TabsPanelsPage()},
                {typeof(NotificationsPage).Name, new NotificationsPage()},
                {typeof(ConnectionsPage).Name, new ConnectionsPage()},
                {typeof(CredentialsPage).Name, new CredentialsPage()},
                {typeof(SqlServerPage).Name, new SqlServerPage(_connectionsService)},
                {typeof(UpdatesPage).Name, new UpdatesPage(_appUpdater, _showWindowAction)},
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
                lstOptionPages.AddObject(page);
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


        private void LstOptionPages_SelectedIndexChanged(object sender, System.EventArgs e)
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