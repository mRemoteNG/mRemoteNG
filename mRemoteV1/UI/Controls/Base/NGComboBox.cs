using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{

    //warning: THe DropDown style rendering is glitchy in this control, only use DropDownList or correct the rendering method
    class NGComboBox : ComboBox
    {
        private ThemeManager _themeManager;
        public enum MouseState
        {
            HOVER,
            DOWN,
            OUT
        }
        public MouseState _mice { get; set; }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Background");
                ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Foreground");
                DrawMode = DrawMode.OwnerDrawFixed;
                SetStyle(ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.UserPaint, true);
                DrawItem += NG_DrawItem;

                _mice = MouseState.OUT;
                MouseEnter += (sender, args) =>
                {
                    _mice = MouseState.HOVER;
                    Invalidate();
                };
                MouseLeave += (sender, args) =>
                {
                    _mice = MouseState.OUT;
                    Invalidate();
                };
                MouseDown += (sender, args) =>
                {
                    if (args.Button == MouseButtons.Left)
                    {
                        _mice = MouseState.DOWN;
                        Invalidate();
                    }
                };
                MouseUp += (sender, args) =>
                {
                    _mice = MouseState.OUT;

                    Invalidate();
                };
            } 
        }

        private void NG_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            Brush itemBrush= new SolidBrush(_themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Foreground"));

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                itemBrush = new SolidBrush(_themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Selected_Foreground"));
                e.Graphics.FillRectangle(new SolidBrush(_themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Selected_Background")), e.Bounds);
            }
            else
                e.Graphics.FillRectangle(new SolidBrush(_themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Background")), e.Bounds);

            e.Graphics.DrawString(Items[index].ToString(), e.Font, itemBrush, e.Bounds, StringFormat.GenericDefault);
  
            e.DrawFocusRectangle();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Colors
            Color Border = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Border");
            Color Back = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Background");
            Color Fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Foreground");
            Color ButtBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Background");
            Color ButtFore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Foreground");

            if (_mice == MouseState.HOVER)
            {
                Border = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_MouseOver_Border");
                ButtBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_MouseOver_Background");
                ButtFore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_MouseOver_Foreground");
            }if (DroppedDown)
            {
                Border = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_MouseOver_Border");
                ButtBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Pressed_Background");
                ButtFore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Pressed_Foreground");
            }

            
  
            e.Graphics.Clear(Back);
                
            //Border
            using (Pen p = new Pen(Border))
            {
                Rectangle boxRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, boxRect);
            }
            //Button
            using (SolidBrush b = new SolidBrush(ButtBack))
            {
                e.Graphics.FillRectangle(b, Width - 18, 2, 16, Height - 4);
            }

            //Arrow
            e.Graphics.DrawString("q", new Font("Wingdings 3", 8f), new SolidBrush(ButtFore), Width-17, Height/2 -5);
 
            //Text
            Rectangle textRect = new Rectangle(2, 2, Width - 20, Height - 4);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, Fore, Back, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }
}
