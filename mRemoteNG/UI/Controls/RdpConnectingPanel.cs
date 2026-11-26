using System;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    /// <summary>
    /// Covers the RDP control's unpainted surface without hiding its native window.
    /// The owner dismisses this on OnConnected, before an interactive remote logon.
    /// </summary>
    internal sealed class RdpConnectingPanel : Panel
    {
        private readonly Label _message;

        internal RdpConnectingPanel(Control host, string message)
        {
            Dock = DockStyle.Fill;
            TabStop = false;
            Visible = false;
            _message = new Label
            {
                Dock = DockStyle.Fill,
                Text = message,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                TabStop = false
            };
            Controls.Add(_message);
            host.Controls.Add(this);
        }

        internal void ApplyColors(Color background, Color foreground)
        {
            RunOnUiThread(() =>
            {
                BackColor = background;
                ForeColor = foreground;
                _message.BackColor = background;
                _message.ForeColor = foreground;
            });
        }

        internal void ShowConnecting()
        {
            RunOnUiThread(() =>
            {
                Show();
                BringToFront();
            });
        }

        internal void Dismiss()
        {
            RunOnUiThread(Hide);
        }

        internal void DisposeOnUiThread()
        {
            RunOnUiThread(Dispose);
        }

        private void RunOnUiThread(Action action)
        {
            if (IsDisposed || Disposing)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(() => RunOnUiThread(action)));
                }
                catch (InvalidOperationException)
                {
                    // A late RDP event may race with disposal of the tab's window handle.
                    if (!IsDisposed && !Disposing && IsHandleCreated)
                        throw;
                }
                return;
            }

            action();
        }
    }
}
