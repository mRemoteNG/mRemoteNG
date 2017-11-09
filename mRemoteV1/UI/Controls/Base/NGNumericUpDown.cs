using mRemoteNG.Themes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Repaint of the NumericUpDown, the composite control buttons are replaced because the 
    //original ones cannot be themed due to protected inheritance 
    internal class NGNumericUpDown : NumericUpDown
    {

        private ThemeManager _themeManager;
        private NGButton Up;
        private NGButton Down;

        public NGNumericUpDown() : base()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Tools.DesignModeTest.IsInDesignMode(this)) return;
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background"); 
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            //Hide those nonthemable butons
            Controls[0].Hide();
            //Add new themable buttons
            Up = new NGButton
            {
                Text = "p",
                Font = new Font("Wingdings 3", 6f)
            };
            Up.SetBounds(Width - 17, 1, 16, Height / 2 - 1);
            Up.Click += Up_Click;
            Down = new NGButton
            {
                Text = "q",
                Font = new Font("Wingdings 3", 6f)
            };
            Down.SetBounds(Width - 17, Height/2, 16, Height / 2 - 1);
            Down.Click += Down_Click;
            Controls.Add(Up);
            Controls.Add(Down);
            Invalidate();
        }

        private void Down_Click(object sender, EventArgs e)
        {
            DownButton();
        }

        private void Up_Click(object sender, EventArgs e)
        {
            UpButton();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                if (_themeManager.ThemingActive)
                {
                    if (Enabled)
                    {
                        ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                        BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
                    }
                    else
                    {
                        BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Disabled_Background");
                    }
                }
            }
            base.OnEnabledChanged(e);
            Invalidate();
        }


        //Redrawing border
        protected override void OnPaint(PaintEventArgs e)
        { 
            base.OnPaint(e);
            if (Tools.DesignModeTest.IsInDesignMode(this)) return;
            if (!_themeManager.ThemingActive) return;
            //Fix Border
            if (BorderStyle != BorderStyle.None)
                e.Graphics.DrawRectangle(new Pen(_themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Border"), 1), 0, 0, Width - 1, Height - 1);
        }

 
    }
}
