using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Repaint of the NumericUpDown, the composite control buttons are replaced because the 
    //original ones cannot be themed due to protected inheritance 
    class NGNumericUpDown : NumericUpDown
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
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                if (_themeManager.ThemingActive)
                {
                    ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background"); 
                    SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
                    //Hide those nonthemable butons
                    Controls[0].Hide();
                    //Add new themable buttons
                    Up = new NGButton();
                    Up.Text = "p";
                    Up.Font = new Font("Wingdings 3", 6f);
                    Up.SetBounds(Width - 17, 1, 16, Height / 2 - 1);
                    Up.Click += Up_Click;
                    Down = new NGButton();
                    Down.Text = "q";
                    Down.Font = new Font("Wingdings 3", 6f);
                    Down.SetBounds(Width - 17, Height/2, 16, Height / 2 - 1);
                    Down.Click += Down_Click;
                    Controls.Add(Up);
                    Controls.Add(Down);
                    Invalidate();
                }
            }

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
                    if (base.Enabled)
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
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                if (_themeManager.ThemingActive)
                {
                    //Fix Border
                    if (BorderStyle != BorderStyle.None)
                    e.Graphics.DrawRectangle(new Pen(_themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Border"), 1), 0, 0, base.Width - 1, base.Height - 1);
                }
            } 

        }

 
    }
}
