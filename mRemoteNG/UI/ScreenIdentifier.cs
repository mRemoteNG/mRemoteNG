using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI
{
    // Shows a click-through overlay on a screen (border + number) so the user can tell which screen a menu entry refers to
    [SupportedOSPlatform("windows")]
    public static class ScreenIdentifier
    {
        private static IdentifierForm _current;

        public static void Show(Screen screen, int screenNumber)
        {
            Hide();
            _current = new IdentifierForm(screen, screenNumber);
            _current.Show();
        }

        public static void Hide()
        {
            if (_current == null) return;
            _current.Close();
            _current.Dispose();
            _current = null;
        }

        private sealed class IdentifierForm : Form
        {
            private const int WS_EX_TOPMOST = 0x00000008;
            private const int WS_EX_TRANSPARENT = 0x00000020;
            private const int WS_EX_TOOLWINDOW = 0x00000080;
            private const int WS_EX_NOACTIVATE = 0x08000000;

            private static readonly Color AccentColor = Color.FromArgb(0, 120, 215);
            private static readonly Color KeyColor = Color.Magenta;

            private readonly string _number;
            private readonly string _details;

            public IdentifierForm(Screen screen, int screenNumber)
            {
                _number = screenNumber.ToString();
                _details = $"{screen.Bounds.Width} x {screen.Bounds.Height}" +
                           (screen.Primary ? $" ({Language.PrimaryScreen})" : string.Empty);

                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                // set before the handle exists so the window is created directly on the target screen
                Bounds = screen.Bounds;
                BackColor = KeyColor;
                TransparencyKey = KeyColor;
                DoubleBuffered = true;
            }

            protected override bool ShowWithoutActivation => true;

            protected override CreateParams CreateParams
            {
                get
                {
                    // topmost via style rather than the TopMost property, which would activate the window and close the menu
                    var cp = base.CreateParams;
                    cp.ExStyle |= WS_EX_TOPMOST | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
                    return cp;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var g = e.Graphics;
                var client = ClientRectangle;

                const int borderWidth = 10;
                using (var borderPen = new Pen(AccentColor, borderWidth))
                {
                    var borderRect = client;
                    borderRect.Inflate(-borderWidth / 2, -borderWidth / 2);
                    g.DrawRectangle(borderPen, borderRect);
                }

                var boxSize = Math.Min(client.Width, client.Height) / 4;
                var box = new Rectangle(client.Left + (client.Width - boxSize) / 2,
                                        client.Top + (client.Height - boxSize) / 2,
                                        boxSize, boxSize);
                using (var boxBrush = new SolidBrush(Color.FromArgb(32, 32, 32)))
                using (var boxPen = new Pen(AccentColor, 4))
                {
                    g.FillRectangle(boxBrush, box);
                    g.DrawRectangle(boxPen, box);
                }

                var numberRect = new Rectangle(box.Left, box.Top, box.Width, box.Height * 3 / 4);
                var detailsRect = new Rectangle(box.Left, box.Top + box.Height * 3 / 4 - box.Height / 10, box.Width, box.Height / 4);
                using (var numberFont = new Font("Segoe UI", boxSize * 0.5f, FontStyle.Bold, GraphicsUnit.Pixel))
                using (var detailsFont = new Font("Segoe UI", Math.Max(10, boxSize * 0.08f), FontStyle.Regular, GraphicsUnit.Pixel))
                {
                    var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;
                    TextRenderer.DrawText(g, _number, numberFont, numberRect, Color.White, flags);
                    TextRenderer.DrawText(g, _details, detailsFont, detailsRect, Color.Gainsboro, flags);
                }
            }
        }
    }
}
