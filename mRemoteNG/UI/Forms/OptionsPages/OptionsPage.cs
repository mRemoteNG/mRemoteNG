using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public class OptionsPage : UserControl
    {
        protected OptionsPage()
        {
            InitializeComponent();
            //ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }

        #region Public Properties

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Browsable(false)] public virtual string PageName { get; set; }

        public virtual Icon PageIcon { get; protected set; }
        public virtual Image IconImage => PageIcon?.ToBitmap();

        #endregion

        #region Public Methods

        public virtual void ApplyLanguage()
        {
        }

        public virtual void LoadSettings()
        {
        }

        public virtual void SaveSettings()
        {
        }

        public virtual void RevertSettings()
        {
        }

        /// <summary>
        /// Checks if registry settings were applied and disables the corresponding UI controls.
        /// If any settings are applied, it also displays an information label.
        /// </summary>
        public virtual void LoadRegistrySettings()
        {
        }

        #endregion

        protected virtual void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            Invalidate();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // OptionsPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "OptionsPage";
            ResumeLayout(false);
        }

        /// <summary>
        /// Disables the specified control by setting its Enabled property to false.
        /// For TextBox controls, additionally sets the ReadOnly property based on the Enabled state.
        /// Does nothing if the control is null.
        /// </summary>
        /// <param name="control">The control to be disabled.</param>
        protected static void DisableControl(Control control)
        {
            if (control == null) return;

            control.Enabled = false;

            if (control is TextBox)
            {
                // If it's a TextBox, set the ReadOnly property
                ((TextBox)control).ReadOnly = control.Enabled;
            }
        }
    }
    internal class DropdownList
    {
        public int Index { get; set; }
        public string DisplayString { get; set; }

        public DropdownList(int argIndex, string argDisplay)
        {
            Index = argIndex;
            DisplayString = argDisplay;
        }
    }
}