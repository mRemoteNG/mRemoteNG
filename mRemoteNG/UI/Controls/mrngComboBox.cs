using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    //Extended ComboBox class, the NGComboBox onPaint completely repaint the control as does the item painting
    //warning: THe DropDown style rendering is glitchy in this control, only use DropDownList or correct the rendering method
    internal class MrngComboBox : ComboBox
    {
        private ThemeManager _themeManager;

        public enum MouseState
        {
            HOVER,
            DOWN,
            OUT
        }

        public MouseState _mice { get; set; }

        public MrngComboBox()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
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
                if (args.Button != MouseButtons.Left) return;
                _mice = MouseState.DOWN;
                Invalidate();
            };
            MouseUp += (sender, args) =>
            {
                _mice = MouseState.OUT;

                Invalidate();
            };
            Invalidate();
        }

        private void NG_DrawItem(object sender, DrawItemEventArgs e)
        {
            var index = e.Index >= 0 ? e.Index : 0;
            Brush itemBrush = new SolidBrush(_themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Foreground"));

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                itemBrush = new SolidBrush(
                                           _themeManager
                                               .ActiveTheme.ExtendedPalette.getColor("List_Item_Selected_Foreground"));
                e.Graphics.FillRectangle(
                                         new SolidBrush(_themeManager
                                                        .ActiveTheme.ExtendedPalette
                                                        .getColor("List_Item_Selected_Background")),
                                         e.Bounds);
            }
            else
                e.Graphics.FillRectangle(
                                         new SolidBrush(_themeManager
                                                        .ActiveTheme.ExtendedPalette.getColor("ComboBox_Background")),
                                         e.Bounds);

            if (Items.Count > 0)
            {
                if (string.IsNullOrEmpty(DisplayMember))
                    e.Graphics.DrawString(Items[index].ToString(), e.Font, itemBrush, e.Bounds,
                                          StringFormat.GenericDefault);
                else
                {
                    if (Items[index].GetType().GetProperty(DisplayMember) != null)
                    {
                        e.Graphics.DrawString(
                                              Items[index]
                                                  .GetType().GetProperty(DisplayMember)?.GetValue(Items[index], null)
                                                  .ToString(),
                                              e.Font, itemBrush, e.Bounds, StringFormat.GenericDefault);
                    }
                }
            }

            e.DrawFocusRectangle();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            //Colors
            var Border = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Border");
            var Back = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Background");
            var Fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Foreground");
            var ButtBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Background");
            var ButtFore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Foreground");

            if (_mice == MouseState.HOVER)
            {
                Border = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_MouseOver_Border");
                ButtBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_MouseOver_Background");
                ButtFore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_MouseOver_Foreground");
            }

            if (DroppedDown)
            {
                Border = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_MouseOver_Border");
                ButtBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Pressed_Background");
                ButtFore = _themeManager.ActiveTheme.ExtendedPalette.getColor("ComboBox_Button_Pressed_Foreground");
            }


            e.Graphics.Clear(Back);

            //Border
            using (var p = new Pen(Border))
            {
                var boxRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            //Button
            using (var b = new SolidBrush(ButtBack))
            {
                e.Graphics.FillRectangle(b, Width - 18, 2, 16, Height - 4);
            }

            //Arrow
            e.Graphics.DrawString("\u25BC", Font, new SolidBrush(ButtFore), Width - 17, Height / 2 - 5);

            //Text
            var textRect = new Rectangle(2, 2, Width - 20, Height - 4);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, Fore, Back,
                                  TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NGComboBox
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);
        }
    }
}