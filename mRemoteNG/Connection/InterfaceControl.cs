using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Tabs;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public sealed partial class InterfaceControl
    {
        public ProtocolBase Protocol { get; set; }
        public ConnectionInfo Info { get; set; }
        public ConnectionInfo OriginalInfo { get; set; }
        public ConnectionInfo SSHTunnelInfo { get; set; }

        /// <summary>
        /// The panel where protocol controls (PuTTY, RDP, etc.) should be placed.
        /// Returns SplitContainer.Panel1 if SFTP split view is enabled, otherwise returns this.
        /// </summary>
        public Control ProtocolPanel => _splitView?.Panel1 ?? (Control)this;

        private SplitContainer _splitView;
        private SftpBrowserPanel _sftpPanel;


        public InterfaceControl(Control parent, ProtocolBase protocol, ConnectionInfo info)
        {
            try
            {
                Protocol = protocol;
                Info = info;
                Parent = parent;
                Location = new Point(0, 0);
                Size = Parent.Size;
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                InitializeComponent();
                
                // Enable custom painting for border
                this.Paint += InterfaceControl_Paint;
                
                // Set padding to prevent content from covering the frame border
                UpdatePaddingForFrameColor();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    "Couldn\'t create new InterfaceControl" + Environment.NewLine +
                                                    ex.Message);
            }
        }

        private void InterfaceControl_Paint(object sender, PaintEventArgs e)
        {
            // Draw colored border based on ConnectionFrameColor property
            if (Info?.ConnectionFrameColor != null && Info.ConnectionFrameColor != ConnectionFrameColor.None)
            {
                Color frameColor = GetFrameColor(Info.ConnectionFrameColor);
                int borderWidth = 4; // 4 pixel border for visibility
                
                using (Pen pen = new Pen(frameColor, borderWidth))
                {
                    // Draw border inside the control bounds
                    Rectangle rect = new Rectangle(
                        borderWidth / 2,
                        borderWidth / 2,
                        this.Width - borderWidth,
                        this.Height - borderWidth
                    );
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private void UpdatePaddingForFrameColor()
        {
            // Add padding to prevent content from covering the frame border
            if (Info?.ConnectionFrameColor != null && Info.ConnectionFrameColor != ConnectionFrameColor.None)
            {
                int borderWidth = 4; // Must match the border width in InterfaceControl_Paint
                // Add 2px margin so the border is fully visible and not covered by child controls
                int padding = borderWidth / 2 + 2;
                this.Padding = new Padding(padding);
            }
            else
            {
                this.Padding = new Padding(0);
            }
        }

        private Color GetFrameColor(ConnectionFrameColor frameColor)
        {
            return frameColor switch
            {
                ConnectionFrameColor.Red => Color.FromArgb(220, 53, 69),      // Bootstrap danger red
                ConnectionFrameColor.Yellow => Color.FromArgb(255, 193, 7),   // Warning yellow
                ConnectionFrameColor.Green => Color.FromArgb(40, 167, 69),    // Success green
                ConnectionFrameColor.Blue => Color.FromArgb(0, 123, 255),     // Primary blue
                ConnectionFrameColor.Purple => Color.FromArgb(111, 66, 193),  // Purple
                _ => Color.Transparent
            };
        }

        /// <summary>
        /// Enables MobaXterm-style SFTP browser alongside the terminal.
        /// Creates a vertical SplitContainer: left = terminal, right = SFTP file browser.
        /// </summary>
        public void EnableSftpBrowser(ConnectionInfo connectionInfo)
        {
            try
            {
                _splitView = new SplitContainer
                {
                    Dock = DockStyle.Fill,
                    Orientation = Orientation.Vertical,
                    SplitterWidth = 4,
                    BorderStyle = BorderStyle.None
                };

                Controls.Add(_splitView);
                _splitView.BringToFront();

                // Set Panel2MinSize and FixedPanel after the control is parented and has a real size
                _splitView.Panel2MinSize = 200;
                _splitView.FixedPanel = FixedPanel.Panel2;

                _sftpPanel = new SftpBrowserPanel { Dock = DockStyle.Fill };
                _splitView.Panel2.Controls.Add(_sftpPanel);

                // Set initial splitter — Panel2 (SFTP) gets ~350px, rest goes to terminal
                try
                {
                    int distance = Math.Max(200, _splitView.Width - 350);
                    _splitView.SplitterDistance = distance;
                }
                catch { /* Width may be 0 during init — FixedPanel.Panel2 handles it on resize */ }

                // When splitter moves, resize PuTTY window to fit new Panel1 size
                _splitView.SplitterMoved += (s, args) =>
                {
                    _lastSplitterDistance = _splitView.SplitterDistance;
                    SplitView_SplitterMoved(s, args);
                };

                // Wire up close/toggle button
                _sftpPanel.CloseRequested += (s, args) => ToggleSftpPanel();

                // Create a small "SFTP" toggle button anchored to the right edge of Panel1
                // Shown only when SFTP panel is collapsed
                _btnShowSftp = new Button
                {
                    Text = "SFTP",
                    Width = 22,
                    Dock = DockStyle.Right,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 7F),
                    BackColor = System.Drawing.Color.FromArgb(50, 50, 50),
                    ForeColor = System.Drawing.Color.LimeGreen,
                    Visible = false,
                    Cursor = Cursors.Hand
                };
                _btnShowSftp.FlatAppearance.BorderSize = 0;
                _btnShowSftp.Click += (s, args) => ToggleSftpPanel();
                // Draw text vertically
                _btnShowSftp.Paint += (s, pe) =>
                {
                    pe.Graphics.Clear(_btnShowSftp.BackColor);
                    using var sf = new System.Drawing.StringFormat { FormatFlags = System.Drawing.StringFormatFlags.DirectionVertical };
                    using var brush = new System.Drawing.SolidBrush(_btnShowSftp.ForeColor);
                    pe.Graphics.DrawString("SFTP", _btnShowSftp.Font, brush, 3, 4, sf);
                };
                _splitView.Panel1.Controls.Add(_btnShowSftp);
                _btnShowSftp.BringToFront();

                // Connect SFTP with retry (SSH session may not be ready yet)
                _sftpPanel.ConnectWithRetry(connectionInfo.Hostname, connectionInfo.Username, connectionInfo.Password, connectionInfo.Port);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("InterfaceControl.EnableSftpBrowser failed", ex);
            }
        }

        private int _lastSplitterDistance;
        private Button _btnShowSftp;

        /// <summary>
        /// Toggles the SFTP panel visibility by collapsing/expanding Panel2.
        /// </summary>
        public void ToggleSftpPanel()
        {
            if (_splitView == null) return;
            _splitView.Panel2Collapsed = !_splitView.Panel2Collapsed;
            if (!_splitView.Panel2Collapsed && _lastSplitterDistance > 0)
            {
                try { _splitView.SplitterDistance = _lastSplitterDistance; }
                catch (InvalidOperationException) { /* Splitter distance out of range during resize */ }
            }

            // Show/hide the "Show SFTP" toggle button
            if (_btnShowSftp != null)
                _btnShowSftp.Visible = _splitView.Panel2Collapsed;

            // Trigger resize so terminal redraws
            SplitView_SplitterMoved(null, null);
        }

        private void SplitView_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // Trigger the tab's Resize event chain which PuttyBase subscribes to
            if (Parent is ConnectionTab tab)
            {
                // Temporarily adjust size to force a Resize event
                var sz = tab.ClientSize;
                tab.ClientSize = new Size(sz.Width, sz.Height - 1);
                tab.ClientSize = sz;
            }
        }

        public static InterfaceControl FindInterfaceControl(DockPanel DockPnl)
        {
            // instead of repeating the code, call the routine using ConnectionTab if called by DockPanel
            if (DockPnl.ActiveDocument is ConnectionTab ct)
                return FindInterfaceControl(ct);
            return null;
        }

        public static InterfaceControl FindInterfaceControl(ConnectionTab tab)
        {
            if (tab.Controls.Count < 1) return null;
            // if the tab has more than one controls and the second is an InterfaceControl than it must be a connection through SSH tunnel
            // and the first Control is the SSH tunnel connection and thus the second control must be returned.
            if (tab.Controls.Count > 1)
            {
                if (tab.Controls[1] is InterfaceControl ic1)
                    return ic1;
            }
            if (tab.Controls[0] is InterfaceControl ic0)
                return ic0;

            return null;
        }
    }
}