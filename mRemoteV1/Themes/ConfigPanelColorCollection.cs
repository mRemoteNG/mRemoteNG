using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using mRemoteNG.My;
using mRemoteNG.Tools;

namespace mRemoteNG.Themes
{
    class ConfigPanelColorCollection
    {
        private Color _configPanelBackgroundColor;
        private Color _configPanelTextColor;
        private Color _configPanelCategoryTextColor;
        private Color _configPanelHelpBackgroundColor;
        private Color _configPanelHelpTextColor;
        private Color _configPanelGridLineColor;

        #region Properties
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelBackgroundColor")]
        public Color ConfigPanelBackgroundColor
        {
            get
            {
                return _configPanelBackgroundColor;
            }
            set
            {
                if (_configPanelBackgroundColor == value || value.A < 255)
                {
                    return;
                }
                _configPanelBackgroundColor = value;
                NotifyPropertyChanged("ConfigPanelBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelTextColor")]
        public Color ConfigPanelTextColor
        {
            get
            {
                return _configPanelTextColor;
            }
            set
            {
                if (_configPanelTextColor == value)
                {
                    return;
                }
                _configPanelTextColor = value;
                NotifyPropertyChanged("ConfigPanelTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelCategoryTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelCategoryTextColor")]
        public Color ConfigPanelCategoryTextColor
        {
            get
            {
                return _configPanelCategoryTextColor;
            }
            set
            {
                if (_configPanelCategoryTextColor == value)
                {
                    return;
                }
                _configPanelCategoryTextColor = value;
                NotifyPropertyChanged("ConfigPanelCategoryTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelHelpBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelHelpBackgroundColor")]
        public Color ConfigPanelHelpBackgroundColor
        {
            get
            {
                return _configPanelHelpBackgroundColor;
            }
            set
            {
                if (_configPanelHelpBackgroundColor == value || value.A < 255)
                {
                    return;
                }
                _configPanelHelpBackgroundColor = value;
                NotifyPropertyChanged("ConfigPanelHelpBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelHelpTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelHelpTextColor")]
        public Color ConfigPanelHelpTextColor
        {
            get
            {
                return _configPanelHelpTextColor;
            }
            set
            {
                if (_configPanelHelpTextColor == value)
                {
                    return;
                }
                _configPanelHelpTextColor = value;
                NotifyPropertyChanged("ConfigPanelHelpTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelGridLineColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelGridLineColor")]
        public Color ConfigPanelGridLineColor
        {
            get
            {
                return _configPanelGridLineColor;
            }
            set
            {
                if (_configPanelGridLineColor == value)
                {
                    return;
                }
                _configPanelGridLineColor = value;
                NotifyPropertyChanged("ConfigPanelGridLineColor");
            }
        }
        #endregion

        public ConfigPanelColorCollection()
        {
            _configPanelBackgroundColor = SystemColors.Window;
            _configPanelTextColor = SystemColors.WindowText;
            _configPanelCategoryTextColor = SystemColors.ControlText;
            _configPanelHelpBackgroundColor = SystemColors.Control;
            _configPanelHelpTextColor = SystemColors.ControlText;
            _configPanelGridLineColor = SystemColors.InactiveBorder;
        }

        public void ApplyColors()
        {
            ApplyToolbarColor();
            ApplyPanelColor();
            ApplyHelpColor();
            ApplyCategoryColor();
        }

        private void ApplyToolbarColor()
        {
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.BackColor = this.ToolbarBackgroundColor;
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.ForeColor = this.ToolbarTextColor;
        }

        private void ApplyPanelColor()
        {
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.ViewBackColor = this.ConfigPanelBackgroundColor;
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.ViewForeColor = this.ConfigPanelTextColor;
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.LineColor = this.ConfigPanelGridLineColor;
        }

        private void ApplyHelpColor()
        {
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.HelpBackColor = this.ConfigPanelHelpBackgroundColor;
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.HelpForeColor = this.ConfigPanelHelpTextColor;
        }

        private void ApplyCategoryColor()
        {
            mRemoteNG.App.Runtime.Windows.configForm.pGrid.CategoryForeColor = this.ConfigPanelCategoryTextColor;
        }
    }
}