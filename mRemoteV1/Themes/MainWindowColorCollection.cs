using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using mRemoteNG.My;
using mRemoteNG.Tools;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Themes
{
    class MainWindowColorCollection
    {
        private Color _windowBackgroundColor;
        private Color _menuBackgroundColor;
        private Color _menuTextColor;
        private Color _toolbarBackgroundColor;
        private Color _toolbarTextColor;

        #region Properties
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameWindowBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionWindowBackgroundColor")]
        public Color WindowBackgroundColor
        {
            get
            {
                return (_windowBackgroundColor);
            }
            set
            {
                if (_windowBackgroundColor == value)
                {
                    return;
                }
                _windowBackgroundColor = value;
                NotifyPropertyChanged("WindowBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuBackgroundColor")]
        public Color MenuBackgroundColor
        {
            get
            {
                return _menuBackgroundColor;
            }
            set
            {
                if (_menuBackgroundColor == value)
                {
                    return;
                }
                _menuBackgroundColor = value;
                NotifyPropertyChanged("MenuBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuTextColor")]
        public Color MenuTextColor
        {
            get
            {
                return _menuTextColor;
            }
            set
            {
                if (_menuTextColor == value)
                {
                    return;
                }
                _menuTextColor = value;
                NotifyPropertyChanged("MenuTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarBackgroundColor")]
        public Color ToolbarBackgroundColor
        {
            get
            {
                return _toolbarBackgroundColor;
            }
            set
            {
                if (_toolbarBackgroundColor == value || value.A < 255)
                {
                    return;
                }
                _toolbarBackgroundColor = value;
                NotifyPropertyChanged("ToolbarBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarTextColor")]
        public Color ToolbarTextColor
        {
            get
            {
                return _toolbarTextColor;
            }
            set
            {
                if (_toolbarTextColor == value)
                {
                    return;
                }
                _toolbarTextColor = value;
                NotifyPropertyChanged("ToolbarTextColor");
            }
        }
        #endregion

        public MainWindowColorCollection()
        {
            _windowBackgroundColor = SystemColors.AppWorkspace;
            _menuBackgroundColor = SystemColors.Control;
            _menuTextColor = SystemColors.ControlText;
            _toolbarBackgroundColor = SystemColors.Control;
            _toolbarTextColor = SystemColors.ControlText;
        }

        public void ApplyColors()
        {
            ApplyMainFormColors();
            ApplyToolStripColors();
            ApplyDockPanelColors();
            ApplyExternalToolsToolStripColors();
            ApplyQuickConnectToolStripColors();
            ApplyMenuColors(mRemoteNG.App.Runtime.MainForm.msMain.Items);
        }

        private void ApplyMainFormColors()
        {
            MenuStrip mainMenu = mRemoteNG.App.Runtime.MainForm.msMain;
            mainMenu.BackColor = this.ToolbarBackgroundColor;
            mainMenu.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyDockPanelColors()
        {
            DockPanel dockPanel = mRemoteNG.App.Runtime.MainForm.pnlDock;
            dockPanel.DockBackColor = this.WindowBackgroundColor;
        }

        private void ApplyExternalToolsToolStripColors()
        {
            ToolStrip externalTools = mRemoteNG.App.Runtime.MainForm.tsExternalTools;
            externalTools.BackColor = this.ToolbarBackgroundColor;
            externalTools.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyQuickConnectToolStripColors()
        {
            mRemoteNG.App.Runtime.MainForm.tsQuickConnect.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.MainForm.tsQuickConnect.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyToolStripColors()
        {
            ApplyToolStripContainerColors();
            ApplyToolStripContentPanelColors();
            ApplyTopToolStripColors();
            ApplyBottomToolStripColors();
            ApplyLeftToolStripColors();
            AppyRightToolStripColors();
        }

        private void ApplyToolStripContainerColors()
        {
            mRemoteNG.App.Runtime.MainForm.tsContainer.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.MainForm.tsContainer.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyToolStripContentPanelColors()
        {
            mRemoteNG.App.Runtime.MainForm.tsContainer.ContentPanel.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.MainForm.tsContainer.ContentPanel.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyTopToolStripColors()
        {
            mRemoteNG.App.Runtime.MainForm.tsContainer.TopToolStripPanel.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.MainForm.tsContainer.TopToolStripPanel.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyBottomToolStripColors()
        {
            mRemoteNG.App.Runtime.MainForm.tsContainer.BottomToolStripPanel.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.MainForm.tsContainer.BottomToolStripPanel.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyLeftToolStripColors()
        {
            mRemoteNG.App.Runtime.MainForm.tsContainer.LeftToolStripPanel.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.MainForm.tsContainer.LeftToolStripPanel.ForeColor = this.ToolbarTextColor;
        }

        private void AppyRightToolStripColors()
        {
            mRemoteNG.App.Runtime.MainForm.tsContainer.RightToolStripPanel.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.MainForm.tsContainer.RightToolStripPanel.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyMenuColors(ToolStripItemCollection itemCollection)
        {
            ToolStripMenuItem menuItem = default(ToolStripMenuItem);
            foreach (ToolStripItem item in itemCollection)
            {
                item.BackColor = this.MenuBackgroundColor;
                item.ForeColor = this.MenuTextColor;
                menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    ApplyMenuColors(menuItem.DropDownItems);
                }
            }
        }
    }
}