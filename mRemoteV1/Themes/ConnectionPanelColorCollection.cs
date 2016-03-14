using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using mRemoteNG.My;
using mRemoteNG.Tools;

namespace mRemoteNG.Themes
{
    class ConnectionPanelColorCollection
    {
        private Color _connectionsPanelBackgroundColor;
        private Color _connectionsPanelTextColor;
        private Color _connectionsPanelTreeLineColor;
        private Color _searchBoxBackgroundColor;
        private Color _searchBoxTextPromptColor;
        private Color _searchBoxTextColor;

        #region Properties
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelBackgroundColor")]
        public Color ConnectionsPanelBackgroundColor
        {
            get { return _connectionsPanelBackgroundColor; }
            set
            {
                if (_connectionsPanelBackgroundColor == value || value.A < 255)
                {
                    return;
                }
                _connectionsPanelBackgroundColor = value;
                NotifyPropertyChanged("ConnectionsPanelBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelTextColor")]
        public Color ConnectionsPanelTextColor
        {
            get { return _connectionsPanelTextColor; }
            set
            {
                if (_connectionsPanelTextColor == value)
                {
                    return;
                }
                _connectionsPanelTextColor = value;
                NotifyPropertyChanged("ConnectionsPanelTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelTreeLineColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelTreeLineColor")]
        public Color ConnectionsPanelTreeLineColor
        {
            get
            {
                return _connectionsPanelTreeLineColor;
            }
            set
            {
                if (_connectionsPanelTreeLineColor == value)
                {
                    return;
                }
                _connectionsPanelTreeLineColor = value;
                NotifyPropertyChanged("ConnectionsPanelTreeLineColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxBackgroundColor")]
        public Color SearchBoxBackgroundColor
        {
            get
            {
                return _searchBoxBackgroundColor;
            }
            set
            {
                if (_searchBoxBackgroundColor == value || value.A < 255)
                {
                    return;
                }
                _searchBoxBackgroundColor = value;
                NotifyPropertyChanged("SearchBoxBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxTextPromptColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxTextPromptColor")]
        public Color SearchBoxTextPromptColor
        {
            get
            {
                return _searchBoxTextPromptColor;
            }
            set
            {
                if (_searchBoxTextPromptColor == value)
                {
                    return;
                }
                _searchBoxTextPromptColor = value;
                NotifyPropertyChanged("SearchBoxTextPromptColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxTextColor")]
        public Color SearchBoxTextColor
        {
            get
            {
                return _searchBoxTextColor;
            }
            set
            {
                if (_searchBoxTextColor == value)
                {
                    return;
                }
                _searchBoxTextColor = value;
                NotifyPropertyChanged("SearchBoxTextColor");
            }
        }
        #endregion

        public ConnectionPanelColorCollection()
        {
            _connectionsPanelBackgroundColor = SystemColors.Window;
            _connectionsPanelTextColor = SystemColors.WindowText;
            _connectionsPanelTreeLineColor = Color.Black;
            _searchBoxBackgroundColor = SystemColors.Window;
            _searchBoxTextPromptColor = SystemColors.GrayText;
            _searchBoxTextColor = SystemColors.WindowText;
        }

        public void ApplyColors()
        {
            ApplyTopMenuColors();
            ApplyTreePanelColors();
            ApplySearchBarColors();
        }
        
        private void ApplyTopMenuColors()
        {
            mRemoteNG.App.Runtime.Windows.treeForm.msMain.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.Windows.treeForm.msMain.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyTreePanelColors()
        {
            mRemoteNG.App.Runtime.Windows.treeForm.tvConnections.BackColor = this.ConnectionsPanelBackgroundColor;
            mRemoteNG.App.Runtime.Windows.treeForm.tvConnections.ForeColor = this.ConnectionsPanelTextColor;
            mRemoteNG.App.Runtime.Windows.treeForm.tvConnections.LineColor = this.ConnectionsPanelTreeLineColor;
            mRemoteNG.App.Runtime.Windows.treeForm.BackColor = this.ToolbarBackgroundColor;
        }

        private void ApplySearchBarColors()
        {
            mRemoteNG.App.Runtime.Windows.treeForm.txtSearch.BackColor = this.SearchBoxBackgroundColor;
            mRemoteNG.App.Runtime.Windows.treeForm.txtSearch.ForeColor = this.SearchBoxTextPromptColor;
        }
    }
}